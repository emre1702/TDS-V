/// <reference path="../types-ragemp/index.d.ts" />

let bombdata = {
    changed: false,
    gotbomb: false,
    placestoplant: [] as Vector3Mp[],
    plantdefuseevent: false,
    isplanting: false,
    isdefusing: false,
    plantdefusestarttick: 0,
    plantedpos: null as Vector3Mp,
    draw: {
        backrect: null as cRectangle,
        progrect: null as cRectangle,
        text: null as cText
    }
}

// onPlayerStartPlanting & onPlayerStopPlanting //

function updatePlantDefuseProgress() {
	let tickswasted = getTick() - bombdata.plantdefusestarttick;
    if ( tickswasted < lobbysettings.bombplanttime ) {
        mp.game.controls.disableControlAction( 0, 24, true );
		let progress = tickswasted / lobbysettings.bombplanttime;
        bombdata.draw.progrect.setWidth( 0.078 * progress );
	}
}

function checkPlant() {
	let isonplacetoplant = false;
	let playerpos = mp.players.local.position;
	for ( let i = 0; i < bombdata.placestoplant.length && !isonplacetoplant; i++ ) {
		let pos = bombdata.placestoplant[i];
		if ( mp.game.gameplay.getDistanceBetweenCoords( playerpos.x, playerpos.y, playerpos.z, pos.x, pos.y, pos.z, true ) <= 5 )
			isonplacetoplant = true;
	}
	if ( isonplacetoplant ) {
		bombdata.plantdefusestarttick = getTick();
        bombdata.isplanting = true;
        bombdata.draw.backrect = new cRectangle( 0.46, 0.7, 0.08, 0.02, [0, 0, 0, 187], Alignment.LEFT, true );
        bombdata.draw.progrect = new cRectangle( 0.461, 0.701, 0.078, 0.018, [0, 180, 0, 187], Alignment.LEFT, true );
        bombdata.draw.text = new cText( getLang( "round", "planting" ), 0.5, 0.7, 0, [255, 255, 255, 255], [1.0, 0.4], true, Alignment.CENTER, true );
        callRemote( "onPlayerStartPlanting" );
	}
}

function checkDefuse() {
	let playerpos = mp.players.local.position;
	if ( mp.game.gameplay.getDistanceBetweenCoords( playerpos.x, playerpos.y, playerpos.z, bombdata.plantedpos.x, bombdata.plantedpos.y, bombdata.plantedpos.z, true ) <= 5 ) {
		bombdata.plantdefusestarttick = getTick();
        bombdata.isdefusing = true;
        bombdata.draw.backrect = new cRectangle( 0.46, 0.7, 0.08, 0.02, [0, 0, 0, 187], Alignment.LEFT, true );
        bombdata.draw.progrect = new cRectangle( 0.461, 0.701, 0.078, 0.018, [180, 0, 0, 187], Alignment.LEFT, true );
        bombdata.draw.text = new cText( getLang( "round", "defusing" ), 0.5, 0.7, 0, [255, 255, 255, 255], [1.0, 0.4], true, Alignment.CENTER, true );
		callRemote( "onPlayerStartDefusing" );
	}
}

function removeBombDrawings() {
    if ( bombdata.draw.backrect != null ) {
        bombdata.draw.backrect.remove();
        bombdata.draw.backrect = null;
        bombdata.draw.progrect.remove();
        bombdata.draw.progrect = null;
        bombdata.draw.text.remove();
        bombdata.draw.text = null;
    }
}

function checkPlantDefuseStop() {
	if ( bombdata.isplanting ) {
		bombdata.isplanting = false;
		callRemote( "onPlayerStopPlanting" );
	} else if ( bombdata.isdefusing ) {
		bombdata.isdefusing = false;
		callRemote( "onPlayerStopDefusing" );
    }
    removeBombDrawings();
}


function checkPlantDefuseExtended() {
    //let currentweapon = mp.game.invoke( '0x6678C142FAC881BA', localPlayer.handle );
    if ( currentWeapon == WeaponHash.OldUnarmed ) {
        if ( !mp.players.local.isDeadOrDying( true ) ) {
            mp.game.controls.disableControlAction( 0, 24, true );
            return true;
        }
    }
    return false;
}


function checkPlantDefuse() {
    if ( !bombdata.isdefusing && !bombdata.isplanting ) {
        if ( mp.game.controls.isControlJustPressed( 0, 24 ) ) {
            if ( checkPlantDefuseExtended() ) {
                if ( bombdata.gotbomb ) {
                    checkPlant();
                    return;
                } else {
                    checkDefuse();
                    return;
                }
            }
        }
    } else {
        if ( !mp.game.controls.isDisabledControlJustReleased( 0, 24 ) ) {
            if ( checkPlantDefuseExtended() ) {
                updatePlantDefuseProgress();
            } else 
                checkPlantDefuseStop();
        } else 
            checkPlantDefuseStop();
    }

}

function localPlayerGotBomb( placestoplant ) {
    log( "localPlayerGotBomb" );
	bombdata.changed = true;
	bombdata.gotbomb = true;
	let i = placestoplant.length;
	while ( i-- )
		bombdata.placestoplant[i] = placestoplant[i];
    bombdata.plantdefuseevent = true;
    mp.events.add( "render", checkPlantDefuse );
}

function localPlayerPlantedBomb() {
	bombdata.gotbomb = false;
	bombdata.plantdefuseevent = false;
    bombdata.isplanting = false;
    mp.events.remove( "render", checkPlantDefuse );
    removeBombDrawings();
}

function bombPlanted( pos, candefuse ) {
	if ( candefuse ) {
		bombdata.changed = true;
		bombdata.plantedpos = pos;
        bombdata.plantdefuseevent = true;
        mp.events.add( "render", checkPlantDefuse );
    }
	setRoundTimeLeft( lobbysettings.bombdetonatetime );
}

function bombDetonated() {
	mp.game.cam.shakeGameplayCam( "LARGE_EXPLOSION_SHAKE", 1.0 );
	new Timer( mp.game.cam.stopGameplayCamShaking, 4000, 1 );
}


function removeBombThings() {
    if ( bombdata.changed ) {
        if ( bombdata.plantdefuseevent )
            mp.events.remove( "checkPlantDefuse", checkPlantDefuse );
        removeBombDrawings();
		bombdata = {
			changed: false,
			gotbomb: false,
			placestoplant: [],
			plantdefuseevent: false,
			isplanting: false,
			isdefusing: false,
			plantdefusestarttick: 0,
            plantedpos: null,
            draw: {
                backrect: null as cRectangle,
                progrect: null as cRectangle,
                text: null as cText
            }
		}
	}
}