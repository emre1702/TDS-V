﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.server.instance.lobby {
    partial class MapCreateLobby {

        public override void AddPlayer ( Client player, bool spectator = false ) {
            base.AddPlayer ( player, spectator );

            player.TriggerEvent ( "onClientPlayerJoinMapCreatorLobby" );
        }
    }
}
