using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Maps;
using TDS_Shared.Core;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Utility;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class RoundFightLobbyMapHandler : BaseLobbyMapHandler, IRoundFightLobbyMapHandler
    {
        public MapDto? CurrentMap { get; private set; }
        public List<MapDto> Maps { get; private set; } = new List<MapDto>();

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
            Maps = maps.Where(m => m.TeamSpawnsList.TeamSpawns.Length == Lobby.Teams.Count - 1).ToList();
            _mapsJson = syncjson ?? Serializer.ToBrowser(Maps.Select(m => m.BrowserSyncedData).ToList());
        }

        private void CreateMapLimitBlips(MapDto map)
        {
            if (map.LimitInfo.Edges is null)
                return;
            int i = 0;
            NAPI.Task.Run(() =>
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
