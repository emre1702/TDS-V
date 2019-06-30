namespace TDS_Server.Instance
{
    using GTANetworkAPI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TDS_Common.Default;
    using TDS_Server.Instance.Player;
    using TDS_Server.Manager.Utility;
    using TDS_Server_DB.Entity.Lobby;

    partial class Damagesys
    {
        private Dictionary<short, LobbyKillingspreeRewards> _killingSpreeRewards = new Dictionary<short, LobbyKillingspreeRewards>();
        private static readonly Dictionary<short, string> _shortTimeKillingSpreeSounds = new Dictionary<short, string>
        {
            [2] = DCustomSound.DoubleKill,
            [3] = DCustomSound.TripleKill,
            [4] = DCustomSound.UltraKill,
            [5] = DCustomSound.Rampage
        };
        private static readonly Dictionary<short, string> _longTimeKillingSpreeSounds = new Dictionary<short, string>
        {
            [3] = DCustomSound.KillingSpree,
            [4] = DCustomSound.Dominating,
            [5] = DCustomSound.MultiKill,
            [6] = DCustomSound.Unstoppable,
            [7] = DCustomSound.WickedSick,
            [8] = DCustomSound.MonsterKill,
            [9] = DCustomSound.Godlike,
            [10] = DCustomSound.HolyShit
        };


        private void InitKillingSpreeRewards(ICollection<LobbyKillingspreeRewards> killingspreeRewards)
        {
            if (killingspreeRewards.Count == 0)
                return;
            _killingSpreeRewards = killingspreeRewards.ToDictionary(v => v.KillsAmount, v => v);
        }

        private void KillingSpreeDeath(TDSPlayer player)
        {
            player.KillingSpree = 0;
        }

        private void KillingSpreeKill(TDSPlayer player)
        {
            ++player.KillingSpree;

            var timeNow = DateTime.UtcNow;
            if (player.LastKillAt == null)
            {
                player.LastKillAt = timeNow;
                return;
            }

            bool playLongTimeKillSound = true;
            var timeSpanSinceLastKill = DateTime.UtcNow - player.LastKillAt.Value;
            if (timeSpanSinceLastKill.TotalSeconds <= SettingsManager.KillingSpreeMaxSecondsUntilNextKill)
            {
                short playSoundIndex = Math.Min(player.KillingSpree, _shortTimeKillingSpreeSounds.Keys.Max());
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayCustomSound, _shortTimeKillingSpreeSounds[playSoundIndex]);
                //if (player.KillingSpree <= 5)
                //    playLongTimeKillSound = false;
            }

            if (playLongTimeKillSound)
            {
                short playSoundIndex = Math.Min(player.KillingSpree, _longTimeKillingSpreeSounds.Keys.Max());
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayCustomSound, _longTimeKillingSpreeSounds[playSoundIndex]);
            }

            player.LastKillAt = timeNow;
        }



        private static readonly Dictionary<int, Tuple<string, int, int>> sSpreeReward =
            new Dictionary<int, Tuple<string, int, int>>
            {
                [3] = new Tuple<string, int, int>("healtharmor", 30, 0),
                [5] = new Tuple<string, int, int>("healtharmor", 50, 0),
                [10] = new Tuple<string, int, int>("healtharmor", 100, 0),
                [15] = new Tuple<string, int, int>("healtharmor", 100, 0)
            };

        private void CheckKillingSpree(TDSPlayer character)
        {
            if (!sSpreeReward.ContainsKey(character.KillingSpree))
                return;
            if (character.CurrentLobby == null)
                return;

            Tuple<string, int, int> reward = sSpreeReward[character.KillingSpree];
            string rewardtyp = reward.Item1;
            if (rewardtyp == "healtharmor")
            {
                int bonus = reward.Item2;
                character.CurrentLobby.SendAllPlayerLangNotification((lang) =>
                {
                    return Utils.GetReplaced(lang.KILLING_SPREE_HEALTHARMOR, character.Client.Name,
                        character.KillingSpree.ToString(), bonus.ToString());
                });
                character.AddHPArmor(bonus);
            }
        }
    }
}