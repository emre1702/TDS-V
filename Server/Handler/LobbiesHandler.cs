using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.CustomLobby;
using TDS.Server.Data.Models.Map;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Maps;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Utility;
using TDS.Shared.Default;
using MapType = TDS.Server.Data.Enums.MapType;

namespace TDS.Server.Handler
{
    //Todo: Add team check for special gamemodes (e.g. ArmsRace allow only 1 team)
    public class LobbiesHandler : DatabaseEntityWrapper
    {
        public Dictionary<int, IBaseLobby> LobbiesByIndex { get; } = new Dictionary<int, IBaseLobby>();

        private readonly MapsLoadingHandler _mapsHandler;
        private readonly ILobbiesProvider _lobbiesProvider;
        private IArena? _arena;
        private ICharCreateLobby? _charCreateLobby;
        private string? _customLobbyDatas;
        private IDamageTestLobby? _damageTestLobby;
        private IGangLobby? _gangLobby;
        private IBaseLobby? _mainMenu;
        private IMapCreatorLobby? _mapCreateLobby;

        public LobbiesHandler(
            TDSDbContext dbContext,
            MapsLoadingHandler mapsHandler,
            EventsHandler eventsHandler,
            ILobbiesProvider lobbiesProvider,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler) : base(dbContext)
        {
            _mapsHandler = mapsHandler;
            _lobbiesProvider = lobbiesProvider;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.LobbyCreated += AddLobby;
            eventsHandler.LobbyRemoved += RemoveLobby;

            remoteBrowserEventsHandler.Add(ToServerEvent.CreateCustomLobby, CreateCustomLobby);
            remoteBrowserEventsHandler.Add(ToServerEvent.JoinLobby, OnJoinLobby);
            remoteBrowserEventsHandler.Add(ToServerEvent.JoinLobbyWithPassword, OnJoinLobbyWithPassword);
            remoteBrowserEventsHandler.Add(ToServerEvent.LoadDatasForCustomLobby, LoadDatas);
        }

        public ICharCreateLobby CharCreateLobbyDummy => _charCreateLobby ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.CharCreateLobby).Cast<ICharCreateLobby>().First();

