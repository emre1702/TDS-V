/// <reference path="types-ragemp/index.d.ts" />
// source: https://gt-mp.net/forum/thread/689-display-cash-native/?postID=4592&highlight=Money#post4592 

mp.events.add ( "onClientMoneyChange", money => {
	log( "onClientMoneyChange start" );
	currentmoney = money;
	log( "onClientMoneyChange end" );
} );

mp.events.add ( "render", () => {
	if ( currentmoney != null ) {
		mp.game.graphics.drawText( "$" + currentmoney, 7, [115, 186, 131, 255], 1.0, 1.0, true, res.x-90, 50 );
	}
} );