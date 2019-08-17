using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Sync;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Manager.Utility
{
    internal static class LobbyManager
    {
        public static List<Lobby> Lobbies { get; } = new List<Lobby>();

        public static Lobby MainMenu => Lobbies.Where(l => l.IsOfficial && l.LobbyEntity.Type == ELobbyType.MainMenu).First();
        public static Arena Arena => Lobbies.Where(l => l.IsOfficial && l.LobbyEntity.Type == ELobbyType.Arena).Cast<Arena>().First();


        public static async Task LoadAllLobbies(TDSNewContext dbcontext)
        {
            dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            var temporaryLobbies = await dbcontext.Lobbies.Where(l => l.IsTemporary).ToListAsync();
            dbcontext.Lobbies.RemoveRange(temporaryLobbies);
            await dbcontext.SaveChangesAsync();
            dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            List<Lobbies> lobbies = await dbcontext.Lobbies
                .Include(l => l.LobbyRewards)
                .Include(l => l.LobbyRoundSettings)
                .Include(l => l.LobbyMapSettings)
                .Include(l => l.Teams)
                .Include(l => l.LobbyWeapons)
                .Include(l => l.LobbyMaps)
                .ThenInclude((LobbyMaps map) => map.Map)
                .Include(l => l.Owner)
                .ToListAsync();
            foreach (Lobbies lobbysetting in lobbies)
            {
                ELobbyType type = lobbysetting.Type;
                var lobby = type switch
                {
                    ELobbyType.FightLobby => new FightLobby(lobbysetting),

                    ELobbyType.Arena => new Arena(lobbysetting),
                    //case ELobbyType.GangLobby:
                    //    lobby = new GangLobby(lobbysetting);
                    //    break;
                    //case ELobbyType.MapCreateLobby:

                    //    break;
                    _ => new Lobby(lobbysetting),
                };
                Lobbies.Add(lobby);
                Lobby.LobbiesByIndex[lobby.Id] = lobby;
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
                    arena.SetMapList(MapsLoader.AllMaps.Where(m => m.Info.Type == Enum.EMapType.Normal).ToList());
                    return;
                }

                // All Bombs
                if (mapAssignment.MapId == -3)
                {
                    arena.SetMapList(MapsLoader.AllMaps.Where(m => m.Info.Type == Enum.EMapType.Bomb).ToList());
                    return;
                }

                lobbyMapsList.Add(MapsLoader.AllMaps.FirstOrDefault(m => m.SyncedData.Name == mapAssignment.Map.Name));
            }
            arena.SetMapList(lobbyMapsList);
        }

        public static Lobby? GetLobby(int id)
        {
            Lobby.LobbiesByIndex.TryGetValue(id, out Lobby? lobby);
            return lobby;
        }

        public static async Task CreateCustomLobby(TDSPlayer player, string dataJson)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<CustomLobbyData>(dataJson);
                if (!IsCustomLobbyNameAllowed(data.Name))
                {
                    player.Client.TriggerEvent(DToClientEvent.CreateCustomLobbyResponse, player.Language.CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR);
                    return;
                }
                bool nameAlreadyInUse = Lobbies.Any(lobby => lobby.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase));
                if (nameAlreadyInUse)
                {
                    player.Client.TriggerEvent(DToClientEvent.CreateCustomLobbyResponse, player.Language.CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR);
                    return;
                }

                Lobbies entity = new Lobbies
                {
                    Name = data.Name,
                    OwnerId = player.Entity?.Id ?? 0,
                    AmountLifes = data.AmountLifes,
                    IsOfficial = false,
                    IsTemporary = true,
                    LobbyRoundSettings = new LobbyRoundSettings
                    { 
                        RoundTime = data.RoundTime, CountdownTime = data.CountdownTime, BombDetonateTimeMs = data.BombDetonateTimeMs,
                        BombDefuseTimeMs = data.BombDefuseTimeMs, BombPlantTimeMs = data.BombPlantTimeMs, MixTeamsAfterRound = data.MixTeamsAfterRound
                    },
                    LobbyMapSettings = new LobbyMapSettings
                    {
                        MapLimitTime = data.MapLimitTime,
                        MapLimitType = data.MapLimitType
                    },
                    LobbyMaps = new HashSet<LobbyMaps> { new LobbyMaps { MapId = -1 } },
                    LobbyWeapons = new HashSet<LobbyWeapons>
                    {
                        new LobbyWeapons { Hash = EWeaponHash.AssaultRifle, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.Revolver, Ammo = 500 },
                        new LobbyWeapons { Hash = EWeaponHash.UpnAtomizer, Ammo = 500 },
                        new LobbyWeapons { Hash = EWeaponHash.SMG, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.MicroSMG, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.UnholyHellbringer, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.AssaultShotgun, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.CarbineRifleMK2, Ammo = 2000 }
                    },
                    Password = data.Password,
                    SpawnAgainAfterDeathMs = data.SpawnAgainAfterDeathMs,
                    StartArmor = data.StartArmor,
                    StartHealth = data.StartHealth,
                    Teams = new HashSet<Teams>
                    {
                        new Teams { Index = 0, Name = "Spectator", ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 1004114196 },
                        new Teams { Index = 1, Name = "SWAT", ColorR = 0, ColorG = 150, ColorB = 0, BlipColor = 52, SkinHash = -1920001264 },
                        new Teams { Index = 2, Name = "Terrorist", ColorR = 150, ColorG = 0, ColorB = 0, BlipColor = 1, SkinHash = 275618457 },
                    },
                    Type = ELobbyType.Arena
                };
                //entity.LobbyMaps.Add(new LobbyMaps { MapId = -1 });

                Arena arena = new Arena(entity);
                arena.DbContext.Entry(entity).State = EntityState.Added;
                await arena.DbContext.SaveChangesAsync();

                await arena.DbContext.Entry(entity).Reference(e => e.Owner).LoadAsync();

                AddLobby(arena);
                AddMapsToArena(arena, entity);

                CustomLobbyMenuSync.RemovePlayer(player);
                await arena.AddPlayer(player, null);
            }
            catch
            {
                player.Client.TriggerEvent(DToClientEvent.CreateCustomLobbyResponse, player.Language.CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR);
            }
        }

        private static bool IsCustomLobbyNameAllowed(string name)
        {
            if (name.Length < 3 || name.Length > 50)
                return false;

            if (name.StartsWith("mapcreator-", StringComparison.CurrentCultureIgnoreCase))
                return false;

            return true;
        }

        public static void AddLobby(Lobby lobby)
        {
            Lobbies.Add(lobby);
            Lobby.LobbiesByIndex[lobby.Id] = lobby;
        }

        public static void RemoveLobby(Lobby lobby)
        {
            Lobbies.Remove(lobby);
            CustomLobbyMenuSync.SyncLobbyRemoved(lobby);
        }
    }
}