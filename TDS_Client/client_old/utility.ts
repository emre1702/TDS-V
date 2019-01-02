/*function vector3Lerp( start: { x, y, z }, end: { x, y, z }, fraction: number ) {
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

function getPlayerByName( name: string ) {
    mp.players.forEach( ( player: PlayerMp ) => {
		if ( player.name == name )
			return player;
	} );
	return null;
}

function distance( vector1: Vector3Mp, vector2: Vector3Mp, useZ = true ) {
	return mp.game.gameplay.getDistanceBetweenCoords( vector1.x, vector1.y, vector1.z, vector2.x, vector2.y, vector2.z, useZ );
}*/