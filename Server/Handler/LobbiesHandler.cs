using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models.CustomLobby;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Maps;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;
using MapType = TDS_Server.Data.Enums.MapType;

namespace TDS_Server.Handler
{
    //Todo: Add team check for special gamemodes (e.g. ArmsRace allow only 1 team)
    public class LobbiesHandler : DatabaseEntityWrapper
    {
        #region Public Fields

        public readonly Dictionary<int, ILobby> LobbiesByIndex = new Dictionary<int, ILobby>();

        #endregion Public Fields

        #region Private Fields

        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint> { 0 };
        private readonly EventsHandler _eventsHandler;
        private readonly MapsLoadingHandler _mapsHandler;
        private readonly Serializer _serializer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISettingsHandler _settingsHandler;
        private readonly IEntitiesStaticConnector _entitiesStaticConnector;
        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;
        private IArena? _arena;
        private ICharCreateLobby? _charCreateLobby;
        private string? _customLobbyDatas;
        private IGangLobby? _gangLobby;
        private ILobby? _mainMenu;
        private IMapCreateLobby? _mapCreateLobby;

        #endregion Private Fields
        #region Public Constructors

        public LobbiesHandler(
            TDSDbContext dbContext,
            ISettingsHandler settingsHandler,
            Serializer serializer,
            MapsLoadingHandler mapsHandler,
            ILoggingHandler loggingHandler,
            IServiceProvider serviceProvider,
            EventsHandler eventsHandler,
            IEntitiesStaticConnector entitiesStaticConnector,
            IEntitiesByInterfaceCreator entitiesByInterfaceCreator) : base(dbContext, loggingHandler)
        {
            _serializer = serializer;
            _mapsHandler = mapsHandler;
            _serviceProvider = serviceProvider;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _entitiesStaticConnector = entitiesStaticConnector;
            _entitiesByInterfaceCreator = entitiesByInterfaceCreator;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.LobbyCreated += AddLobby;
        }

        #endregion Public Constructors


        #region Public Properties

        public ICharCreateLobby CharCreateLobbyDummy => _charCreateLobby ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.CharCreateLobby).Cast<ICharCreateLobby>().First();

        public List<ILobby> Lobbies { get; } = new List<ILobby>();

