using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Sync;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Rest;
using TDS_Server.Instance.GameModes;
using TDS_Common.Manager.Utility;
using TDS_Server_DB.Entity.LobbyEntities;
using TDS_Server.Manager.Logs;
using TDS_Server.Enums;
using EMapType = TDS_Common.Enum.EMapType;
using TDS_Server.Dto.CustomLobby;

namespace TDS_Server.Core.Manager.Utility
{
    internal static class LobbyManager
    {
        public static List<Lobby> Lobbies { get; } = new List<Lobby>();

        public static Lobby MainMenu => _mainMenu ?? (_mainMenu = 
            Lobbies.Where(l => l.IsOfficial && l.LobbyEntity.Type == ELobbyType.MainMenu).First());
        public static Arena Arena => _arena ?? (_arena = 
            Lobbies.Where(l => l.IsOfficial && l.LobbyEntity.Type == ELobbyType.Arena && !l.IsGangActionLobby).Cast<Arena>().First());
        public static MapCreateLobby MapCreateLobbyDummy => _mapCreateLobby ??(_mapCreateLobby = 
            Lobbies.Where(l => l.IsOfficial && l.LobbyEntity.Type == ELobbyType.MapCreateLobby).Cast<MapCreateLobby>().First());
        public static GangLobby GangLobby => _gangLobby ?? (_gangLobby = 
            Lobbies.Where(l => l.IsOfficial && l.LobbyEntity.Type == ELobbyType.GangLobby).Cast<GangLobby>().First());

        private static Lobby? _mainMenu;
        private static Arena? _arena;
        private static MapCreateLobby? _mapCreateLobby;
        private static GangLobby? _gangLobby;

        private static string? _customLobbyDatas;


        public static async Task LoadAllLobbies(TDSDbContext dbcontext)
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
                .Include(l => l.FightSettings)
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
                    ELobbyType.MapCreateLobby => new MapCreateLobby(lobbysetting),

                    ELobbyType.GangLobby => new GangLobby(lobbysetting),

