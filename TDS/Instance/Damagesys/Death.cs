using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Instance.Lobby;
using TDS.Instance.Player;
using TDS.Instance.Utility;
using TDS.Manager.Player;
using TDS.Manager.Utility;

namespace TDS.Instance
{

    partial class Damagesys
    {

        private static readonly Dictionary<TDSPlayer, Timer> sDeadTimer = new Dictionary<TDSPlayer, Timer>();
        public Dictionary<Client, uint> PlayerAssists = new Dictionary<Client, uint>(),
                                        PlayerKills = new Dictionary<Client, uint>();

        public void OnPlayerDeath(TDSPlayer player, Client killer, uint weapon)
        {
            if (!sDeadTimer.ContainsKey(player))
            {
                player.Client.Freeze(true);

                TDSPlayer killercharacter = GetKiller(player, killer.GetChar());
                killer = killercharacter.Client;

                PlayerSpree.Remove(player);

                if (player.Lifes > 0)
                {
                    // Kill //
                    if (killercharacter != player)
                    {
                        ++killercharacter.CurrentRoundStats.Kills;
                        if (!PlayerKills.ContainsKey(killer))
                            PlayerKills.TryAdd(killer, 0);
                        PlayerKills[killer]++;

                        // Killingspree //
                        AddToKillingSpree(killercharacter);
                    }

                    // Death //
                    ++player.CurrentRoundStats.Deaths;

                    // Assist //
                    CheckForAssist(player, killer);
                }
            }
        }

        private TDSPlayer GetKiller(TDSPlayer player, TDSPlayer possiblekiller)
        {
            if (player.Client != possiblekiller.Client && possiblekiller.Client != null && possiblekiller.Client.Exists)
                return possiblekiller;

            if (player.LastHitter != null)
                return player.LastHitter;

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
                            if (targetcharacter.CurrentLobby is Arena)
                                ++targetcharacter.CurrentRoundStats.Assists;
                            NAPI.Notification.SendNotificationToPlayer(target, Utils.GetReplaced(character.Language.GOT_ASSIST, character.Client.Name));

                            if (!PlayerAssists.ContainsKey(target))
                                PlayerAssists[target] = 0;
                            PlayerAssists[target]++;
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

        public void CheckLastHitter(TDSPlayer character)
        {
            if (character.LastHitter != null)
            {
                TDSPlayer lastHitterCharacter = character.LastHitter;
                if (lastHitterCharacter.Client.Exists)
                {
                    if (character.CurrentLobby == lastHitterCharacter.CurrentLobby)
                        if (lastHitterCharacter.Lifes > 0)
                        {
                            ++lastHitterCharacter.CurrentRoundStats.Kills;
                            NAPI.Notification.SendNotificationToPlayer(lastHitterCharacter.Client, Utils.GetReplaced(lastHitterCharacter.Language.GOT_LAST_HITTED_KILL, character.Client.Name));
                            AddToKillingSpree(lastHitterCharacter);
                        }
                }
                character.LastHitter = null;
            }
        }
    }

}
