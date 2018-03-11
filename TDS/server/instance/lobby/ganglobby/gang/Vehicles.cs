using System;
using System.Collections.Generic;
using System.Data;
using TDS.server.instance.player;
using TDS.server.manager.database;

namespace TDS.server.instance.lobby.ganglobby {

	partial class Gang {

		private Dictionary<string, int> vehicleAmounts = new Dictionary<string, int> ();
		private object vehicleAmountsLocker = new object();

		private void AddVehicle ( string name, int amount ) {
			if ( !vehicleAmounts.ContainsKey ( name ) )
				vehicleAmounts[name] = amount;
			else 
				vehicleAmounts[name] += amount;
		}

		private void RemoveVehicle ( string name, int amount ) {
			if ( vehicleAmounts.ContainsKey ( name ) )
				vehicleAmounts[name] -= amount;
		}

		private async void AddPlayerVehicle ( uint uid  ) {
			DataTable playervehicles = await Database.ExecResult ( $"SELECT * FROM playervehicles WHERE uid = {uid};" );
			foreach ( DataRow row in playervehicles.Rows ) {
				AddVehicle ( row["vehicle"].ToString (), Convert.ToInt32 ( row["amount"] ) );
			}
		}

		private async void RemovePlayerVehicle ( uint uid ) {
			DataTable playervehicles = await Database.ExecResult ( $"SELECT * FROM playervehicles WHERE uid = {uid};" );
			foreach ( DataRow row in playervehicles.Rows ) {
				RemoveVehicle ( row["vehicle"].ToString (), Convert.ToInt32 ( row["amount"] ) );
			}
		}

	}
}