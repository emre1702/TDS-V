using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Players;

namespace TDS_Server.Core.Damagesystem
{
    partial class Damagesys
    {
        private readonly Dictionary<ITDSPlayer, ITDSPlayer> _deadTimer = new Dictionary<ITDSPlayer, ITDSPlayer>();

        public void RewardLastHitter(ITDSPlayer player, out ITDSPlayer? killer)
        {
            killer = null;
            if (player.Deathmatch.LastHitter is null)
                return;

            ITDSPlayer lastHitter = player.Deathmatch.LastHitter;
            player.Deathmatch.LastHitter = null;

            if (player.Lobby != lastHitter.Lobby)
                return;

            if (lastHitter.Lifes == 0)
                return;

            if (lastHitter.CurrentRoundStats != null)
                ++lastHitter.CurrentRoundStats.Kills;
            KillingSpreeKill(lastHitter);
            lastHitter.SendNotification(string.Format(lastHitter.Language.GOT_LAST_HITTED_KILL, player.DisplayName));
            killer = lastHitter;
        }

        public ITDSPlayer GetKiller(ITDSPlayer player, ITDSPlayer? possiblekiller)
        {
            // It's the killer from the Death event //
            if (player != possiblekiller && possiblekiller != null)
                return possiblekiller;

            // It's the last hitter //
            if (player.Deathmatch.LastHitter != null)
                return player.Deathmatch.LastHitter;

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

            var lobbyPlayersHandler = player.Lobby?.Players as IRoundFightLobbyPlayers;

            // Death //
            if (player.LobbyStats != null && lobbyPlayersHandler?.SavePlayerLobbyStats == true)
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

            if (lobbyPlayersHandler?.SavePlayerLobbyStats == true && player.Lobby?.IsOfficial == true)
            {
                _loggingHandler.LogKill(player, killer, weapon);
            }
        }

        private void CheckForAssist(ITDSPlayer player, ITDSPlayer killer)
        {
            if (!_allHitters.ContainsKey(player))
                return;

            int halfarmorhp = (player.Lobby!.Entity.FightSettings.StartArmor + player.Lobby!.Entity.FightSettings.StartHealth) / 2;
            foreach (KeyValuePair<ITDSPlayer, int> entry in _allHitters[player])
            {
                if (entry.Value >= halfarmorhp)
                {
                    ITDSPlayer possibleAssister = entry.Key;
                    if (possibleAssister.Lobby == player.Lobby && possibleAssister != killer && possibleAssister.CurrentRoundStats is { })
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
    }
}
