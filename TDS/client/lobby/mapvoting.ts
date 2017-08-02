/// <reference path="../types-gt-mp/index.d.ts" />

let mapmenu = API.createMenu( "Map-Vote", "Maps", 0, 0, 4 );
mapmenu.ResetKey( menuControl.Back );
mapmenu.Visible = false;
let clickmapmenuevent = null;
let cooldown = 0;
let mapvotings = {};
let mapvotingmaps = [];
let showmapmenu = false;
let lastselectedmap = "";

API.onKeyDown.connect( function ( sender, key ) {
	if ( key.KeyCode == Keys.M ) {
		if ( !mapmenu.Visible ) {
			let tick = API.getGlobalTime();
			if ( cooldown <= tick ) {
				cooldown = tick + 3000;
				API.triggerServerEvent( "onMapMenuOpen" );
			}
		} else {
			mapMenuClose();
			API.showCursor( false );
		}
	}
} );

API.onUpdate.connect( function () {
	if ( showmapmenu )
		API.drawMenu( mapmenu );
	let mapvotinglength = mapvotingmaps.length;
	if ( mapvotinglength > 0 ) {
		let counter = 0;
		for ( let i = mapvotinglength - 1; i >= 0; i-- ) {
			let selectedthis = lastselectedmap == mapvotingmaps[i];
			API.drawText(( i + 1 ) + ". " + mapvotingmaps[i] + " [" + mapvotings[mapvotingmaps[i]] + "]", res.Width - 5, res.Height * 0.8 - ( res.Height * 0.02 * counter ), ( selectedthis ? 0.7 : 0.6 ), 255, 255, 255, 255, 0, 2, true, false, ( selectedthis ? 1 : 0 ) )
			counter++;
		}
	}
} );

function mapMenuItemClick ( sender, item, index ) {
	API.triggerServerEvent( "onMapVotingRequest", item.Text );
	mapMenuClose();
	API.showCursor( false );
}

function putMapSortedByAmountVotes ( mapname, amountvotes ) {
	for ( let i = mapvotingmaps.length - 1; i >= 0; i-- ) {
		if ( mapvotings[mapvotingmaps[i]] < amountvotes ) {
			mapvotingmaps.splice( i, 0, mapname );
			return;
		}
	}
	mapvotingmaps.unshift( mapname );
}

API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {

		case "onMapMenuOpen":
			mapmenu.Clear();
			for ( let j = 0; j < 10; j++ )
			for ( let i = 0; i < args[0].Count; i++ ) {
				let mapitem = API.createMenuItem( args[0][i], "placeholder" );
				mapmenu.AddItem( mapitem );
			}
			mapmenu.Visible = true;
			showmapmenu = true;
			clickmapmenuevent = mapmenu.OnItemSelect.connect( mapMenuItemClick );
			API.showCursor( true );
			break;

		case "onNewMapForVoting":
			let index = mapvotingmaps.length;
			mapvotingmaps[index] = args[0];
			mapvotings[args[0]] = 0;
			// args[0] = mapname (add vote)
			break;

		case "onAddVoteToMap":
			// args[0] = mapname (add vote)
			// args[1] = old mapname (remove vote) OR undefined
			let indexmap = mapvotingmaps.indexOf( args[0] );
			mapvotingmaps.splice( indexmap, 1 );
			mapvotings[args[0]]++;
			putMapSortedByAmountVotes( args[0], mapvotings[args[0]] );
			if ( args.length > 1 ) {
				let indexoldmap = mapvotingmaps.indexOf( args[1] );
				mapvotingmaps.splice( indexoldmap, 1 );
				mapvotings[args[1]]--;
				putMapSortedByAmountVotes( args[1], mapvotings[args[1]] );
			}	
			
			break;

		case "onMapVotingSyncOnJoin":
			mapvotings = {};
			mapvotingmaps = [];
			lastselectedmap = "";
			for ( let i = 0; i < args[0].Count; i++ ) {
				mapvotingmaps[i] = args[0][i];
				mapvotings[args[0][i]] = args[1][i];
			}
			break;

		case "onClientRoundEnd":
			mapvotings = {};
			mapvotingmaps = [];
			lastselectedmap = "";
			break;

		case "onClientPlayerLeaveLobby":
			mapvotings = {};
			mapvotingmaps = [];
			lastselectedmap = "";
			mapMenuClose();
			break;
	}
} );

function mapMenuClose() {
	mapmenu.Visible = false;
	showmapmenu = false;
	if ( clickmapmenuevent != null ) {
		clickmapmenuevent.disconnect();
		clickmapmenuevent = null;
	}
}

/* for ( var i = 0; i < VoicePlayers.length; i++ ) {
	API.drawText( VoicePlayers[i].k, 10, res.Height * 0.5 + ( res.Height * 0.02 * ( i - 1 )), 0.2, 255, 255, 255, 255, 0, 0, false, true, 0 );
}*/