        public IMapCreateLobby MapCreateLobbyDummy => _mapCreateLobby ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.MapCreateLobby).Cast<IMapCreateLobby>().First();

        public IArena Arena => _arena ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.Arena && !l.IsGangActionLobby).Cast<IArena>().First();

        public IGangLobby GangLobby => _gangLobby ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.GangLobby).Cast<IGangLobby>().First();

        public ILobby MainMenu => _mainMenu ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.MainMenu).First();

        #endregion Public Properties

        #region Public Methods

        public void AddLobby(ILobby lobby)
        {
            Lobbies.Add(lobby);
            LobbiesByIndex[lobby.Id] = lobby;
            _dimensionsUsed.Add(lobby.Dimension);
        }

        public async Task<object?> CreateCustomLobby(ITDSPlayer player, ArraySegment<object> args)
        {
            try
            {
                string dataJson = (string)args[0];
                var data = _serializer.FromBrowser<CustomLobbyData>(dataJson);
                if (!IsCustomLobbyNameAllowed(data.Name))
                {
                    return player.Language.CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR;
                }
                bool nameAlreadyInUse = Lobbies.Any(lobby => lobby.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase));
                if (nameAlreadyInUse)
                {
                    return player.Language.CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR;
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
                    LobbyWeapons = data.Weapons is { } ? data.Weapons.Select(w => new LobbyWeapons
                    {
                        Hash = w.WeaponHash,
                        Ammo = w.Ammo,
                        Damage = w.Damage,
                        HeadMultiplicator = w.HeadshotMultiplicator
                    }).ToHashSet()
                    : Arena.Entity.LobbyWeapons.Select(w => new LobbyWeapons
                    {
                        Hash = w.Hash,
                        Ammo = w.Ammo,
                        Damage = w.Damage,
                        HeadMultiplicator = w.HeadMultiplicator
                    }).ToHashSet(),
                    ArmsRaceWeapons = data.ArmsRaceWeapons.Select(w => new LobbyArmsRaceWeapons 
                    {
                        WeaponHash = w.WeaponHash,
                        AtKill = w.AtKill
                    }).ToHashSet(),
                    Password = data.Password,
                    //Todo: Add ArmsRaceWeapons (first in Angular)
                    Teams = data.Teams.Select((t, index) =>
                    {
                        var color = SharedUtils.GetColorFromHtmlRgba(t.Color) ?? System.Drawing.Color.FromArgb(255, 255, 255);
                        return new Teams
                        {
                            Name = t.Name,
                            BlipColor = (byte)t.BlipColor,
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

                var arena = await AltAsync.Do(() =>
                {
                    return _entitiesByInterfaceCreator.Create<IArena>(entity, false)!;
                });
                await arena.AddToDB();
                await AltAsync.Do(() =>
                {
                    _eventsHandler.OnLobbyCreated(arena);

                    AddMapsToArena(arena, entity);

                    _eventsHandler.OnCustomLobbyMenuLeave(player);
                    _eventsHandler.OnCustomLobbyCreated(arena);
                });

                await arena.AddPlayer(player, null);
                return null;
            }
            catch
            {
                return player.Language.CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR;
            }
        }

        public HashSet<LobbyWeapons> GetAllPossibleLobbyWeapons(MapType type)
            => _entitiesStaticConnector.GetAllowedWeapons(type);

        public uint GetFreeDimension()
        {
            uint tryid = 0;
            while (_dimensionsUsed.Contains(tryid))
                ++tryid;
            return tryid;
        }

        public ILobby? GetLobby(int id)
        {
            LobbiesByIndex.TryGetValue(id, out ILobby? lobby);
            return lobby;
        }

        public async ValueTask<object?> LoadDatas(ITDSPlayer player, ArraySegment<object> args)
        {
            if (_customLobbyDatas is null)
            {
                var model = new DataForCustomLobbyCreation
                {
                    WeaponDatas = await ExecuteForDBAsync(async dbContext =>
                    {
                        return await dbContext.Weapons.Select(w => new CustomLobbyWeaponData
                        {
                            WeaponHash = w.Hash,
                            Ammo = 9999,
                            Damage = w.Damage,
                            HeadshotMultiplicator = w.HeadShotDamageModifier
                        }).ToListAsync();
                    }),

                    ArenaWeaponDatas = Arena.Entity.LobbyWeapons.Select(w => new CustomLobbyWeaponData
                    {
                        WeaponHash = w.Hash,
                        Ammo = w.Ammo,
                        Damage = w.Damage,
                        HeadshotMultiplicator = w.HeadMultiplicator
                    }).ToList(),

                    ArenaArmsRaceWeaponDatas = Arena.Entity.ArmsRaceWeapons.Select(w => new CustomLobbyArmsRaceWeaponData
                    {
                        WeaponHash = w.WeaponHash,
                        AtKill = w.AtKill
                    }).ToList()
                    
                };

                _customLobbyDatas = _serializer.ToBrowser(model);
            }

            return _customLobbyDatas;
        }

        public void LoadLobbies()
        {
            var lobbies = ExecuteForDB(dbContext =>
            {
                var temporaryLobbies = dbContext.Lobbies.Where(l => l.IsTemporary).ToList();
                dbContext.Lobbies.RemoveRange(temporaryLobbies);
                dbContext.SaveChanges();
                //Todo: Add QueryTrackingBehavior NoTracking to every constructor and TrackAll at the end
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return dbContext.Lobbies
                    .Include(l => l.LobbyRewards)
                    .Include(l => l.LobbyRoundSettings)
                    .Include(l => l.LobbyMapSettings)
                    .Include(l => l.Teams)
                    .Include(l => l.LobbyWeapons)
                    .Include(l => l.LobbyMaps)
                    .ThenInclude((LobbyMaps map) => map.Map)
                    .Include(l => l.Owner)
                    .Include(l => l.FightSettings)
                    .Include(l => l.ArmsRaceWeapons)
                    .ToList();
            }).Result;

            foreach (Lobbies lobbysetting in lobbies)
            {
                LobbyType type = lobbysetting.Type;
                ILobby? lobby = type switch
                {
                    LobbyType.FightLobby => _entitiesByInterfaceCreator.Create<IFightLobby>(lobbysetting, false),

                    LobbyType.Arena => _entitiesByInterfaceCreator.Create<IArena>(lobbysetting, false),

                    LobbyType.MapCreateLobby => _entitiesByInterfaceCreator.Create<IMapCreateLobby>(lobbysetting),

                    LobbyType.GangLobby => _entitiesByInterfaceCreator.Create<IGangLobby>(lobbysetting),

                    LobbyType.CharCreateLobby => _entitiesByInterfaceCreator.Create<ICharCreateLobby>(lobbysetting),

                    _ => _entitiesByInterfaceCreator.Create<ILobby>(lobbysetting, false),
                };
                if (lobby is null)
                    continue;
                if (lobby is IArena arena)
                {
                    AddMapsToArena(arena, lobbysetting);
                }
                _eventsHandler.OnLobbyCreated(lobby);
            }

            _settingsHandler.SyncedSettings.ArenaLobbyId = Arena.Id;
            _settingsHandler.SyncedSettings.CharCreatorLobbyId = CharCreateLobbyDummy.Id;
            _settingsHandler.SyncedSettings.MapCreatorLobbyId = MapCreateLobbyDummy.Id;
            _settingsHandler.SyncedSettings.GangLobbyLobbyId = GangLobby.Id;

            ExecuteForDB(dbContext =>
            {
                _entitiesStaticConnector.InitGamemodes(dbContext);
            }).Wait();
        }

        public async Task<object?> OnJoinLobby(ITDSPlayer player, ArraySegment<object> args)
        {
            int index = (int)args[0];

            if (LobbiesByIndex.ContainsKey(index))
            {
                ILobby? lobby = LobbiesByIndex[index];
                if (lobby is IMapCreateLobby)
                {
                    if (await lobby.IsPlayerBaned(player))
                        return null;

                    lobby = _entitiesByInterfaceCreator.Create<IMapCreateLobby>(player);
                    if (lobby is null)
                        return null;
                    await lobby.AddToDB();
                    _eventsHandler.OnLobbyCreated(lobby);
                }
                else if (lobby is ICharCreateLobby)
                {
                    if (await lobby.IsPlayerBaned(player))
                        return null;

                    lobby = _entitiesByInterfaceCreator.Create<ICharCreateLobby>(player);
                    if (lobby is null)
                        return null;
                    await lobby.AddToDB();
                    _eventsHandler.OnLobbyCreated(lobby);
                }
                await lobby.AddPlayer(player, null);
                return null;
            }
            else
            {
                await AltAsync.Do(() => player.SendMessage(player.Language.LOBBY_DOESNT_EXIST));
                //todo Remove lobby at client view and check, why he saw this lobby
                return null;
            }
        }

        public async Task<object?> OnJoinLobbyWithPassword(ITDSPlayer player, ArraySegment<object> args)
        {
            int index = (int)args[0];
            string? password = args.Count > 1 ? (string)args[1] : null;

            if (LobbiesByIndex.ContainsKey(index))
            {
                ILobby lobby = LobbiesByIndex[index];
                if (password != null && lobby.Entity.Password != password)
                {
                    await AltAsync.Do(() => player.SendMessage(player.Language.WRONG_PASSWORD));
                    return null;
                }

                await lobby.AddPlayer(player, null);
                return null;
            }
            else
            {
                await AltAsync.Do(() => player.SendMessage(player.Language.LOBBY_DOESNT_EXIST));
                //todo Remove lobby at client view and check, why he saw this lobby
                return null;
            }
        }

        public void RemoveLobby(ILobby lobby)
        {
            LobbiesByIndex.Remove(lobby.Entity.Id);
            Lobbies.Remove(lobby);
            _dimensionsUsed.Remove(lobby.Dimension);

            if (!lobby.IsOfficial)
                _eventsHandler.OnCustomLobbyRemoved(lobby);
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
                    LoggingHandler.LogError(ex);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void AddMapsToArena(IArena arena, Lobbies lobbySetting)
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
            arena.SetMapsList(lobbyMapsList);
        }

        private async void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            await MainMenu.AddPlayer(player, null);
        }

        private bool IsCustomLobbyNameAllowed(string name)
        {
            if (name.Length < 3 || name.Length > 50)
                return false;

            if (name.StartsWith("mapcreator-", StringComparison.CurrentCultureIgnoreCase))
                return false;

            return true;
        }

        #endregion Private Methods
    }
}
