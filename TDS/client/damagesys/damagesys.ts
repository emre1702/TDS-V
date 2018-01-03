/// <reference path="../types-ragemp/index.d.ts" />
// Körperteile: https://pastebin.com/AGQWgCct

let damagesysdata = {
    lastarmorhp: 200
}

mp.events.add( "render", () => {
    if ( !rounddata.infight )
        return;
    let armorhp = mp.players.local.getHealth() + mp.players.local.getArmour();
    if ( armorhp < damagesysdata.lastarmorhp )
        showBloodscreen();
    damagesysdata.lastarmorhp = armorhp;
} );

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
		mp.events.callRemote( "onPlayerHitOtherPlayer", target, weapon, hithead );
	}
} );

mp.players.local.setCanAttackFriendly( false, false ); */


/*mp.events.add( "render", () => {
	if ( mp.players.local.health + mp.players.local.armour < damagesysdata.lasthparmor ) {
		//show bloodscreen
	}
} ); */