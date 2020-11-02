using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models.CustomLobby;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Extensions;
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
        public readonly Dictionary<int, IBaseLobby> LobbiesByIndex = new Dictionary<int, IBaseLobby>();

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
            ILobbiesProvider lobbiesProvider) : base(dbContext)
        {
            _mapsHandler = mapsHandler;
            _lobbiesProvider = lobbiesProvider;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.LobbyCreated += AddLobby;
            eventsHandler.LobbyRemoved += RemoveLobby;
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

        public async Task<object?> CreateCustomLobby(ITDSPlayer player, ArraySegment<object> args)
        {
            try
            {
                await Task.Yield();
                string dataJson = (string)args[0];
                var data = Serializer.FromBrowser<CustomLobbyData>(dataJson);
                if (!IsCustomLobbyNameAllowed(data.Name))
                {
                    return player.Language.CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR;
                }
                bool nameAlreadyInUse = Lobbies.Any(lobby => lobby.Entity.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase));
                if (nameAlreadyInUse)
                {
                    return player.Language.CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR;
                }

                var entity = new Lobbies
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
                    ArmsRaceWeapons = data.ArmsRaceWeapons?.Select(w => new LobbyArmsRaceWeapons
                    {
                        WeaponHash = w.WeaponHash,
                        AtKill = w.AtKill
                    })?.ToHashSet(),
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

                var arena = _lobbiesProvider.Create<IArena>(entity);

                AddMapsToArena(arena, entity);

                await arena.Players.AddPlayer(player, 0).ConfigureAwait(false);
                return null;
            }
            catch
            {
                return player.Language.CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR;
            }
        }

        public IBaseLobby? GetLobby(int id)
        {
            LobbiesByIndex.TryGetValue(id, out IBaseLobby? lobby);
            return lobby;
        }

#pragma warning disable IDE0060 // Remove unused parameter

        public async ValueTask<object?> LoadDatas(ITDSPlayer player, ArraySegment<object> _)
#pragma warning restore IDE0060 // Remove unused parameter
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

        public async Task<object?> OnJoinLobby(ITDSPlayer player, ArraySegment<object> args)
        {
            int index = (int)args[0];

            if (LobbiesByIndex.ContainsKey(index))
            {
                IBaseLobby lobby = LobbiesByIndex[index];
                if (lobby is IMapCreatorLobby)
                {
                    if (await lobby.Bans.CheckIsBanned(player).ConfigureAwait(false))
                        return null;

                    lobby = _lobbiesProvider.Create<IMapCreatorLobby>(player);
                }
                else if (lobby is ICharCreateLobby)
                {
                    if (await lobby.Bans.CheckIsBanned(player).ConfigureAwait(false))
                        return null;

                    lobby = _lobbiesProvider.Create<ICharCreateLobby>(player);
                }
                else if (lobby is IDamageTestLobby)
                {
                    lobby = _lobbiesProvider.Create<IDamageTestLobby>(player);
                }
                await lobby.Players.AddPlayer(player, 0).ConfigureAwait(false);
                return null;
            }
            else
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.LOBBY_DOESNT_EXIST));
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
                IBaseLobby lobby = LobbiesByIndex[index];
                if (password != null && lobby.Entity.Password != password)
                {
                    NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.WRONG_PASSWORD));
                    return null;
                }

                await lobby.Players.AddPlayer(player, 0).ConfigureAwait(false);
                return null;
            }
            else
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.LOBBY_DOESNT_EXIST));
                //todo Remove lobby at client view and check, why he saw this lobby
                return null;
            }
        }

        public void RemoveLobby(IBaseLobby lobby)
        {
            LobbiesByIndex.Remove(lobby.Entity.Id);
            Lobbies.Remove(lobby);

            //Todo: How to do that? Put it in Lobby? check it on event handlers?
            //if (!lobby.IsOfficial)
            //    _eventsHandler.OnCustomLobbyRemoved(lobby);
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

        private void AddMapsToArena(IArena arena, Lobbies lobbySetting)
        {
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
                        var map = _mapsHandler.GetMapByName(mapAssignment.Map.Name);
                        if (map is null)
                            map = _mapsHandler.GetNewCreatedMap(mapAssignment.Map.Name);
                        if (map is null)
                            map = _mapsHandler.GetNeedCheckMap(mapAssignment.Map.Name);
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
