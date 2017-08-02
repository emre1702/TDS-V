/// <reference path="../types-gt-mp/index.d.ts" />
// Körperteile: https://pastebin.com/AGQWgCct

var bloodscreenbrowser;

function Vector3Lerp( start: Vector3, end: Vector3, fraction: number ) {
	return new Vector3(
		( start.X + ( end.X - start.X ) * fraction ),
		( start.Y + ( end.Y - start.Y ) * fraction ),
		( start.Z + ( end.Z - start.Z ) * fraction )
	);
}

API.onLocalPlayerShoot.connect( function ( weaponUsed, aimCoords ) {
	var pos = API.getEntityPosition( API.getLocalPlayer() );
	var endpos = Vector3Lerp( pos, aimCoords, 1.1 );

	var raycast = API.createRaycast( pos, endpos, 8, null );

	if ( raycast.didHitEntity ) {
		var hitentityhandle = raycast.hitEntity;
		var hithead = false;
		//if (weaponUsed == 100416529 || weaponUsed == 205991906 || weaponUsed == 952879014) {
		var neckpos = API.returnNative( "GET_PED_BONE_COORDS", 5, hitentityhandle, 39317 );
		if ( aimCoords.Z > neckpos.Z ) {
			hithead = true;
		}
		//}
		API.triggerServerEvent( "onPlayerHitOtherPlayer", hitentityhandle, weaponUsed, hithead );
	}
} );

API.onResourceStart.connect( function () {
	API.callNative( "NETWORK_SET_FRIENDLY_FIRE_OPTION", false );
	/*let res = API.getScreenResolutionMaintainRatio();
	bloodscreenbrowser = API.createCefBrowser( res.Width, res.Height );
	API.waitUntilCefBrowserInit( bloodscreenbrowser );
	API.setCefBrowserPosition( bloodscreenbrowser, 0, 0 );
	API.setCefBrowserHeadless( bloodscreenbrowser, false );
	API.loadPageCefBrowser( bloodscreenbrowser, "client/window/damagesys/bloodscreen.html" );*/
} );

/*API.onPlayerArmorChange.connect( function ( oldvalue ) {
	let newvalue = API.getPlayerArmor( API.getLocalPlayer() );
	if ( newvalue < oldvalue )
		bloodscreenbrowser.call( "showBloodscreen" );
} );

API.onPlayerHealthChange.connect( function ( oldvalue ) {
	let newvalue = API.getPlayerHealth( API.getLocalPlayer() );
	if ( newvalue < oldvalue )
		bloodscreenbrowser.call( "showBloodscreen" );
} );*/
