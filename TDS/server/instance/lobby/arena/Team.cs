using GTANetworkAPI;
using System.Collections.Generic;

namespace TDS.server.instance.lobby {

    partial class Arena {

        public override void AddTeam ( string name, PedHash hash, string colorstring = "s" ) {
            base.AddTeam ( name, hash, colorstring );

            spawnCounter[(uint) teamSkins.Count - 1] = 0;
        }
    }
}
