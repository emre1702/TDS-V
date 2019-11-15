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
        private TDSPlayer _attackLeader;
        private Gang _attackerGang;
        private Gang _ownerGang;

        public GangActionLobby(EGangActionType type, TDSPlayer attacker, Gang ownerGang) : base(CreateEntity(type, attacker, ownerGang))
        {
            _attackLeader = attacker;

            _attackerGang = attacker.Gang;
            _ownerGang = ownerGang;

            _attackerGang.InAction = true;
            _ownerGang.InAction = true;
        }

        private static Lobbies CreateEntity(EGangActionType type, TDSPlayer attacker, Gang ownerGang)
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
                Name = $"[GW] {attacker.Gang.Entity.Short} vs. {ownerGang.Entity.Short}",
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
    }
}
