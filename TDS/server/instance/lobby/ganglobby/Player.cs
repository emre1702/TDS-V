using TDS.server.instance.player;

namespace TDS.server.instance.lobby {
    partial class GangLobby {

        public override void AddPlayer ( Character character, bool spectator = false ) {
            base.AddPlayer ( character, spectator );

            character.Player.Freeze ( false );
        }
    }
}
