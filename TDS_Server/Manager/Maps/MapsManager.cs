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
    using Newtonsoft.Json;
    using TDS_Common.Dto.Map;
    using TDS_Common.Enum;
    using TDS_Server.Entity;
    using TDS_Server.Manager.Logs;
    using TDS_Server.Manager.Utility;

    static partial class MapsManager
    {
        public static List<MapFileDto> AllMaps = new List<MapFileDto>();
        public static ConcurrentDictionary<string, string> MapPathByName = new ConcurrentDictionary<string, string>();   // mapnames in lower case
                                                                                                                         //public static ConcurrentDictionary<string, string> MapCreator = new ConcurrentDictionary<string, string>();

        private static readonly XmlReaderSettings _xmlReaderSettings = new XmlReaderSettings() { Async = true };

        public static void LoadMaps(TDSNewContext dbcontext)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MapFileDto));
            var directoryInfo = new DirectoryInfo(SettingsManager.MapsPath);

            foreach (var fileInfo in directoryInfo.EnumerateFiles("*.xml", SearchOption.AllDirectories))
            {
                using XmlReader reader = XmlReader.Create(fileInfo.OpenRead(), _xmlReaderSettings);
                if (!serializer.CanDeserialize(reader))
                {
                    ErrorLogsManager.Log($"Could not deserialize file {fileInfo.FullName}.", Environment.StackTrace);
                    continue;
                }

                MapFileDto map = (MapFileDto) serializer.Deserialize(reader);
                AllMaps.Add(map);

                if (map.LimitInfo.Center == null)
                    map.LimitInfo.Center = GetCenter(map);

                if (map.BombInfo != null)
                    map.BombInfo.PlantPositionsJson = JsonConvert.SerializeObject(map.BombInfo.PlantPositions);
                map.LimitInfo.EdgesJson = JsonConvert.SerializeObject(map.LimitInfo.Edges);

                map.SyncedData.Name = map.Info.Name;
                map.SyncedData.Description[(int)ELanguage.English] = map.Descriptions?.English;
                map.SyncedData.Description[(int)ELanguage.German] = map.Descriptions?.German;
                map.SyncedData.Type = map.Info.Type;
            }

            dbcontext.Maps.Include(m => m.Creator).Load();

            // maps with id < 0 are reserved (e.g. all, normals, bombs)
            dbcontext.Maps.RemoveRange(
                dbcontext.Maps.Where(
                    m => m.Id > 0 && !MapPathByName.ContainsKey(m.Name.ToLower())
                )
            );

            // Add maps to database which don't exist //
            if (dbcontext.Maps.Where(m => m.Id > 0).Count() != AllMaps.Count)
            {
                foreach (var map in AllMaps)
                {
                    if (dbcontext.Maps.Where(m => m.Name == map.SyncedData.Name).Any())
                        continue;

                    if (map.Info.CreatorId == null || dbcontext.Players.Find(map.Info.CreatorId) == null)
                        dbcontext.Maps.Add(new Maps() { Name = map.SyncedData.Name, CreatorId = null });
                    else
                        dbcontext.Maps.Add(new Maps() { Name = map.SyncedData.Name, CreatorId = map.Info.CreatorId });
                }
            }
            dbcontext.SaveChanges();

            // Load name of creators for Maps //
            foreach (var map in AllMaps)
            {
                map.SyncedData.CreatorName = dbcontext.Maps.Where(m => m.Name == map.SyncedData.Name).Select(m => m.Creator.Name).FirstOrDefault();
            }
        }

        private static MapPositionDto? GetCenterOfPositions(MapPositionDto[] positions, float zpos = 0)
        {
            if (positions.Length <= 2)
                return null;

            float centerX = 0.0f;
            float centerY = 0.0f;
            float centerZ = 0.0f;


            foreach (MapPositionDto point in positions)
            {
                centerX += point.X;
                centerY += point.Y;
                centerZ += Math.Abs(zpos - (-1)) < 0.001 ? (point.Z ?? 0) : zpos;
            }

            return new MapPositionDto { X = centerX / positions.Length, Y = centerY / positions.Length, Z = centerZ / positions.Length };
        }

        private static MapPositionDto? GetCenterByLimits(MapFileDto map, float zpos)
        {
            return GetCenterOfPositions(map.LimitInfo.Edges, zpos) ?? GetCenterBySpawns(map);
        }

        private static MapPositionDto? GetCenterBySpawns(MapFileDto map)
        {
            int amountteams = map.TeamSpawnsList.TeamSpawns.Length;
            if (amountteams == 1)
            {
                return map.TeamSpawnsList.TeamSpawns[0].Spawns.FirstOrDefault();
            }
            else if (amountteams > 1)
            {
                MapPositionDto[] positions = map.TeamSpawnsList.TeamSpawns
                    .Select(entry => entry.Spawns[0])
                    .ToArray();
                return GetCenterOfPositions(positions);
            }
            return null;
        }

        private static MapPositionDto? GetCenter(MapFileDto map)
        {
            if (map.LimitInfo.Edges.Length == 0)
                return GetCenterBySpawns(map);

            float zpos = GetCenterZPos(map);
            return GetCenterByLimits(map, zpos);

        }

        private static float GetCenterZPos(MapFileDto map)
        {
            var teamSpawns1 = map.TeamSpawnsList.TeamSpawns.FirstOrDefault();
            var teamSpawns2 = map.TeamSpawnsList.TeamSpawns.LastOrDefault();
            if (teamSpawns1 == null)
                return 0;
            var spawn1 = teamSpawns1.Spawns.FirstOrDefault();
            var spawn2 = teamSpawns2.Spawns.FirstOrDefault();
            return ((spawn1.Z ?? 0) + (spawn2.Z ?? 0)) / 2;
        }

        // WAS ALREADY DEACTIVATED 
        /*private static Map getMapDataOther ( string path ) {
			Map map = new Map ();
			XmlGroup mapdata = API.loadXml ( path );

			bool teamexists = true;
			int teamcounter = 1;

			// Map-Info //
			foreach ( xmlElement element in mapdata.getElementsByType ( "map" ) ) {
				map.name = element.getElementData<string> ( "name" );
				map.type = element.getElementData<string> ( "type" );
			}

			// Team-Spawns //
			while ( teamexists ) {
				bool gotone = false;
				foreach ( xmlElement element in mapdata.getElementsByType ( "team" + teamcounter ) ) {
					gotone = true;
					double x = element.getElementData<double> ( "x" );
					double y = element.getElementData<double> ( "y" );
					double z = element.getElementData<double> ( "z" );
					double xrot = element.getElementData<double> ( "xrot" );
					double yrot = element.getElementData<double> ( "yrot" );
					double zrot = element.getElementData<double> ( "zrot" );
					map.teamSpawns[teamcounter].Add ( new Vector3 ( x, y, z ) );
					map.teamRots[teamcounter].Add ( new Vector3 ( xrot, yrot, zrot ) );
				}
				if ( !gotone )
					teamexists = false;
				else
					teamcounter++;
			}

			// Map-Begrenzungen //
			foreach ( xmlElement element in mapdata.getElementsByType ( "limit" ) ) {
				double x = element.getElementData<double> ( "x" );
				double y = element.getElementData<double> ( "y" );
				double z = element.getElementData<double> ( "z" );
				map.maplimit.Add ( new Vector3 ( x, y, z ) );
			}

			return map;
		}*/
    }

}
