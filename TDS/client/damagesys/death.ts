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
        scaleformMessage.showWastedMessage();
        mp.game.audio.requestScriptAudioBank( "HUD_MINI_GAME_SOUNDSET", true );  //
        mp.game.audio.playSoundFrontend( -1, "CHECKPOINT_NORMAL", "HUD_MINI_GAME_SOUNDSET", true );  //
	}
} );