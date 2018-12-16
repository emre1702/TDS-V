namespace TDS.Manager.Maps
{

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using GTANetworkAPI;
    using TDS.Dto;
    using TDS.Enum;
    using TDS.Instance.Lobby;
    using TDS.Manager.Logs;
    using TDS.Manager.Utility;

    static partial class Maps
    {
        private static readonly XmlReaderSettings settings = new XmlReaderSettings();

        public static List<MapDto> allMaps = new List<MapDto>();
        public static List<MapSynedDataDto> allMapsSync = new List<MapSynedDataDto>();

        public static ConcurrentDictionary<string, string> MapPathByName = new ConcurrentDictionary<string, string>();   // mapnames in lower case
        public static ConcurrentDictionary<string, string> MapCreator = new ConcurrentDictionary<string, string>();

        public static async Task LoadMaps()
        {
            settings.Async = true;
            IEnumerable<string> directories = Directory.EnumerateDirectories(SettingsManager.MapsPath);
            foreach (string dir in directories)
            {
                string creator = Path.GetFileName(dir);
                IEnumerable<string> files = Directory.EnumerateFiles(dir, "*.xml");
                foreach (string filepath in files)
                {
                    string filename = Path.GetFileNameWithoutExtension(filepath);
                    MapCreator[filename] = creator;
                    MapDto map = new MapDto();
                    if (await map.AddInfos(filename).ConfigureAwait(false))
                    {
                        if (map.SyncedData.Name != null)
                        {
                            allMaps.Add(map);
                            allMapsSync.Add(map.SyncedData);

                            MapPathByName[map.SyncedData.Name.ToLower()] = filename;
                        }
                        else
                            Error.Log("Map " + filename + " got no name!", Environment.StackTrace, null);
                    }
                }
            }
        }

        private static Vector3 GetCenterOfPositions(IEnumerable<Vector3> poly, float zpos = -1)
        {
            int length = poly.Count();
            if (poly.Count() <= 2)
                return null;

            float centerX = 0.0f;
            float centerY = 0.0f;
            float centerZ = 0.0f;


            foreach (Vector3 point in poly)
            {
                centerX += point.X;
                centerY += point.Y;
                centerZ += Math.Abs(zpos - (-1)) < 0.001 ? point.Z : zpos;
            }

            return new Vector3(centerX / length, centerY / length, centerZ / length);
        }

        private static Vector3 GetCenterByLimits(MapDto map, float zpos)
        {
            return GetCenterOfPositions(map.MapLimits, zpos) ?? GetCenterBySpawns(map);
        }

        private static Vector3 GetCenterBySpawns(MapDto map)
        {
            int amountteams = map.TeamSpawns.Count;
            if (amountteams == 1)
            {
                return map.TeamSpawns[0][0].Position;
            }
            else if (amountteams > 1)
            {
                IEnumerable<Vector3> positions = map.TeamSpawns.Select(entry => entry[0].Position);
                return GetCenterOfPositions(positions);
            }
            return new Vector3();
        }

        private static Vector3 GetCenter(MapDto map)
        {
            if (map.MapLimits.Count > 0)
            {
                float zpos = map.TeamSpawns.Select(entry => entry[0].Position.Z).FirstOrDefault();
                return GetCenterByLimits(map, zpos);
            }
            else
            {
                return GetCenterBySpawns(map);
            }
        }

        private static async Task<bool> AddInfos(this MapDto map, string mapfilename)
        {
            string path = SettingsManager.MapsPath + MapCreator[mapfilename] + "/" + mapfilename + ".xml";
            try
            {
                using (XmlReader reader = XmlReader.Create(path, settings))
                {
                    MapSynedDataDto syncdata = new MapSynedDataDto();
                    map.SyncedData = syncdata;
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "map")
                            {
                                syncdata.Name = reader["name"];
                                if (reader.GetAttribute("type") != null)
                                    syncdata.Type = reader["type"] == "normal" ? EMapType.Normal : EMapType.Bomb;
                            }
                            else if (reader.Name == "english" || reader.Name == "german")
                            {
                                syncdata.Description[reader.Name == "english" ? ELanguage.English : ELanguage.German] = await reader.ReadElementContentAsStringAsync().ConfigureAwait(false);
                            }
                            else if (reader.Name == "limit")
                            {
                                Vector3 pos = new Vector3(reader["x"].ToFloat(), reader["y"].ToFloat(), 0);
                                map.MapLimits.Add(pos);
                            }
                            else if (reader.Name == "center")
                            {
                                map.MapCenter = new Vector3(reader["x"].ToFloat(), reader["y"].ToFloat(), reader["z"].ToFloat());
                            }
                            else if (reader.Name == "bomb")
                            {
                                Vector3 pos = new Vector3(reader["x"].ToFloat(), reader["y"].ToFloat(), reader["z"].ToFloat());
                                map.BombPlantPlaces.Add(pos);
                            }
                            else if (reader.Name.StartsWith("team"))
                            {
                                int teamnumber = Convert.ToInt32(reader.Name.Substring(4));
                                if (map.TeamSpawns.Count < teamnumber)
                                {
                                    map.TeamSpawns.Add(new List<PositionRotationDto>()); 
                                }
                                map.TeamSpawns[teamnumber-1].Add(new PositionRotationDto
                                {
                                    Position = new Vector3(reader["x"].ToFloat(), reader["y"].ToFloat(), reader["z"].ToFloat()),
                                    Rotation = reader["rot"].ToFloat()
                                });
                            }
                        }
                    }
                }
                if (map.MapCenter == null)
                {
                    map.MapCenter = GetCenter(map);
                }
                return true;
            }
            catch (Exception ex)
            {
                Error.Log("Error in Manager.Map.GetMapClass (" + path + ")\n" + ex.ToString(), Environment.StackTrace, null);
                return false;
            }
        }

        public static async Task<MapDto> GetMapClass(string mapname, Arena lobby)
        {
            MapDto map = new MapDto();
            if (await map.AddInfos(MapPathByName[mapname.ToLower()]).ConfigureAwait(false))
            {
                return map;
            }
            return lobby.GetRandomMap();
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
