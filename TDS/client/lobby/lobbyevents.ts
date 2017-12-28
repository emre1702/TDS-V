/// <reference path="../types-ragemp/index.d.ts" />

mp.events.add( "onClientPlayerJoinLobby", ( isspectator: boolean, mapname: string, teamnames, teamcolors, countdowntime, roundtime, bombdetonatetime, bombplanttime, bombdefusetime, roundendtime ) => {
	log( "onClientPlayerJoinLobby start" );
	rounddata.isspectator = isspectator;
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
	log( "onClientPlayerJoinLobby end" );
} );

mp.events.add( "onClientPlayerLeaveLobby", ( player: MpPlayer ) => {
	log( "onClientPlayerLeaveLobby start" );
	if ( mp.players.local == player ) {
		toggleFightMode( false );
		removeBombThings();
		removeRoundThings( true );
		stopCountdownCamera();
		//localPlayerLeftLobbyMapVoting();
		removeRoundInfo();
	}
	log( "onClientPlayerLeaveLobby end" );
} );

mp.events.add( "onClientJoinMainMenu", () => {
	mp.game.cam.doScreenFadeIn( 100 );
} );
