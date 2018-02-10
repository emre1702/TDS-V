/// <reference path="../types-ragemp/index.d.ts" />
// Körperteile: https://pastebin.com/AGQWgCct

let damagesysdata = {
    lastarmorhp: 200,
    shotsdoneinround: 0,
    shooting: false
}

var getCurrentWeapon = () => mp.game.invoke( '0x6678C142FAC881BA', localPlayer.handle );


mp.events.add( "render", () => {
    if ( !rounddata.infight )
        return;
    let armorhp = mp.players.local.getHealth() + mp.players.local.getArmour();
    if ( armorhp < damagesysdata.lastarmorhp )
        showBloodscreen();
    damagesysdata.lastarmorhp = armorhp;
    checkShooting();
} );

function checkShooting() {
    if ( localPlayer.isShooting() ) {
        damagesysdata.shooting = true;
    } else if ( damagesysdata.shooting ) {
        damagesysdata.shooting = false;
        let ammo = getWeaponAmmo( currentWeapon );
        damagesysdata.shotsdoneinround += ( currentAmmo - ammo );
        currentAmmo = ammo;
    }
}

mp.events.add( "playerWeaponShot", ( hitpos ) => {
    let startpos = localPlayer.getBoneCoords( 6286, 0, 0, 0 );
    let endpos = vector3Lerp( startpos, hitpos, 1.02 ) as MpVector3;
    let raycast = mp.raycasting.testPointToPoint( startpos, endpos, localPlayer, 8 ) as { position: { x, y, z }, surfaceNormal: any, entity: MpEntity };
    if ( typeof raycast !== "undefined" ) { // hit nothing {
        let player = mp.players.atHandle( raycast.entity );
        mp.gui.chat.push( player.name );
        callRemote( "onPlayerHitOtherPlayer", player, false );
        return true;
    }
} );

mp.players.local.setCanAttackFriendly( false, false );


/* let bloodscreenbrowser;

mp.events.add( "playerWeaponShoot", ( shotPosition: { x, y, z }, target ) => {

	if ( target != null ) {
		let weapon = mp.players.local.weapon;
		var hithead = false;
		//if (weaponUsed == 100416529 || weaponUsed == 205991906 || weaponUsed == 952879014) {
		//var neckpos = API.returnNative( "GET_PED_BONE_COORDS", 5, hitentityhandle, 39317 );
		//if ( aimCoords.Z > neckpos.Z ) {
		//	hithead = true;
		//}
		//}
		callRemote( "onPlayerHitOtherPlayer", target, weapon, hithead );
	}
} );

mp.players.local.setCanAttackFriendly( false, false ); */


/*mp.events.add( "render", () => {
	if ( mp.players.local.health + mp.players.local.armour < damagesysdata.lasthparmor ) {
		//show bloodscreen
	}
} ); */