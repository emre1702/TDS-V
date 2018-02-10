﻿/// <reference path="../types-ragemp/index.d.ts" />


mp.events.add( "playerSpawn", ( player: PlayerMp ) => {
	if ( player == localPlayer ) {
		mp.game.cam.doScreenFadeIn( 2000 );
	}
} );

mp.events.add( "playerDeath", ( player: PlayerMp, reason, killer ) => {
	if ( player == localPlayer ) {
		mp.game.cam.doScreenFadeOut( 2000 );
		mp.game.gameplay.ignoreNextRestart( true );
		mp.game.gameplay.setFadeOutAfterDeath( false );
	}
} );