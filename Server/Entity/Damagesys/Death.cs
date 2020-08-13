using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Entity.Player;

namespace TDS_Server.Entity.Damagesys
{
    partial class Damagesys
    {
        #region Private Fields

        private readonly Dictionary<ITDSPlayer, ITDSPlayer> _deadTimer = new Dictionary<ITDSPlayer, ITDSPlayer>();

        #endregion Private Fields

        #region Public Methods

        public void CheckLastHitter(ITDSPlayer character, out ITDSPlayer? killer)
        {
            killer = null;
            if (character.LastHitter is null)
                return;

            ITDSPlayer lastHitterCharacter = character.LastHitter;
            character.LastHitter = null;

            if (character.Lobby != lastHitterCharacter.Lobby)
                return;

            if (lastHitterCharacter.Lifes == 0)
                return;

            if (lastHitterCharacter.CurrentRoundStats != null)
                ++lastHitterCharacter.CurrentRoundStats.Kills;
            KillingSpreeKill(lastHitterCharacter);
            lastHitterCharacter.SendNotification(string.Format(lastHitterCharacter.Language.GOT_LAST_HITTED_KILL, character.DisplayName));
            killer = lastHitterCharacter;
        }

        public ITDSPlayer GetKiller(ITDSPlayer player, ITDSPlayer? possiblekiller)
        {
            // It's the killer from the Death event //
            if (player != possiblekiller && possiblekiller != null)
                return possiblekiller;

            // It's the last hitter //
            if (player.LastHitter != null)
                return player.LastHitter;

            // It's a suicide //
            return player;
        }

        /// <summary>
        /// Gets called BEFORE decrementing the life of the player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="killer"></param>
        /// <param name="weapon"></param>
        public void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            if (_deadTimer.ContainsKey(player))
                return;

            player.Freeze(true);

            KillingSpreeDeath(player);

            if (player.Lifes <= 0)
                return;

            killer = GetKiller(player, killer);

            // Death //
            if (player.LobbyStats != null && player.Lobby?.SavePlayerLobbyStats == true)
            {
                ++player.LobbyStats.Deaths;
                ++player.LobbyStats.TotalDeaths;
            }

            // Kill //
            if (killer != player)
            {
                if (killer.CurrentRoundStats != null)
                {
                    ++killer.CurrentRoundStats.Kills;
                }

                KillingSpreeKill(killer);

                // Assist //
                CheckForAssist(player, killer);
            }

            if (player.Lobby?.SavePlayerLobbyStats == true && player.Lobby?.IsOfficial == true)
            {
                _loggingHandler.LogKill(player, killer, weapon);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void CheckForAssist(ITDSPlayer player, ITDSPlayer killer)
        {
            if (!_allHitters.ContainsKey(player))
                return;

            int halfarmorhp = player.Lobby!.StartTotalHP / 2;
            foreach (KeyValuePair<ITDSPlayer, int> entry in _allHitters[player])
            {
                if (entry.Value >= halfarmorhp)
                {
                    ITDSPlayer possibleAssister = entry.Key;
                    if (possibleAssister.LoggedIn && possibleAssister.Lobby == player.Lobby && possibleAssister != killer && possibleAssister.CurrentRoundStats is { })
                    {
                        ++possibleAssister.CurrentRoundStats.Assists;
                        possibleAssister.SendNotification(string.Format(possibleAssister.Language.GOT_ASSIST, player.DisplayName));
                    }
                    if (killer != possibleAssister ||
                        halfarmorhp % 2 != 0 ||
                        entry.Value != halfarmorhp / 2 ||
                        _allHitters[player].Count > 2)
                        break;
                }
            }
        }

        #endregion Private Methods
    }
}
