/// <reference path="types-gt-mp/index.d.ts" />

API.setPedCanRagdoll( true );
API.disableFingerPointing( true );

function Vector3Lerp( start: Vector3, end: Vector3, fraction: number ) {
	return new Vector3(
		( start.X + ( end.X - start.X ) * fraction ),
		( start.Y + ( end.Y - start.Y ) * fraction ),
		( start.Z + ( end.Z - start.Z ) * fraction )
	);
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

/**
*	fix for cursor-problems
*/ 
API.onKeyDown.connect( function ( sender, e ) {
	if ( e.KeyCode == Keys.End ) {
		if ( API.isCursorShown() ) {
			API.showCursor( false );
			nothidecursor = 0;
		} else {
			API.showCursor( true );
			nothidecursor = 1;
		}
	}
} );