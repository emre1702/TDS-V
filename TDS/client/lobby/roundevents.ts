/// <reference path="../types-gt-mp/index.d.ts" />


API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {

		case "sendClientMapData":
			log( "sendClientMapData start" );
			loadMapLimitData( args[0] );
			log( "sendClientMapData end" );
			break;

		case "onClientCountdownStart":
			log( "onClientCountdownStart start" );
			startCountdown();
			if ( rounddata.isspectator )
				startSpectate();	
			rounddata.mapinfo.setText( args[0] );
			log( "onClientCountdownStart end" );
			break;

		case "onClientRoundStart":
			log( "onClientRoundStart start" );
			endCountdown();
			startMapLimit();
			rounddata.isspectator = args[0];
			if ( !rounddata.isspectator ) {
				rounddata.infight = true;
				startMapLimit();
				createTeamBlips ( args[1] );
			}
			log( "onClientRoundStart end" );
			break;

		case "onClientRoundEnd": 
			log( "onClientRoundEnd start" );
			rounddata.infight = false;
			emptyMapLimit();
			removeRoundThings( false );
			stopCountdown();
			stopTeamBlips();
			log( "onClientRoundEnd end" );
			break;

		case "onClientSpectateMode":
			log( "onClientSpectateMode start" );
			rounddata.isspectator = true;
			startSpectate();
			log( "onClientSpectateMode end" );
			break;

		case "onClientPlayerJoinLobby":
			log( "onClientPlayerJoinLobby start" );
			rounddata.isspectator = args[0];
			lobbysettings.countdowntime = args[1];
			// args[2] is used in roundinfo -> roundtime //
			setMapInfo( args[3] );
			log( "onClientPlayerJoinLobby end" );
			break;

		case "onClientPlayerLeaveLobby":
			log( "onClientPlayerLeaveLobby start" );
			rounddata.infight = false;
			removeRoundThings( true );
			log( "onClientPlayerLeaveLobby end" );
			break;

		case "onClientPlayerDeath":
			log( "onClientPlayerDeath start" );
			if ( API.getLocalPlayer() == args[0] ) {
				rounddata.infight = false;
				stopMapLimitCheck();
			} else
				removeTeammateFromTeamBlips( API.getPlayerName( args[0] ) );
			log( "onClientPlayerDeath end" );
			break;

		case "onClientPlayerQuit":
			log( "onClientPlayerQuit start" );
			removeTeammateFromTeamBlips( API.getPlayerName( args[0] ) );
			log( "onClientPlayerQuit end" );
			break;
	}
} );