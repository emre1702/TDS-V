namespace TDS_Server.Core.Damagesystem
{
    using GTANetworkAPI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TDS_Common.Default;
    using TDS_Server.Instance.PlayerInstance;
    using TDS_Server.Manager.Utility;
    using TDS_Server.Database.Entity.LobbyEntities;

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
            if (killingspreeRewards is null || killingspreeRewards.Count == 0)
                return;
            _killingSpreeRewards = killingspreeRewards.ToDictionary(v => v.KillsAmount, v => v);
        }

        private static void KillingSpreeDeath(TDSPlayer player)
        {
            player.KillingSpree = 0;
        }

        private static void KillingSpreeKill(TDSPlayer player)
        {
            ++player.KillingSpree;

            var timeNow = DateTime.UtcNow;
            if (player.LastKillAt is null)
            {
                player.LastKillAt = timeNow;
                return;
            }

            bool playLongTimeKillSound = true;

            if (_shortTimeKillingSpreeSounds.Keys.Min() <= player.ShortTimeKillingSpree)
            {
                short playSoundIndex = Math.Min(player.ShortTimeKillingSpree, _shortTimeKillingSpreeSounds.Keys.Max());
                player.CurrentLobby?.SendAllPlayerEvent(DToClientEvent.PlayCustomSound, null, _shortTimeKillingSpreeSounds[playSoundIndex]);
                //if (player.KillingSpree <= 5)
                //    playLongTimeKillSound = false;
            }

            if (playLongTimeKillSound && _longTimeKillingSpreeSounds.Keys.Min() <= player.KillingSpree)
            {
                short playSoundIndex = Math.Min(player.KillingSpree, _longTimeKillingSpreeSounds.Keys.Max());
                player.CurrentLobby?.SendAllPlayerEvent(DToClientEvent.PlayCustomSound, null, _longTimeKillingSpreeSounds[playSoundIndex]);
            }

            player.LastKillAt = timeNow;
        }



        /*private static readonly Dictionary<int, Tuple<string, int, int>> sSpreeReward =
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
            if (character.CurrentLobby is null)
                return;

            Tuple<string, int, int> reward = sSpreeReward[character.KillingSpree];
            string rewardtyp = reward.Item1;
            if (rewardtyp == "healtharmor")
            {
                int bonus = reward.Item2;
                character.CurrentLobby.SendAllPlayerLangNotification((lang) =>
                {
                    return Utils.GetReplaced(lang.KILLING_SPREE_HEALTHARMOR, character.Player.Name,
                        character.KillingSpree.ToString(), bonus.ToString());
                });
                character.AddHPArmor(bonus);
            }
        }*/
    }
}