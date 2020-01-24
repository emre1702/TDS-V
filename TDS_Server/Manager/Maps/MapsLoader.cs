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


        public static async Task LoadDefaultMaps(TDSDbContext dbcontext)
        {
            AllMaps = await LoadMaps(dbcontext, ServerConstants.MapsPath, false);
        }

        public static async Task<List<MapDto>> LoadMaps(TDSDbContext dbcontext, string path, bool isOnlySaved)
        {
            List<MapDto> list = LoadMapsInDirectory(path, isOnlySaved);

            await dbcontext.Maps.Include(m => m.Creator).Include(m => m.PlayerMapRatings).LoadAsync();

            await SaveMapsInDB(dbcontext, list);

            // Load name of creator and Id for Maps //
            LoadMapDBInfos(dbcontext, list);

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
            var allMapsSyncData = AllMaps.Union(MapCreator.AllCreatingMaps).Select(m => m.SyncedData);

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

            if (map.LimitInfo.Center is null || 
                (map.LimitInfo.Center.X == 0 && map.LimitInfo.Center.Y == 0 && map.LimitInfo.Center.Z == 0))
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
            return AllMaps.FirstOrDefault(m => m.SyncedData.Id == id);
        }

        public static MapDto? GetMapByName(string mapName)
        {
            return AllMaps.FirstOrDefault(m => m.Info.Name == mapName);
        }

        private static async Task SaveMapsInDB(TDSDbContext dbContext, List<MapDto> maps)
        {
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var mapsToAdd = maps.Where(m => !dbContext.Maps.Any(map => map.Name == m.Info.Name));

            foreach (var map in mapsToAdd)
            {

                if (map.Info.CreatorId is null || !(await dbContext.Players.AnyAsync(p => p.Id == map.Info.CreatorId)))
                    await dbContext.Maps.AddAsync(new DB.Rest.Maps() { Name = map.Info.Name, CreatorId = null });
                else
                    await dbContext.Maps.AddAsync(new DB.Rest.Maps() { Name = map.Info.Name, CreatorId = map.Info.CreatorId });
            }
            await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        private static void LoadMapDBInfos(TDSDbContext dbContext, List<MapDto> maps)
        {
            foreach (var map in maps)
            {
                DB.Rest.Maps dbMap = dbContext.Maps
                    .Where(m => m.Name == map.Info.Name)
                    .Include(m => m.Creator)
                    .First();
                map.SyncedData.CreatorName = dbMap.Creator?.Name ?? "?";
                map.SyncedData.Id = dbMap.Id;
                map.LoadMapRatings(dbContext);
            }
        }
    }
}
