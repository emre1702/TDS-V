/// <reference path="../types-gt-mp/index.d.ts" />
// Körperteile: https://pastebin.com/AGQWgCct

var bloodscreenbrowser;

API.onLocalPlayerShoot.connect( function ( weaponUsed, aimCoords ) {
	var frompos = API.getEntityPosition( API.getLocalPlayer() );

	var dir = aimCoords.Subtract( frompos );
	var distance = dir.Length();
	dir.Normalize();

	dir.X *= distance * 1.05;
	dir.Y *= distance * 1.05;
	dir.Z *= distance * 1.05;

	var topos = frompos.Add( dir ); 

	var raycast = API.createRaycast( frompos, topos, 8, null );

	if ( raycast.didHitEntity ) {
		var hitentityhandle = raycast.hitEntity;
		var hithead = false;
		//if (weaponUsed == 100416529 || weaponUsed == 205991906 || weaponUsed == 952879014) {
		//var neckpos = API.returnNative( "GET_PED_BONE_COORDS", 5, hitentityhandle, 39317 );
		//if ( aimCoords.Z > neckpos.Z ) {
		//	hithead = true;
		//}
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
