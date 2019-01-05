﻿using GTANetworkAPI;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {
    partial class MapCreateLobby {

        public override bool AddPlayer ( Character character, bool spectator = false ) {
			if ( !base.AddPlayer ( character, spectator ) )
				return false;

            NAPI.ClientEvent.TriggerClientEvent ( character.Player, "onClientPlayerJoinMapCreatorLobby" );
            character.Player.Freeze ( false );

			return true;
        }
    }
}