using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;

namespace TDS_Server.server.instance.lobby.ganglobby {

    /*public partial class Gang {
        private static Dictionary<uint, Gang> gangByUID = new Dictionary<uint, Gang> ();
        private static Dictionary<uint, Gang> playerMemberOfGang = new Dictionary<uint, Gang> ();

        private uint uid;
        private string name;
        private string shortname;
        private uint owneruid;

        public Gang ( uint uid, string name, string shortname, uint owneruid ) {
            this.uid = uid;
            this.name = name;
            this.shortname = shortname;
            this.owneruid = owneruid;
        }

        public static Gang GetGangByUID ( uint uid ) {
            return gangByUID.ContainsKey ( uid ) ? gangByUID[uid] : null;
        }

        public static async void LoadGangFromDatabase ( ) {
            DataTable gangtable = await Database.ExecResult ( "SELECT * FROM gang" );
            foreach ( DataRow row in gangtable.Rows ) {
                uint uid = Convert.ToUInt32 ( row["uid"] );
                Gang gang = new Gang ( uid, row["name"].ToString(), row["shortname"].ToString (), Convert.ToUInt32 ( row["owneruid"] ) );
                gangByUID[uid] = gang;
            }

            DataTable gangmembertable = await Database.ExecResult ( "SELECT * FROM gangmember" );
            foreach ( DataRow row in gangmembertable.Rows ) {
                uint ganguid = Convert.ToUInt32 ( row["ganguid"] );
                if ( gangByUID.ContainsKey ( ganguid ) ) {
                    Gang gang = gangByUID[ganguid];
                    uint memberuid = Convert.ToUInt32 ( row["memberuid"] );
                    playerMemberOfGang[memberuid] = gang;
                    gang.membersRank[memberuid] = Convert.ToUInt32 ( row["rank"] );
                } else {
                    Log.Error ( $"Gang with uid {ganguid} doesn't exist, but player with uid {row["memberuid"].ToString ()} is in that gang!" );
                }
            }

			DataTable gangvehicletable = await Database.ExecResult ( "SELECT * FROM playervehicles" );
			foreach ( DataRow row in gangvehicletable.Rows ) {
				uint playeruid = Convert.ToUInt32 ( row["uid"] );
				if ( playerMemberOfGang.ContainsKey ( playeruid ) ) {
					string vehicle = Convert.ToString ( row["vehicle"] );
					int amount = Convert.ToInt32 ( row["amount"] );
					playerMemberOfGang[playeruid].AddVehicle ( vehicle, amount );
				}
			}
        }
    }*/
}