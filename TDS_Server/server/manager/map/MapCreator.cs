using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TDS.server.extend;
using TDS.server.instance.map;
using TDS.server.manager.logs;
using TDS.server.manager.utility;

namespace TDS.server.manager.map {

    static partial class Map {

        private const string newMapsPath = "bridge/resources/TDS-V/newmaps/";

        private static string GetXmlStringByMap ( CreatedMap map, uint playeruid ) {
            StringBuilder builder = new StringBuilder ();
            builder.AppendLine ( "<MapData>" )
                .AppendLine ( "\t<map creator='" + playeruid + "' name='" + map.Name + "' type='" + map.Type + "' minplayers='" + map.MinPlayers + "' maxplayers='" + map.MaxPlayers + "' />" )
                .AppendLine ( "\t<english>" + map.Descriptions.English + "</english>" )
                .AppendLine ( "\t<german>" + map.Descriptions.German + "</german>" );

            if ( map.MapCenter != null ) {
                builder.AppendLine ( "\t<center x='" + map.MapCenter.X + "' y='" + map.MapCenter.Y + "' z='" + map.MapCenter.Z + "' />" );
            }

            if ( map.Type == "bomb" ) {
                for ( int i = 0; i < map.BombPlaces.Length; ++i ) {
                    Position pos = map.BombPlaces[i];
                    builder.AppendLine ( "\t<bomb x='" + pos.X + "' y='" + pos.Y + "' z='" + ( pos.Z - 1 ) + "' />" );
                }
            }

            for ( int i = 0; i < map.MapSpawns.Length; ++i ) {
                TeamSpawn spawn = map.MapSpawns[i];
                builder.AppendLine ( "\t<team" + spawn.Team + " x='" + spawn.X + "' y='" + spawn.Y + "' z='" + spawn.Z + "' rot='" + spawn.Rot + "' />" );
            }

            for ( int i = 0; i < map.MapLimitPositions.Length; ++i ) {
                Position pos = map.MapLimitPositions[i];
                builder.AppendLine ( "\t<limit x='" + pos.X + "' y='" + pos.Y + "' z='" + pos.Z + "' />" );
            }

            builder.AppendLine ( "</MapData>" );
            return builder.ToString ();
        }

        public static async void CreateNewMap ( string content, uint playeruid ) {
            try {
                CreatedMap map = JsonConvert.DeserializeObject<CreatedMap> ( content );
                string path = newMapsPath + playeruid + "/";
                Directory.CreateDirectory ( path );
                using ( StreamWriter writer = File.CreateText ( path + Utility.GetTimespan () + ".xml" ) ) {
                    await writer.WriteAsync ( GetXmlStringByMap ( map, playeruid ) );
                }
            } catch ( Exception ex ) {
                Log.Error ( ex.ToString (), "MapCreator" );
            }
        }

        public static bool DoesMapNameExist ( string mapname ) {
            return MapPathByName.ContainsKey ( mapname.ToLower () );
        }

        private static List<string> GetAllNewMapFileNames ( ) {
            List<string> files = new List<string> ();
            string[] subdirectories = Directory.GetDirectories ( newMapsPath );
            foreach ( string directory in subdirectories ) {
                string uid = Path.GetDirectoryName ( directory );
                string[] filesinpath = Directory.GetFiles ( directory );
                foreach ( string file in filesinpath ) {
                    string filename = Path.GetFileNameWithoutExtension ( file );
                    files.Add ( uid + "/" + filename );
                }
            }
            return files;
        }

        private static List<string> GetOwnNewMapFileNames ( uint uid ) {
            List<string> files = new List<string> ();
            if ( Directory.Exists ( newMapsPath + uid + "/" ) ) {
                string[] filesinpath = Directory.GetFiles ( newMapsPath + uid + "/" );
                foreach ( string file in filesinpath ) { 
                    string filename = Path.GetFileNameWithoutExtension ( file );
                    files.Add ( uid + "/" + filename );
                }
            }
            return files;
        } 

        public static void RequestNewMapsList ( Client player, bool requestall = false ) {
            uint uid = player.GetChar ().UID;
            List<string> filenames; 
            if ( requestall )
                filenames = GetAllNewMapFileNames ();
            else
                filenames = GetOwnNewMapFileNames ( uid );
            NAPI.ClientEvent.TriggerClientEvent ( player, "requestNewMapsList", JsonConvert.SerializeObject ( filenames ) );

        }
    }
}
