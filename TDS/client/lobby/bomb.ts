/// <reference path="../types-ragemp/index.d.ts" />

let bombdata = {
	changed: false,
	gotbomb: false,
	placestoplant: [] as MpVector3[],
	plantdefuseevent: false,
	isplanting: false,
	isdefusing: false,
	plantdefusestarttick: 0,
    plantedpos: null as MpVector3,
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
		if ( bombdata.isplanting ) {
            updatePlantDefuseProgress();
		} else {
			bombdata.plantdefusestarttick = getTick();
            bombdata.isplanting = true;
            bombdata.draw.backrect = new cRectangle( 0.46, 0.7, 0.08, 0.02, [0, 0, 0, 187], Alignment.CENTER, true );
            bombdata.draw.progrect = new cRectangle( 0.461, 0.701, 0.078, 0.018, [0, 180, 0, 187], Alignment.CENTER, true );
            bombdata.draw.text = new cText( getLang( "round", "planting" ), 0.5, 0.71, 0, [255, 255, 255, 255], [1.0, 0.4], true, Alignment.CENTER, true );
			mp.events.callRemote( "onPlayerStartPlanting" );
		}
	} else
		checkPlantDefuseStop();
}

function checkDefuse() {
	let playerpos = mp.players.local.position;
	if ( mp.game.gameplay.getDistanceBetweenCoords( playerpos.x, playerpos.y, playerpos.z, bombdata.plantedpos.x, bombdata.plantedpos.y, bombdata.plantedpos.z, true ) <= 5 ) {
		if ( bombdata.isdefusing ) {
            updatePlantDefuseProgress();
		} else {
			bombdata.plantdefusestarttick = getTick();
            bombdata.isdefusing = true;
            bombdata.draw.backrect = new cRectangle( 0.46, 0.7, 0.08, 0.02, [0, 0, 0, 187], Alignment.CENTER, true );
            bombdata.draw.progrect = new cRectangle( 0.461, 0.701, 0.078, 0.018, [180, 0, 0, 187], Alignment.CENTER, true );
            bombdata.draw.text = new cText( getLang( "round", "defusing" ), 0.5, 0.71, 0, [255, 255, 255, 255], [1.0, 0.4], true, Alignment.CENTER, true );
			mp.events.callRemote( "onPlayerStartDefusing" );
		}
	} else
		checkPlantDefuseStop();
}

function checkPlantDefuseStop() {
	if ( bombdata.isplanting ) {
		bombdata.isplanting = false;
		mp.events.callRemote( "onPlayerStopPlanting" );
	} else if ( bombdata.isdefusing ) {
		bombdata.isdefusing = false;
		mp.events.callRemote( "onPlayerStopDefusing" );
    }
    if ( bombdata.draw.backrect != null ) {
        bombdata.draw.backrect.remove();
        bombdata.draw.backrect = null;
        bombdata.draw.progrect.remove();
        bombdata.draw.progrect = null;
        bombdata.draw.text.remove();
        bombdata.draw.text = null;
    }
}

function checkPlantDefuse() {
    if ( mp.players.local.weapon == WeaponHash.Unarmed ) {
        if ( mp.game.controls.isControlJustPressed( 0, 24 ) ) {
            if ( !mp.players.local.isDeadOrDying( true ) ) {
                mp.game.controls.disableControlAction ( 0, 24, true );
			    if ( bombdata.gotbomb ) {
				    checkPlant();
				    return;
			    } else {
				    checkDefuse();
				    return;
			    }
			} else
				checkPlantDefuseStop();
		} else
			checkPlantDefuseStop();
	} else
		checkPlantDefuseStop();
}

function localPlayerGotBomb( placestoplant ) {
	bombdata.changed = true;
	bombdata.gotbomb = true;
	let i = placestoplant.length;
	while ( i-- )
		bombdata.placestoplant[i] = placestoplant[i];
    bombdata.plantdefuseevent = true;
    mp.events.add( "checkPlantDefuse", checkPlantDefuse );
}

function localPlayerPlantedBomb() {
	bombdata.gotbomb = false;
	bombdata.plantdefuseevent = false;
    bombdata.isplanting = false;
    mp.events.remove( "checkPlantDefuse", checkPlantDefuse );
}

function bombPlanted( pos, candefuse ) {
	if ( candefuse ) {
		bombdata.changed = true;
		bombdata.plantedpos = pos;
		bombdata.plantdefuseevent = true;
    }
    mp.events.add( "checkPlantDefuse", checkPlantDefuse );
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
        if ( bombdata.draw.backrect != null ) {
            bombdata.draw.backrect.remove();
            bombdata.draw.backrect = null;
            bombdata.draw.progrect.remove();
            bombdata.draw.progrect = null;
            bombdata.draw.text.remove();
            bombdata.draw.text = null;
        }
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