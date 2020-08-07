using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
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
        #region Public Fields

        /// <summary>
        /// Former: AllMaps
        /// </summary>
        public List<MapDto> DefaultMaps = new List<MapDto>();

        public List<MapDto> NeedCheckMaps = new List<MapDto>();
        public List<MapDto> NewCreatedMaps = new List<MapDto>();
        public List<MapDto> SavedMaps = new List<MapDto>();

        #endregion Public Fields

        #region Private Fields

        private readonly TDSDbContext _dbContext;
        private readonly EventsHandler _eventsHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(MapDto));

        #endregion Private Fields

        #region Public Constructors

        public MapsLoadingHandler(TDSDbContext dbContext, EventsHandler eventsHandler, Serializer serializer, ILoggingHandler loggingHandler, ISettingsHandler settingsHandler)
            => (_dbContext, _eventsHandler, _serializer, _loggingHandler, _settingsHandler) = (dbContext, eventsHandler, serializer, loggingHandler, settingsHandler);

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<MapDto> AllCreatingMaps => NewCreatedMaps.Union(NeedCheckMaps);

        #endregion Public Properties

        #region Public Methods

        // .Union(_savedMaps)
        public object? GetAllMapsForCustomLobby(ITDSPlayer player, ref ArraySegment<object> args)
        {
            var allMapsSyncData = DefaultMaps.Union(NewCreatedMaps).Union(NeedCheckMaps).Select(m => m.BrowserSyncedData);

            return _serializer.ToBrowser(allMapsSyncData);
        }

        public MapDto? GetMapById(int id)
        {
            return DefaultMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == id);
        }

        public MapDto? GetMapByName(string mapName)
        {
            return DefaultMaps.FirstOrDefault(m => m.Info.Name == mapName);
        }

        public MapDto? GetRandomNewMap()
        {
            if (NewCreatedMaps.Count == 0)
                return null;
            var list = NewCreatedMaps.Where(m => m.Ratings.Count < _settingsHandler.ServerSettings.MapRatingAmountForCheck).ToList();
            if (list.Count == 0)
                return null;
            return SharedUtils.GetRandom(list);
        }

        public void LoadAllMaps()
        {
            var allDbMaps = _dbContext.Maps.Include(m => m.Creator).Include(m => m.PlayerMapRatings).ToList();

            NewCreatedMaps = LoadMaps(Constants.NewMapsPath, false, allDbMaps);
            foreach (var map in NewCreatedMaps)
            {
                // Player shouldn't be able to see the creator of the map (so they don't rate it
                // depending of the creator)
                map.BrowserSyncedData.CreatorName = string.Empty;
                map.Info.IsNewMap = true;
            }

            SavedMaps = LoadMaps(Constants.SavedMapsPath, true, allDbMaps);
            foreach (var map in SavedMaps)
            {
                map.Info.IsNewMap = true;
            }

            NeedCheckMaps = LoadMaps(Constants.NeedCheckMapsPath, false, allDbMaps);
            foreach (var map in NewCreatedMaps)
            {
                map.Info.IsNewMap = true;
            }

            DefaultMaps = LoadMaps(Constants.MapsPath, false, allDbMaps);

            _eventsHandler.OnMapsLoaded();
        }

        #endregion Public Methods

        #region Private Methods

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

            map.CreateJsons(_serializer);
            map.LoadSyncedData();
            return map;
        }

        private List<MapDto> LoadMaps(string path, bool isOnlySaved, List<DB.Rest.Maps> allDbMaps)
        {
            List<MapDto> list = LoadMapsInDirectory(path, isOnlySaved);

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

        private List<MapDto> LoadMapsInDirectory(string path, bool isOnlySaved)
        {
            var list = new List<MapDto>();
            var directoryInfo = new DirectoryInfo(path);
            foreach (var fileInfo in directoryInfo.EnumerateFiles("*.map", SearchOption.AllDirectories))
            {
                MapDto? map = LoadMap(fileInfo, isOnlySaved);
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
        #endregion Private Methods
    }
}
