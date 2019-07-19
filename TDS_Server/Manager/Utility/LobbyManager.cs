using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Maps;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Manager.Utility
{
    internal static class LobbyManager
    {
        public static List<Lobby> Lobbies { get; } = new List<Lobby>();
        public static List<TDSPlayer> PlayerInCustomLobbyMenu { get; } = new List<TDSPlayer>();

        public static Lobby MainMenu => Lobbies.Where(l => l.IsOfficial && l.LobbyEntity.Type == ELobbyType.MainMenu).First();
        public static Arena Arena => Lobbies.Where(l => l.IsOfficial && l.LobbyEntity.Type == ELobbyType.Arena).Cast<Arena>().First();

        static LobbyManager()
        {
            CustomEventManager.OnPlayerLoggedOut += (player) =>
            {
                SetPlayerInCustomLobbyMenu(player, false);
            };
        }

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
                    //    lobby = new GangLobby(lobbysetting);
                    //    break;
                    //case ELobbyType.MapCreateLobby:

                    //    break;
                    default:
                        lobby = new Lobby(lobbysetting);
                        break;
                }
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

        public static Lobby GetLobby(int id)
        {
            Lobby.LobbiesByIndex.TryGetValue(id, out Lobby lobby);
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
                    DieAfterOutsideMapLimitTime = data.DieAfterOutsideMapLimitTime,
                    IsOfficial = false,
                    IsTemporary = true,
                    LobbyRoundSettings = new LobbyRoundSettings
                    { 
                        RoundTime = data.RoundTime, CountdownTime = data.CountdownTime, BombDetonateTimeMs = data.BombDetonateTimeMs,
                        BombDefuseTimeMs = data.BombDefuseTimeMs, BombPlantTimeMs = data.BombPlantTimeMs, MixTeamsAfterRound = data.MixTeamsAfterRound
                    },
                    LobbyMapSettings = new LobbyMapSettings
                    {
                        MapLimitType = EMapLimitType.Block
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

                PlayerInCustomLobbyMenu.Remove(player);
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

        public static void SetPlayerInCustomLobbyMenu(TDSPlayer player, bool isIn)
        {
            if (isIn)
            {
                PlayerInCustomLobbyMenu.Add(player);
                List<CustomLobbyData> lobbyDatas = Lobbies.Where(l => !l.IsOfficial && l.LobbyEntity.Type != ELobbyType.MapCreateLobby)
                                                            .Select(l => GetCustomLobbyData(l))
                                                            .ToList();

                player.Client.TriggerEvent(DToClientEvent.SyncAllCustomLobbies, JsonConvert.SerializeObject(lobbyDatas));
            }
            else
                PlayerInCustomLobbyMenu.Remove(player);
        }

        public static void AddLobby(Lobby lobby)
        {
            Lobbies.Add(lobby);
            Lobby.LobbiesByIndex[lobby.Id] = lobby;
            if (!lobby.IsOfficial && lobby.LobbyEntity.Type != ELobbyType.MapCreateLobby)
            {
                string json = JsonConvert.SerializeObject(GetCustomLobbyData(lobby));
                for (int i = PlayerInCustomLobbyMenu.Count - 1; i >= 0; --i)
                {
                    TDSPlayer player = PlayerInCustomLobbyMenu[i];
                    if (!player.LoggedIn)
                    {
                        PlayerInCustomLobbyMenu.RemoveAt(i);
                        continue;
                    }
                    player.Client.TriggerEvent(DToClientEvent.AddCustomLobby, json);
                }
            }
        }

        public static void RemoveLobby(Lobby lobby)
        {
            Lobbies.Remove(lobby);
            if (!lobby.IsOfficial && lobby.LobbyEntity.Type != ELobbyType.MapCreateLobby)
            {
                for (int i = PlayerInCustomLobbyMenu.Count - 1; i >= 0; --i)
                {
                    TDSPlayer player = PlayerInCustomLobbyMenu[i];
                    if (!player.LoggedIn)
                    {
                        PlayerInCustomLobbyMenu.RemoveAt(i);
                        continue;
                    }
                    player.Client.TriggerEvent(DToClientEvent.RemoveCustomLobby, lobby.Id);
                }
            }
        }

        private static CustomLobbyData GetCustomLobbyData(Lobby lobby)
        {
            return new CustomLobbyData
            {
                AmountLifes = lobby.LobbyEntity.AmountLifes ?? 1,
                DieAfterOutsideMapLimitTime = lobby.LobbyEntity.DieAfterOutsideMapLimitTime,
                LobbyId = lobby.LobbyEntity.Id,
                Name = lobby.LobbyEntity.Name,
                OwnerName = lobby.OwnerName,
                Password = lobby.LobbyEntity.Password,
                SpawnAgainAfterDeathMs = lobby.LobbyEntity.SpawnAgainAfterDeathMs,
                StartArmor = lobby.LobbyEntity.StartArmor,
                StartHealth = lobby.LobbyEntity.StartHealth,

                RoundTime = lobby.LobbyEntity.LobbyRoundSettings.RoundTime,
                MixTeamsAfterRound = lobby.LobbyEntity.LobbyRoundSettings.MixTeamsAfterRound,
                CountdownTime = lobby.LobbyEntity.LobbyRoundSettings.CountdownTime,
                BombPlantTimeMs = lobby.LobbyEntity.LobbyRoundSettings.BombPlantTimeMs,
                BombDetonateTimeMs = lobby.LobbyEntity.LobbyRoundSettings.BombDetonateTimeMs,
                BombDefuseTimeMs = lobby.LobbyEntity.LobbyRoundSettings.BombDefuseTimeMs,
            };
        }
    }
}