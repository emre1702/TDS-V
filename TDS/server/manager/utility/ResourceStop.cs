namespace TDS.server.manager.utility {

	using System;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using logs;
	using player;

	class ResourceStop : Script {

		public ResourceStop () {
			this.API.OnResourceStop += this.OnResourceStop;
		}

		public void OnResourceStop () {
			this.SaveAllInDatabase ();
			this.RemoveAllCreated ();
		}

		private async void SaveAllInDatabase () {
			try {
				await Log.SaveInDatabase ().ConfigureAwait ( false );

				List<Client> players = this.API.GetAllPlayers ();
				foreach ( Client player in players ) {
					await Account.SavePlayerData ( player ).ConfigureAwait ( false );
				}
			} catch ( Exception ex ) {
				Log.Error ( "Error in SaveAllInDatabase:" + ex.Message );
			}
		}

		private void RemoveAllCreated () {
			List<NetHandle> blips = this.API.GetAllBlips ();
			foreach ( NetHandle blip in blips )
				this.API.DeleteEntity ( blip );

			List<NetHandle> markers = this.API.GetAllMarkers ();
			foreach ( NetHandle marker in markers )
				this.API.DeleteEntity ( marker );

			List<NetHandle> peds = this.API.GetAllPeds ();
			foreach ( NetHandle ped in peds )
				this.API.DeleteEntity ( ped );

			List<NetHandle> pickups = this.API.GetAllPickups ();
			foreach ( NetHandle pickup in pickups )
				this.API.DeleteEntity ( pickup );

			List<NetHandle> vehicles = this.API.GetAllVehicles ();
			foreach ( NetHandle vehicle in vehicles )
				this.API.DeleteEntity ( vehicle );

			List<NetHandle> objects = this.API.GetAllObjects ();
			foreach ( NetHandle obj in objects )
				this.API.DeleteEntity ( obj );
		}
	}

}
