﻿namespace TDS.server.manager.utility {

	using System;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using logs;
	using player;
    using TDS.server.extend;

    class ResourceStop : Script {

		public ResourceStop () {
		}

        [ServerEvent(Event.ResourceStop)]
		public void OnResourceStop () {
			SaveAllInDatabase ();
			RemoveAllCreated ();
		}

		private void SaveAllInDatabase () {
			try {
				Log.SaveInDatabase ();

				List<Client> players = NAPI.Pools.GetAllPlayers ();
				foreach ( Client player in players ) {
                    player.GetChar ().SaveData ();
                }

                Season.SaveSeason ();
			} catch ( Exception ex ) {
				Log.Error ( ex.ToString() );
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