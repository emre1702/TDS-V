namespace TDS.server.manager.map {

	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
    using System.Threading.Tasks;
	using System.Xml;
	using GTANetworkAPI;
	using instance.lobby;
	using logs;
    using TDS.server.enums;
    using utility;

	static partial class Map {

		private const string mapsPath = "bridge/resources/TDS-V/maps/";
        
        private static readonly XmlReaderSettings settings = new XmlReaderSettings ();

        public static List<instance.map.Map> allMaps = new List<instance.map.Map> ();
        public static List<instance.map.MapSync> allMapsSync = new List<instance.map.MapSync> ();

		public static ConcurrentDictionary<string, string> MapPathByName = new ConcurrentDictionary<string, string> ();   // mapnames in lower case
		public static ConcurrentDictionary<string, string> MapCreator = new ConcurrentDictionary<string, string> ();

		public static async Task MapOnStart () {
			settings.Async = true;
			IEnumerable<string> directories = Directory.EnumerateDirectories ( mapsPath );
			foreach ( string dir in directories ) {
				string creator = Path.GetFileName ( dir );
				IEnumerable<string> files = Directory.EnumerateFiles ( dir, "*.xml" );
				foreach ( string filepath in files ) {
					string filename = Path.GetFileNameWithoutExtension ( filepath );
					MapCreator[filename] = creator;
                    instance.map.Map map = new instance.map.Map ();
                    if ( await map.AddInfos ( filename ).ConfigureAwait ( false ) ) {
						if ( map.SyncData.Name != null ) {
                            allMaps.Add ( map );
                            allMapsSync.Add ( map.SyncData );

                            MapPathByName[map.SyncData.Name.ToLower()] = filename;
						} else
							Log.Error ( "Map " + filename + " got no name!" );
					}
				}
			}
		}

		private static Vector3 GetCenterOfPositions ( List<Vector3> poly, float zpos = -1 ) {
            int length = poly.Count;
            if ( length <= 2 )
                return null;

            float centerX = 0.0f;
			float centerY = 0.0f;
			float centerZ = 0.0f;
			

			foreach ( Vector3 point in poly ) {
				centerX += point.X;
				centerY += point.Y;
				centerZ += Math.Abs ( zpos - ( -1 ) ) < 0.001 ? point.Z : zpos;
			}

			return new Vector3 ( centerX / length, centerY / length, centerZ / length );
		}

		private static Vector3 GetCenterByLimits ( instance.map.Map map, float zpos ) {
			return GetCenterOfPositions ( map.MapLimits, zpos ) ?? GetCenterBySpawns ( map );
		}

		private static Vector3 GetCenterBySpawns ( instance.map.Map map ) {
			int amountteams = map.TeamSpawns.Count;
			if ( amountteams == 1 ) {
				foreach ( KeyValuePair<int, List<Vector3>> entry in map.TeamSpawns ) {
					return entry.Value[0];
				}
			} else if ( amountteams > 1 ) {
				List<Vector3> positions = map.TeamSpawns.Select ( entry => entry.Value[0] ).ToList ();
				return GetCenterOfPositions ( positions );
			}
			return new Vector3 ();
		}

        private static Vector3 GetCenter ( instance.map.Map map ) {
            if ( map.MapLimits.Count > 0 ) {
                float zpos = map.TeamSpawns.Select ( entry => entry.Value[0].Z ).FirstOrDefault ();
                return GetCenterByLimits ( map, zpos );
            } else {
                return GetCenterBySpawns ( map );
            }
        }

		private static async Task<bool> AddInfos ( this instance.map.Map map, string mapfilename ) {
			string path = mapsPath + MapCreator[mapfilename] + "/" + mapfilename + ".xml";
			try {
				using ( XmlReader reader = XmlReader.Create ( path, settings ) ) {
                    instance.map.MapSync syncdata = new instance.map.MapSync ();
                    map.SyncData = syncdata;
                    while ( await reader.ReadAsync ().ConfigureAwait ( false ) ) {
						if ( reader.NodeType == XmlNodeType.Element ) {
							if ( reader.Name == "map" ) {
                                syncdata.Name = reader["name"];
								if ( reader.GetAttribute ( "type" ) != null )
                                    syncdata.Type = reader["type"] == "normal" ? MapType.NORMAL : MapType.BOMB;
							} else if ( reader.Name == "english" || reader.Name == "german" ) {
                                syncdata.Description[reader.Name == "english" ? Language.ENGLISH : Language.GERMAN] = await reader.ReadElementContentAsStringAsync ().ConfigureAwait ( false );
							} else if ( reader.Name == "limit" ) {
								Vector3 pos = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), 0 );
								map.MapLimits.Add ( pos );
							} else if ( reader.Name == "center" ) {
								map.MapCenter = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), reader["z"].ToFloat () );
							} else if ( reader.Name == "bomb" ) {
								Vector3 pos = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), reader["z"].ToFloat () );
								map.BombPlantPlaces.Add ( pos );
							} else if ( reader.Name.StartsWith ( "team" ) ) {
								int teamnumber = Convert.ToInt32 ( reader.Name.Substring ( 4 ) );
								if ( !map.TeamSpawns.ContainsKey ( teamnumber ) ) {
									map.TeamSpawns[teamnumber] = new List<Vector3> ();
									map.TeamRots[teamnumber] = new List<Vector3> ();
								}
								Vector3 spawn = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), reader["z"].ToFloat () );
								map.TeamSpawns[teamnumber].Add ( spawn );
								Vector3 rot = new Vector3 ( 0, 0, reader["rot"].ToFloat () );
								map.TeamRots[teamnumber].Add ( rot );
							}
						}
					}
				}
				if ( map.MapCenter == null ) {
                    map.MapCenter = GetCenter ( map );
				}
				return true;
			} catch ( Exception ex ) {
				Log.Error ( "Error in Manager.Map.GetMapClass (" + path + ")\n"+ex.ToString() );
				return false;
			}
		}

		public static async Task<instance.map.Map> GetMapClass ( string mapname, Arena lobby ) {
			instance.map.Map map = new instance.map.Map ();
			if ( await map.AddInfos ( MapPathByName[mapname.ToLower()] ).ConfigureAwait ( false ) ) {
				return map;
			}
            return lobby.GetRandomMap ();
		}

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
