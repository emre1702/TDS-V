using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using System;

namespace Manager {
	class ResourceStop : Script {
		public ResourceStop ( ) {
			API.onResourceStop += this.OnResourceStop;
		}

		public void OnResourceStop () {
			SaveAllInDatabase ();
			RemoveAllCreated ();
		}

		private async void SaveAllInDatabase () {
			try {
				await Manager.Log.SaveInDatabase ().ConfigureAwait ( false );

				List<Client> players = API.getAllPlayers ();
				for ( int i = 0; i < players.Count; i++ ) {
					await Account.SavePlayerData ( players[i] ).ConfigureAwait ( false );
				}
			} catch ( Exception ex ) {
				API.consoleOutput ( "Error in SaveAllInDatabase:" + ex.Message );
			}
		}

		private void RemoveAllCreated ( ) {
			List<NetHandle> blips = API.getAllBlips ();
			for ( int i = 0; i < blips.Count; i++ )
				API.deleteEntity ( blips[i] );

			List<NetHandle> markers = API.getAllMarkers ();
			for ( int i = 0; i < markers.Count; i++ )
				API.deleteEntity ( markers[i] );

			List<NetHandle> peds = API.getAllPeds ();
			for ( int i = 0; i < peds.Count; i++ )
				API.deleteEntity ( peds[i] );

			List<NetHandle> pickups = API.getAllPickups ();
			for ( int i = 0; i < pickups.Count; i++ )
				API.deleteEntity ( pickups[i] );

			List<NetHandle> vehicles = API.getAllVehicles ();
			for ( int i = 0; i < vehicles.Count; i++ )
				API.deleteEntity ( vehicles[i] );

			List<NetHandle> objects = API.getAllObjects ();
			for ( int i = 0; i < objects.Count; i++ )
				API.deleteEntity ( objects[i] );

		}
	}
}