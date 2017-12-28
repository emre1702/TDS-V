using GTANetworkAPI;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.lobby.interfaces;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {
    class LobbyEvents : Script {

        public LobbyEvents ( ) {
            Event.OnClientEventTrigger += OnClientEventTrigger;
            Event.OnPlayerDisconnected += OnPlayerDisconnected;
            Event.OnPlayerSpawn += OnPlayerSpawn;
            Event.OnPlayerWeaponSwitch += OnPlayerWeaponSwitch;
            Event.OnEntityEnterColShape += OnEntityEnterColShape;
        }

        private void OnClientEventTrigger ( Client player, string eventName, params dynamic[] args ) {
            switch ( eventName ) {
                #region Lobby
                case "joinLobby":
                    if ( Lobby.SLobbiesByIndex.ContainsKey ( args[0] ) ) {
                        Lobby lobby = Lobby.SLobbiesByIndex[args[0]];
                        if ( lobby is Arena arenalobby )
                            arenalobby.AddPlayer ( player, args[1] );
                        else
                            lobby.AddPlayer ( player, args[1] );
                        NAPI.Util.ConsoleOutput ( "JoinLobby " + player.Name + " - " + lobby.Name );
                    } else {
                        /* player.sendNotification (  lobby doesn't exist ); */
                        NAPI.ClientEvent.TriggerClientEvent ( player, "onClientJoinMainMenu" );  //TODO is that needed?
                    }
                    break;
                #endregion

                #region Spectate
                case "spectateNext":       
                    Character character = player.GetChar ();
                    if ( !( character.Lobby is Arena arena ) )
                        return;
                    if ( character.Lifes == 0 &&
                        ( arena.status == LobbyStatus.ROUND || character.Team == 0 && arena.status == LobbyStatus.COUNTDOWN ) ) {
                        if ( character.Team == 0 )
                            arena.SpectateAllTeams ( player, args[0] );    
                        else
                            arena.SpectateTeammate ( player, args[0] );     
                    }
                    break;
                #endregion

                #region Round
                case "onPlayerWasTooLongOutsideMap":
                    if ( !( player.GetChar ().Lobby is Arena lobby0 ) )
                        return;

                    lobby0.KillPlayer ( player, "too_long_outside_map" );    //TODO
                    break;
                #endregion

                #region MapVote
                case "onMapMenuOpen":
                    if ( !( player.GetChar ().Lobby is Arena lobby5 ) )
                        return;

                    lobby5.SendMapsForVoting ( player );      
                    break;

                case "onMapVotingRequest":
                    if ( !( player.GetChar ().Lobby is Arena lobby6 ) )
                        return;

                    lobby6.AddMapToVoting ( player, args[0] ); 
                    break;

                case "onVoteForMap":
                    if ( !( player.GetChar ().Lobby is Arena lobby7 ) )
                        return;

                    lobby7.AddVoteToMap ( player, args[0] );  
                    break;
                #endregion

                #region Bomb
                case "onPlayerStartPlanting":
                    if ( !( player.GetChar ().Lobby is Arena lobby1 ) )
                        return;
                    lobby1.StartBombPlanting ( player );  
                    break;

                case "onPlayerStopPlanting":
                    if ( !( player.GetChar ().Lobby is Arena lobby2 ) )
                        return;
                    lobby2.StopBombPlanting ( player );    
                    break;

                case "onPlayerStartDefusing":
                    if ( !( player.GetChar ().Lobby is Arena lobby3 ) )
                        return;
                    lobby3.StartBombDefusing ( player );   
                    break;

                case "onPlayerStopDefusing":
                    if ( !( player.GetChar ().Lobby is Arena lobby4 ) )
                        return;
                    lobby4.StopBombDefusing ( player );      //TODO
                    break;
                #endregion

                #region Freecam
                case "setFreecamObjectPositionTo":
                    //player.GetChar ().Lobby.SetPlayerFreecamPos ( player, args[0] );    //TODO
                    break;
                #endregion

                default:
                    player.GetChar ().Lobby.OnClientEventTrigger ( player, eventName, args );
                    break;
            }
        }

        private void OnEntityEnterColShape ( ColShape shape, NetHandle entity ) {
            Client player = NAPI.Player.GetPlayerFromHandle ( entity );
            if ( player.Exists ) {
                player.GetChar ().Lobby.OnEntityEnterColShape ( shape, player );
            }
        }

        private void OnPlayerDisconnected ( Client player, byte type, string reason ) {
            player.GetChar ().Lobby.OnPlayerDisconnected ( player, type, reason );
        }

        private void OnPlayerWeaponSwitch ( Client player, WeaponHash oldweapon, WeaponHash newweapon ) {
            if ( player.GetChar ().Lobby is IFight fightlobby )
                fightlobby.OnPlayerWeaponSwitch ( player, oldweapon, newweapon );
        }

        private void OnPlayerSpawn ( Client player ) {
            player.GetChar ().Lobby.OnPlayerSpawn ( player );
        }
    }
}
