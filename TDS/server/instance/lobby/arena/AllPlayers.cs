using GTANetworkAPI;
using System.Collections.Generic;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby
{
    partial class Arena {

        private void SetAllPlayersInCountdown ( ) {
            spectatingMe.Clear ();
            FuncIterateAllPlayers ( ( character, teamID ) => {
                SetPlayerReadyForRound ( character );
                NAPI.ClientEvent.TriggerClientEvent ( character.Player, "onClientCountdownStart" );
                if ( teamID == 0 )
                    SpectateAllTeams ( character );
            } );
            if ( currentMap.SyncData.Type == enums.MapType.BOMB )
                GiveBombToRandomTerrorist ();
        }

        private void SendPlayerAmountInFightInfo ( Client player ) {
            List<uint> amountinteams = new List<uint> ();
            List<uint> amountaliveinteams = new List<uint> ();
            for ( int i = 1; i < Players.Count; i++ ) {
                amountinteams.Add ( (uint) Players[i].Count );
                amountaliveinteams.Add ( (uint) alivePlayers[i].Count );
            }
            PlayerAmountInFightSync ( player, amountinteams, amountaliveinteams );
        }
    }
}
