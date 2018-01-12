/*namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using GTANetworkAPI;

	partial class OldLobby : Script {

		private static readonly Dictionary<NetHandle, NetHandle> playerCamObject = new Dictionary<NetHandle, NetHandle> ();
		private static Dictionary<NetHandle, bool> freecamControlsDisabled = new Dictionary<NetHandle, bool> ();

		private void StartPlayerFreecam ( Client player ) {
			if ( playerCamObject.ContainsKey ( player ) )
				StopPlayerFreecam ( player );
			Object obj = NAPI.Object.CreateObject ( -1358020705, NAPI.Entity.GetEntityPosition ( player ), new Vector3 ( 0.0, 0.0, 0.0 ) ); //We create the object from server side, so later if we want to sync the object's position we can send the pos back here.
			playerCamObject[player] = obj;

			NAPI.ClientEvent.TriggerClientEvent ( player, "startFreecam", obj.Handle );

			NAPI.Player.FreezePlayer ( player, true );
			NAPI.Entity.SetEntityPosition ( player, new Vector3 ( 0.0, 0.0, 200.0 ) );
			NAPI.Entity.SetEntityTransparency ( player, 0 );
		}

		private void StopPlayerFreecam ( Client player, bool finaly = false ) {
			if ( playerCamObject.ContainsKey ( player ) ) {
				NAPI.Entity.DeleteEntity ( playerCamObject[player] );
				playerCamObject.Remove ( player );
			}
			if ( finaly )
				NAPI.ClientEvent.TriggerClientEvent ( player, "stopFreecam" );
		}

		private void SetPlayerFreecamPos ( Client player, Vector3 to ) {
			if ( playerCamObject.ContainsKey ( player ) )
				NAPI.Entity.SetEntityPosition ( playerCamObject[player], to );
		}
	}

}         */
