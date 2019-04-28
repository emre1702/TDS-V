namespace TDS_Server.Manager.Maps
{

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.EntityFrameworkCore;
    using TDS_Common.Dto.Map;
    using TDS_Server.Entity;
    using TDS_Server.Manager.Helper;
    using TDS_Server.Manager.Logs;
    using TDS_Server.Manager.Utility;

    static class MapsLoader
    {
        public static List<MapDto> AllMaps = new List<MapDto>();
        public static ConcurrentDictionary<string, string> MapPathByName = new ConcurrentDictionary<string, string>();   // mapnames in lower case
                                                                                                                         //public static ConcurrentDictionary<string, string> MapCreator = new ConcurrentDictionary<string, string>();

        private static readonly XmlReaderSettings _xmlReaderSettings = new XmlReaderSettings() { Async = true };
        private static readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(MapDto));

        public static void LoadMaps(TDSNewContext dbcontext)
        {
            AllMaps = LoadMapsInDirectory(SettingsManager.MapsPath);

            dbcontext.Maps.Include(m => m.Creator).Load();

            SaveMapsInDB(dbcontext);

            // Load name of creators for Maps //
            LoadCreatorNames(dbcontext);
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
            using XmlReader reader = XmlReader.Create(fileInfo.OpenRead(), _xmlReaderSettings);
            if (!_xmlSerializer.CanDeserialize(reader))
            {
                ErrorLogsManager.Log($"Could not deserialize file {fileInfo.FullName}.", Environment.StackTrace);
                return null;
            }

            MapDto map = (MapDto)_xmlSerializer.Deserialize(reader);
            AllMaps.Add(map);

            if (map.LimitInfo.Center == null)
                map.LimitInfo.Center = map.GetCenter();

            map.CreateJsons();
            map.LoadSyncedData();
            return map;
        }

        private static void SaveMapsInDB(TDSNewContext dbContext)
        {
            dbContext.Maps.RemoveRange(
               dbContext.Maps.Where(
                   m => m.Id > 0 && !MapPathByName.ContainsKey(m.Name.ToLower())
               )
           );

            if (dbContext.Maps.Where(m => m.Id > 0).Count() != AllMaps.Count)
            {
                foreach (var map in AllMaps)
                {
                    if (dbContext.Maps.Where(m => m.Name == map.Info.Name).Any())
                        continue;

                    if (map.Info.CreatorId == null || dbContext.Players.Find(map.Info.CreatorId) == null)
                        dbContext.Maps.Add(new Maps() { Name = map.Info.Name, CreatorId = null });
                    else
                        dbContext.Maps.Add(new Maps() { Name = map.Info.Name, CreatorId = map.Info.CreatorId });
                }
            }
            dbContext.SaveChanges();
        }

        private static void LoadCreatorNames(TDSNewContext dbContext)
        {
            foreach (var map in AllMaps)
            {
                map.SyncedData.CreatorName = dbContext.Maps
                    .Where(m => m.Name == map.Info.Name)
                    .Select(m => m.Creator.Name)
                    .FirstOrDefault();
            }
        }
       
    }

}
