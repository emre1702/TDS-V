/// <reference path="types-ragemp/index.d.ts" />

var res = mp.game.graphics.getScreenActiveResolution ( 0, 0 );
var nothidecursor = 0;
var currentmoney = null;
var localPlayer = mp.players.local;
var gameplayCam = mp.cameras.new( "gameplay" ) as MpCamera;
var ischatopen = false;
var currentweapon = 2725352035;


mp.events.add( "onClientPlayerWeaponChange", function ( weapon ) {
    currentweapon = weapon;
} );