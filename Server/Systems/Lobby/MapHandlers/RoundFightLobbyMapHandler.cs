using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS.Server.Data.Models.Map;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Maps;
using TDS.Shared.Core;
using TDS.Shared.Data.Default;
using TDS.Shared.Data.Utility;

namespace TDS.Server.LobbySystem.MapHandlers
{
    public class RoundFightLobbyMapHandler : BaseLobbyMapHandler, IRoundFightLobbyMapHandler
    {
        public MapDto? CurrentMap { get; private set; }
        public List<MapDto> Maps { get; private set; } = new List<MapDto>();
        public MapRetrieveType MapRetrieveType { get; protected set; }

        protected new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;
        private readonly ISettingsHandler _settingsHandler;
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private string _mapsJson = string.Empty;

        public RoundFightLobbyMapHandler(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, ISettingsHandler settingsHandler, MapsLoadingHandler mapsLoadingHandler)
            : base(lobby, events)
        {
            _settingsHandler = settingsHandler;
            _mapsLoadingHandler = mapsLoadingHandler;

            events.RoundClear += RoundClear;
            events.RequestNewMap += GetNextMap;
            events.InitNewMap += Events_InitNewMap;
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);

            if (Events.RoundClear is { })
                Events.RoundClear -= RoundClear;
            Events.RequestNewMap -= GetNextMap;
            Events.InitNewMap -= Events_InitNewMap;
        }

        public void SetMapList(IEnumerable<MapDto> maps, string? syncjson = null)
        {
            // Only choose maps with team-amount same as this lobby got teams (without spectator)
            Maps = maps.Where(m => m.TeamSpawnsList.TeamSpawns.Count == Lobby.Teams.Count - 1).ToList();
            _mapsJson = syncjson ?? Serializer.ToBrowser(Maps.Select(m => m.BrowserSyncedData).ToList());
        }

        private void CreateMapLimitBlips(MapDto map)
        {
            if (map.LimitInfo.Edges is null)
                return;
            int i = 0;
            NAPI.Task.RunSafe(() =>
            {
                foreach (var edge in map.LimitInfo.Edges)
                {
                    var blip = NAPI.Blip.CreateBlip(SharedConstants.MapLimitBlipSprite, edge.ToVector3(), 1f, 0, name: "Limit " + ++i, dimension: Dimension) as ITDSBlip;
                    AddMapBlip(blip!);
                }
            });
        }

        protected virtual void Events_InitNewMap(MapDto map)
        {
            CurrentMap = map;
            CreateMapLimitBlips(map);
        }

        public virtual MapDto? GetNextMap()
            => GetRandomMap();

        public string GetMapsJson() => _mapsJson;

        private MapDto? GetRandomMap()
        {
            MapRetrieveType = MapRetrieveType.Random;
            lock (Maps)
            {
                if (Maps.Count == 0)
                    return null;
                if (Maps.Count == 1)
                    return Maps[0];
            }

            if (GetShouldUseNewMap())
            {
                var map = _mapsLoadingHandler.GetRandomNewMap();
                if (map is { })
                    return map;
            }

            lock (Maps)
            {
                var allowedMaps = GetMapsForCurrentPlayerAmount();
                if (allowedMaps.Any())
                    return GetRandomMapFromList(allowedMaps);

                return GetRandomMapFromList(Maps);
            }
        }

        private MapDto GetRandomMapFromList(List<MapDto> list)
        {
            var sumRatings = (int)Math.Floor(list.Sum(m => m.RatingAverage));
            var chooseAtRating = SharedUtils.Rnd.Next(sumRatings) + 1;
            double currentlyAtRating = 0;
            foreach (var map in list)
            {
                currentlyAtRating += map.RatingAverage;
                if (chooseAtRating <= currentlyAtRating)
                    return map;
            }

            // if I did a mistake, just return anything
            return SharedUtils.GetRandom(list);
        }

        private ValueTask RoundClear()
        {
            DeleteMapBlips();
            return default;
        }

        private bool GetShouldUseNewMap()
            => Lobby.IsOfficial && SharedUtils.Rnd.NextDouble() * 100 <= _settingsHandler.ServerSettings.ArenaNewMapProbabilityPercent;

        private List<MapDto> GetMapsForCurrentPlayerAmount()
        {
            var currentPlayersCount = Lobby.Players.Count;
            return Maps.Where(m => currentPlayersCount >= m.Info.MinPlayers && currentPlayersCount <= m.Info.MaxPlayers).ToList();
        }
    }
}
