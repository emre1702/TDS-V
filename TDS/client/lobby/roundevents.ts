/// <reference path="../types-gt-mp/index.d.ts" />

API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {

		case "sendClientMapData":
			log( "sendClientMapData start" );
			if ( args[0].Count > 0 )
				loadMapLimitData( args[0] );
			loadMapMiddleForCamera( args[1] );
			log( "sendClientMapData end" );
			break;

		case "onClientCountdownStart":
			log( "onClientCountdownStart roundevents start " );
			if ( cameradata.timer != null )
				cameradata.timer.kill();
			if ( !( 1 in args) ) {
				startCountdown();
				cameradata.timer = new Timer( setCameraGoTowardsPlayer, lobbysettings.countdowntime * 1000 * 0.1, 1 );
			} else {
				let seconds = Math.ceil( args[1] / 1000 );
				startCountdownAfterwards( lobbysettings.countdowntime - seconds );
				if ( args[1] > lobbysettings.countdowntime * 1000 * 0.1 ) {
					setCameraGoTowardsPlayer( lobbysettings.countdowntime * 1000 - args[1] );
				} else {
					cameradata.timer = new Timer( setCameraGoTowardsPlayer, lobbysettings.countdowntime * 1000 * 0.1 - args[1], 1 );
				}
			}
			if ( rounddata.isspectator )
				startSpectate();	
			rounddata.mapinfo.setText( args[0] );
			log( "onClientCountdownStart roundevents end" );
			break;

		case "onClientRoundStart":
			log( "onClientRoundStart roundevents start" );
			stopCountdownCamera();
			endCountdown();
			rounddata.isspectator = args[0];
			if ( !rounddata.isspectator ) {
				rounddata.infight = true;
				startMapLimit();
				createTeamBlips ( args[1] );
			}
			log( "onClientRoundStart roundevents end" );
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