        public IDamageTestLobby DamageTestLobbyDummy => _damageTestLobby ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.DamageTestLobby).Cast<IDamageTestLobby>().First();

        public List<IBaseLobby> Lobbies { get; } = new List<IBaseLobby>();

        public IMapCreatorLobby MapCreateLobbyDummy => _mapCreateLobby ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.MapCreateLobby).Cast<IMapCreatorLobby>().First();

        public IArena Arena => _arena ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.Arena).Cast<IArena>().First();

        public IGangLobby GangLobby => _gangLobby ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.GangLobby).Cast<IGangLobby>().First();

        public IBaseLobby MainMenu => _mainMenu ??=
            Lobbies.Where(l => l.IsOfficial && l.Entity.Type == LobbyType.MainMenu).First();

        public void AddLobby(IBaseLobby lobby)
        {
            Lobbies.Add(lobby);
            LobbiesByIndex[lobby.Entity.Id] = lobby;
        }

        private async Task<object?> CreateCustomLobby(RemoteBrowserEventArgs args)
        {
            try
            {
                await Task.Yield();
                string dataJson = (string)args.Args[0];
                var data = Serializer.FromBrowser<CustomLobbyData>(dataJson);
                if (!IsCustomLobbyNameAllowed(data.Name))
                {
                    return args.Player.Language.CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR;
                }
                bool nameAlreadyInUse = Lobbies.Any(lobby => lobby.Entity.Name.Equals(data.Name, StringComparison.OrdinalIgnoreCase));
                if (nameAlreadyInUse)
                {
                    return args.Player.Language.CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR;
                }

                var lobbyMaps = data.Maps.Select(m => new LobbyMaps { MapId = m }).ToHashSet();
                var maps = GetMapsForArena(lobbyMaps.Select(m => m.MapId));
                if (maps.Count == 0)
                {
                    return args.Player.Language.CUSTOM_LOBBY_CREATOR_NO_MAP_FOUND;
                }

                var entity = new Lobbies
                {
                    Name = data.Name,
                    OwnerId = args.Player.Entity?.Id ?? 0,
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
                    LobbyMaps = lobbyMaps,
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
                    ArmsRaceWeapons = data.ArmsRaceWeapons?.Select(w => new LobbyArmsRaceWeapons
                    {
                        WeaponHash = w.WeaponHash,
                        AtKill = w.AtKill
                    })?.ToHashSet(),
                    Password = data.Password,
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

                var arena = _lobbiesProvider.Create<IArena>(entity);

                arena.MapHandler.SetMapList(maps);

                await arena.Players.AddPlayer(args.Player, 0).ConfigureAwait(false);
                return null;
            }
            catch
            {
                return args.Player.Language.CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR;
            }
        }

        public IBaseLobby? GetLobby(int id)
        {
            LobbiesByIndex.TryGetValue(id, out IBaseLobby? lobby);
            return lobby;
        }

        private async ValueTask<object?> LoadDatas(RemoteBrowserEventArgs _)
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
                        }).ToListAsync().ConfigureAwait(false);
                    }).ConfigureAwait(false),

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

                _customLobbyDatas = Serializer.ToBrowser(model);
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

            foreach (Lobbies lobbyEntity in lobbies)
            {
                var lobby = _lobbiesProvider.Create(lobbyEntity.Type, lobbyEntity);

                if (lobby is IArena arena)
                    AddMapsToArena(arena, lobbyEntity);
                //lobby.Events.LobbyRemoveAfter += RemoveLobby;
            }
        }

        private async Task<object?> OnJoinLobby(RemoteBrowserEventArgs args)
        {
            int index = (int)args.Args[0];

            if (LobbiesByIndex.ContainsKey(index))
            {
                IBaseLobby lobby = LobbiesByIndex[index];
                if (lobby is IMapCreatorLobby)
                {
                    if (await lobby.Bans.CheckIsBanned(args.Player).ConfigureAwait(false))
                        return null;

                    lobby = _lobbiesProvider.Create<IMapCreatorLobby>(args.Player);
                }
                else if (lobby is ICharCreateLobby)
                {
                    if (await lobby.Bans.CheckIsBanned(args.Player).ConfigureAwait(false))
                        return null;

                    lobby = _lobbiesProvider.Create<ICharCreateLobby>(args.Player);
                }
                else if (lobby is IDamageTestLobby)
                {
                    lobby = _lobbiesProvider.Create<IDamageTestLobby>(args.Player);
                }
                await lobby.Players.AddPlayer(args.Player, 0).ConfigureAwait(false);
                return null;
            }
            else
            {
                NAPI.Task.RunSafe(() => args.Player.SendChatMessage(args.Player.Language.LOBBY_DOESNT_EXIST));
                //todo Remove lobby at client view and check, why he saw this lobby
                return null;
            }
        }

        private async Task<object?> OnJoinLobbyWithPassword(RemoteBrowserEventArgs args)
        {
            int index = (int)args.Args[0];
            string? password = args.Args.Count > 1 ? (string)args.Args[1] : null;

            if (LobbiesByIndex.ContainsKey(index))
            {
                IBaseLobby lobby = LobbiesByIndex[index];
                if (password != null && lobby.Entity.Password != password)
                {
                    NAPI.Task.RunSafe(() => args.Player.SendChatMessage(args.Player.Language.WRONG_PASSWORD));
                    return null;
                }

                await lobby.Players.AddPlayer(args.Player, 0).ConfigureAwait(false);
                return null;
            }
            else
            {
                NAPI.Task.RunSafe(() => args.Player.SendChatMessage(args.Player.Language.LOBBY_DOESNT_EXIST));
                //todo Remove lobby at client view and check, why he saw this lobby
                return null;
            }
        }

        public void RemoveLobby(IBaseLobby lobby)
        {
            LobbiesByIndex.Remove(lobby.Entity.Id);
            Lobbies.Remove(lobby);
        }

        public async Task SaveAll()
        {
            foreach (var lobby in Lobbies)
            {
                try
                {
                    await lobby.Database.Save().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LoggingHandler.Instance.LogError(ex);
                }
            }
        }

        private List<MapDto> GetMapsForArena(IEnumerable<int> mapIds)
        {
            List<MapDto> lobbyMapsList = new List<MapDto>();
            foreach (var mapId in mapIds)
            {
                switch (mapId)
                {
                    case (int)DefaultMapIds.AllWithoutGangwars:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type != MapType.Gangwar));
                        break;

                    case (int)DefaultMapIds.Normals:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type == MapType.Normal));
                        break;

                    case (int)DefaultMapIds.Bombs:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type == MapType.Bomb));
                        break;

                    case (int)DefaultMapIds.Snipers:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type == MapType.Sniper));
                        break;

                    case (int)DefaultMapIds.Gangwars:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type == MapType.Gangwar));
                        break;

                    default:
                        var map = _mapsHandler.GetMapById(mapId);
                        if (map is null)
                            map = _mapsHandler.GetNewCreatedMap(mapId);
                        if (map is null)
                            map = _mapsHandler.GetNeedCheckMap(mapId);
                        if (map is { })
                            lobbyMapsList.Add(map);
                        break;
                }
            }
            return lobbyMapsList;
        }

        private void AddMapsToArena(IArena arena, Lobbies lobbySetting)
        {
            var mapsList = GetMapsForArena(lobbySetting.LobbyMaps.Select(m => m.MapId));
            List<MapDto> lobbyMapsList = new List<MapDto>();
            foreach (var mapAssignment in lobbySetting.LobbyMaps)
            {
                switch (mapAssignment.MapId)
                {
                    case (int)DefaultMapIds.AllWithoutGangwars:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type != MapType.Gangwar));
                        break;

                    case (int)DefaultMapIds.Normals:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type == MapType.Normal));
                        break;

                    case (int)DefaultMapIds.Bombs:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type == MapType.Bomb));
                        break;

                    case (int)DefaultMapIds.Snipers:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type == MapType.Sniper));
                        break;

                    case (int)DefaultMapIds.Gangwars:
                        lobbyMapsList.AddRange(_mapsHandler.GetDefaultMaps().Where(m => m.Info.Type == MapType.Gangwar));
                        break;

                    default:
                        var map = _mapsHandler.GetMapById(mapAssignment.MapId);
                        if (map is null)
                            map = _mapsHandler.GetNewCreatedMap(mapAssignment.MapId);
                        if (map is null)
                            map = _mapsHandler.GetNeedCheckMap(mapAssignment.MapId);
                        if (map is { })
                            lobbyMapsList.Add(map);
                        break;
                }
            }
            arena.MapHandler.SetMapList(lobbyMapsList);
        }

        private async void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            try
            {
                await MainMenu.Players.AddPlayer(player, 0).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private bool IsCustomLobbyNameAllowed(string name)
        {
            if (name.Length < 3 || name.Length > 50)
                return false;

            if (name.StartsWith("mapcreator-", StringComparison.CurrentCultureIgnoreCase))
                return false;

            return true;
        }
    }
}