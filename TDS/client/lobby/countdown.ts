/// <reference path="../types-ragemp/index.d.ts" />

let countdowndata = {
	sounds: [
		"go",
		"1",
		"2",
		"3"
	],
	text: null,
	timer: null,
}


function countdownFunc( counter ) {
	log( "countdownFunc" );
	if ( --counter > 0 ) {
        countdowndata.text.setText( counter + "" );
		countdowndata.text.blendTextScale( [6.0, 6.0], 1000 );
		countdowndata.timer = new Timer( countdownFunc, 1000, 1, counter );
        if ( counter in countdowndata.sounds ) {
            playSound( countdowndata.sounds[counter] );
		}
	}
}


function startCountdown() {
	log( "startCountdown" );
	countdowndata.text = new cText( Math.floor( lobbysettings.countdowntime / 1000 )+"", 0.5, 0.2, 0, [255, 255, 255, 255], [2.0, 2.0], true, Alignment.CENTER, true );
	countdowndata.timer = new Timer( countdownFunc, lobbysettings.countdowntime % 1000, 1, Math.floor ( lobbysettings.countdowntime / 1000 ) + 1 );
}


function startCountdownAfterwards ( timeremaining ) {
    log( "startCountdownAfterwards" );
    countdowndata.text = new cText( timeremaining.toString() + "", 0.5, 0.2, 0, [255, 255, 255, 255], [2.0, 2.0], true, Alignment.CENTER, true );
	countdownFunc( timeremaining + 1 );
}


function endCountdown() {
	log( "endCountdown" );
	if ( countdowndata.text == null ) {
        countdowndata.text = new cText( "GO", 0.5, 0.2, 0, [255, 255, 255, 255], [2.0, 2.0], true, Alignment.CENTER, true );
	} else
		countdowndata.text.setText( "GO" );
	if ( countdowndata.timer != null )
		countdowndata.timer.kill();
    playSound( countdowndata.sounds[0] );
	countdowndata.timer = new Timer( stopCountdown, 2000, 1 );
}


function stopCountdown() {
	log( "stopCountdown" );
	if ( countdowndata.text != null ) {
		countdowndata.text.remove();
		countdowndata.text = null;
	}
	if ( countdowndata.timer != null ) {
		countdowndata.timer.kill();
		countdowndata.timer = null;
	}
}