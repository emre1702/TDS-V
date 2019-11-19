using System;
using System.Threading.Tasks;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Gang;

namespace TDS_Server.Instance.Utility
{
    class GangwarArea
    {
        public GangwarAreas Entity { get; private set; }
        public Gang? Owner { get; private set; }

        public bool HasCooldown
        {
            get => (DateTime.UtcNow - Entity.LastAttacked).TotalMinutes < SettingsManager.ServerSettings.GangwarAreaAttackCooldownMinutes;
            set
            {
                if (value)
                    Entity.LastAttacked = DateTime.UtcNow;
                else 
                    Entity.LastAttacked = DateTime.UtcNow.AddMinutes(-SettingsManager.ServerSettings.GangwarAreaAttackCooldownMinutes);
            }
        }

        public GangwarArea(GangwarAreas entity)
        {
            Entity = entity;

            if (entity.OwnerGangId != 0)
            {
                Owner = Gang.GetById(entity.OwnerGangId);
            }
        }

        public void SetInPreparation()
        {
            //Todo inform the owner gang
        }

        public void SetInAttack()
        {
            //Todo inform the owner gang
        }

        public Task SetDefended()
        {
            using var dbContext = new TDSDbContext();
            dbContext.Attach(Entity);

            ++Entity.DefendCount;
            ++Entity.AttackCount;
            Entity.LastAttacked = DateTime.UtcNow;

            return dbContext.SaveChangesAsync();
        }

        public Task SetCaptured(Gang newOwner)
        {
            if (Owner is { })
            {
                //Todo inform the owner
            }
            Owner = newOwner;

            using var dbContext = new TDSDbContext();
            dbContext.Attach(Entity);

            Entity.OwnerGangId = newOwner.Entity.Id;
            Entity.DefendCount = 0;
            ++Entity.AttackCount;
            Entity.LastAttacked = DateTime.UtcNow;

            return dbContext.SaveChangesAsync();
        }


        public bool IsAtTarget(TDSPlayer player)
        {
            if (!player.LoggedIn)
                return false;
            if (player.CurrentLobby != LobbyManager.GangLobby)
                return false;
            if (player.Client.Dead)
                return false;

            //Todo Is in the skull or whatever
            return true;
        }
    }
}
