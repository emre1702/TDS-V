/// <reference path="../types-gt-mp/index.d.ts" />

let mapvotedata = {
	menu: API.createMenu( "Map-Vote", "Maps", 0, 0, 4 ),
	clickevent: null,
	menucooldown: 0,
	votecooldown: 0,
	votings: {},
	votingmaps: [],
	showmenu: false,
	lastselectedmap: ""
};
mapvotedata.menu.ResetKey( menuControl.Back ); 
mapvotedata.menu.Visible = false;


API.onKeyDown.connect( function ( sender, key ) {
	if ( key.KeyCode == Keys.M ) {
		if ( !mapvotedata.menu.Visible ) {
			let tick = API.getGlobalTime();
			if ( mapvotedata.menucooldown <= tick ) {
				mapvotedata.menucooldown = tick + 3000;
				API.triggerServerEvent( "onMapMenuOpen" );
			}
		} else {
			mapMenuClose();
			API.showCursor( false );
		}
	} else if ( mapvotedata.votingmaps.length > 0 ) {
		let vote = 0;
		if ( key.KeyCode == Keys.F1 ) 
			vote = 1;
		else if ( key.KeyCode == Keys.F2 ) 
			vote = 2;
		else if ( key.KeyCode == Keys.F3 )
			vote = 3;
		else if ( key.KeyCode == Keys.F4 )
			vote = 4;
		else if ( key.KeyCode == Keys.F5 )
			vote = 5;
		else if ( key.KeyCode == Keys.F6 )
			vote = 6;
		if ( vote > 0 ) {
			if ( vote < mapvotedata.votingmaps.length ) {
				let tick = API.getGlobalTime();
				if ( mapvotedata.votecooldown <= tick ) {
					mapvotedata.votecooldown = tick + 500;
					API.triggerServerEvent( "onVoteForMap", mapvotedata.votingmaps[vote] );
					mapvotedata.lastselectedmap = mapvotedata.votingmaps[vote];
				}
			}
		}
	}
} );

API.onUpdate.connect( function () {
	if ( mapvotedata.showmenu )
		API.drawMenu( mapvotedata.menu );
	let mapvotinglength = mapvotedata.votingmaps.length;
	if ( mapvotinglength > 0 ) {
		let counter = 0;
		for ( let i = mapvotinglength - 1; i >= 0; i-- ) {
			let selectedthis = mapvotedata.lastselectedmap == mapvotedata.votingmaps[i];
			API.drawText( "F" + ( i + 1 ) + " - " + mapvotedata.votingmaps[i] + " [" + mapvotedata.votings[mapvotedata.votingmaps[i]] + "]", res.Width - 5, res.Height * 0.8 - ( res.Height * 0.04 * counter ), 0.6, selectedthis ? 0 : 255, selectedthis ? 150 : 255, selectedthis ? 0 : 255, 255, 0, 2, true, true, 0 )
			counter++;
		}
	}
} );

function mapMenuItemClick( sender, item, index ) {
	mapvotedata.lastselectedmap = item.Text;
	API.triggerServerEvent( "onMapVotingRequest", item.Text );
	mapMenuClose();
	API.showCursor( false );
}

function putMapSortedByAmountVotes ( mapname, amountvotes ) {
	for ( let i = mapvotedata.votingmaps.length - 1; i >= 0; i-- ) {
		if ( mapvotedata.votings[mapvotedata.votingmaps[i]] < amountvotes ) {
			mapvotedata.votingmaps.splice( i, 0, mapname );
			return;
		}
	}
	mapvotedata.votingmaps.unshift( mapname );
}

API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {

		case "onMapMenuOpen":
			log( "onMapMenuOpen mapvoting start" );
			mapvotedata.menu.Clear();
			for ( let i = 0; i < args[0].Count; i++ ) {
				let mapitem = API.createMenuItem( args[0][i], args[1][i] );
				mapvotedata.menu.AddItem( mapitem );
			}
			mapvotedata.menu.Visible = true;
			mapvotedata.showmenu = true;
			mapvotedata.clickevent = mapvotedata.menu.OnItemSelect.connect( mapMenuItemClick );
			API.showCursor( true );
			log( "onMapMenuOpen mapvoting end" );
			break;

		case "onNewMapForVoting":
			log( "onNewMapForVoting mapvoting start" );
			let index = mapvotedata.votingmaps.length;
			mapvotedata.votingmaps[index] = args[0];
			mapvotedata.votings[args[0]] = 0;
			// args[0] = mapname (add vote)
			log( "onNewMapForVoting mapvoting end" );
			break;

		case "onMapRemoveFromVoting":
			log( "onMapRemoveFromVoting mapvoting start" );
			mapvotedata.votings[args[0]] = undefined;
			mapvotedata.votingmaps.splice( mapvotedata.votingmaps.indexOf( args[0] ), 1 );
			log( "onMapRemoveFromVoting mapvoting end" );
			break;

		case "onAddVoteToMap":
			log( "onAddVoteToMap mapvoting start" );
			// args[0] = onAddVoteToMap (add vote)
			// args[1] = old mapname (remove vote) OR undefined
			let indexmap = mapvotedata.votingmaps.indexOf( args[0] );
			mapvotedata.votingmaps.splice( indexmap, 1 );
			mapvotedata.votings[args[0]]++;
			putMapSortedByAmountVotes( args[0], mapvotedata.votings[args[0]] );
			if ( args.length > 1 ) {
				let indexoldmap = mapvotedata.votingmaps.indexOf( args[1] );
				mapvotedata.votingmaps.splice( indexoldmap, 1 );
				mapvotedata.votings[args[1]]--;
				putMapSortedByAmountVotes( args[1], mapvotedata.votings[args[1]] );
			}	
			log( "onAddVoteToMap mapvoting end" );
			break;

		case "onMapVotingSyncOnJoin":
			log( "onMapVotingSyncOnJoin mapvoting start" );
			mapvotedata.votings = {};
			mapvotedata.votingmaps = [];
			mapvotedata.lastselectedmap = "";
			for ( let i = 0; i < args[0].Count; i++ ) {
				mapvotedata.votingmaps[i] = args[0][i];
				mapvotedata.votings[args[0][i]] = args[1][i];
			}
			log( "onMapVotingSyncOnJoin mapvoting end" );
			break;
	}
} );

function localPlayerLeftLobbyMapVoting() {
	stopMapVoting()
	mapMenuClose();
}

function stopMapVoting() {
	mapvotedata.votings = {};
	mapvotedata.votingmaps = [];
	mapvotedata.lastselectedmap = "";
}

function mapMenuClose() {
	mapvotedata.menu.Visible = false;
	mapvotedata.showmenu = false;
	if ( mapvotedata.clickevent != null ) {
		mapvotedata.clickevent.disconnect();
		mapvotedata.clickevent = null;
	}
}

/* for ( var i = 0; i < VoicePlayers.length; i++ ) {
	API.drawText( VoicePlayers[i].k, 10, res.Height * 0.5 + ( res.Height * 0.02 * ( i - 1 )), 0.2, 255, 255, 255, 255, 0, 0, false, true, 0 );
}*/
