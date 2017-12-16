﻿/// <reference path="../types-ragemp/index.d.ts" />

API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {

		case "onClientPlayerJoinLobby":
			log( "onClientPlayerJoinLobby start" );
			// args[0] => bool isspectator
			// args[1] => mapname
			// args[2] => teamnames
			// args[3] => teamcolors
			// args[4] => countdown-time
			// args[5] => round-time
			// args[6] => bomb-detonate-time
			// args[7] => bomb-plant-time
			// args[8] => bomb-defuse-time
			rounddata.isspectator = args[0];
			setMapInfo( args[1] );
			addTeamInfos ( args[2], args[3] );
			lobbysettings.countdowntime = args[4];
			roundinfo.roundtime = args[5];
			lobbysettings.bombdetonatetime = args[6];
			lobbysettings.bombplanttime = args[7];
			lobbysettings.bombdefusetime = args[8];
			lobbysettings.roundendtime = args[9];
			log( "onClientPlayerJoinLobby end" );
			break;

		case "onClientPlayerLeaveLobby":
			log( "onClientPlayerLeaveLobby start" );
			if ( API.getLocalPlayer().Equals( args[0] ) ) {
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

		case "onClientJoinMainMenu":
			API.fadeScreenIn( 100 );
			break;

	}
} );