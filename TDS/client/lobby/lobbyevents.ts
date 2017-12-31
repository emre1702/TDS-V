/// <reference path="../types-ragemp/index.d.ts" />

mp.events.add( "onClientPlayerJoinLobby", ( isspectator, mapname, teamnames, teamcolors, countdowntime, roundtime, bombdetonatetime, bombplanttime, bombdefusetime, roundendtime ) => {
	log( "onClientPlayerJoinLobby" );
	rounddata.isspectator = isspectator == 1;
	setMapInfo( mapname );
	teamnames = JSON.parse( teamnames );
	teamcolors = JSON.parse( teamcolors );
	addTeamInfos( teamnames, teamcolors );
	lobbysettings.countdowntime = countdowntime;
	roundinfo.roundtime = roundtime;
	lobbysettings.bombdetonatetime = bombdetonatetime;
	lobbysettings.bombplanttime = bombplanttime;
	lobbysettings.bombdefusetime = bombdefusetime;
	lobbysettings.roundendtime = roundendtime;
} );

mp.events.add( "onClientPlayerLeaveLobby", ( playerID: number ) => {
    log( "onClientPlayerLeaveLobby" );
    let player = mp.players.at( playerID );
	if ( mp.players.local == player ) {
		toggleFightMode( false );
		removeBombThings();
		removeRoundThings( true );
		stopCountdownCamera();
		//localPlayerLeftLobbyMapVoting();
		removeRoundInfo();
	}
} );

mp.events.add( "onClientJoinMainMenu", () => {
	mp.game.cam.doScreenFadeIn( 100 );
} );

mp.events.add( "testit", ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 ) => {
	mp.gui.chat.push( "1: " + arg1 + " - " + typeof ( arg1 ) );
	mp.gui.chat.push( "2: " + arg2 + " - " + typeof ( arg2 ) );
	mp.gui.chat.push( "3: " + arg3 + " - " + typeof ( arg3 ) );
	mp.gui.chat.push( "4: " + arg4 + " - " + typeof ( arg4 ) );
	//mp.gui.chat.push( "5: " + arg5 + " - " + typeof ( arg5 ) );
	//mp.gui.chat.push( "6: " + arg6 + " - " + typeof ( arg6 ) );
	//mp.gui.chat.push( "7: " + arg7 + " - " + typeof ( arg7 ) );
	//mp.gui.chat.push( "8: " + arg8 + " - " + typeof ( arg8 ) );
} );