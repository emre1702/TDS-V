using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Entity;
using TDS.Instance.Lobby;

namespace TDS.Manager.Utility
{
    static class LobbyManager
    {
        public static async Task LoadAllLobbiesWithTeams()
        {
            using (TDSNewContext dbcontext = new TDSNewContext())
            {
                dbcontext.RemoveRange(dbcontext.Lobbies.Where(l => l.IsTemporary));
                await dbcontext.SaveChangesAsync();

                List<Lobbies> teamlist = await dbcontext.Lobbies.Include(l => l.Teams).AsNoTracking().ToListAsync();
                foreach (Lobbies lobbysetting in teamlist)
                {
                    Lobby lobby = new Lobby(lobbysetting);
                    foreach (Teams team in lobbysetting.Teams)
                    {
                        lobby.AddTeam(team);
                    }
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
