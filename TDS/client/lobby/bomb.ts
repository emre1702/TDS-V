/// <reference path="../types-gt-mp/index.d.ts" />

let bombdata = {
	changed: false,
	gotbomb: false,
	placestoplant: [],
	plantdefuseevent: null,
	isplanting: false,
	isdefusing: false,
	plantdefusestarttick: 0,
	plantedpos: null
}

// onPlayerStartPlanting & onPlayerStopPlanting //

function drawPlant() {
	let tickswasted = API.getGlobalTime() - bombdata.plantdefusestarttick;
	if ( tickswasted < lobbysettings.bombplanttime ) {
		API.drawRectangle( res.Width * 0.46, res.Height * 0.7, res.Width * 0.08, res.Height * 0.02, 0, 0, 0, 187 );
		let progress = tickswasted / lobbysettings.bombplanttime;
		API.drawRectangle( res.Width * 0.461, res.Height * 0.701, res.Width * 0.078 * progress, res.Height * 0.018, 0, 180, 0, 187 );
		API.drawText( getLang( "round", "planting" ), res.Width * 0.5, res.Height * 0.71, 0.4, 255, 255, 255, 255, 0, 1, true, true, 0 );
	}
}

function checkPlant() {
	let isonplacetoplant = false;
	let playerpos = API.getEntityPosition( API.getLocalPlayer() );
	for ( let i = 0; i < bombdata.placestoplant.length && !isonplacetoplant; i++ ) {
		if ( playerpos.DistanceTo( bombdata.placestoplant[i] ) <= 5 )
			isonplacetoplant = true;
	}
	if ( isonplacetoplant ) {
		if ( bombdata.isplanting ) {
			drawPlant();
		} else {
			bombdata.plantdefusestarttick = API.getGlobalTime();
			bombdata.isplanting = true;
			API.triggerServerEvent( "onPlayerStartPlanting" );
		}
	} else
		checkPlantDefuseStop();
}

function drawDefuse() {
	let tickswasted = API.getGlobalTime() - bombdata.plantdefusestarttick;
	if ( tickswasted < lobbysettings.bombdefusetime ) {
		API.drawRectangle( res.Width * 0.46, res.Height * 0.7, res.Width * 0.08, res.Height * 0.02, 0, 0, 0, 187 );
		let progress = tickswasted / lobbysettings.bombdefusetime;
		API.drawRectangle( res.Width * 0.461, res.Height * 0.701, res.Width * 0.078 * progress, res.Height * 0.018, 180, 0, 0, 187 );
		API.drawText( getLang( "round", "defusing" ), res.Width * 0.5, res.Height * 0.71, 0.4, 255, 255, 255, 255, 0, 1, true, true, 0 );
	}
}

function checkDefuse() {
	let playerpos = API.getEntityPosition( API.getLocalPlayer() );
	if ( playerpos.DistanceTo( bombdata.plantedpos ) <= 5 ) {
		if ( bombdata.isdefusing ) {
			drawDefuse();
		} else {
			bombdata.plantdefusestarttick = API.getGlobalTime();
			bombdata.isdefusing = true;
			API.triggerServerEvent( "onPlayerStartDefusing" );
		}
	} else
		checkPlantDefuseStop();
}

function checkPlantDefuseStop() {
	if ( bombdata.isplanting ) {
		bombdata.isplanting = false;
		API.triggerServerEvent( "onPlayerStopPlanting" );
	} else if ( bombdata.isdefusing ) {
		bombdata.isdefusing = false;
		API.triggerServerEvent( "onPlayerStopDefusing" );
	}
}

function checkPlantDefuse() {
	if ( API.getPlayerCurrentWeapon() == -1569615261 ) {
		API.disableControlThisFrame( 24 );
		if ( API.isDisabledControlPressed( 24 ) ) {
			let localplayer = API.getLocalPlayer();
			if ( !API.isPlayerDead( localplayer ) ) {
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
	let i = placestoplant.Count;
	while ( i-- )
		bombdata.placestoplant[i] = placestoplant[i];
	bombdata.plantdefuseevent = API.onUpdate.connect( checkPlantDefuse );
}

function localPlayerPlantedBomb() {
	bombdata.gotbomb = false;
	bombdata.plantdefuseevent.disconnect();
	bombdata.plantdefuseevent = null;
	bombdata.isplanting = false;
}

function bombPlanted( pos, candefuse ) {
	if ( candefuse ) {
		bombdata.changed = true;
		bombdata.plantedpos = pos;
		bombdata.plantdefuseevent = API.onUpdate.connect( checkPlantDefuse );
	}
	setRoundTimeLeft( lobbysettings.bombdetonatetime );
}


function removeBombThings() {
	if ( bombdata.changed ) {
		if ( bombdata.plantdefuseevent != null ) {
			bombdata.plantdefuseevent.disconnect();
		}
		bombdata = {
			changed: false,
			gotbomb: false,
			placestoplant: [],
			plantdefuseevent: null,
			isplanting: false,
			isdefusing: false,
			plantdefusestarttick: 0,
			plantedpos: null
		}
	}
}