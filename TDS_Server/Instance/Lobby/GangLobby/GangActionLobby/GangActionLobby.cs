using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Enum;
using TDS_Server.Enum;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Instance.Lobby
{
    abstract partial class GangActionLobby : FightLobby
    {
        protected TDSPlayer AttackLeader;
        protected Gang AttackerGang;
        protected Gang OwnerGang;

        public GangActionLobby(EGangActionType type, TDSPlayer attacker, Gang ownerGang, string actionShort) : base(CreateEntity(type, attacker, ownerGang, actionShort))
        {
            SetPositionOnPlayerAdd = false;

            AttackerGang = attacker.Gang;
            OwnerGang = ownerGang;

            AttackerGang.InAction = true;
            OwnerGang.InAction = true;

            AttackLeader = attacker;
            SetAttackLeader(attacker);
        }

        private static Lobbies CreateEntity(EGangActionType type, TDSPlayer attacker, Gang ownerGang, string actionShort)
        {
            var dummyDBTeam = LobbyManager.MainMenu.Teams[0].Entity.DeepCopy();

            var attackerDBTeam = attacker.Gang.Entity.Team.DeepCopy();
            attackerDBTeam.Index = 1;

            var ownerDBTeam = ownerGang.Entity.Team.DeepCopy();
            ownerDBTeam.Index = 2;

            var lobbyType = type switch
            {
                EGangActionType.Gangwar => ELobbyType.GangwarLobby,
                _ => ELobbyType.GangwarLobby
                
            };

            var lobby = new Lobbies 
            {
                IsOfficial = true,
                IsTemporary = true,
                OwnerId = attacker.Entity!.Id,
                Name = $"[{actionShort}-Preparation] {attacker.Gang.Entity.Short}",
                Type = lobbyType,
                Teams = new List<Teams>
                {
                    dummyDBTeam,
                    attackerDBTeam,
                    ownerDBTeam
                },
            };


            return lobby;  
        }

        protected virtual string ActionTypeName => "Action";
        protected virtual string ActionTypeShort => "-";
    }
}
