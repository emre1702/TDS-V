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

		private void SaveAllInDatabase () {
			try {
				Log.SaveInDatabase ();

				List<Client> players = NAPI.Pools.GetAllPlayers ();
				foreach ( Client player in players ) {
					Account.SavePlayerData ( player );
				}

                Season.SaveSeason ();
			} catch ( Exception ex ) {
				Log.Error ( "Error in SaveAllInDatabase:" + ex.Message );
			}
		}

		private void RemoveAllCreated () {
			 List<Blip> blips = NAPI.Pools.GetAllBlips ();
			foreach ( Blip blip in blips )
				NAPI.Entity.DeleteEntity ( blip );

			List<Marker> markers = NAPI.Pools.GetAllMarkers ();
			foreach ( Marker marker in markers )
                NAPI.Entity.DeleteEntity ( marker );

			//List<NetHandle> peds = NAPI.Pools.GetAllPeds ();
			//foreach ( NetHandle ped in peds )
            //    NAPI.Entity.DeleteEntity ( ped ); 

			List<Pickup> pickups = NAPI.Pools.GetAllPickups ();
			foreach ( Pickup pickup in pickups )
                NAPI.Entity.DeleteEntity ( pickup );

			List<Vehicle> vehicles = NAPI.Pools.GetAllVehicles ();
			foreach ( Vehicle vehicle in vehicles )
                NAPI.Entity.DeleteEntity ( vehicle );

			List<GTANetworkAPI.Object> objects = NAPI.Pools.GetAllObjects ();
			foreach ( GTANetworkAPI.Object obj in objects )
                NAPI.Entity.DeleteEntity ( obj );  
		}
	}

}
