namespace TDS.server.manager.utility {

    using System;
    using System.Threading.Tasks;
    using GTANetworkAPI;
    using map;
    using TDS.server.manager.lobby;

    class ResourceStart : Script {

		public ResourceStart () {
			NAPI.Server.SetGamemodeName ( "TDS" );
            StartMethods ();
        }

		private async void StartMethods () {
			try {
                await Map.MapOnStart().ConfigureAwait ( false );
                NAPI.Task.Run ( () => { 
                    MainMenu.Create ();
                    Arena.Create ();
                } );
            } catch ( Exception ex ) {
				NAPI.Util.ConsoleOutput ( "Error in StartMethods:" + ex.Message );
			}
		}
	}

}
