﻿using GTANetworkAPI;
using System.Collections.Generic;

namespace TDS.server.instance.lobby {

    partial class Arena {

        public override void AddTeam ( string name, PedHash hash, string colorstring = "s" ) {
            base.AddTeam ( name, hash, colorstring );

            spawnCounter[(uint) teamSkins.Count - 1] = 0;
        }


        /// <summary>
        /// Returns the amount of teams with enough players alive.
        /// Used to check if round ended.
        /// </summary>
        /// <param name="minalive">Amount of alive players needed in a team to get considered as "still in round".</param>
        /// <returns>Amount teams still in round.</returns>
        private int GetTeamAmountStillInRound ( int minalive = 1 ) {
            int amount = 0;
            for ( int i = 1; i < alivePlayers.Count; i++ )
                if ( alivePlayers[i].Count >= minalive )
                    amount++;
            return amount;
        }
    }
}
