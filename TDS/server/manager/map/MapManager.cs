using System.Xml;
using System.Collections.Generic;
using System;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server;
using System.IO;

namespace Manager {
	class Map {
		private static string mapsPath = "resources/TDS/server/maps/";
		public static List<string> mapNames = new List<string> {};
		public static Dictionary<string, List<string>> mapDescriptions = new Dictionary<string, List<string>> ();
		public static Dictionary<string, string> mapByName = new Dictionary<string, string> ();

		public static void MapOnStart () {
			for ( int i = 0; i < Language.languages.Count; i++ ) {
				mapDescriptions[Language.languages[i]] = new List<string> ();
			}

			IEnumerable<string> files = Directory.EnumerateFiles ( mapsPath, "*.xml" );
			Class.Map map = new Class.Map ();
			foreach ( string filepath in files ) {
				string filename = Path.GetFileNameWithoutExtension ( filepath );
				if ( AddInfosToMapClass ( map, filename, out map ) ) {
					if ( map.name != null ) {
						mapNames.Add ( map.name );
						for ( int i = 0; i < Language.languages.Count; i++ ) {
							mapDescriptions[Language.languages[i]].Add ( map.description[Language.languages[i]] );
						}
						mapByName[map.name] = filename;
					} else
						Log.Error ( "Map " + filename + " got no name!" );
				}
			}
		}

		private static bool AddInfosToMapClass ( Class.Map map, string mapname, out Class.Map newmap ) {
			try {
				string path = mapsPath + mapname + ".xml";
				map.type = "arena";
				map.description = new Dictionary<string, string> {
					{ "english", "No info available!" },
					{ "german", "Keine Info verfügbar!" }
				};
				map.teamSpawns = new Dictionary<int, List<Vector3>> ();
				map.teamRots = new Dictionary<int, List<Vector3>> ();
				map.mapLimits = new List<Vector3> ();
				map.created = true;

				XmlReader reader = XmlReader.Create ( path );
				while ( reader.Read () ) {
					if ( reader.NodeType == XmlNodeType.Element ) {
						if ( reader.Name == "map" ) {
							map.name = reader["name"];
							if ( reader.GetAttribute ( "type" ) != null )
								map.type = reader["type"];
						} else if ( reader.Name == "english" || reader.Name == "german" ) {
							map.description[reader.Name] = reader.Value;
						} else if ( reader.Name == "limit" ) {
							Vector3 pos = new Vector3 ( float.Parse ( reader["x"] ), float.Parse ( reader["y"] ), 0 );
							map.mapLimits.Add ( pos );
						} else if ( reader.Name.StartsWith ( "team" ) ) {
							int teamnumber = Convert.ToInt32 ( reader.Name.Substring ( 4 ) );
							if ( !map.teamSpawns.ContainsKey ( teamnumber ) ) {
								map.teamSpawns[teamnumber] = new List<Vector3> ();
								map.teamRots[teamnumber] = new List<Vector3> ();
							}
							Vector3 spawn = new Vector3 ( float.Parse ( reader["x"] ), float.Parse ( reader["y"] ), float.Parse ( reader["z"] ) );
							map.teamSpawns[teamnumber].Add ( spawn );
							Vector3 rot = new Vector3 ( float.Parse ( reader["xrot"] ), float.Parse ( reader["yrot"] ), float.Parse ( reader["zrot"] ) );
							map.teamRots[teamnumber].Add ( rot );
						}
					}
				}
				newmap = map;
				return true;
			} catch ( Exception e ) {
				Log.Error ( "Error in Manager.Map.GetMapClass: " + e.ToString () );
				newmap = map;
				return false;
			}
		}

		public static Class.Map GetMapClass ( string mapname, Class.Lobby lobby ) {
			Class.Map map = new Class.Map ();
			if ( AddInfosToMapClass ( map, mapByName[mapname], out map ) ) {
				return map;
			} else
				return lobby.GetRandomMap ();
		}

		/*private static Map getMapDataOther ( string path ) {
			Map map = new Map ();
			XmlGroup mapdata = API.shared.shared.loadXml ( path );

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


