/// <reference path="types-ragemp/index.d.ts" />
// source: https://gt-mp.net/forum/thread/689-display-cash-native/?postID=4592&highlight=Money#post4592 

let moneydata = {
	text: null as cText	
}

mp.events.add ( "onClientMoneyChange", money => {
	log( "onClientMoneyChange" );
	currentmoney = money;
    if ( moneydata.text == null ) {
        moneydata.text = new cText( "$" + currentmoney, 0.99, 0.01, 7, [115, 186, 131, 255], [1.0, 1.0], true, Alignment.RIGHT, true );
    } else
		moneydata.text.setText( "$" + currentmoney );
} );


let testit1 = ( arg1, arg2 ) => {
    mp.gui.chat.push( "M " + arg1 + " - " + arg2 );
};
mp.keys.bind( 77, true, testit1 );

mp.keys.bind( 68, false, () => {
    mp.keys.unbind( 77, testit1 );
} );

mp.keys.bind( 0x65, false, () => {
    mp.keys.unbind( 77, true, testit1 );
} );

mp.keys.bind( 40, false, () => {
    mp.keys.bind( 77, true, testit1 );
} );