/// <reference path="types-ragemp/index.d.ts" />

var res = mp.game.graphics.getScreenActiveResolution ( 0, 0 );
var nothidecursor = 0;
var currentmoney = null;
var currentadminlvl = 0;
var localPlayer = mp.players.local;
var gameplayCam = mp.cameras.new( "gameplay" ) as CameraMp;
var ischatopen = false;
var currentWeapon = 2725352035;
var currentAmmo = 0;

var getWeaponAmmo = ( weaponhash ) => mp.game.invoke( '0x2406A9C8DA99D3F4', localPlayer.handle, weaponhash );

mp.events.add( "onClientPlayerWeaponChange", function ( weapon ) {
    currentWeapon = weapon;
    currentAmmo = getWeaponAmmo( weapon );
} );