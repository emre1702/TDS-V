using System.Xml;
using System.Collections.Generic;
using System;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server;


namespace Manager {
	static class Map {
		private static List<string> xmlFilePaths = new List<string> {
			"test"
		};
		private static Random rnd = new Random ();

		private static Class.Map GetMapClass ( string path ) {
			Class.Map map = new Class.Map ();
			XmlReader reader = XmlReader.Create ( path );
			while ( reader.Read () ) {
				if ( reader.NodeType == XmlNodeType.Element ) {
					if ( reader.Name == "map" ) {
						map.name = reader["name"];
						map.type = reader["type"] ?? "arena";
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
			return map;
		}

		public static Class.Map GetRandomMap ( ) {
			int random = rnd.Next ( 0, xmlFilePaths.Count );
			return GetMapClass ( "resources/TDS/server/maps/" + xmlFilePaths[random] + ".xml" );
		}


		/*private static Map getMapDataOther ( string path ) {
			Map map = new Map ();
			XmlGroup mapdata = API.shared.loadXml ( path );

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


