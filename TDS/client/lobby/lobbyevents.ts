/// <reference path="../types-gt-mp/index.d.ts" />

API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {

		case "onClientPlayerJoinLobby":
			log( "onClientPlayerJoinLobby start" );
			// args[0] => bool isspectator
			// args[1] => mapname
			// args[2] => settings-array:
				// args[2][0] => teamnames
				// args[2][1] => teamcolors
				// args[2][2] => countdown-time
				// args[2][3] => round-time
				// args[2][4] => bomb-detonate-time
				// args[2][5] => bomb-plant-time
				// args[2][6] => bomb-defuse-time
			rounddata.isspectator = args[0];
			setMapInfo( args[1] );
			addTeamInfos ( args[2][0], args[2][1] );
			lobbysettings.countdowntime = args[2][2];
			roundinfo.roundtime = args[2][3];
			lobbysettings.bombdetonatetime = parseInt( args[2][4] );
			lobbysettings.bombplanttime = parseInt( args[2][5] );
			lobbysettings.bombdefusetime = parseInt ( args[2][6] );
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