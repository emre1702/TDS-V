using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Dto.Map;
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
        public static ConcurrentDictionary<string, string> MapPathByName = new ConcurrentDictionary<string, string>();   // mapnames in lower case
                                                                                                                         //public static ConcurrentDictionary<string, string> MapCreator = new ConcurrentDictionary<string, string>();

        private static readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(MapDto));

        public static async Task LoadMaps(TDSNewContext dbcontext)
        {
            AllMaps = LoadMapsInDirectory(SettingsManager.MapsPath);

            await dbcontext.Maps.Include(m => m.Creator).LoadAsync();

            await SaveMapsInDB(dbcontext);

            // Load name of creators for Maps //
            await LoadCreatorNames(dbcontext);
        }

        public static List<MapDto> LoadMapsInDirectory(string path)
        {
            var list = new List<MapDto>();
            var directoryInfo = new DirectoryInfo(path);
            foreach (var fileInfo in directoryInfo.EnumerateFiles("*.xml", SearchOption.AllDirectories))
            {
                MapDto? map = LoadMap(fileInfo);
                if (map == null)
                    continue;
                list.Add(map);
            }
            return list;
        }

        public static MapDto? LoadMap(FileInfo fileInfo)
        {
            using XmlReader reader = XmlReader.Create(fileInfo.OpenText());
            if (!_xmlSerializer.CanDeserialize(reader))
            {
                ErrorLogsManager.Log($"Could not deserialize file {fileInfo.FullName}.", Environment.StackTrace);
                return null;
            }

            MapDto map = (MapDto)_xmlSerializer.Deserialize(reader);
            AllMaps.Add(map);

            if (map.LimitInfo.Center == null)
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

        public static MapDto? GetMapByName(string mapName)
        {
            return AllMaps.FirstOrDefault(m => m.Info.Name == mapName);
        }

        private static async Task SaveMapsInDB(TDSNewContext dbContext)
        {
            var removeMapsInDB = (await dbContext.Maps.Where(m => m.Id > 0 && !m.InTesting).ToListAsync())
                            .Where(m => !MapPathByName.ContainsKey(m.Name.ToLower()));

            dbContext.Maps.RemoveRange(removeMapsInDB);

            if ((await dbContext.Maps.Where(m => m.Id > 0 && !m.InTesting).CountAsync()) != AllMaps.Count)
            {
                foreach (var map in AllMaps)
                {
                    if (await dbContext.Maps.Where(m => m.Name == map.Info.Name).AnyAsync())
                        continue;

                    if (map.Info.CreatorId == null || !(await dbContext.Players.AnyAsync(p => p.Id == map.Info.CreatorId)))
                        await dbContext.Maps.AddAsync(new DB.Maps() { Name = map.Info.Name, CreatorId = null });
                    else
                        await dbContext.Maps.AddAsync(new DB.Maps() { Name = map.Info.Name, CreatorId = map.Info.CreatorId });
                }
            }
            await dbContext.SaveChangesAsync();
        }

        private static async Task LoadCreatorNames(TDSNewContext dbContext)
        {
            foreach (var map in AllMaps)
            {
                map.SyncedData.CreatorName = await dbContext.Maps
                    .Where(m => m.Name == map.Info.Name)
                    .Select(m => m.Creator.Name)
                    .FirstOrDefaultAsync();
            }
        }
    }
}