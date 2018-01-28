using System;
using System.Data;
using System.Threading.Tasks;
using TDS.server.manager.database;

namespace TDS.server.manager.utility {
    class Season {

        private const int stopSeasonAfterMonths = 1;

        private static void EndSeason ( DataRow row, int currentmonth ) {
            Database.Exec ( "INSERT INTO season (month) VALUES (" + currentmonth + ")" );
        }

        public static void SaveSeason ( ) {

        }

        private static bool DidSeasonEnd ( DataRow row, int currentmonth ) {
            int lastmonth = Convert.ToInt32 ( row["month"] );
            if ( currentmonth < lastmonth )
                currentmonth += 12;
            if ( currentmonth - lastmonth >= stopSeasonAfterMonths ) {
                return true;
            }   
            return false;
        }

        private static void LoadSeasonData ( DataRow row ) {

        }


        public async static void LoadSeason ( ) {
            int currentmonth = DateTime.Now.Month;
            DataTable result = await Database.ExecResult ( "SELECT * FROM season ORDER BY id DESC LIMIT 1" ).ConfigureAwait ( false );
            if ( result.Rows.Count > 0 ) {
                DataRow row = result.Rows[0];
                if ( DidSeasonEnd ( row, currentmonth ) )
                    EndSeason ( row, currentmonth );
                else
                    LoadSeasonData ( row );
            } else
                Database.Exec ( "INSERT INTO season (month) VALUES (" + currentmonth + ")" );

            
        }
    }
}
