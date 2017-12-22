namespace TDS.server.manager.utility {

	using System;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using logs;
	using player;

	class ResourceStop : Script {

		public ResourceStop () {
			Event.OnResourceStop += OnResourceStop;
		}

		public void OnResourceStop () {
			SaveAllInDatabase ();
			RemoveAllCreated ();
		}

		private async void SaveAllInDatabase () {
			try {
				await Log.SaveInDatabase ().ConfigureAwait ( false );

				List<Client> players = API.GetAllPlayers ();
				foreach ( Client player in players ) {
					await Account.SavePlayerData ( player ).ConfigureAwait ( false );
				}
			} catch ( Exception ex ) {
				Log.Error ( "Error in SaveAllInDatabase:" + ex.Message );
			}
		}

		private void RemoveAllCreated () {
			List<NetHandle> blips = API.GetAllBlips ();
			foreach ( NetHandle blip in blips )
				API.DeleteEntity ( blip );

			List<NetHandle> markers = API.GetAllMarkers ();
			foreach ( NetHandle marker in markers )
				API.DeleteEntity ( marker );

			List<NetHandle> peds = API.GetAllPeds ();
			foreach ( NetHandle ped in peds )
				API.DeleteEntity ( ped );

			List<NetHandle> pickups = API.GetAllPickups ();
			foreach ( NetHandle pickup in pickups )
				API.DeleteEntity ( pickup );

			List<NetHandle> vehicles = API.GetAllVehicles ();
			foreach ( NetHandle vehicle in vehicles )
				API.DeleteEntity ( vehicle );

			List<NetHandle> objects = API.GetAllObjects ();
			foreach ( NetHandle obj in objects )
				API.DeleteEntity ( obj );
		}
	}

}
