using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Lobby;

namespace TDS.Manager.Utility
{
    static class LobbyManager
    {
        public static async Task LoadAllLobbies()
        {
            using (TDSNewContext dbcontext = new TDSNewContext())
            {
                dbcontext.RemoveRange(dbcontext.Lobbies.Where(l => l.IsTemporary));
                await dbcontext.SaveChangesAsync();

                List<Lobbies> teamlist = await dbcontext.Lobbies
                    .Include(l => l.Teams)
                    .Include(l => l.LobbyWeapons)
                    .AsNoTracking()
                    .ToListAsync();
                foreach (Lobbies lobbysetting in teamlist)
                {
                    ELobbyType type = (ELobbyType)lobbysetting.Type;
                    Lobby lobby;
                    switch (type)
                    {
                        case ELobbyType.FightLobby:
                            lobby = new FightLobby(lobbysetting);
                            break;
                        case ELobbyType.Arena:
                            lobby = new Arena(lobbysetting);
                            break;
                        //case ELobbyType.GangLobby:
#warning todo Add after implementation of lobbies
                            //lobby = new GangLobby(lobbysetting);
                            //break;
                        //case ELobbyType.MapCreateLobby:
         
                        //    break;
                        default:
                            lobby = new Lobby(lobbysetting);
                            break;
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
