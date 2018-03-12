using System;
using System.Data;
using System.Threading.Tasks;
using TDS.server.manager.database;

namespace TDS.server.manager.utility {
    class Season {

        private const int stopSeasonAfterMonths = 1;

		class SeasonTops {
			public uint Killer = 0;
			public uint Kills = 0;
			public uint Assister = 0;
			public uint Assists = 0;
			public uint Damager = 0;
			public uint Damage = 0;
			public uint KDer = 0;
			public float KD = 0;
		}

		private async static Task<SeasonTops> GetSeasonTopUIDs ( ) {
			SeasonTops tops = new SeasonTops ();

			DataTable topkillertable = await Database.ExecResult ( "SELECT uid, arenakills FROM playerarenastats ORDER BY arenakills DESC LIMIT 1;" );
			if ( topkillertable.Rows.Count > 0 ) {
				DataTable topassistertable = await Database.ExecResult ( "SELECT uid, arenaassists FROM playerarenastats ORDER BY arenaassists DESC LIMIT 1;" );
				DataTable topdamagertable = await Database.ExecResult ( "SELECT uid, arenadamage FROM playerarenastats ORDER BY arenadamage DESC LIMIT 1;" );
				DataTable topkdtable = await Database.ExecResult ( "SELECT uid, arenakills/arenadeaths as kd FROM playerarenastats ORDER BY arenakills/arenadeaths DESC LIMIT 1;" );

				tops.Killer = Convert.ToUInt32 ( topkillertable.Rows[0]["uid"] );
				tops.Assister = Convert.ToUInt32 ( topassistertable.Rows[0]["uid"] );
				tops.Damager = Convert.ToUInt32 ( topdamagertable.Rows[0]["uid"] );
				tops.KDer = Convert.ToUInt32 ( topkdtable.Rows[0]["uid"] );

				tops.Kills = Convert.ToUInt32 ( topkillertable.Rows[0]["arenakills"] );
				tops.Assists = Convert.ToUInt32 ( topassistertable.Rows[0]["arenaassists"] );
				tops.Damage = Convert.ToUInt32 ( topdamagertable.Rows[0]["arenadamage"] );
				tops.KD = Convert.ToSingle ( topkdtable.Rows[0]["arenakills/arenadeaths"] );
			}
			return tops;
		}

        private async static void EndSeason ( DataRow row, int currentmonth ) {
			SeasonTops tops = await GetSeasonTopUIDs ();

			Database.Exec ( $"INSERT INTO season (month, topkiller, topkills, topassister, topassists, topdamager, topdamage, topkder, topkd) VALUES " +
				$"({currentmonth}, {tops.Killer}, {tops.Kills}, {tops.Assister}, {tops.Assists}, {tops.Damager}, {tops.Damage}, {tops.KDer}, {tops.KD});" );

            // Load kills and deaths of all players and reward them here //

            Database.Exec ( "UPDATE playerarenastats SET currentkills = 0, currentassists = 0, currentdeaths = 0,  currentdamage = 0" );
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
            DataTable result = await Database.ExecResult ( "SELECT month FROM season ORDER BY id DESC LIMIT 1" ).ConfigureAwait ( false );
            if ( result.Rows.Count > 0 ) {
                DataRow row = result.Rows[0];
                if ( DidSeasonEnd ( row, currentmonth ) )
                    EndSeason ( row, currentmonth );
                else
                    LoadSeasonData ( row );
            } else
                Database.Exec ( $"INSERT INTO season (month) VALUES ({currentmonth})" );

            
        }
    }
}
