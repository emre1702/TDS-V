using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Instance
{

    partial class Damagesys
    {

        private static readonly Dictionary<TDSPlayer, TDSTimer> sDeadTimer = new Dictionary<TDSPlayer, TDSTimer>();

        public void OnPlayerDeath(TDSPlayer player, Client killer, uint weapon)
        {
            if (sDeadTimer.ContainsKey(player))
                return;
            Workaround.FreezePlayer(player.Client, true);

            player.KillingSpree = 0;

            if (player.Lifes <= 0)
                return;

            // Death //
            ++player.CurrentLobbyStats.Deaths;
            ++player.CurrentLobbyStats.TotalDeaths;

            TDSPlayer killercharacter = GetKiller(player, killer);
            killer = killercharacter.Client;

            // Kill //
            if (killercharacter != player)
            {
                ++killercharacter.CurrentRoundStats.Kills;
                ++killercharacter.KillingSpree;
            }

            // Assist //
            CheckForAssist(player, killer);
        }

        private TDSPlayer GetKiller(TDSPlayer player, Client possiblekiller)
        {
            // It's the killer from the Death event //
            if (player.Client != possiblekiller && possiblekiller != null && possiblekiller.Exists)
                return possiblekiller.GetChar();

            // It's the last hitter //
            if (player.LastHitter != null)
                return player.LastHitter;

            // It's a suicide //
            return player;
        }

        private void CheckForAssist(TDSPlayer character, Client killer)
        {
            if (allHitters.ContainsKey(character))
            {
                int halfarmorhp = character.CurrentLobby.StartTotalHP / 2;
                foreach (KeyValuePair<TDSPlayer, int> entry in allHitters[character])
                {
                    if (entry.Value >= halfarmorhp)
                    {
                        TDSPlayer targetcharacter = entry.Key;
                        Client target = targetcharacter.Client;
                        if (target.Exists && targetcharacter.CurrentLobby == character.CurrentLobby && killer != target)
                        {
                           ++targetcharacter.CurrentRoundStats.Assists;
                            NAPI.Notification.SendNotificationToPlayer(target, Utils.GetReplaced(character.Language.GOT_ASSIST, character.Client.Name));
                        }
                        if (killer != target ||
                            halfarmorhp % 2 != 0 ||
                            entry.Value != halfarmorhp / 2 ||
                            allHitters[character].Count > 2)
                            return;
                    }
                }
            }
        }

        public void CheckLastHitter(TDSPlayer character, out TDSPlayer killer)
        {
            killer = null;
            if (character.LastHitter != null)
            {
                TDSPlayer lastHitterCharacter = character.LastHitter;
                if (lastHitterCharacter.Client.Exists)
                {
                    if (character.CurrentLobby == lastHitterCharacter.CurrentLobby)
                        if (lastHitterCharacter.Lifes > 0)
                        {
                            ++lastHitterCharacter.CurrentRoundStats.Kills;
                            ++lastHitterCharacter.KillingSpree;
                            NAPI.Notification.SendNotificationToPlayer(lastHitterCharacter.Client, Utils.GetReplaced(lastHitterCharacter.Language.GOT_LAST_HITTED_KILL, character.Client.Name));
                            killer = lastHitterCharacter;
                        }
                }
                character.LastHitter = null;
            }
        }
    }

}
