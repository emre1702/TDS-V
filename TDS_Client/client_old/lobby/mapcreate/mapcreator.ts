/*
let mapcreatordata = {
    browser: null as BrowserMp
};

function startMapCreator() {
    if ( mapcreatordata.browser !== null )
        return;

    mapcreatordata.browser = mp.browsers.new( "package://TDS-V/window/mapcreator/index.html" );
}

function stopMapCreator() {
    if ( mapcreatordata.browser === null )
        return;

    mapcreatordata.browser.destroy();
    mapcreatordata.browser = null;
}

mp.events.add(ECustomBrowserRemoteEvents.RequestCurrentPositionForMapCreator, () => {
    if ( mapcreatordata.browser === null )
        return;
    let position = localPlayer.position;
    let x = Math.round( position.x * 100 ) / 100;
    let y = Math.round( position.y * 100 ) / 100;
    let z = Math.round( position.z * 100 ) / 100;
    let rotation = Math.round( localPlayer.getRotation( 2 ).z * 100 ) / 100;
    mapcreatordata.browser.execute( "loadPositionFromClient (" + x + ", " + y + ", " + z + ", " + rotation + "); " );
} );

mp.events.add(ECustomBrowserRemoteEvents.GotoPositionByMapCreator, ( x: number, y: number, z: number, rot: number ) => {
    if ( mapcreatordata.browser === null )
        return;
    localPlayer.position = { x: x, y: y, z: z } as Vector3Mp;
    if ( !isNaN( rot ) )
        localPlayer.setRotation( 0.0, 0.0, rot, 2, true );
} );

mp.events.add( "CheckMapName", ( name ) => {
    EventsSender.SendCooldown( "checkMapName", name );
} );

mp.events.add( "SendMapNameCheckResult", ( alreadyinuse ) => {
    mapcreatordata.browser.execute( "loadResultOfMapNameCheck(" + alreadyinuse + ");" );
} );

mp.events.add( "SendMapFromCreator", ( mapjson ) => {
    EventsSender.SendCooldown( "sendMapFromCreator", mapjson );
} );*/


