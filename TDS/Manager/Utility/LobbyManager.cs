using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS.Entity;
using TDS.Instance.Lobby;

namespace TDS.Manager.Utility
{
    static class LobbyManager
    {
        public static async Task LoadAllLobbiesWithTeams(TDSNewContext dbcontext)
        {
            dbcontext.RemoveRange(dbcontext.Lobbies.Where(l => l.IsTemporary));
            foreach (Lobbies lobbysetting in await dbcontext.Lobbies.AsNoTracking().ToListAsync())
            {
                new Lobby(lobbysetting);
            }

            foreach (Teams team in await dbcontext.Teams.AsNoTracking().ToListAsync())
            {
                if (!team.Lobby.HasValue)
                {
                    Lobby.SpectatorTeam = team;
                    foreach (var lobby in Lobby.LobbiesByIndex.Values)
                    {
                        lobby.AddTeam(team);
                    }
                }
                else
                {
                    Lobby.LobbiesByIndex[team.Lobby.Value].AddTeam(team);
                }
            }
        }

        public static Lobby GetLobby(uint id)
        {
            Lobby.LobbiesByIndex.TryGetValue(id, out Lobby lobby);
            return lobby;
        }

    }
}
