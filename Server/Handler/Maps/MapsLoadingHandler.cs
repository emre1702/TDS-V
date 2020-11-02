using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gangs;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Utility;

using DB = TDS_Server.Database.Entity;

namespace TDS_Server.Handler.Maps
{
    public class MapsLoadingHandler
    {
        private List<MapDto> _defaultMaps = new List<MapDto>();

        private List<MapDto> _needCheckMaps = new List<MapDto>();
        private List<MapDto> _newCreatedMaps = new List<MapDto>();
        private List<MapDto> _savedMaps = new List<MapDto>();

        private readonly TDSDbContext _dbContext;
        private readonly EventsHandler _eventsHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(MapDto));

        public MapsLoadingHandler(TDSDbContext dbContext, EventsHandler eventsHandler, ILoggingHandler loggingHandler, ISettingsHandler settingsHandler)
            => (_dbContext, _eventsHandler, _loggingHandler, _settingsHandler) = (dbContext, eventsHandler, loggingHandler, settingsHandler);

        public IEnumerable<MapDto> GetAllMapsInCreating()
        {
            lock (_newCreatedMaps)
            {
                lock (_needCheckMaps)
                {
                    return _newCreatedMaps.Union(_needCheckMaps);
                }
            }
        }

        public object? GetAllMapsForCustomLobby(ITDSPlayer player, ref ArraySegment<object> args)
        {
            lock (_defaultMaps)
            {
                lock (_newCreatedMaps)
                {
                    lock (_needCheckMaps)
                    {
                        var allMapsSyncData = _defaultMaps.Union(_newCreatedMaps).Union(_needCheckMaps).Select(m => m.BrowserSyncedData);

                        return Serializer.ToBrowser(allMapsSyncData);
                    }
                }
            }
        }

        public int GetNextNewCreatedMapId()
        {
            lock (_savedMaps)
            {
                return _savedMaps.Min(m => m.BrowserSyncedData.Id) - 1;
            }
        }

        public MapDto? GetGangwarAreaMap(int mapId)
        { 
            lock (_defaultMaps) 
            { 
                return _defaultMaps.FirstOrDefault(m => m.Info.Type == MapType.Gangwar && m.BrowserSyncedData.Id == mapId);
            }
        }

        public void AddNewCreatedMap(MapDto map)
        {
            lock (_newCreatedMaps)
            {
                _newCreatedMaps.Add(map);
            }
        }

        public void AddSavedMap(MapDto map)
        {
            lock (_savedMaps)
            {
                _savedMaps.Add(map);
            }      
        }

