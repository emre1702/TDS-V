namespace TDS.server.manager.utility {

	using System;
	using GTANetworkAPI;
	using map;

	class ResourceStart : Script {

		public ResourceStart () {
			NAPI.Server.SetGamemodeName ( "TDS" );
			//API.SetMapName ( "Los Santos" );
			StartMethods ();
		}

		private async void StartMethods () {
			try {
				await Map.MapOnStart ().ConfigureAwait ( false );
			} catch ( Exception ex ) {
				NAPI.Util.ConsoleOutput ( "Error in StartMethods:" + ex.Message );
			}
		}
	}

}
