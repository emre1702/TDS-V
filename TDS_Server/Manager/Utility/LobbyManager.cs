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
using TDS_Server.Enum;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Maps;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server_DB.Entity.Rest;
using Z.EntityFramework.Plus;

namespace TDS_Server.Manager.Utility
{
    internal static class LobbyManager
    {
        public static List<Lobby> Lobbies = new List<Lobby>();
        private static List<TDSPlayer> _playerInCustomLobbyMenu = new List<TDSPlayer>();

        public static Lobby MainMenu => Lobbies[0];
        public static Arena Arena => (Arena)Lobbies[1];

        public static async Task LoadAllLobbies(TDSNewContext dbcontext)
        {
            dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            await dbcontext.Lobbies
                .Where(l => l.IsTemporary)
                .DeleteAsync();
            await dbcontext.SaveChangesAsync();
            dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            List<Lobbies> lobbies = await dbcontext.Lobbies
                .Include(l => l.LobbyRewards)
                .Include(l => l.LobbyRoundSettings)
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
                    LobbyMaps = new List<LobbyMaps> { new LobbyMaps { MapId = -1 } },
                    LobbyRoundSettings = new LobbyRoundSettings
                        { RoundTime = 240, CountdownTime = 5, BombDetonateTimeMs = 45000,
                          BombDefuseTimeMs = 8000, BombPlantTimeMs = 3000, MixTeamsAfterRound = true
                        },
                    LobbyWeapons = new List<LobbyWeapons>
                    {
                        new LobbyWeapons { Hash = EWeaponHash.AssaultRifle, Lobby = 1, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.Revolver, Lobby = 1, Ammo = 500 },
                        new LobbyWeapons { Hash = EWeaponHash.UpnAtomizer, Lobby = 1, Ammo = 500 },
                        new LobbyWeapons { Hash = EWeaponHash.SMG, Lobby = 1, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.MicroSMG, Lobby = 1, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.UnholyHellbringer, Lobby = 1, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.AssaultShotgun, Lobby = 1, Ammo = 2000 },
                        new LobbyWeapons { Hash = EWeaponHash.CarbineRifleMK2, Lobby = 1, Ammo = 2000 }
                    },
                    Password = data.Password,
                    SpawnAgainAfterDeathMs = data.SpawnAgainAfterDeathMs,
                    StartArmor = data.StartArmor,
                    StartHealth = data.StartHealth,
                    Teams = new List<Teams>
                    {
                        //new Teams { Index = 0, Name = "Spectator", Lobby = 1, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 1004114196 },
                        new Teams { Index = 0, Name = "SWAT", ColorR = 0, ColorG = 150, ColorB = 0, BlipColor = 52, SkinHash = -1920001264 },
                        new Teams { Index = 1, Name = "Terrorist", ColorR = 150, ColorG = 0, ColorB = 0, BlipColor = 1, SkinHash = 275618457 },
                    },
                    Type = ELobbyType.Arena
                };

                using (var dbContext = new TDSNewContext())
                {
                    dbContext.Lobbies.Add(entity);
                    await dbContext.SaveChangesAsync();
                }
                Arena arena = new Arena(entity);

                Lobbies.Add(arena);
                AddMapsToArena(arena, entity);

                _playerInCustomLobbyMenu.Remove(player);
                await arena.AddPlayer(player, null);
                SyncNewCustomLobby(arena);
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

        public static void SetPlayerInCustomLobbyMenu(TDSPlayer player)
        {
            _playerInCustomLobbyMenu.Add(player);
        }

        private static CustomLobbyData GetCustomLobbyData(Arena arena)
        {
            return new CustomLobbyData
            {
                AmountLifes = arena.LobbyEntity.AmountLifes ?? 1,
                DieAfterOutsideMapLimitTime = arena.LobbyEntity.DieAfterOutsideMapLimitTime,
                LobbyId = arena.LobbyEntity.Id,
                Name = arena.LobbyEntity.Name,
                OwnerName = arena.OwnerName,
                Password = arena.LobbyEntity.Password,
                SpawnAgainAfterDeathMs = arena.LobbyEntity.SpawnAgainAfterDeathMs,
                StartArmor = arena.LobbyEntity.StartArmor,
                StartHealth = arena.LobbyEntity.StartHealth
            };
        }

        private static void SyncNewCustomLobby(Arena arena)
        {
            var data = GetCustomLobbyData(arena);
            string json = JsonConvert.SerializeObject(data);

            for (int i = _playerInCustomLobbyMenu.Count - 1; i >= 0; --i)
            {
                TDSPlayer player = _playerInCustomLobbyMenu[i];
                if (!player.LoggedIn)
                {
                    _playerInCustomLobbyMenu.RemoveAt(i);
                    continue;
                }
                player.Client.TriggerEvent(DToClientEvent.SyncNewCustomLobby, json);
            }
        }

        // todo After custom lobby testing: Add DToClientEvent.RemoveCustomLobby with id on lobby remove to remove the lobby from custom lobby menu.
    }
}