using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.CustomLobby;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Entities.GameModes;
using TDS_Server.Handler.Entities.GameModes.Bomb;
using TDS_Server.Handler.Entities.GameModes.Normal;
using TDS_Server.Handler.Entities.GameModes.Sniper;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Maps;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;
using TDS_Shared.Core;
using MapType = TDS_Server.Data.Enums.MapType;

namespace TDS_Server.Handler
{
    public class LobbiesHandler
    {
        public List<Lobby> Lobbies { get; } = new List<Lobby>();

        public Lobby MainMenu => _mainMenu ?? (_mainMenu =
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.MainMenu).First());
        public Arena Arena => _arena ?? (_arena =
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.Arena && !l.IsGangActionLobby).Cast<Arena>().First());
        public MapCreateLobby MapCreateLobbyDummy => _mapCreateLobby ?? (_mapCreateLobby =
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.MapCreateLobby).Cast<MapCreateLobby>().First());
        public GangLobby GangLobby => _gangLobby ?? (_gangLobby =
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.GangLobby).Cast<GangLobby>().First());

        public readonly Dictionary<int, Lobby> LobbiesByIndex = new Dictionary<int, Lobby>();
        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint> { 0 };

        private Lobby? _mainMenu;
        private Arena? _arena;
        private MapCreateLobby? _mapCreateLobby;
        private GangLobby? _gangLobby;

        private string? _customLobbyDatas;

        private readonly Serializer _serializer;
        private readonly TDSDbContext _dbContext;
        private readonly MapsLoadingHandler _mapsHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventsHandler _eventsHandler;
        private readonly ISettingsHandler _settingsHandler;

        public LobbiesHandler(
            TDSDbContext dbContext,
            ISettingsHandler settingsHandler,
            Serializer serializer,
            MapsLoadingHandler mapsHandler,
            ILoggingHandler loggingHandler,
            IServiceProvider serviceProvider,
            EventsHandler eventsHandler)
        {
            _dbContext = dbContext;
            _serializer = serializer;
            _mapsHandler = mapsHandler;
            _serviceProvider = serviceProvider;
            _loggingHandler = loggingHandler;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
        }

        public void LoadLobbies()
        {
            var temporaryLobbies = _dbContext.Lobbies.Where(l => l.IsTemporary).ToList();
            _dbContext.Lobbies.RemoveRange(temporaryLobbies);
            _dbContext.SaveChanges();
            //Todo: Add QueryTrackingBehavior NoTracking to every constructor and TrackAll at the end
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            List<Lobbies> lobbies = _dbContext.Lobbies
                .Include(l => l.LobbyRewards)
                .Include(l => l.LobbyRoundSettings)
                .Include(l => l.LobbyMapSettings)
                .Include(l => l.Teams)
                .Include(l => l.LobbyWeapons)
                .Include(l => l.LobbyMaps)
                .ThenInclude((LobbyMaps map) => map.Map)
                .Include(l => l.Owner)
                .Include(l => l.FightSettings)
                .ToList();
            foreach (Lobbies lobbysetting in lobbies)
            {
                LobbyType type = lobbysetting.Type;
                var lobby = type switch
                {
                    LobbyType.FightLobby => ActivatorUtilities.CreateInstance<FightLobby>(_serviceProvider, lobbysetting, false),

                    LobbyType.Arena => ActivatorUtilities.CreateInstance<Arena>(_serviceProvider, lobbysetting, false),
                    //case LobbyType.GangLobby:
                    //    lobby = new GangLobby(lobbysetting);
                    //    break;
                    LobbyType.MapCreateLobby => ActivatorUtilities.CreateInstance<MapCreateLobby>(_serviceProvider, lobbysetting),

                    LobbyType.GangLobby => ActivatorUtilities.CreateInstance<GangLobby>(_serviceProvider, lobbysetting),

                    _ => ActivatorUtilities.CreateInstance<Lobby>(_serviceProvider, lobbysetting, false),
                };
                if (lobby is Arena arena)
                {
                    AddMapsToArena(arena, lobbysetting);
                }
            }

            _settingsHandler.SyncedSettings.ArenaLobbyId = Arena.Id;
            _settingsHandler.SyncedSettings.MapCreatorLobbyId = MapCreateLobbyDummy.Id;

            Normal.Init(_dbContext);
            Gangwar.Init(_dbContext);
            Bomb.Init(_dbContext);
            Sniper.Init(_dbContext);
        }

        private async void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            if (!(player is TDSPlayer tdsPlayer))
                return;
            await MainMenu.AddPlayer(tdsPlayer, null);
        }

        private void AddMapsToArena(Arena arena, Lobbies lobbySetting)
        {
            List<MapDto> lobbyMapsList = new List<MapDto>();
            foreach (var mapAssignment in lobbySetting.LobbyMaps)
            {
                switch (mapAssignment.MapId)
                {
                    case (int)DefaultMapIds.AllWithoutGangwars:
                        lobbyMapsList.AddRange(_mapsHandler.DefaultMaps.Where(m => m.Info.Type != MapType.Gangwar));
                        break;

                    case (int)DefaultMapIds.Normals:
                        lobbyMapsList.AddRange(_mapsHandler.DefaultMaps.Where(m => m.Info.Type == MapType.Normal));
                        break;

                    case (int)DefaultMapIds.Bombs:
                        lobbyMapsList.AddRange(_mapsHandler.DefaultMaps.Where(m => m.Info.Type == MapType.Bomb));
                        break;

                    case (int)DefaultMapIds.Snipers:
                        lobbyMapsList.AddRange(_mapsHandler.DefaultMaps.Where(m => m.Info.Type == MapType.Sniper));
                        break;

                    case (int)DefaultMapIds.Gangwars:
                        lobbyMapsList.AddRange(_mapsHandler.DefaultMaps.Where(m => m.Info.Type == MapType.Gangwar));
                        break;

                    default:
                        var map = _mapsHandler.DefaultMaps.FirstOrDefault(m => m.BrowserSyncedData.Name == mapAssignment.Map.Name);
                        if (map is null)
                            map = _mapsHandler.NewCreatedMaps.FirstOrDefault(m => m.BrowserSyncedData.Name == mapAssignment.Map.Name);
                        if (map is null)
                            map = _mapsHandler.NeedCheckMaps.FirstOrDefault(m => m.BrowserSyncedData.Name == mapAssignment.Map.Name);
                        if (map is { })
                            lobbyMapsList.Add(map);
                        break;
                }
            }
            arena.SetMapList(lobbyMapsList);
        }

        public Lobby? GetLobby(int id)
        {
            LobbiesByIndex.TryGetValue(id, out Lobby? lobby);
            return lobby;
        }

        public async Task CreateCustomLobby(ITDSPlayer player, string dataJson)
        {
            try
            {
                var data = _serializer.FromBrowser<CustomLobbyData>(dataJson);
                if (!IsCustomLobbyNameAllowed(data.Name))
                {
                    player.SendEvent(ToClientEvent.CreateCustomLobbyResponse, player.Language.CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR);
                    return;
                }
                bool nameAlreadyInUse = Lobbies.Any(lobby => lobby.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase));
                if (nameAlreadyInUse)
                {
                    player.SendEvent(ToClientEvent.CreateCustomLobbyResponse, player.Language.CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR);
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
                        RoundTime = data.RoundTime,
                        CountdownTime = data.CountdownTime,
                        BombDetonateTimeMs = data.BombDetonateTimeMs,
                        BombDefuseTimeMs = data.BombDefuseTimeMs,
                        BombPlantTimeMs = data.BombPlantTimeMs,
                        MixTeamsAfterRound = data.MixTeamsAfterRound,
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
                        var color = SharedUtils.GetColorFromHtmlRgba(t.Color) ?? System.Drawing.Color.FromArgb(255, 255, 255);
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
                    Type = LobbyType.Arena
                };
                //entity.LobbyMaps.Add(new LobbyMaps { MapId = -1 });


                Arena arena = ActivatorUtilities.CreateInstance<Arena>(_serviceProvider, entity);
                await arena.AddToDB();

                AddMapsToArena(arena, entity);

                _eventsHandler.OnCustomLobbyMenuLeave(player);
                _eventsHandler.OnCustomLobbyCreated(arena);

                await arena.AddPlayer(player, null);
            }
            catch
            {
                player.SendEvent(ToClientEvent.CreateCustomLobbyResponse, player.Language.CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR);
            }
        }

        public async ValueTask<object?> LoadDatas(ITDSPlayer player, object[] args)
        {
            if (_customLobbyDatas is null)
            {
                var model = new DataForCustomLobbyCreation
                {
                    WeaponDatas = await _dbContext.Weapons.Select(w => new CustomLobbyWeaponData
                    {
                        WeaponHash = w.Hash,
                        Ammo = 9999,
                        Damage = w.Damage,
                        HeadshotMultiplicator = w.HeadShotDamageModifier
                    }).ToListAsync(),

                    ArenaWeaponDatas = Arena.Entity.LobbyWeapons.Select(w => new CustomLobbyWeaponData
                    {
                        WeaponHash = w.Hash,
                        Ammo = w.Ammo,
                        Damage = w.Damage,
                        HeadshotMultiplicator = w.HeadMultiplicator
                    }).ToList()
                };
                _customLobbyDatas = _serializer.ToBrowser(model);
            }

            return _customLobbyDatas;
        }

        private bool IsCustomLobbyNameAllowed(string name)
        {
            if (name.Length < 3 || name.Length > 50)
                return false;

            if (name.StartsWith("mapcreator-", StringComparison.CurrentCultureIgnoreCase))
                return false;

            return true;
        }

        public void AddLobby(Lobby lobby)
        {
            Lobbies.Add(lobby);
            LobbiesByIndex[lobby.Id] = lobby;
            _dimensionsUsed.Add(lobby.Dimension);
        }

        public void RemoveLobby(Lobby lobby)
        {
            LobbiesByIndex.Remove(lobby.Entity.Id);
            Lobbies.Remove(lobby);
            _dimensionsUsed.Remove(lobby.Dimension);

            if (!lobby.IsOfficial)
                _eventsHandler.OnCustomLobbyRemoved(lobby);
        }

        public uint GetFreeDimension()
        {
            uint tryid = 0;
            while (_dimensionsUsed.Contains(tryid))
                ++tryid;
            return tryid;
        }

        public async Task SaveAll()
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
                    _loggingHandler.LogError(ex);
                }
            }
        }

        public HashSet<LobbyWeapons> GetAllPossibleLobbyWeapons(MapType type)
            => type switch
            {
                MapType.Bomb => Bomb.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
                MapType.Sniper => Sniper.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
                _ => Normal.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
            };
    }
}
