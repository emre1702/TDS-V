/// <reference path="../types-gt-mp/index.d.ts" />

API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {

		case "onClientMapChange":
			log( "onClientMapChange start" );
			if ( args[0].Count > 0 )
				loadMapLimitData( args[0] );
			loadMapMiddleForCamera( args[1] );
			log( "onClientMapChange end" );
			break;

		case "onClientCountdownStart":
			log( "onClientCountdownStart start " );
			if ( cameradata.timer != null )
				cameradata.timer.kill();
			if ( !( 1 in args) ) {
				startCountdown();
				cameradata.timer = new Timer( setCameraGoTowardsPlayer, lobbysettings.countdowntime * 0.1, 1 );
			} else {
				startCountdownAfterwards( Math.ceil ( ( lobbysettings.countdowntime - args[1] ) / 1000 ) );
				if ( args[1] > lobbysettings.countdowntime * 0.1 ) {
					setCameraGoTowardsPlayer( lobbysettings.countdowntime - args[1] );
				} else {
					cameradata.timer = new Timer( setCameraGoTowardsPlayer, lobbysettings.countdowntime * 0.1 - args[1], 1 );
				}
			}
			if ( rounddata.isspectator )
				startSpectate();	
			rounddata.mapinfo.setText( args[0] );
			log( "onClientCountdownStart end" );
			break;

		case "onClientRoundStart":
			log( "onClientRoundStart start" );
			stopCountdownCamera();
			endCountdown();
			rounddata.isspectator = args[0];
			if ( !rounddata.isspectator ) {
				startMapLimit();
				createTeamBlips( args[1] );
				toggleFightMode( true );
			}
			roundStartedRoundInfo( args )
			log( "onClientRoundStart end" );
			break;

		case "onClientRoundEnd": 
			log( "onClientRoundEnd start" );
			toggleFightMode( false );
			removeBombThings();
			emptyMapLimit();
			removeRoundThings( false );
			stopCountdown();
			stopCountdownCamera();
			stopTeamBlips();
			removeRoundInfo();
			stopMapVoting();
			log( "onClientRoundEnd end" );
			break;

		case "onClientPlayerSpectateMode":
			log( "onClientPlayerSpectateMode start" );
			rounddata.isspectator = true;
			startSpectate();
			log( "onClientPlayerSpectateMode end" );
			break;

		case "onClientPlayerDeath":
			log( "onClientPlayerDeath start" );
			if ( API.getLocalPlayer() == args[0] ) {
				toggleFightMode( false );
				removeBombThings();
			} else {
				removeTeammateFromTeamBlips( API.getPlayerName( args[0] ) );
				playerDeathRoundInfo( args[1], args[2] );
			}
			log( "onClientPlayerDeath end" );
			break;

		case "onClientPlayerQuit":
			log( "onClientPlayerQuit start" );
			removeTeammateFromTeamBlips( API.getPlayerName( args[0] ) );
			log( "onClientPlayerQuit end" );
			break;

		case "onClientPlayerGotBomb":
			localPlayerGotBomb( args[0] );
			break;

		case "onClientPlayerPlantedBomb":
			localPlayerPlantedBomb();
			break;

		case "onClientBombPlanted":
			bombPlanted ( args[0], args[1] );
			break;

		
	}
} );