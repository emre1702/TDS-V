/// <reference path="../types-gt-mp/index.d.ts" />

API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {

		case "sendClientMapData":
			loadMapLimitData( args[0] );
			break;

		case "onClientCountdownStart":
			startCountdown();
			if ( rounddata.isspectator )
				startSpectate();	
			rounddata.mapinfo.setText( args[0] );
			break;

		case "onClientRoundStart":
			endCountdown();
			startMapLimit();
			rounddata.isspectator = args[0];
			if ( !rounddata.isspectator ) {
				startMapLimit();
				createTeamBlips ( args[1] );
			}
			break;

		case "onClientRoundEnd": 
			emptyMapLimit();
			removeRoundThings( false );
			stopCountdown();
			stopTeamBlips();
			break;

		case "onClientSpectateMode":
			rounddata.isspectator = true;
			startSpectate();
			break;

		case "onClientPlayerJoinLobby":
			rounddata.isspectator = args[0];
			lobbysettings.countdowntime = args[1];
			// args[2] is used in roundinfo -> roundtime //
			setMapInfo( args[3] );
			break;

		case "onClientPlayerLeaveLobby":
			removeRoundThings( true );
			break;

		case "onClientPlayerDeath":
			if ( API.getLocalPlayer() == args[0] ) {
				stopMapLimitCheck();
			} else
				removeTeammateFromTeamBlips( API.getPlayerName( args[0] ) );
			break;

		case "onClientPlayerQuit":
			removeTeammateFromTeamBlips( API.getPlayerName( args[0] ) );
			break;
	}
} );