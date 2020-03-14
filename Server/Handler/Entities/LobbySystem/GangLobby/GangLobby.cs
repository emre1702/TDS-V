using System.Collections.Generic;
using System.Linq;
using TDS_Shared.Data.Enums;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Manager.Utility;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Handler.Entities.LobbySystem.GangLobby
{
    partial class GangLobby : FightLobby
    {
        public GangLobby(Lobbies lobbyEntity) : base(lobbyEntity)
        {
            foreach (var team in Teams)
            {
                var teamId = team.Entity.Id;
                var gang = Gang.GetByTeamId(teamId);
                if (gang != null)
                {
                    gang.GangLobbyTeam = team;
                }
            }
        }

        public static IEnumerable<GangLobby> GetAllDerivedLobbies()
        {
            return LobbyManager.Lobbies.Where(l => l is GangLobby && l.Type != ELobbyType.GangLobby).Cast<GangLobby>();
        }
    }
}