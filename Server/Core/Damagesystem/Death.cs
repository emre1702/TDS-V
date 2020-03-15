using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Instance.Utility;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Core.Damagesystem
{
    partial class Damagesys
    {
        private static readonly Dictionary<TDSPlayer, TDSTimer> sDeadTimer = new Dictionary<TDSPlayer, TDSTimer>();

        public void OnPlayerDeath(TDSPlayer player, TDSPlayer killer, uint weapon)
        {
            if (sDeadTimer.ContainsKey(player))
                return;
            Workaround.FreezePlayer(player.Player!, true);

            KillingSpreeDeath(player);

            if (player.Lifes <= 0)
                return;

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
            }

            // Assist //
            CheckForAssist(player, killer.Player!);

            if (player.Lobby?.SavePlayerLobbyStats == true && player.Lobby?.IsOfficial == true)
            {
                KillLogsManager.Log(player, killer, weapon);
            }
            
        }

        public TDSPlayer GetKiller(TDSPlayer player, Player? possiblekiller)
        {
            // It's the killer from the Death event //
            if (player.Player != possiblekiller && possiblekiller != null && possiblekiller.Exists)
                return possiblekiller.GetChar();

            // It's the last hitter //
            if (player.LastHitter != null)
                return player.LastHitter;

            // It's a suicide //
            return player;
        }

        private void CheckForAssist(TDSPlayer character, Player killerClient)
        {
            if (!_allHitters.ContainsKey(character))
                return;

            int halfarmorhp = character.Lobby!.StartTotalHP / 2;
            foreach (KeyValuePair<TDSPlayer, int> entry in _allHitters[character])
            {
                if (entry.Value >= halfarmorhp)
                {
                    TDSPlayer target = entry.Key;
                    Player? targetClient = target.Player;
                    if (targetClient is { } && targetClient.Exists && target.Lobby == character.Lobby && killerClient != targetClient && target.CurrentRoundStats != null)
                    {
                        ++target.CurrentRoundStats.Assists;
                        target.SendNotification(Utils.GetReplaced(character.Language.GOT_ASSIST, character.DisplayName));
                    }
                    if (killerClient != targetClient ||
                        halfarmorhp % 2 != 0 ||
                        entry.Value != halfarmorhp / 2 ||
                        _allHitters[character].Count > 2)
                        return;
                }
            }
        }

        public void CheckLastHitter(TDSPlayer character, out TDSPlayer? killer)
        {
            killer = null;
            if (character.LastHitter is null)
                return;

            TDSPlayer lastHitterCharacter = character.LastHitter;
            character.LastHitter = null;

            if (lastHitterCharacter.Player is null || !lastHitterCharacter.Player.Exists)
                return;

            if (character.Lobby != lastHitterCharacter.Lobby)
                return;

            if (lastHitterCharacter.Lifes == 0)
                return;

            if (lastHitterCharacter.CurrentRoundStats != null)
                ++lastHitterCharacter.CurrentRoundStats.Kills;
            KillingSpreeKill(lastHitterCharacter);
            lastHitterCharacter.SendNotification(Utils.GetReplaced(lastHitterCharacter.Language.GOT_LAST_HITTED_KILL, character.DisplayName));
            killer = lastHitterCharacter;
        }
    }
}
