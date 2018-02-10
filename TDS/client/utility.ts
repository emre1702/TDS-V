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
	return Date.now();
}

/**
*	fix for cursor-problems
*/
mp.keys.bind( 0x23, true, function () {	
	if ( mp.gui.cursor.visible ) {
		mp.gui.cursor.visible = false;
        nothidecursor = 0;
        mp.gui.chat.activate( true );
	} else {
		mp.gui.cursor.visible = true;
        nothidecursor = 1;
        mp.gui.chat.activate( false );
	}
} );

function toggleCursor( bool ) {
    if ( bool ) {
        ++nothidecursor;
        mp.gui.cursor.visible = true;
        mp.gui.chat.activate( false );
    } else {
        if ( --nothidecursor <= 0 ) {
            mp.gui.cursor.visible = false;
            mp.gui.chat.activate( true );
        }
    }
}

function getPlayerByName( name: string ) {
    mp.players.forEach( ( player: PlayerMp ) => {
		if ( player.name == name )
			return player;
	} );
	return null;
}

function distance( vector1: Vector3Mp, vector2: Vector3Mp, useZ = true ) {
	return mp.game.gameplay.getDistanceBetweenCoords( vector1.x, vector1.y, vector1.z, vector2.x, vector2.y, vector2.z, useZ );
}

mp.events.add( "testit", ( str ) => {
    mp.gui.chat.push( str );
} );