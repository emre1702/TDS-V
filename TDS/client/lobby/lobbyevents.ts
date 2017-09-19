/// <reference path="../types-gt-mp/index.d.ts" />

API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {

		case "onClientPlayerJoinLobby":
			log( "onClientPlayerJoinLobby start" );
			rounddata.isspectator = args[0];
			lobbysettings.countdowntime = args[1];
			// args[2] is used in roundinfo -> roundtime //
			setMapInfo( args[3] );
			lobbyJoinRoundInfo( args );
			log( "onClientPlayerJoinLobby end" );
			break;

		case "onClientPlayerLeaveLobby":
			log( "onClientPlayerLeaveLobby start" );
			if ( API.getLocalPlayer() == args[0] ) {
				toggleFightMode( false );
				removeBombThings();
				removeRoundThings( true );
				stopCountdownCamera();
				localPlayerLeftLobbyMapVoting();
				removeRoundInfo();
			} else {
				removeTeammateFromTeamBlips( API.getPlayerName( args[0] ) );
			}
			log( "onClientPlayerLeaveLobby end" );
			break;

	}
} );