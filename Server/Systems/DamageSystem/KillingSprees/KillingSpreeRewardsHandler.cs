using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.DamageSystem.KillingSprees
{
    internal class KillingSpreeRewardsHandler
    {
        private Dictionary<short, LobbyKillingspreeRewards>? _killingSpreeRewards;

        internal void Init(ICollection<LobbyKillingspreeRewards> killingspreeRewards)
        {
            if (killingspreeRewards is null || killingspreeRewards.Count == 0)
                return;
            _killingSpreeRewards = killingspreeRewards.ToDictionary(v => v.KillsAmount, v => v);
        }

        internal void PlayerDeath(ITDSPlayer killer)
        {
            if (killer.Lobby is null)
                return;
            if (_killingSpreeRewards is null)
                return;

            if (!_killingSpreeRewards.TryGetValue(killer.Deathmatch.KillingSpree, out LobbyKillingspreeRewards? reward))
                return;

            if (reward.HealthOrArmor.HasValue)
            {
                killer.HealthAndArmor.Add(reward.HealthOrArmor.Value, out int givenEffectiveHp);
                killer.Lobby.Notifications.Send(lang
                    => string.Format(lang.KILLING_SPREE_HEALTHARMOR, killer.DisplayName, killer.Deathmatch.KillingSpree, givenEffectiveHp));
            }
        }
    }
}
