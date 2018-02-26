using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TDS.server.extend;
using TDS.server.manager.database;

namespace TDS.server.manager.map {

    static partial class Map {
    
        private static Dictionary<string, uint[]> mapRatingDict = new Dictionary<string, uint[]> ();
        private static Dictionary<uint, Dictionary<string, uint>> playerMapRating = new Dictionary<uint, Dictionary<string, uint>> ();


        public static void AddPlayerMapRating ( Client player, string mapname, uint rating ) {
            if ( !DoesMapNameExist ( mapname ) )
                return;

            uint uid = player.GetChar ().UID;
            if ( !playerMapRating.ContainsKey ( uid ) )
                playerMapRating[uid] = new Dictionary<string, uint> ();
            if ( !playerMapRating[uid].ContainsKey ( mapname ) )
                Database.Exec ( $"INSERT INTO playermaprating (uid, mapname, rating) VALUES ({uid}, '{mapname}', {rating});" );
            else {
                Database.Exec ( $"UPDATE playermaprating SET rating = {rating} WHERE uid = {uid} AND mapname = '{mapname}';" );
                RemoveRatingFromMap ( mapname, playerMapRating[uid][mapname] );
            }
            AddRatingToMap ( mapname, rating );
            playerMapRating[uid][mapname] = rating;
        }

        public static async void LoadMapRatingsFromDatabase ( ) {
            DataTable result = await Database.ExecResult ( "SELECT * FROM playermaprating" );
            foreach ( DataRow row in result.Rows ) {
                uint uid = Convert.ToUInt32 ( row["uid"] );
                string mapname = row["mapname"].ToString ();
                uint rating = Convert.ToUInt32 ( row["rating"] );
                if ( !playerMapRating.ContainsKey ( uid ) )
                    playerMapRating[uid] = new Dictionary<string, uint> ();
                playerMapRating[uid][mapname] = rating;
                AddRatingToMap ( mapname, rating );
            }          
        }

        private static void AddRatingToMap ( string mapname, uint rating ) {
            if ( !mapRatingDict.ContainsKey ( mapname ) )
                mapRatingDict[mapname] = new uint[] { 0, 0, 0, 0, 0 };
            ++mapRatingDict[mapname][rating-1];
        }

        private static void RemoveRatingFromMap ( string mapname, uint rating ) {
            if ( !mapRatingDict.ContainsKey ( mapname ) )
                return;
            --mapRatingDict[mapname][rating-1];
        }

        public static void SendPlayerHisRatings ( Client player ) {
            uint uid = player.GetChar ().UID;
            if ( playerMapRating.ContainsKey ( uid ) )
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientLoadOwnMapRatings", JsonConvert.SerializeObject ( playerMapRating[uid] ) );
        }

    }
}