/* let mapcreatordata = {
	main: {
		menu: null as NativeUI.UIMenu,
		send: null as NativeUI.UIMenuColoredItem,
	},
	spawn: {
		menu: null as NativeUI.UIMenu,
		team: null as NativeUI.UIMenuListItem,
		add: null as NativeUI.UIMenuColoredItem,
		peds: {},
	},
	maplimit: {
		menu: null as NativeUI.UIMenu,
		add: null as NativeUI.UIMenuColoredItem,
		markers: [],
		objects: [],
	},
	selected: null as LocalHandle,
	updateevent: null as IConnectedEvent,

	wdown: false,
	adown: false,
	ddown: false,
	sdown: false,
	qdown: false,
	edown: false,
	shiftdown: false,
	altdown: false
};


function startMapCreatorMenu() {
	mapcreatordata.main.menu.Visible = true;
	mapcreatordata.updateevent = API.onUpdate.connect( mapcreatorOnUpdate );
}

function stopMapCreatorMenu() {
	mapcreatordata.main.menu.Visible = false;
	if ( mapcreatordata.updateevent != null ) {
		mapcreatordata.updateevent.disconnect();
		mapcreatordata.updateevent = null;
	}	
	mapcreatorSelectEntity();
}

function toggleMapCreatorMenu() {
	if ( mapcreatordata.main.menu.Visible )
		stopMapCreatorMenu();
	else 
		startMapCreatorMenu();
}

function mapcreatorDrawMapLimit() {
	let length = mapcreatordata.maplimit.markers.length;
	if ( length > 1 ) {
		for ( let i = 0; i < length; i++ ) {
			let j = i - 1;
			if ( j < 0 )
				j = length - 1;
			let pos1 = API.getEntityPosition( mapcreatordata.maplimit.markers[i] );
			let pos2 = API.getEntityPosition( mapcreatordata.maplimit.markers[j] );
			API.drawLine( pos1, pos2, 200, 150, 0, 0 );
			for ( let k = 1; k < 5; k++ ) {
				pos1.Z += 1;
				pos2.Z += 1;
				API.drawLine( pos1, pos2, 200, 150, 0, 0 );
			}
			API.drawLine( new Vector3( pos1.X, pos1.Y, pos1.Z - 20 ), new Vector3( pos1.X, pos1.Y, pos1.Z + 50 ), 200, 150, 0, 0 );
		}
	}
}

function mapcreatorOnUpdate() {
	mapcreatorDrawMapLimit();

	if ( mapcreatordata.selected != null ) {
		let to = null;
		let rot = null;
		var camRot = API.getGameplayCamRot();
		var camDir = API.getGameplayCamDir();
		let objpos = API.getEntityPosition( mapcreatordata.selected );

		var multiply = 1;
		if ( mapcreatordata.shiftdown )
			multiply = 3;
		if ( mapcreatordata.altdown )
			multiply = 0.5;

		if ( mapcreatordata.wdown ) {
			to = vector3Lerp( objpos, objpos.Add( camDir.Multiply( multiply ) ), 1.0 );
		}
		if ( mapcreatordata.sdown ) {
			if ( to != null ) objpos = to;
			to = vector3Lerp( objpos, objpos.Subtract( camDir.Multiply( multiply ) ), 1.0 );
		}
		if ( mapcreatordata.adown ) {
			if ( to != null ) objpos = to;
			let pos2 = getPositionInFront( multiply, objpos, camRot.Z, 90 );
			to = vector3Lerp( objpos, pos2, 1.0 );
		}
		if ( mapcreatordata.ddown ) {
			if ( to != null ) objpos = to;
			let pos2 = getPositionInFront( multiply, objpos, camRot.Z, -90 );
			to = vector3Lerp( objpos, pos2, 1.0 );
		}
		if ( mapcreatordata.edown ) {
			rot = API.getEntityRotation( mapcreatordata.selected );
			rot.Z -= 3 * multiply;
		}
		if ( mapcreatordata.qdown ) {
			rot = API.getEntityRotation( mapcreatordata.selected );
			rot.Z += 3 * multiply;
		}

		if ( to != null ) {
			API.setEntityPosition( mapcreatordata.selected, to );
		}
		if ( rot != null ) {
			API.setEntityRotation( mapcreatordata.selected, rot );
		}

	}
}

function mapcreatorKeyDown( e ) {
	if ( e.KeyCode === Keys.W )
		mapcreatordata.wdown = true;
	else if ( e.KeyCode === Keys.A )
		mapcreatordata.adown = true;
	else if ( e.KeyCode === Keys.S )
		mapcreatordata.sdown = true;
	else if ( e.KeyCode === Keys.D )
		mapcreatordata.ddown = true;
	else if ( e.KeyCode === Keys.E )
		mapcreatordata.edown = true;
	else if ( e.KeyCode === Keys.Q )
		mapcreatordata.qdown = true;

	else if ( e.KeyCode === Keys.RShiftKey || e.KeyCode === Keys.ShiftKey )
		mapcreatordata.shiftdown = true;
	else if ( e.KeyCode === Keys.Menu || e.KeyCode === Keys.RMenu )
		mapcreatordata.altdown = true;

	else if ( e.KeyCode === Keys.M )
		stopMapCreatorMenu();

	else if ( e.KeyCode == Keys.R )
		mapcreatorSelectEntity();

	else if ( e.KeyCode == Keys.G )
		if ( mapcreatordata.selected != null )
			API.setEntityPositionFrozen( mapcreatordata.selected, !API.isEntityPositionFrozen( mapcreatordata.selected ) );
}

function mapcreatorKeyUp( e ) {
	if ( e.KeyCode === Keys.W )
		mapcreatordata.wdown = false;
	else if ( e.KeyCode === Keys.A )
		mapcreatordata.adown = false;
	else if ( e.KeyCode === Keys.S )
		mapcreatordata.sdown = false;
	else if ( e.KeyCode === Keys.D )
		mapcreatordata.ddown = false;
	else if ( e.KeyCode === Keys.E )
		mapcreatordata.edown = false;
	else if ( e.KeyCode === Keys.Q )
		mapcreatordata.qdown = false;


	else if ( e.KeyCode === Keys.RShiftKey || e.KeyCode === Keys.ShiftKey )
		mapcreatordata.shiftdown = false;
	else if ( e.KeyCode === Keys.Menu || e.KeyCode === Keys.RMenu )
		mapcreatordata.altdown = false;
	
}


function createMapCreator () {
	let menu = API.createMenu( "Map-Creator", res.Width, res.Height * 0.5, 6 );
	menu.Visible = false;
	let send = API.createColoredItem( "send", "Sends your map.", "00A000", "00E000" );
	menu.AddItem( send );


	let spawnpointmenu = API.addSubMenu( menu, "spawn-point", "Adds spawn-points." );
	let teamlist = new List( String );
	for ( let i = 1; i <= 20; i++ )
		teamlist.Add( i );
	let team = API.createListItem( "team-number", "Sets the team-number.", teamlist, 1 );
	spawnpointmenu.AddItem( team );
	let addspawnpoint = API.createColoredItem( "add spawn-point", "Adds a spawn-point.", "00A000", "00E000" );
	spawnpointmenu.AddItem( addspawnpoint );

	let maplimitmenu = API.addSubMenu( menu, "map-limit", "Sets the corners for the map-limit (optional)." );
	let addmaplimit = API.createColoredItem( "add corner", "Adds a corner for the map-limit.", "00A000", "00E000" );
	maplimitmenu.AddItem( addmaplimit );

	mapcreatordata.main.menu = menu;
	mapcreatordata.main.send = send;
	mapcreatordata.spawn.menu = spawnpointmenu;
	mapcreatordata.spawn.team = team;
	mapcreatordata.spawn.add = addspawnpoint;
	mapcreatordata.maplimit.menu = maplimitmenu;
	mapcreatordata.maplimit.add = addmaplimit;

	changeMapCreatorLanguage();

	addspawnpoint.Activated.connect( addSpawnPoint );
	addmaplimit.Activated.connect( addMapLimit );
	send.Activated.connect( mapcreatorSendMap );

}

function addSpawnPoint ( sender, selectedItem ) {
	let lookingatpoint = API.getEntityPosition( freecamdata.CameraObject );
	lookingatpoint.Z = API.getGroundHeight( lookingatpoint );
	let ped = API.createPed( -1686040670, lookingatpoint, 0 );
	API.setEntityPositionFrozen( ped, true );
	let team = mapcreatordata.spawn.team.CurrentItem();
	if ( !( team in mapcreatordata.spawn.peds ) )
		mapcreatordata.spawn.peds[team] = [];
	mapcreatordata.spawn.peds[team].push( ped );
	mapcreatorSelectEntity( ped );
}

function addMapLimit( sender, selectedItem ) {
	let camRot = API.getGameplayCamRot();
	let lookingatpoint = getPositionInFront( 3, API.getEntityPosition( freecamdata.CameraObject ), camRot, 0 );
	lookingatpoint.Z = API.getGroundHeight( lookingatpoint );
	let object = API.createObject( -89848631, lookingatpoint, new Vector3() );
	API.setEntityPositionFrozen( object, true );
	let place = mapcreatordata.maplimit.objects.push( object );
	mapcreatorSelectEntity( object );
}

function mapcreatorGetSpawnsList() {
	let teamspawns = new List( List );
	let teamamount = 0;
	let teamnumbers = new List( String );
	for ( let team in mapcreatordata.spawn.peds ) {
		teamamount++;
		teamnumbers.Add( team );
	}
	if ( teamamount > 0 ) {
		teamnumbers.Sort();
		for ( let i = 0; i < teamnumbers.Count; i++ ) {
			let spawns = new List( Vector3 );
			let team = teamnumbers[i];
			for ( let j in mapcreatordata.spawn.peds[team] ) {
				spawns.Add( mapcreatordata.spawn.peds[team][j] );
			}
			teamspawns.Add( spawns );
		}
	}
	return teamspawns
}

function mapcreatorGetMapLimitList() {

}

function mapcreatorSendMap( sender, selectedItem ) {
	if ( Object.keys( mapcreatordata.spawn.peds ).length > 0 ) {
		let teamspawns = mapcreatorGetSpawnsList();
	}
	
}

function mapcreatorSelectEntity( ent: LocalHandle = null ) {
	if ( mapcreatordata.selected != null ) {
		API.setEntityTransparency( mapcreatordata.selected, 255 );
		mapcreatordata.selected = null;
	}
	if ( ent != null ) {
		API.setEntityTransparency( ent, 150 );
		mapcreatordata.selected = ent;
	} else {
		let camRot = API.getGameplayCamRot();
		let pos1 = API.getEntityPosition( freecamdata.CameraObject );
		let pos2 = getPositionInFront( 10, pos1, camRot, 0 );
		let raycast = API.createRaycast( pos1, pos2, 4 | 8 | 12 | 16, null );
		if ( raycast.didHitEntity ) {
			let entity = raycast.hitEntity;
			for ( let i = 0; i < mapcreatordata.maplimit.objects.length; i++ ) {
				if ( mapcreatordata.maplimit.objects[i].Equals( entity ) ) {
					mapcreatorSelectEntity( mapcreatordata.maplimit.objects[i] );
					return;
				}
			}
			let arr = {};
			for ( let team in mapcreatordata.spawn.peds ) {
				let teampeds = mapcreatordata.spawn.peds[team];
				for ( let i = 0; i < teampeds.length; i++ ) {
					if ( teampeds[i].Equals( entity ) ) {
						mapcreatorSelectEntity( teampeds[i] );
						return;
					}
				}
			}
		}
	}
}


function changeMapCreatorLanguage() {
	mapcreatordata.main.send.Text = getLang( "mapcreator_menu", "send_text" );
	mapcreatordata.main.send.Description = getLang( "mapcreator_menu", "send_description" );

	API.setMenuTitle( mapcreatordata.spawn.menu, getLang( "mapcreator_menu", "spawnpoint_text" ) );
	API.setMenuSubtitle( mapcreatordata.spawn.menu, getLang( "mapcreator_menu", "spawnpoint_description" ) );
	mapcreatordata.spawn.add.Text = getLang( "mapcreator_menu", "spawnpoint_add_text" );
	mapcreatordata.spawn.add.Description = getLang( "mapcreator_menu", "spawnpoint_add_description" );
	mapcreatordata.spawn.team.Text = getLang( "mapcreator_menu", "team_text" );
	mapcreatordata.spawn.team.Description = getLang( "mapcreator_menu", "team_description" );

	API.setMenuTitle( mapcreatordata.maplimit.menu, getLang( "mapcreator_menu", "maplimit_text" ) );
	API.setMenuSubtitle( mapcreatordata.maplimit.menu, getLang( "mapcreator_menu", "maplimit_description" ) );
	mapcreatordata.maplimit.add.Text = getLang( "mapcreator_menu", "maplimit_add_text" );
	mapcreatordata.maplimit.add.Description = getLang( "mapcreator_menu", "maplimit_add_description" );
}
*/
