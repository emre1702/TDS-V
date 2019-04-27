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

        public static void LoadMaps(TDSNewContext dbcontext)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MapDto));
            var directoryInfo = new DirectoryInfo(SettingsManager.MapsPath);

            foreach (var fileInfo in directoryInfo.EnumerateFiles("*.xml", SearchOption.AllDirectories))
            {
                using XmlReader reader = XmlReader.Create(fileInfo.OpenRead(), _xmlReaderSettings);
                if (!serializer.CanDeserialize(reader))
                {
                    ErrorLogsManager.Log($"Could not deserialize file {fileInfo.FullName}.", Environment.StackTrace);
                    continue;
                }

                MapDto map = (MapDto) serializer.Deserialize(reader);
                AllMaps.Add(map);

                if (map.LimitInfo.Center == null)
                    map.LimitInfo.Center = map.GetCenter();

                map.CreateJsons();
                map.LoadSyncedData();
            }

            dbcontext.Maps.Include(m => m.Creator).Load();

            SaveMapsInDB(dbcontext);

            // Load name of creators for Maps //
            LoadCreatorNames(dbcontext);
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
