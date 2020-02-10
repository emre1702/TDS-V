using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Helper;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using DB = TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Maps
{
    internal static class MapsLoader
    {
        public static List<MapDto> AllMaps { get; private set; } = new List<MapDto>();

        private static readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(MapDto));


        public static async Task LoadDefaultMaps(TDSDbContext dbcontext, List<DB.Rest.Maps> allDbMaps)
        {
            AllMaps = await LoadMaps(dbcontext, ServerConstants.MapsPath, false, allDbMaps);
        }

        public static async Task<List<MapDto>> LoadMaps(TDSDbContext dbcontext, string path, bool isOnlySaved, List<DB.Rest.Maps> allDbMaps)
        {
            List<MapDto> list = LoadMapsInDirectory(path, isOnlySaved);

            if (isOnlySaved)
            {
                LoadSavedMapsFakeDBInfos(list);
                return list;
            }
                
            await SaveMapsInDB(dbcontext, list, allDbMaps);

            // Load name of creator, Id and rating for Maps //
            LoadMapsDBInfos(dbcontext, list, allDbMaps);

            return list;
        }

        public static List<MapDto> LoadMapsInDirectory(string path, bool isOnlySaved)
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

        public static object? GetAllMapsForCustomLobby(TDSPlayer player, object[] args)
        {
            var allMapsSyncData = AllMaps.Union(MapCreator.NewCreatedMaps).Union(MapCreator.NeedCheckMaps).Select(m => m.BrowserSyncedData);

            return Serializer.ToBrowser(allMapsSyncData);

        }

        public static MapDto? LoadMap(FileInfo fileInfo, bool isOnlySaved)
        {
            using XmlReader reader = XmlReader.Create(fileInfo.OpenText());
            if (!_xmlSerializer.CanDeserialize(reader))
            {
                ErrorLogsManager.Log($"Could not deserialize file {fileInfo.FullName}.", Environment.StackTrace);
                return null;
            }

            MapDto map = (MapDto)_xmlSerializer.Deserialize(reader);
            map.Info.FilePath = fileInfo.FullName;

            if (isOnlySaved)
                return map;

            if (map.Info.Type != Enums.EMapType.Gangwar &&
                (map.LimitInfo.Center is null || 
                (map.LimitInfo.Center.X == 0 && map.LimitInfo.Center.Y == 0 && map.LimitInfo.Center.Z == 0)))
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

        public static MapDto? GetMapById(int id)
        {
            return AllMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == id);
        }

        public static MapDto? GetMapByName(string mapName)
        {
            return AllMaps.FirstOrDefault(m => m.Info.Name == mapName);
        }

        private static async Task SaveMapsInDB(TDSDbContext dbContext, List<MapDto> maps, List<DB.Rest.Maps> allDbMap)
        {
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var mapsToAdd = maps.Where(m => !allDbMap.Any(map => map.Name == m.Info.Name));

            foreach (var map in mapsToAdd)
            {
                DB.Rest.Maps dbMap;
                if (map.Info.CreatorId is null || !(await dbContext.Players.AnyAsync(p => p.Id == map.Info.CreatorId)))
                    dbMap = new DB.Rest.Maps() { Name = map.Info.Name, CreatorId = null };                    
                else
                    dbMap = new DB.Rest.Maps() { Name = map.Info.Name, CreatorId = map.Info.CreatorId };
                dbContext.Maps.Add(dbMap);

            }
            await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        private static void LoadMapsDBInfos(TDSDbContext dbContext, List<MapDto> maps, List<DB.Rest.Maps> allDbMap)
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
    }
}
