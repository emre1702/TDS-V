using GTANetworkAPI;
using System.Collections.Generic;

namespace TDS.server.instance.lobby
{
    partial class Arena {

        private void SetAllPlayersInCountdown ( ) {
            spectatingMe = new Dictionary<NetHandle, List<NetHandle>> ();
            FuncIterateAllPlayers ( ( playerhandle, teamID ) => {
                Client player = NAPI.Player.GetPlayerFromHandle ( playerhandle );
                SetPlayerReadyForRound ( player, (uint) teamID );
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientCountdownStart", currentMap.Name );
                if ( teamID == 0 )
                    SpectateAllTeams ( player );
            } );
            if ( currentMap.Type == enums.MapType.BOMB )
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