        public MapDto? GetMapById(int id)
        {
            lock (_defaultMaps)
            {
                return _defaultMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == id);
            }
        }

        public MapDto? GetSavedMap(int mapId)
        {
            lock (_savedMaps)
            {
                return _savedMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
            }
        }

        public MapDto? GetNewCreatedMap(int mapId)
        {
            lock (_newCreatedMaps)
            {
                return _newCreatedMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
            }
        }

        public MapDto? GetNewCreatedMap(string mapName)
        {
            lock (_newCreatedMaps)
            {
                return _newCreatedMaps.FirstOrDefault(m => m.BrowserSyncedData.Name == mapName);
            }
        }

        public MapDto? GetNeedCheckMap(int mapId)
        {
            lock (_needCheckMaps)
            {
                return _needCheckMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
            }
        }

        public MapDto? GetNeedCheckMap(string mapName)
        {
            lock (_needCheckMaps)
            {
                return _needCheckMaps.FirstOrDefault(m => m.BrowserSyncedData.Name == mapName);
            }
        }

        public IEnumerable<MapDto> GetGangwarMapsWithoutGangwarAreas(List<GangwarAreas> gangwarAreas)
        {
            lock (_defaultMaps)
            {
                return _defaultMaps
                    .Where(map => map.BrowserSyncedData.Type == TDS_Shared.Data.Enums.MapType.Gangwar 
                                && !gangwarAreas.Any(a => a.MapId == map.BrowserSyncedData.Id));
            }
        }

        internal bool RemoveSavedMap(MapDto map)
        {
            lock (_savedMaps)
            {
                return _savedMaps.Remove(map);
            }
        }

        internal bool RemoveNewCreatedMap(MapDto map)
        {
            lock (_newCreatedMaps)
            {
                return _newCreatedMaps.Remove(map);
            }
        }

        internal bool RemoveNeedCheckMap(MapDto map)
        {
            lock (_needCheckMaps)
            {
                return _needCheckMaps.Remove(map);
            }
        }

        public MapDto? GetMapByName(string mapName)
        {
            lock (_defaultMaps)
            {
                return _defaultMaps.FirstOrDefault(m => m.Info.Name == mapName);
            }
        }

        public MapDto? GetRandomNewMap()
        {
            List<MapDto>? listOfPossibleMaps;
            lock (_newCreatedMaps)
            {
                if (_newCreatedMaps.Count == 0)
                    return null;
                listOfPossibleMaps = _newCreatedMaps.Where(m => m.Ratings.Count < _settingsHandler.ServerSettings.MapRatingAmountForCheck).ToList();
            }
            if (listOfPossibleMaps.Count == 0)
                return null;
            return SharedUtils.GetRandom(listOfPossibleMaps);
        }

        public void LoadAllMaps()
        {
            var allDbMaps = _dbContext.Maps.Include(m => m.Creator).Include(m => m.PlayerMapRatings).ToList();

            _newCreatedMaps = LoadMaps(Constants.NewMapsPath, false, allDbMaps);
            foreach (var map in _newCreatedMaps)
            {
                // Player shouldn't be able to see the creator of the map (so they don't rate it
                // depending of the creator)
                map.BrowserSyncedData.CreatorName = string.Empty;
                map.Info.IsNewMap = true;
            }

            _savedMaps = LoadMaps(Constants.SavedMapsPath, true, allDbMaps);
            foreach (var map in _savedMaps)
            {
                map.Info.IsNewMap = true;
            }

            _needCheckMaps = LoadMaps(Constants.NeedCheckMapsPath, false, allDbMaps);
            foreach (var map in _newCreatedMaps)
            {
                map.Info.IsNewMap = true;
            }

            _defaultMaps = LoadMaps(Constants.MapsPath, false, allDbMaps);

            _eventsHandler.OnMapsLoaded();
        }

        private static void LoadMapsDBInfos(List<MapDto> maps, List<DB.Rest.Maps> allDbMap)
        {
            foreach (var map in maps)
            {
                var dbMap = allDbMap.First(m => m.Name == map.Info.Name);

                map.BrowserSyncedData.CreatorName = dbMap.Creator?.Name ?? "?";
                map.BrowserSyncedData.Id = dbMap.Id;
                map.Ratings = dbMap.PlayerMapRatings.ToList();
                map.RatingAverage = map.Ratings.Count > 0 ? map.Ratings.Average(r => r.Rating) : 5;
            }
        }

        private static void LoadSavedMapsFakeDBInfos(List<MapDto> maps)
        {
            int negativeIdCounterForSaveMaps = 0;
            foreach (var map in maps)
            {
                map.BrowserSyncedData.CreatorName = "?";
                map.BrowserSyncedData.Id = --negativeIdCounterForSaveMaps;
                map.Ratings = new List<DB.Player.PlayerMapRatings>();
                map.RatingAverage = 5;
            }
        }

        private MapDto? LoadMap(FileInfo fileInfo, bool isOnlySaved)
        {
            using XmlReader reader = XmlReader.Create(fileInfo.OpenText());
            if (!_xmlSerializer.CanDeserialize(reader))
            {
                _loggingHandler.LogError($"Could not deserialize file {fileInfo.FullName}.", Environment.StackTrace);
                return null;
            }

            MapDto map = (MapDto)_xmlSerializer.Deserialize(reader);
            map.Info.FilePath = fileInfo.FullName;

            if (isOnlySaved)
                return map;

            map.LimitInfo.Center = map.GetCenter();

            uint teamId = 0;
            foreach (var mapTeamSpawns in map.TeamSpawnsList.TeamSpawns)
            {
                mapTeamSpawns.TeamID = ++teamId;
            }

            map.CreateJsons();
            map.LoadSyncedData();
            return map;
        }

        private List<MapDto> LoadMaps(string path, bool isOnlySaved, List<DB.Rest.Maps> allDbMaps)
        {
            var list = LoadMapsInDirectory(path, isOnlySaved);

            if (isOnlySaved)
            {
                LoadSavedMapsFakeDBInfos(list);
                return list;
            }

            SaveMapsInDB(list, allDbMaps);

            // Load name of creator, Id and rating for Maps //
            LoadMapsDBInfos(list, allDbMaps);

            return list;
        }

        internal List<MapDto> GetSavedMaps()
        {
            lock (_savedMaps)
            {
                return _savedMaps.ToList();
            }
        }

        internal List<MapDto> GetNewCreatedMaps()
        {
            lock (_newCreatedMaps)
            {
                return _newCreatedMaps.ToList();
            }
        }

        internal List<MapDto> GetDefaultMaps()
        {
            lock (_defaultMaps)
            {
                return _defaultMaps.ToList();
            }
        }

        internal List<MapDto> GetNeedCheckMaps()
        {
            lock (_needCheckMaps)
            {
                return _needCheckMaps.ToList();
            }
        }

        private List<MapDto> LoadMapsInDirectory(string path, bool isOnlySaved)
        {
            var list = new List<MapDto>();
            var directoryInfo = new DirectoryInfo(path);
            foreach (var fileInfo in directoryInfo.EnumerateFiles("*.map", SearchOption.AllDirectories))
            {
                var map = LoadMap(fileInfo, isOnlySaved);
                if (map is null)
                    continue;
                list.Add(map);
            }
            return list;
        }

        private void SaveMapsInDB(List<MapDto> maps, List<DB.Rest.Maps> allDbMap)
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var mapsToAdd = maps.Where(m => !allDbMap.Any(map => map.Name == m.Info.Name)).ToList();

            foreach (var map in mapsToAdd)
            {
                DB.Rest.Maps dbMap;
                if (map.Info.CreatorId is null || !_dbContext.Players.Any(p => p.Id == map.Info.CreatorId))
                    dbMap = new DB.Rest.Maps() { Name = map.Info.Name, CreatorId = null };
                else
                    dbMap = new DB.Rest.Maps() { Name = map.Info.Name, CreatorId = map.Info.CreatorId };
                _dbContext.Maps.Add(dbMap);
                allDbMap.Add(dbMap);
            }
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
