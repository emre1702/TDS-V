using System.Xml;
using System.Collections.Generic;
using System;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.API;
using System.IO;
using Manager;

namespace Manager {
	static class Map {
		private static string mapsPath = "resources/TDS/server/maps/";
		public static List<string> normalMapNames = new List<string> ();
		public static List<string> hostageMapNames = new List<string> ();
		public static Dictionary<string, List<string>> mapDescriptions = new Dictionary<string, List<string>> {
			{ "english", new List<string>() },
			{ "german", new List<string>() },
		};
		public static Dictionary<string, string> mapByName = new Dictionary<string, string> ();
		public static Dictionary<string, string> mapCreator = new Dictionary<string, string> ();

		public static void MapOnStart () {
			IEnumerable<string> directories = Directory.EnumerateDirectories ( mapsPath );
			Class.Map map = new Class.Map ();
			foreach ( string dir in directories ) {
				string creator = Path.GetFileName ( dir );
				IEnumerable<string> files = Directory.EnumerateFiles ( dir, "*.xml" );
				foreach ( string filepath in files ) {
					string filename = Path.GetFileNameWithoutExtension ( filepath );
					mapCreator[filename] = creator;
					if ( map.AddInfos ( filename ) ) {
						if ( map.name != null ) {
							if ( map.type == "normal" )
								normalMapNames.Add ( map.name );
							else if ( map.type == "hostage" )
								hostageMapNames.Add ( map.name );
							mapDescriptions["english"].Add ( map.description["english"] );
							mapDescriptions["german"].Add ( map.description["german"] );
							mapByName[map.name] = filename;
						} else
							Log.Error ( "Map " + filename + " got no name!" );
					}
				}
			}
		}

		private static bool AddInfos ( this Class.Map map, string mapfilename ) {
			string path = mapsPath + mapCreator[mapfilename] + "/" + mapfilename + ".xml";
			try {
				using ( XmlReader reader = XmlReader.Create ( path ) ) {
					while ( reader.Read () ) {
						if ( reader.NodeType == XmlNodeType.Element ) {
							if ( reader.Name == "map" ) {
								map.name = reader["name"];
								if ( reader.GetAttribute ( "type" ) != null )
									map.type = reader["type"];
							} else if ( reader.Name == "english" || reader.Name == "german" ) {
								map.description[reader.Name] = reader.ReadString ();
							} else if ( reader.Name == "limit" ) {
								Vector3 pos = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), 0 );
								map.mapLimits.Add ( pos );
							} else if ( reader.Name == "middle" ) {
								map.mapmid = new Vector3 ( reader["x"].ToFloat (), reader["y"].ToFloat (), reader["z"].ToFloat () );
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
				return true;
			} catch ( Exception e ) {
				Log.Error ( "Error in Manager.Map.GetMapClass: " + e.ToString ()+" ("+path+")" );
				return false;
			}
		}

		public static Class.Map GetMapClass ( string mapname, Class.Lobby lobby ) {
			Class.Map map = new Class.Map ();
			if ( map.AddInfos ( mapByName[mapname] ) ) {
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


