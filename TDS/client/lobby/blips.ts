/// <reference path="../types-gt-mp/index.d.ts" />

let blipsdata = {
	teamblips: {},
	updateteamblipsevent: null
}


function updateTeamBlipPositions() {
	for ( var playername in blipsdata.teamblips ) {
		var player = API.getPlayerByName( playername );
		if ( player != null ) {
			let pos = API.getEntityPosition( player );
			API.setBlipPosition( blipsdata.teamblips[playername], pos );
		}
	}
}


function createTeamBlips ( playerlist ) {
	blipsdata.teamblips = {};
	let localplayer = API.getLocalPlayer();
	let dimension = API.getEntityDimension( localplayer );
	for ( let i = 0; i < playerlist.Count; i++ ) {
		if ( !localplayer.Equals( playerlist[i] ) ) {
			let blip = API.createBlip( API.getEntityPosition( playerlist[i] ) );
			API.setEntityDimension( blip, dimension );
			API.setBlipSprite( blip, 0 );
			blipsdata.teamblips[API.getPlayerName( playerlist[i] )] = blip;
		}
	}
	blipsdata.updateteamblipsevent = API.onUpdate.connect( updateTeamBlipPositions );
}


function removeTeammateFromTeamBlips ( playername ) {
	if ( blipsdata.teamblips[playername] != undefined ) {
		API.deleteEntity( blipsdata.teamblips[playername] );
		delete blipsdata.teamblips[playername];
	}
}


function stopTeamBlips() {
	if ( blipsdata.updateteamblipsevent != null ) {
		blipsdata.updateteamblipsevent.disconnect();
		blipsdata.updateteamblipsevent = null;
	}
	for ( var playername in blipsdata.teamblips ) {
		API.deleteEntity( blipsdata.teamblips[playername] );
	}
	blipsdata.teamblips = {};
}