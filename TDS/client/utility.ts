/// <reference path="types-gt-mp/index.d.ts" />


function Vector3Lerp( start: Vector3, end: Vector3, fraction: number ) {
	return new Vector3(
		( start.X + ( end.X - start.X ) * fraction ),
		( start.Y + ( end.Y - start.Y ) * fraction ),
		( start.Z + ( end.Z - start.Z ) * fraction )
	);
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

API.setPedCanRagdoll( true );
API.disableFingerPointing( true );