                    _ => new Lobby(lobbysetting),
                };
                Lobbies.Add(lobby);
                Lobby.LobbiesByIndex[lobby.Id] = lobby;
                if (lobby is Arena arena)
                {
                    AddMapsToArena(arena, lobbysetting);
                }
            }

            SettingsManager.SyncedSettings.ArenaLobbyId = Arena.Id;
            SettingsManager.SyncedSettings.MapCreatorLobbyId = MapCreateLobbyDummy.Id;
        }

        private static void AddMapsToArena(Arena arena, Lobbies lobbySetting)
        {
            List<MapDto> lobbyMapsList = new List<MapDto>();
            foreach (var mapAssignment in lobbySetting.LobbyMaps)
            {
                switch (mapAssignment.MapId)
                {
                    case (int)EDefaultMapIds.AllWithoutGangwars:
                        lobbyMapsList.AddRange(MapsLoader.AllMaps.Where(m => m.Info.Type != Enums.EMapType.Gangwar));
                        break;

                    case (int)EDefaultMapIds.Normals:
                        lobbyMapsList.AddRange(MapsLoader.AllMaps.Where(m => m.Info.Type == Enums.EMapType.Normal));
                        break;

                    case (int)EDefaultMapIds.Bombs:
                        lobbyMapsList.AddRange(MapsLoader.AllMaps.Where(m => m.Info.Type == Enums.EMapType.Bomb));
                        break;

                    case (int)EDefaultMapIds.Snipers:
                        lobbyMapsList.AddRange(MapsLoader.AllMaps.Where(m => m.Info.Type == Enums.EMapType.Sniper));
                        break;

                    case (int)EDefaultMapIds.Gangwars:
                        lobbyMapsList.AddRange(MapsLoader.AllMaps.Where(m => m.Info.Type == Enums.EMapType.Gangwar));
                        break;

                    default:
                        var map = MapsLoader.AllMaps.FirstOrDefault(m => m.BrowserSyncedData.Name == mapAssignment.Map.Name);
                        if (map is null)
                            map = MapCreator.NewCreatedMaps.FirstOrDefault(m => m.BrowserSyncedData.Name == mapAssignment.Map.Name);
                        if (map is null)
                            map = MapCreator.NeedCheckMaps.FirstOrDefault(m => m.BrowserSyncedData.Name == mapAssignment.Map.Name);
                        if (map is { })
                            lobbyMapsList.Add(map);
                        break;
                }
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
                var data = Serializer.FromBrowser<CustomLobbyData>(dataJson);
                if (!IsCustomLobbyNameAllowed(data.Name))
                {
                    player.Player?.TriggerEvent(DToClientEvent.CreateCustomLobbyResponse, player.Language.CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR);
                    return;
                }
                bool nameAlreadyInUse = Lobbies.Any(lobby => lobby.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase));
                if (nameAlreadyInUse)
                {
                    player.Player?.TriggerEvent(DToClientEvent.CreateCustomLobbyResponse, player.Language.CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR);
                    return;
                }

                Lobbies entity = new Lobbies
                {
                    Name = data.Name,
                    OwnerId = player.Entity?.Id ?? 0,
                    IsOfficial = false,
                    IsTemporary = true,
                    FightSettings = new LobbyFightSettings
                    {
                        AmountLifes = data.AmountLifes,
                        SpawnAgainAfterDeathMs = data.SpawnAgainAfterDeathMs,
                        StartArmor = data.StartArmor,
                        StartHealth = data.StartHealth,
                    },
                    LobbyRoundSettings = new LobbyRoundSettings
                    { 
                        RoundTime = data.RoundTime, CountdownTime = data.CountdownTime, BombDetonateTimeMs = data.BombDetonateTimeMs,
                        BombDefuseTimeMs = data.BombDefuseTimeMs, BombPlantTimeMs = data.BombPlantTimeMs, MixTeamsAfterRound = data.MixTeamsAfterRound,
                        ShowRanking = data.ShowRanking
                    },
                    LobbyMapSettings = new LobbyMapSettings
                    {
                        MapLimitTime = data.MapLimitTime,
                        MapLimitType = data.MapLimitType
                    },
                    LobbyMaps = data.Maps.Select(m => new LobbyMaps { MapId = m }).ToHashSet(),
                    LobbyWeapons = data.Weapons.Select(w => new LobbyWeapons 
                    { 
                        Hash = w.WeaponHash,
                        Ammo = w.Ammo,
                        Damage = w.Damage,
                        HeadMultiplicator = w.HeadshotMultiplicator
                    }).ToHashSet(),      // GetAllPossibleLobbyWeapons(EMapType.Normal),
                    Password = data.Password,
                    Teams = data.Teams.Select((t, index) => 
                    {
                        var color = CommonUtils.GetColorFromHtmlRgba(t.Color) ?? System.Drawing.Color.FromArgb(255, 255, 255);
                        return new Teams 
                        { 
                            Name = t.Name,
                            BlipColor = (short)t.BlipColor,
                            ColorR = color.R,
                            ColorG = color.G,
                            ColorB = color.B,
                            SkinHash = t.SkinHash,
                            Index = (short)index
                        };
                    }).ToHashSet(),
                    Type = ELobbyType.Arena
                };
                //entity.LobbyMaps.Add(new LobbyMaps { MapId = -1 });

                Arena arena = new Arena(entity);
                await arena.AddToDB();

                AddLobby(arena);
                AddMapsToArena(arena, entity);

                CustomLobbyMenuSync.RemovePlayer(player);
                CustomLobbyMenuSync.SyncLobbyAdded(arena);
                await arena.AddPlayer(player, null);
            }
            catch
            {
                player.Player!.TriggerEvent(DToClientEvent.CreateCustomLobbyResponse, player.Language.CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR);
            }
        }

        public static async ValueTask<object?> LoadDatas(TDSPlayer player, object[] args)
        {
            if (_customLobbyDatas is null)
            {
                using var dbContext = new TDSDbContext();
                var model = new DataForCustomLobbyCreation 
                {
                    WeaponDatas = await dbContext.Weapons.Select(w => new CustomLobbyWeaponData
                    {
                        WeaponHash = w.Hash,
                        Ammo = 9999,
                        Damage = w.Damage,
                        HeadshotMultiplicator = w.HeadShotDamageModifier
                    }).ToListAsync(),

                    ArenaWeaponDatas = Arena.LobbyEntity.LobbyWeapons.Select(w => new CustomLobbyWeaponData
                    {
                        WeaponHash = w.Hash,
                        Ammo = w.Ammo,
                        Damage = w.Damage,
                        HeadshotMultiplicator = w.HeadMultiplicator
                    }).ToList()
                };
                _customLobbyDatas = Serializer.ToBrowser(model);
            }

            return _customLobbyDatas;
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

        public static async Task SaveAll()
        {
            foreach (var lobby in Lobbies)
            {
                try
                {
                    await lobby.ExecuteForDBAsync(async dbContext =>
                    {
                        await dbContext.SaveChangesAsync();
                    });
                }
                catch (Exception ex)
                {
                    ErrorLogsManager.Log(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace);
                }
            }
        }

        public static HashSet<LobbyWeapons> GetAllPossibleLobbyWeapons(EMapType type)
        {
            return type switch
            {
                EMapType.Bomb => Bomb.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
                EMapType.Sniper => Sniper.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
                _ => Normal.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
            };
        }
    }
}
