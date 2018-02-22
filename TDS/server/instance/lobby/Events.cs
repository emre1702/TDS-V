using GTANetworkAPI;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.manager.map;

namespace TDS.server.instance.lobby {
    class LobbyEvents : Script {

        public LobbyEvents ( ) { }

        #region Lobby
        [RemoteEvent("joinLobby")]
        public void JoinLobbyEvent ( Client player, int index, bool spectator ) {
            if ( Lobby.SLobbiesByIndex.ContainsKey ( index ) ) {
                Lobby lobby = Lobby.SLobbiesByIndex[index];
                if ( lobby is Arena )
                    manager.lobby.Arena.Join ( player, spectator );
                else
                    lobby.AddPlayer ( player, spectator );
            } else {
                /* player.sendNotification (  lobby doesn't exist ); */
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientJoinMainMenu" );  //TODO is that needed?
            }
        }

        [RemoteEvent( "joinMapCreatorLobby" )]
        public void JoinMapCreatorLobbyEvent ( Client player ) {
            manager.lobby.MapCreatorLobby.Join ( player );
        }
        #endregion

        #region Spectate
        [RemoteEvent("spectateNext")]
        public void SpectateNextEvent ( Client player, bool forward ) {
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
        public void TooLongOutsideMapEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.KillPlayer ( player, "too_long_outside_map" );
        }
        #endregion

        #region MapVote
        [RemoteEvent( "onMapsListRequest" )]
        public void OnMapsListRequestEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.SendMapsForVoting ( player );
        }

        [RemoteEvent ( "onMapVotingRequest" )]
        public void OnMapVotingRequestEvent ( Client player, string mapname ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.AddMapToVoting ( player, mapname );
        }
        
        [RemoteEvent ( "onVoteForMap" )]
        public void OnVoteForMapEvent ( Client player, string mapname ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;

            arena.AddVoteToMap ( player, mapname );
        }
        #endregion

        #region MapCreate 
        [RemoteEvent ( "checkMapName" )]
        public void OnCheckMapNameEvent ( Client player, string mapname ) {
            player.TriggerEvent ( "sendMapNameCheckResult", Map.DoesMapNameExist ( mapname ) );
        }

        [RemoteEvent ( "sendMapFromCreator" )]
        public void SendMapFromCreatorEvent ( Client player, string map ) {
            Map.CreateNewMap ( map, player.GetChar ().UID );
            player.GetChar ().Lobby.RemovePlayerDerived ( player );
        }

        [RemoteEvent ( "requestNewMapsList" )]
        public void RequestNewMapsListEvent ( Client player, bool requestall ) {
            Map.RequestNewMapsList ( player, requestall );
        }
        #endregion

        #region MapRanking
        [RemoteEvent ( "addRatingToMap")]
        public void AddRatingToMapEvent ( Client player, string mapname, uint rating ) {
            Map.AddPlayerMapRating ( player, mapname, rating );
        }
        #endregion

        #region Bomb
        [RemoteEvent ( "onPlayerStartPlanting" )]
        public void onPlayerStartPlantingEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StartBombPlanting ( player );
        }
                      
        [RemoteEvent ( "onPlayerStopPlanting" )]
        public void onPlayerStopPlantingEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StopBombPlanting ( player );
        }

        [RemoteEvent ( "onPlayerStartDefusing" )]
        public void onPlayerStartDefusingEvent ( Client player ) {
            if ( !( player.GetChar ().Lobby is Arena arena ) )
                return;
            arena.StartBombDefusing ( player );
        }
        
        [RemoteEvent ( "onPlayerStopDefusing" )]
        public void onPlayerStopDefusingEvent ( Client player ) {
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
        public void OnPlayerGiveOrderEvent ( Client player, string ordershort ) {
            player.GetChar ().Lobby.SendTeamOrder ( player, ordershort );
        }
        #endregion

        #region RageMP
        [ServerEvent(Event.PlayerEnterColshape)]
        public static void OnPlayerEnterColShape ( ColShape shape, Client player ) {
            player.GetChar ().Lobby.OnPlayerEnterColShape ( shape, player );
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public static void OnPlayerDisconnected ( Client player, DisconnectionType type, string reason ) {
            player.GetChar ().Lobby.OnPlayerDisconnected ( player, type, reason );
        }

        [ServerEvent(Event.PlayerWeaponSwitch)]
        public static void OnPlayerWeaponSwitch ( Client player, WeaponHash oldweapon, WeaponHash newweapon ) {
            Lobby lobby = player.GetChar ().Lobby;
            if ( lobby is Arena arenalobby )
                arenalobby.OnPlayerWeaponSwitch ( player, oldweapon, newweapon );
            else if ( lobby is FightLobby fightlobby )
                fightlobby.OnPlayerWeaponSwitch ( player, oldweapon, newweapon );
        }

        [ServerEvent(Event.PlayerSpawn)]
        public static void OnPlayerSpawn ( Client player ) {
            player.GetChar ().Lobby.OnPlayerSpawn ( player );
        }
        #endregion
    }
}
