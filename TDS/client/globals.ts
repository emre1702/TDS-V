/// <reference path="types-gt-mp/index.d.ts" />

var res = API.getScreenResolutionMaintainRatio();
var nothidecursor = 0;
var soundspath = "client/sounds/";
var currentmoney = null;

/* API.onUpdate.connect( function () {
	// GAME CONTROLS DEBUG
	for ( var control = 0; control < 338; control++ ) {
		if ( API.isControlJustPressed( control ) )
			API.sendChatMessage( "CONTROL PRESSED: " + control );
	}
} );*/