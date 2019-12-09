using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS_Common.Enum;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Lobby;
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
        public FightLobby InLobby { get; set; }

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

            if (entity.OwnerGangId != Gang.None.Entity.Id)
            {
                Owner = Gang.GetById(entity.OwnerGangId);
            }

            InLobby = LobbyManager.GangLobby;
        }

        public void SetInPreparation(GangwarLobby lobby)
        {
            if (Owner is null)
                return;

            foreach (var player in Owner.PlayersOnline)
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, string.Format(player.Language.GANGWAR_OWNER_PREPARATION_INFO, Entity.Map.Name, lobby.AttackerTeam.Entity.Name));
            }
            

        }

        public void SetInAttack(GangwarLobby lobby)
        {
            if (Owner is null)
                return;

            lobby.SendLangMessageToOwner(lang => string.Format(lang.GANGWAR_OWNER_STARTED_INFO, Entity.Map.Name, lobby.AttackerTeam.Entity.Name));
            foreach (var player in Owner.GangLobbyTeam.Players)
            {
                new Invitation(player.Language.GANGWAR_DEFEND_INVITATION, player, null, onAccept: AcceptDefendInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            }
        }

        public Task SetDefended()
        {
            InLobby = LobbyManager.GangLobby;

            using var dbContext = new TDSDbContext();
            dbContext.Attach(Entity);

            ++Entity.DefendCount;
            ++Entity.AttackCount;
            Entity.LastAttacked = DateTime.UtcNow;

            return dbContext.SaveChangesAsync();
        }

        public Task SetCaptured(Gang newOwner)
        {
            InLobby = LobbyManager.GangLobby;
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
            if (player.CurrentLobby?.Type != ELobbyType.GangLobby)
                return false;
            if (player.Client.Dead)
                return false;

            //Todo Is in the skull or whatever
            return true;
        }


        private async void AcceptDefendInvitation(TDSPlayer player, TDSPlayer? sender, Invitation invitation)
        {
            if (!(InLobby is GangwarLobby lobby))
                return;

            if (!await lobby.AddPlayer(player, (uint)lobby.OwnerTeam.Entity.Index))
            {
                invitation.Resend();
                return;
            }
                
            lobby.SendLangNotificationToAttacker(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
            lobby.SendLangNotificationToOwner(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
        }
    }
}

