namespace TDS_Server.Core.Damagesystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TDS_Server.Data.Interfaces;
    using TDS_Server.Database.Entity.LobbyEntities;
    using TDS_Shared.Default;

    partial class Damagesys
    {
        private Dictionary<short, LobbyKillingspreeRewards> _killingSpreeRewards = new Dictionary<short, LobbyKillingspreeRewards>();
        private static readonly Dictionary<short, string> _shortTimeKillingSpreeSounds = new Dictionary<short, string>
        {
            [2] = CustomSound.DoubleKill,
            [3] = CustomSound.TripleKill,
            [4] = CustomSound.UltraKill,
            [5] = CustomSound.Rampage
        };
        private static readonly Dictionary<short, string> _longTimeKillingSpreeSounds = new Dictionary<short, string>
        {
            [3] = CustomSound.KillingSpree,
            [4] = CustomSound.Dominating,
            [5] = CustomSound.MultiKill,
            [6] = CustomSound.Unstoppable,
            [7] = CustomSound.WickedSick,
            [8] = CustomSound.MonsterKill,
            [9] = CustomSound.Godlike,
            [10] = CustomSound.HolyShit
        };


        private void InitKillingSpreeRewards(ICollection<LobbyKillingspreeRewards> killingspreeRewards)
        {
            if (killingspreeRewards is null || killingspreeRewards.Count == 0)
                return;
            _killingSpreeRewards = killingspreeRewards.ToDictionary(v => v.KillsAmount, v => v);
        }

        private void KillingSpreeDeath(ITDSPlayer player)
        {
            player.KillingSpree = 0;
        }

        private void KillingSpreeKill(ITDSPlayer player)
        {
            if (player.Lobby is null)
                return;
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
                _modAPI.Sync.SendEvent(player.Lobby, ToClientEvent.PlayCustomSound, _shortTimeKillingSpreeSounds[playSoundIndex]);
                //if (player.KillingSpree <= 5)
                //    playLongTimeKillSound = false;
            }

            if (playLongTimeKillSound && _longTimeKillingSpreeSounds.Keys.Min() <= player.KillingSpree)
            {
                short playSoundIndex = Math.Min(player.KillingSpree, _longTimeKillingSpreeSounds.Keys.Max());
                _modAPI.Sync.SendEvent(player.Lobby, ToClientEvent.PlayCustomSound, _longTimeKillingSpreeSounds[playSoundIndex]);
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
            if (character.Lobby is null)
                return;

            Tuple<string, int, int> reward = sSpreeReward[character.KillingSpree];
            string rewardtyp = reward.Item1;
            if (rewardtyp == "healtharmor")
            {
                int bonus = reward.Item2;
                character.Lobby.SendAllPlayerLangNotification((lang) =>
                {
                    return string.Format(lang.KILLING_SPREE_HEALTHARMOR, character.Player.Name,
                        character.KillingSpree.ToString(), bonus.ToString());
                });
                character.AddHPArmor(bonus);
            }
        }*/
    }
}
