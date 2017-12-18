/// <reference path="types-ragemp/index.d.ts" />

function vector3Lerp( start: { x, y, z }, end: { x, y, z }, fraction: number ) {
	return {
		x: ( start.x + ( end.x - start.x ) * fraction ),
		y: ( start.y + ( end.y - start.y ) * fraction ),
		z: ( start.z + ( end.z - start.z ) * fraction )
	};
}

//By Don. Ported from C# to JS
function clampAngle( angle ) {
	return ( angle + Math.ceil( -angle / 360 ) * 360 );
}

function getPositionInFront( range, pos, zrot, plusangle ) {
	var angle = clampAngle( zrot ) * ( Math.PI / 180 );
	plusangle = ( clampAngle( plusangle ) * ( Math.PI / 180 ) );

	pos.X += ( range * Math.sin( -angle - plusangle ) );
	pos.Y += ( range * Math.cos( -angle - plusangle ) );
	return pos;
}

function getTick() {
	return new Date().getTime();
}

/**
*	fix for cursor-problems
*/
mp.keys.bind( 0x23, true, function ( sender, e ) {	// end
	if ( mp.gui.cursor.visible ) {
		mp.gui.cursor.visible = false;
		nothidecursor = 0;
	} else {
		mp.gui.cursor.visible = true;
		nothidecursor = 1;
	}
} );

function getPlayerByName( name: string ) {
	mp.players.forEach( ( player, index ) => {
		if ( player.name == name )
			return player;
	} );
	return null;
}