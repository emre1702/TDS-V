using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Default;

namespace TDS_Server.Core.Damagesystem
{
    partial class Damagesys
    {
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

        private void GiveKillingSpreeReward(ITDSPlayer player)
        {
            if (player.Lobby is null)
                return;
            if (!_killingSpreeRewards.TryGetValue(player.Deathmatch.KillingSpree, out LobbyKillingspreeRewards? reward))
                return;

            if (reward.HealthOrArmor.HasValue)
            {
                player.HealthAndArmor.Add(reward.HealthOrArmor.Value, out int givenEffectiveHp);
                player.Lobby.Notifications.Send(lang
                    => string.Format(lang.KILLING_SPREE_HEALTHARMOR, player.DisplayName, player.Deathmatch.KillingSpree, givenEffectiveHp));
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
            player.Deathmatch.KillingSpree = 0;
        }

        private void KillingSpreeKill(ITDSPlayer player)
        {
            if (player.Lobby is null)
                return;
            ++player.Deathmatch.KillingSpree;

            var timeNow = DateTime.UtcNow;
            if (player.Deathmatch.LastKillAt is null)
            {
                player.Deathmatch.LastKillAt = timeNow;
                return;
            }

            PlayKillingSpreeSound(player);
            GiveKillingSpreeReward(player);

            player.Deathmatch.LastKillAt = timeNow;
        }

        private void PlayKillingSpreeSound(ITDSPlayer player)
        {
            bool playLongTimeKillSound = true;
            if (_shortTimeKillingSpreeSounds.Keys.Min() <= player.Deathmatch.ShortTimeKillingSpree)
            {
                short playSoundIndex = Math.Min(player.Deathmatch.ShortTimeKillingSpree, _shortTimeKillingSpreeSounds.Keys.Max());
                player.Lobby?.Sync.TriggerEvent(ToClientEvent.PlayCustomSound, _shortTimeKillingSpreeSounds[playSoundIndex]);
                //if (player.KillingSpree <= 5)
                //    playLongTimeKillSound = false;
            }

            if (playLongTimeKillSound && _longTimeKillingSpreeSounds.Keys.Min() <= player.Deathmatch.KillingSpree)
            {
                short playSoundIndex = Math.Min(player.Deathmatch.KillingSpree, _longTimeKillingSpreeSounds.Keys.Max());
                player.Lobby?.Sync.TriggerEvent(ToClientEvent.PlayCustomSound, _longTimeKillingSpreeSounds[playSoundIndex]);
            }
        }
    }
}
