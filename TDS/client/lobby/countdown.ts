/// <reference path="../types-gt-mp/index.d.ts" />

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


API.onResourceStart.connect( function () {
	for ( var i = 0; i < countdowndata.sounds.length; i++ ) {
		API.preloadAudio( soundspath + countdowndata.sounds[i] );
	}
} );


function countdownFunc( counter ) {
	log( "countdownFunc start" );
	counter--;
	if ( counter > 0 ) {
		countdowndata.text.setText( counter.toString() );
		countdowndata.text.blendTextScale( 6, 1000 );
		countdowndata.timer = new Timer( countdownFunc, 1000, 1, counter );
		if ( countdownsounds[counter] != null ) {
			API.setAudioVolume( 0.3 );
			var audio = API.startAudio( soundspath + countdownsounds[counter], false );
		}
	}
	log( "countdownFunc end" );
}


function startCountdown() {
	log( "startCountdown start" );
	countdowndata.text = new cText( lobbysettings.countdowntime.toString(), res.Width / 2, res.Height * 0.2, 2.0, 255, 255, 255, 255, 0, 1, true );
	countdownFunc( lobbysettings.countdowntime + 1 );
	log( "startCountdown end" );
}


function startCountdownAfterwards ( timeremaining ) {
	log( "startCountdownAfterwards start" );
	countdowndata.text = new cText( timeremaining.toString(), res.Width / 2, res.Height * 0.2, 2.0, 255, 255, 255, 255, 0, 1, true );
	countdownFunc( timeremaining + 1 );
	log( "startCountdownAfterwards end" );
}


function endCountdown() {
	log( "endCountdown start" );
	if ( countdowndata.text == null ) {
		countdowndata.text= new cText( "GO", res.Width / 2, res.Height * 0.2, 2.0, 255, 255, 255, 255, 0, 1, true );
	} else
		countdowndata.text.setText( "GO" );
	if ( countdowndata.timer != null )
		countdowndata.timer.kill();
	API.startAudio( soundspath + countdownsounds[0], false );
	API.setAudioVolume( 0.3 );
	countdowndata.timer = new Timer( stopCountdown, 2000, 1 );
	log( "endCountdown end" );
}


function stopCountdown() {
	log( "stopCountdown start" );
	if ( countdowndata.text != null ) {
		countdowndata.text.remove();
		countdowndata.text = null;
	}
	if ( countdowndata.timer != null ) {
		countdowndata.timer.kill();
		countdowndata.timer = null;
	}
	log( "stopCountdown end" );
}