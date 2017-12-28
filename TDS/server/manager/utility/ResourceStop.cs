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

				List<Client> players = NAPI.Pools.GetAllPlayers ();
				foreach ( Client player in players ) {
					Account.SavePlayerData ( player );
				}
			} catch ( Exception ex ) {
				Log.Error ( "Error in SaveAllInDatabase:" + ex.Message );
			}
		}

		private void RemoveAllCreated () {
			List<NetHandle> blips = NAPI.Pools.GetAllBlips ();
			foreach ( NetHandle blip in blips )
				NAPI.Entity.DeleteEntity ( blip );

			List<NetHandle> markers = NAPI.Pools.GetAllMarkers ();
			foreach ( NetHandle marker in markers )
                NAPI.Entity.DeleteEntity ( marker );

			List<NetHandle> peds = NAPI.Pools.GetAllPeds ();
			foreach ( NetHandle ped in peds )
                NAPI.Entity.DeleteEntity ( ped );

			List<NetHandle> pickups = NAPI.Pools.GetAllPickups ();
			foreach ( NetHandle pickup in pickups )
                NAPI.Entity.DeleteEntity ( pickup );

			List<NetHandle> vehicles = NAPI.Pools.GetAllVehicles ();
			foreach ( NetHandle vehicle in vehicles )
                NAPI.Entity.DeleteEntity ( vehicle );

			List<NetHandle> objects = NAPI.Pools.GetAllObjects ();
			foreach ( NetHandle obj in objects )
                NAPI.Entity.DeleteEntity ( obj );
		}
	}

}
