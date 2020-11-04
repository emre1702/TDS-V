using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.DamageSystem.KillingSprees
{
    internal class KillingSpreeHandler
    {
        private readonly KillingSpreeSoundsHandler _soundsHandler;
        private readonly KillingSpreeRewardsHandler _rewardsHandler;

        internal KillingSpreeHandler()
        {
            _soundsHandler = new KillingSpreeSoundsHandler();
            _rewardsHandler = new KillingSpreeRewardsHandler();
        }

        internal void Init(ICollection<LobbyKillingspreeRewards> killingspreeRewards)
        {
            _rewardsHandler.Init(killingspreeRewards);
        }

        internal void PlayerDeath(ITDSPlayer died, ITDSPlayer killer)
        {
            if (killer.Lobby is null)
                return;
            died.Deathmatch.KillingSpree = 0;

            if (killer != died)
            {
                ++killer.Deathmatch.KillingSpree;

                var timeNow = DateTime.UtcNow;
                if (killer.Deathmatch.LastKillAt is null)
                    killer.Deathmatch.LastKillAt = timeNow;

                _soundsHandler.PlayerDeath(killer);
                _rewardsHandler.PlayerDeath(killer);
            }
        }
    }
}
