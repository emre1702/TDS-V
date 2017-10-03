using System.Xml;
using System.Collections.Generic;
using System;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.API;
using System.IO;
using Manager;
using System.Threading.Tasks;

namespace Manager {
	static class Map {
		private static string mapsPath = "resources/TDS/server/maps/";
		private static XmlReaderSettings settings = new XmlReaderSettings ();

		public static List<string> normalMapNames = new List<string> ();
		public static List<string> bombMapNames = new List<string> ();
		public static Dictionary<string, List<string>> normalMapDescriptions = new Dictionary<string, List<string>> {
			{ "english", new List<string>() },
			{ "german", new List<string>() },
		};
		public static Dictionary<string, List<string>> bombMapDescriptions = new Dictionary<string, List<string>> {
			{ "english", new List<string>() },
			{ "german", new List<string>() },
		};
		public static Dictionary<string, string> mapByName = new Dictionary<string, string> ();
		public static Dictionary<string, string> mapCreator = new Dictionary<string, string> ();

		public static async Task MapOnStart () {
			settings.Async = true;
			IEnumerable<string> directories = Directory.EnumerateDirectories ( mapsPath );
			Class.Map map = new Class.Map ();
			foreach ( string dir in directories ) {
				string creator = Path.GetFileName ( dir );
				IEnumerable<string> files = Directory.EnumerateFiles ( dir, "*.xml" );
				foreach ( string filepath in files ) {
					string filename = Path.GetFileNameWithoutExtension ( filepath );
					mapCreator[filename] = creator;
					if ( await map.AddInfos ( filename ).ConfigureAwait ( false ) ) {
						if ( map.name != null ) {
							if ( map.type == "normal" ) {
								normalMapNames.Add ( map.name );
								normalMapDescriptions["english"].Add ( map.description["english"] );
								normalMapDescriptions["german"].Add ( map.description["german"] );
							} else if ( map.type == "bomb" ) {
								bombMapNames.Add ( map.name );
								bombMapDescriptions["english"].Add ( map.description["english"] );
								bombMapDescriptions["german"].Add ( map.description["german"] );
							}
							
							mapByName[map.name] = filename;
						} else
							Log.Error ( "Map " + filename + " got no name!" );
					}
				}
			}
		}

		private static Vector3 GetCenterOfPositions ( List<Vector3> poly, float zpos = -1 ) {
			float centerX = 0.0f;
			float centerY = 0.0f;
			float centerZ = 0.0f;
			int length = poly.Count;

			foreach ( Vector3 point in poly ) { 
				centerX += point.X;
				centerY += point.Y;
				centerZ += zpos == -1 ? point.Z : zpos;
			}

			return new Vector3 ( centerX / length , centerY / length, centerZ / length );
		}

		private static Vector3 GetCenterByLimits ( Class.Map map, float zpos ) {
			return GetCenterOfPositions ( map.mapLimits, zpos ) ?? GetCenterBySpawns ( map );
		}

		private static Vector3 GetCenterBySpawns ( Class.Map map ) {
			int amountteams = map.teamSpawns.Count;
			if ( amountteams == 1 ) {
				foreach ( KeyValuePair<int, List<Vector3>> entry in map.teamSpawns ) {
					return entry.Value[0];
				}
			} else if ( amountteams > 1 ) {
				List<Vector3> positions = new List<Vector3> ();
				foreach ( KeyValuePair<int, List<Vector3>> entry in map.teamSpawns ) {
					positions.Add ( entry.Value[0] );
				}
				return GetCenterOfPositions ( positions );
			}
			return new Vector3 ();
		}

		private static async Task<bool> AddInfos ( this Class.Map map, string mapfilename ) {
			string path = mapsPath + mapCreator[mapfilename] + "/" + mapfilename + ".xml";
			try {
				using ( XmlReader reader = XmlReader.Create ( path, settings ) ) {
					while ( await reader.ReadAsync ().ConfigureAwait ( false ) ) {
						if ( reader.NodeType == XmlNodeType.Element ) {
							if ( reader.Name == "map" ) {
								map.name = reader["name"];
								if ( reader.GetAttribute ( "type" ) != null )
									map.type = reader["type"];
							} else if ( reader.Name == "english" || reader.Name == "german" ) {
								map.description[reader.Name] = await reader.ReadElementContentAsStringAsync ().ConfigureAwait ( false );
							} else if ( reader.Name == "limit" ) {
								Vector3 pos = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), 0 );
								map.mapLimits.Add ( pos );
							} else if ( reader.Name == "middle" ) {
								map.mapCenter = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), reader["z"].ToFloat () );
							} else if ( reader.Name == "bomb" ) {
								Vector3 pos = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), reader["z"].ToFloat () );
								map.bombPlantPlaces.Add ( pos );
							} else if ( reader.Name.StartsWith ( "team" ) ) {
								int teamnumber = Convert.ToInt16 ( reader.Name.Substring ( 4 ) );
								if ( !map.teamSpawns.ContainsKey ( teamnumber ) ) {
									map.teamSpawns[teamnumber] = new List<Vector3> ();
									map.teamRots[teamnumber] = new List<Vector3> ();
								}
								Vector3 spawn = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), reader["z"].ToFloat () );
								map.teamSpawns[teamnumber].Add ( spawn );
								Vector3 rot = new Vector3 ( 0, 0, reader["rot"].ToFloat () );
								map.teamRots[teamnumber].Add ( rot );
							}
						}
					}
				}
				if ( map.mapCenter == null ) {
					if ( map.mapLimits.Count > 0 ) {
						float zpos = 0;
						foreach ( KeyValuePair<int, List<Vector3>> entry in map.teamSpawns ) {
							zpos = entry.Value[0].Z;
							break;
						}
						map.mapCenter = GetCenterByLimits ( map, zpos );
					} else {
						map.mapCenter = GetCenterBySpawns ( map );
					}
				}
				return true;
			} catch ( Exception e ) {
				Log.Error ( "Error in Manager.Map.GetMapClass: " + e.ToString ()+" ("+path+")" );
				return false;
			}
		}

		public static async Task<Class.Map> GetMapClass ( string mapname, Class.Lobby lobby ) {
			Class.Map map = new Class.Map ();
			if ( await map.AddInfos ( mapByName[mapname] ).ConfigureAwait ( false ) ) {
				return map;
			} else
				return await lobby.GetRandomMap ().ConfigureAwait ( false );
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


