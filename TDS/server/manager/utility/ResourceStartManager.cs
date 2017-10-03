using System;
using GrandTheftMultiplayer.Server.API;

namespace Manager {
	class ResourceStart : Script {
		public ResourceStart ( ) {
			API.setGamemodeName ( "TDS" );
			API.setMapName ( "Los Santos" );
			StartMethods ();
		}

		private async void StartMethods () {
			try { 
				await Manager.Map.MapOnStart ().ConfigureAwait ( false );
			} catch ( Exception ex ) {
				API.consoleOutput ( "Error in StartMethods:" + ex.Message );
			}
		}
	}
}