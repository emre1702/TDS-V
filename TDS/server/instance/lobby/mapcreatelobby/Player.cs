using GTANetworkAPI;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {
    partial class MapCreateLobby {

        public override void AddPlayer ( Character character, bool spectator = false ) {
            base.AddPlayer ( character, spectator );

            NAPI.ClientEvent.TriggerClientEvent ( character.Player, "onClientPlayerJoinMapCreatorLobby" );
            character.Player.Freeze ( false );
        }
    }
}
