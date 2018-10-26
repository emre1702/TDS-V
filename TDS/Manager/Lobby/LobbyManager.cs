using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS.Entity;

namespace TDS.Manager.Lobby
{
    static class LobbyManager
    {
        public static async Task LoadAllLobbies(TDSNewContext dbcontext)
        {
            dbcontext.RemoveRange(dbcontext.Lobbies.Where(l => l.IsTemporary));
            foreach (Lobbies lobbysetting in await dbcontext.Lobbies.AsNoTracking().ToListAsync())
            {
                new Instance.Lobby.Lobby(lobbysetting);
            }
        }

        public static Instance.Lobby.Lobby GetLobby(uint id)
        {
            Instance.Lobby.Lobby.LobbiesByIndex.TryGetValue(id, out Instance.Lobby.Lobby lobby);
            return lobby;
        }

    }
}
