namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using GTANetworkAPI;

	partial class OldLobby : Script {

		private static readonly Dictionary<Client, NetHandle> playerCamObject = new Dictionary<Client, NetHandle> ();
		private static Dictionary<Client, bool> freecamControlsDisabled = new Dictionary<Client, bool> ();

		private void StartPlayerFreecam ( Client player ) {
			if ( playerCamObject.ContainsKey ( player ) )
				StopPlayerFreecam ( player );
			NetHandle obj = API.CreateObject ( -1358020705, API.GetEntityPosition ( player ), new Vector3 ( 0.0, 0.0, 0.0 ) ); //We create the object from server side, so later if we want to sync the object's position we can send the pos back here.
			playerCamObject[player] = obj;

			API.TriggerClientEvent ( player, "startFreecam", obj );

			API.FreezePlayer ( player, true );
			API.SetEntityPosition ( player, new Vector3 ( 0.0, 0.0, 200.0 ) );
			API.SetEntityTransparency ( player, 0 );
		}

		private void StopPlayerFreecam ( Client player, bool finaly = false ) {
			if ( playerCamObject.ContainsKey ( player ) ) {
				API.DeleteEntity ( playerCamObject[player] );
				playerCamObject.Remove ( player );
			}
			if ( finaly )
				API.TriggerClientEvent ( player, "stopFreecam" );
		}

		private void SetPlayerFreecamPos ( Client player, Vector3 to ) {
			if ( playerCamObject.ContainsKey ( player ) )
				API.SetEntityPosition ( playerCamObject[player], to );
		}
	}

}
