namespace TDS.server.manager.utility {

    using System;
    using GTANetworkAPI;
    using map;
    using TDS.server.manager.lobby;

    class ResourceStart : Script {

		public ResourceStart () {
			NAPI.Server.SetGamemodeName ( "tdm" );
            NAPI.Server.SetAutoRespawnAfterDeath ( false );
            NAPI.Server.SetGlobalServerChat ( false );
            StartMethods ();
        }

		private async void StartMethods () {
			try {
                await Map.MapOnStart().ConfigureAwait ( false );
                NAPI.Task.Run ( () => { 
                    MainMenu.Create ();
                    Arena.Create ();
                } );
                Season.LoadSeason ();
            } catch ( Exception ex ) {
				NAPI.Util.ConsoleOutput ( ex.ToString() );
			}
		}
	}

}
