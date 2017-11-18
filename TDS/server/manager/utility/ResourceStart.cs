namespace TDS.server.manager.utility {

	using System;
	using GTANetworkAPI;
	using map;

	class ResourceStart : Script {

		public ResourceStart () {
			this.API.SetGamemodeName ( "TDS" );
			//API.SetMapName ( "Los Santos" );
			this.StartMethods ();
		}

		private async void StartMethods () {
			try {
				await Map.MapOnStart ().ConfigureAwait ( false );
			} catch ( Exception ex ) {
				this.API.ConsoleOutput ( "Error in StartMethods:" + ex.Message );
			}
		}
	}

}
