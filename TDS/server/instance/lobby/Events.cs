using GTANetworkAPI;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {
    class LobbyEvents : Script {

        public LobbyEvents ( ) {
            Event.OnPlayerDisconnected += OnPlayerDisconnected;
            Event.OnPlayerSpawn += OnPlayerSpawn;
            Event.OnPlayerWeaponSwitch += OnPlayerWeaponSwitch;
            Event.OnPlayerEnterColShape += OnPlayerEnterColShape;
        }

        #region Lobby
        [RemoteEvent("joinLobby")]
        private void JoinLobbyEvent ( Client player, int index, bool spectator ) {
            if ( Lobby.SLobbiesByIndex.ContainsKey ( index ) ) {
                Lobby lobby = Lobby.SLobbiesByIndex[index];
                if ( lobby is Arena arenalobby )
                    arenalobby.AddPlayer ( player, spectator );
                else
                    lobby.AddPlayer ( player, spectator );
            } else {
                /* player.sendNotification (  lobby doesn't exist ); */
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientJoinMainMenu" );  //TODO is that needed?
            }
        }
        #endregion

         #region Spectate
        [RemoteEvent("spectateNext")]
        private void SpectateNextEvent ( Client player, bool forward ) {
            Character character = player.GetChar ();
            if ( !( character.Lobby is Arena arena ) )
                return;
            if ( character.Lifes == 0 &&
                ( arena.status == LobbyStatus.ROUND || character.Team == 0 && arena.status == LobbyStatus.COUNTDOWN ) ) {
                if ( character.Team == 0 )
                    arena.SpectateAllTeams ( player, forward );
                else
                    arena.SpectateTeammate ( player, forward );
            }
        }
        #endregion
                
        #region Round
        [RemoteEvent( "onPlayerWasTooLongOutsideMap" )]
        private void TooLongOutsideMapEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.KillPlayer ( player, "too_long_outside_map" );
        }
        #endregion

        #region MapVote
        [RemoteEvent( "onMapsListRequest" )]
        private void OnMapsListRequestEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.SendMapsForVoting ( player );
        }
                
        [RemoteEvent ( "onMapVotingRequest" )]
        private void OnMapVotingRequestEvent ( Client player, string mapname ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.AddMapToVoting ( player, mapname );
        }
        
        [RemoteEvent ( "onVoteForMap" )]
        private void OnVoteForMapEvent ( Client player, string mapname ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.AddVoteToMap ( player, mapname );
        }
        #endregion

        #region Bomb
        [RemoteEvent ( "onPlayerStartPlanting" )]
        private void onPlayerStartPlantingEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StartBombPlanting ( player );
        }
                      
        [RemoteEvent ( "onPlayerStopPlanting" )]
        private void onPlayerStopPlantingEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StopBombPlanting ( player );
        }

        [RemoteEvent ( "onPlayerStartDefusing" )]
        private void onPlayerStartDefusingEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StartBombDefusing ( player );
        }
        
        [RemoteEvent ( "onPlayerStopDefusing" )]
        private void onPlayerStopDefusingEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StopBombDefusing ( player );
        }
        #endregion

        #region Freecam
        /*case "setFreecamObjectPositionTo":
            //player.GetChar ().Lobby.SetPlayerFreecamPos ( player, args[0] );    //TODO
            break; */
        #endregion

        #region Order
        [RemoteEvent ( "onPlayerGiveOrder" )]
        private void OnPlayerGiveOrderEvent ( Client player, string ordershort ) {
            player.GetChar ().Lobby.SendTeamOrder ( player, ordershort );
        }
        #endregion

        private void OnPlayerEnterColShape ( ColShape shape, Client player ) {
            player.GetChar ().Lobby.OnPlayerEnterColShape ( shape, player );
        }

        private void OnPlayerDisconnected ( Client player, byte type, string reason ) {
            player.GetChar ().Lobby.OnPlayerDisconnected ( player, type, reason );
        }

        private void OnPlayerWeaponSwitch ( Client player, WeaponHash oldweapon, WeaponHash newweapon ) {
            Lobby lobby = player.GetChar ().Lobby;
            if ( lobby is Arena arenalobby )
                arenalobby.OnPlayerWeaponSwitch ( player, oldweapon, newweapon );
            else if ( lobby is FightLobby fightlobby )
                fightlobby.OnPlayerWeaponSwitch ( player, oldweapon, newweapon );
        }

        private void OnPlayerSpawn ( Client player ) {
            player.GetChar ().Lobby.OnPlayerSpawn ( player );
        }
    }
}
