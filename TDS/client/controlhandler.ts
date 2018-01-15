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
        mp.game.controls.disableControlAction( 0, 25, true );
        //TODO Disabled because of bride - movement //
        mp.game.controls.disableControlAction( 0, 30, true );
        mp.game.controls.disableControlAction( 0, 31, true );
        mp.game.controls.disableControlAction( 0, 32, true );
        mp.game.controls.disableControlAction( 0, 33, true );
        mp.game.controls.disableControlAction( 0, 34, true );
        mp.game.controls.disableControlAction( 0, 35, true );
        ///////////////////////////////////////////////
    } 
} );

function toggleFightControls( enable: boolean ) {
    controlhandlerdata.fightenabled = enable;
}

