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
        public void JoinLobbyEvent ( Client player, params object[] args ) {
            int index = (int) args[0];
            if ( Lobby.SLobbiesByIndex.ContainsKey ( index ) ) {
                Lobby lobby = Lobby.SLobbiesByIndex[index];
                if ( lobby is Arena arenalobby )
                    arenalobby.AddPlayer ( player, (bool) args[1] );
                else
                    lobby.AddPlayer ( player, (bool) args[1] );
            } else {
                /* player.sendNotification (  lobby doesn't exist ); */
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientJoinMainMenu" );  //TODO is that needed?
            }
        }
        #endregion

         #region Spectate
        [RemoteEvent("spectateNext")]
        public void SpectateNextEvent ( Client player, params object[] args ) {
            Character character = player.GetChar ();
            if ( !( character.Lobby is Arena arena ) )
                return;
            if ( character.Lifes == 0 &&
                ( arena.status == LobbyStatus.ROUND || character.Team == 0 && arena.status == LobbyStatus.COUNTDOWN ) ) {
                if ( character.Team == 0 )
                    arena.SpectateAllTeams ( player, (bool) args[0] );
                else
                    arena.SpectateTeammate ( player, (bool) args[0] );
            }
        }
        #endregion
                
        #region Round
        [RemoteEvent( "onPlayerWasTooLongOutsideMap" )]
        public void TooLongOutsideMapEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.KillPlayer ( player, "too_long_outside_map" );
        }
        #endregion

        #region MapVote
        [RemoteEvent( "onMapsListRequest" )]
        public void OnMapsListRequestEvent ( Client player, params object[] args ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.SendMapsForVoting ( player );
        }
                
        [RemoteEvent ( "onMapVotingRequest" )]
        public void OnMapVotingRequestEvent ( Client player, params object[] args ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.AddMapToVoting ( player, (string) args[0] );
        }
        
        [RemoteEvent ( "onVoteForMap" )]
        public void OnVoteForMapEvent ( Client player, params object[] args ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.AddVoteToMap ( player, (string) args[0] );
        }
        #endregion

        #region Bomb
        [RemoteEvent ( "onPlayerStartPlanting" )]
        public void onPlayerStartPlantingEvent ( Client player, params object[] args ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StartBombPlanting ( player );
        }
                      
        [RemoteEvent ( "onPlayerStopPlanting" )]
        public void onPlayerStopPlantingEvent ( Client player, params object[] args ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StopBombPlanting ( player );
        }

        [RemoteEvent ( "onPlayerStartDefusing" )]
        public void onPlayerStartDefusingEvent ( Client player, params object[] args ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StartBombDefusing ( player );
        }
        
        [RemoteEvent ( "onPlayerStopDefusing" )]
        public void onPlayerStopDefusingEvent ( Client player, params object[] args ) {
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
        public void OnPlayerGiveOrderEvent ( Client player, params object[] args ) {
            player.GetChar ().Lobby.SendTeamOrder ( player, (string) args[0] );
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
