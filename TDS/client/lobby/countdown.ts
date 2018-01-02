/// <reference path="../types-ragemp/index.d.ts" />

let countdownsounds = [
	"go.wav",
	"1.wav",
	"2.wav",
	"3.wav"
];
let countdowndata = {
	sounds: [
		"go.wav",
		"1.wav",
		"2.wav",
		"3.wav"
	],
	text: null,
	timer: null,
}


/*API.onResourceStart.connect( () => {
	for ( let i = 0; i < countdowndata.sounds.length; i++ ) {
		API.preloadAudio( soundspath + countdowndata.sounds[i] );
	}
} );*/


function countdownFunc( counter ) {
	log( "countdownFunc" );
	counter--;;
	if ( counter > 0 ) {
		countdowndata.text.setText( counter.toString() );
		countdowndata.text.blendTextScale( 6, 1000 );
		countdowndata.timer = new Timer( countdownFunc, 1000, 1, counter );
		if ( countdownsounds[counter] != null ) {
			//API.startAudio( soundspath + countdownsounds[counter], false );
			//API.setAudioVolume( 0.2 );
		}
	}
}


function startCountdown() {
	log( "startCountdown" );
	countdowndata.text = new cText( Math.floor( lobbysettings.countdowntime / 1000 ).toString(), 0.5, 0.2, 0, [255, 255, 255, 255], [2.0, 2.0], true, Alignment.CENTER, true );
	countdowndata.timer = new Timer( countdownFunc, lobbysettings.countdowntime % 1000, 1, Math.floor ( lobbysettings.countdowntime / 1000 ) + 1 );
}


function startCountdownAfterwards ( timeremaining ) {
    log( "startCountdownAfterwards" );
    countdowndata.text = new cText( timeremaining.toString(), 0.5, 0.2, 0, [255, 255, 255, 255], [2.0, 2.0], true, Alignment.CENTER, true );
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
	//API.startAudio( soundspath + countdownsounds[0], false );
	//API.setAudioVolume( 0.2 );
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