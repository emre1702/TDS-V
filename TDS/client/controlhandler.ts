let controlhandlerdata = {
    fightenabled: true
}

mp.events.add( "render", () => {
    if ( mp.game.controls.isControlJustPressed( 0, 20 ) ) {
        scoreboardKeyJustPressed();
    } else if ( mp.game.controls.isControlJustReleased( 0, 20 ) ) {
        scoreboardKeyJustReleased();
    }

    if ( !controlhandlerdata.fightenabled ) {
        mp.game.controls.disableControlAction( 0, 24, true );
        mp.game.controls.disableControlAction( 0, 257, true );
    } 
} );

function toggleFightControls( enable: boolean ) {
    log( "toggleFightControls " + enable );
    controlhandlerdata.fightenabled = enable;
    mp.players.local.freezePosition( !enable );     //TODO because of brige-freeze not implemented
}

