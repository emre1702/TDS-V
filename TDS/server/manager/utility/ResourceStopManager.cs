using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using System;

namespace Manager {
	class ResourceStop : Script {
		public ResourceStop ( ) {
			SaveAllInDatabase ();
			RemoveAllCreated ();
		}

		private static async void SaveAllInDatabase () {
			try {
				await Manager.Log.SaveInDatabase ().ConfigureAwait ( false );

				List<Client> players = API.shared.getAllPlayers ();
				for ( int i = 0; i < players.Count; i++ ) {
					await Account.SavePlayerData ( players[i] ).ConfigureAwait ( false );
				}
			} catch ( Exception ex ) {
				API.shared.consoleOutput ( "Error in SaveAllInDatabase:" + ex.Message );
			}
		}

		private static void RemoveAllCreated ( ) {
			List<NetHandle> blips = API.shared.getAllBlips ();
			for ( int i = 0; i < blips.Count; i++ )
				API.shared.deleteEntity ( blips[i] );

			List<NetHandle> markers = API.shared.getAllMarkers ();
			for ( int i = 0; i < markers.Count; i++ )
				API.shared.deleteEntity ( markers[i] );

			List<NetHandle> peds = API.shared.getAllPeds ();
			for ( int i = 0; i < peds.Count; i++ )
				API.shared.deleteEntity ( peds[i] );

			List<NetHandle> pickups = API.shared.getAllPickups ();
			for ( int i = 0; i < pickups.Count; i++ )
				API.shared.deleteEntity ( pickups[i] );

			List<NetHandle> vehicles = API.shared.getAllVehicles ();
			for ( int i = 0; i < vehicles.Count; i++ )
				API.shared.deleteEntity ( vehicles[i] );

			List<NetHandle> objects = API.shared.getAllObjects ();
			for ( int i = 0; i < objects.Count; i++ )
				API.shared.deleteEntity ( objects[i] );

		}
	}
}