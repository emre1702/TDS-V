using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Dto.Map;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Lobby;
using TDS_Server.Manager.Maps;

namespace TDS_Server.Manager.Utility
{
    static class LobbyManager
    {
        public static List<Lobby> Lobbies = new List<Lobby>();

        public static Lobby MainMenu => Lobbies[0];

        public static void LoadAllLobbies(TDSNewContext dbcontext)
        {
            dbcontext.RemoveRange(dbcontext.Lobbies.Where(l => l.IsTemporary));
            dbcontext.SaveChanges();

            List<Lobbies> lobbies = dbcontext.Lobbies
                .Include(l => l.Teams)
                .Include(l => l.LobbyWeapons)
                .Include(l => l.LobbyMaps)
                .ThenInclude((LobbyMaps map) => map.Map)
                .Include(l => l.OwnerNavigation)
                .AsNoTracking()
                .ToList();
            foreach (Lobbies lobbysetting in lobbies)
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
//todo todo Add after implementation of lobbies
                        //lobby = new GangLobby(lobbysetting);
                        //break;
                    //case ELobbyType.MapCreateLobby:
         
                    //    break;
                    default:
                        lobby = new Lobby(lobbysetting);
                        break;
                }
                Lobbies.Add(lobby);
                if (lobby is Arena arena)
                {
                    AddMapsToArena(arena, lobbysetting);
                }
                
            }
        }

        private static void AddMapsToArena(Arena arena, Lobbies lobbySetting)
        {
            List<MapDto> lobbyMapsList = new List<MapDto>();
            foreach (var mapAssignment in lobbySetting.LobbyMaps)
            {
                // All 
                if (mapAssignment.MapId == -1)
                {
                    arena.SetMapList(MapsLoader.AllMaps);
                    return;
                }

                // All Normals
                if (mapAssignment.MapId == -2)
                {
                    arena.SetMapList(MapsLoader.AllMaps.Where(m => m.Info.Type == EMapType.Normal).ToList());
                    return;
                }

                // All Bombs
                if (mapAssignment.MapId == -3)
                {
                    arena.SetMapList(MapsLoader.AllMaps.Where(m => m.Info.Type == EMapType.Bomb).ToList());
                    return;
                }

                lobbyMapsList.Add(MapsLoader.AllMaps.FirstOrDefault(m => m.SyncedData.Name == mapAssignment.Map.Name));
            }
            arena.SetMapList(lobbyMapsList);
        }

        public static Lobby GetLobby(uint id)
        {
            Lobby.LobbiesByIndex.TryGetValue(id, out Lobby lobby);
            return lobby;
        }

    }
}
