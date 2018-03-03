namespace TDS.server.manager.utility {

    using System;
    using GTANetworkAPI;
    using map;
    using TDS.server.instance.lobby.ganglobby;
    using TDS.server.manager.database;
    using TDS.server.manager.lobby;

    class ResourceStart : Script {

		public ResourceStart () {
			NAPI.Server.SetGamemodeName ( "tdm" );
            //NAPI.Server.SetAutoRespawnAfterDeath ( false );
            //NAPI.Server.SetGlobalServerChat ( false );
            StartMethods ();
        }

		private async void StartMethods () {
			try {
                await Database.LoadConnStr ();
                Map.LoadMapRatingsFromDatabase ();
                await Map.MapOnStart().ConfigureAwait ( false );
                Gang.LoadGangFromDatabase ();
                NAPI.Task.Run ( () => { 
                    MainMenu.Create ();
                    Arena.Create ();
                    GangLobby.Create ();
                } );
                Season.LoadSeason ();
            } catch ( Exception ex ) {
				NAPI.Util.ConsoleOutput ( ex.ToString() );
			}
		}
	}

}
