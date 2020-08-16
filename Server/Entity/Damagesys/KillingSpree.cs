namespace TDS_Server.Entity.Damagesys
{
    using AltV.Net;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TDS_Server.Data.Interfaces;
    using TDS_Server.Data.Interfaces.Entities;
    using TDS_Server.Database.Entity.LobbyEntities;
    using TDS_Shared.Default;

    partial class DamageSystem
    {
        #region Private Fields

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

        private static readonly Dictionary<short, string> _shortTimeKillingSpreeSounds = new Dictionary<short, string>
        {
            [2] = CustomSound.DoubleKill,
            [3] = CustomSound.TripleKill,
            [4] = CustomSound.UltraKill,
            [5] = CustomSound.Rampage
        };

        private Dictionary<short, LobbyKillingspreeRewards> _killingSpreeRewards = new Dictionary<short, LobbyKillingspreeRewards>();

        #endregion Private Fields

        #region Private Methods

        private void GiveKillingSpreeReward(ITDSPlayer player)
        {
            if (player.Lobby is null)
                return;
            if (!_killingSpreeRewards.TryGetValue(player.KillingSpree, out LobbyKillingspreeRewards? reward))
                return;

            if (reward.HealthOrArmor.HasValue)
            {
                player.AddHPArmor(reward.HealthOrArmor.Value);
                player.Lobby.SendAllPlayerLangNotification(lang
                    => string.Format(lang.KILLING_SPREE_HEALTHARMOR, player.DisplayName, player.KillingSpree, reward.HealthOrArmor));
            }
        }

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

            PlayKillingSpreeSound(player);
            GiveKillingSpreeReward(player);

            player.LastKillAt = timeNow;
        }

        private void PlayKillingSpreeSound(ITDSPlayer player)
        {
            bool playLongTimeKillSound = true;
            if (_shortTimeKillingSpreeSounds.Keys.Min() <= player.ShortTimeKillingSpree)
            {
                short playSoundIndex = Math.Min(player.ShortTimeKillingSpree, _shortTimeKillingSpreeSounds.Keys.Max());
                player.Lobby?.SendEvent(ToClientEvent.PlayCustomSound, _shortTimeKillingSpreeSounds[playSoundIndex]);

                //if (player.KillingSpree <= 5)
                //    playLongTimeKillSound = false;
            }

            if (playLongTimeKillSound && _longTimeKillingSpreeSounds.Keys.Min() <= player.KillingSpree)
            {
                short playSoundIndex = Math.Min(player.KillingSpree, _longTimeKillingSpreeSounds.Keys.Max());
                player.Lobby?.SendEvent(ToClientEvent.PlayCustomSound, _longTimeKillingSpreeSounds[playSoundIndex]);
            }
        }

        #endregion Private Methods
    }
}
