/// <reference path="../types-ragemp/index.d.ts" />

let mapvotingdata = {
    lastlobbyID: -1,
    lastmapdatas: "",
    inmaplobby: false,
    votings: [] as { name: string, votes: number }[],
    visible: false
}

function openMapVotingMenu() {
    ++nothidecursor;
    mp.gui.cursor.visible = true;
    mapvotingdata.visible = true;
    if ( lobbysettings.id != mapvotingdata.lastlobbyID ) {
        mp.events.callRemote( "onMapsListRequest" );
    } else
        openMapMenuInBrowser( mapvotingdata.lastmapdatas );
        
}

function closeMapVotingMenu() {
    if ( mapvotingdata.visible ) {
        closeMapMenuInBrowser();
        mapvotingdata.visible = false;
        if ( --nothidecursor <= 0 )
            mp.gui.cursor.visible = false;
    }
}
mp.events.add( "closeMapVotingMenu", closeMapVotingMenu );

mp.keys.bind( Keys.M, false, () => {
    //if ( freecamdata.freecamMode ) 
    //  return;
    if ( !mapvotingdata.visible ) {
        if ( !ischatopen )
            if ( mapvotingdata.inmaplobby )
                openMapVotingMenu();
    } else
        closeMapVotingMenu();
} ); 

mp.events.add( "onClientMapsListRequest", ( mapdatasjson: string ) => {
    mapvotingdata.lastmapdatas = mapdatasjson;
    openMapMenuInBrowser( mapdatasjson );
} );

// triggered by browser //
mp.events.add( "onMapMenuVote", ( mapname ) => {
    mp.events.callRemote( "onMapVotingRequest", mapname );
} );

mp.events.add( "onAddVoteToMap", ( mapname, oldmapname ) => {
    addVoteToMapInMapMenuBrowser( mapname, oldmapname );
    let foundmap = false;
    for ( let i = 0; i < mapvotingdata.votings.length && !foundmap; ++i ) {
        if ( mapvotingdata.votings[i].name === mapname ) {
            ++mapvotingdata.votings[i].votes;
            foundmap = true;
        }
    }
    if ( !foundmap )
        mapvotingdata.votings.push( { name: mapname, votes: 1 } );
    if ( typeof oldmapname !== "undefined" ) {
        for ( let i = 0; i < mapvotingdata.votings.length; ++i ) {
            if ( mapvotingdata.votings[i].name === oldmapname ) {
                if ( --mapvotingdata.votings[i].votes <= 0 )
                    mapvotingdata.votings.splice( i, 1 );
                break;
            }
        }
    }
} );

mp.events.add( "onMapVotingSyncOnJoin", ( mapsvotesjson ) => {
    let mapsvotesdict = JSON.parse( mapsvotesjson ) as { [key: string]: number };
    for ( let key in mapsvotesdict ) {
        mapvotingdata.votings.push( { name: key, votes: mapsvotesdict[key] } );
    }
    loadMapVotingsForMapBrowser( mapsvotesjson );
} );




/*let mapvotedata = {
	menu: mp.game.gameplay.( "Map-Vote", "Maps", 0, 0, 4 ),
	clickevent: null,
	menucooldown: 0,
	votecooldown: 0,
	votings: {},
	votingmaps: [],
	lastselectedmap: ""
};
mapvotedata.menu.Visible = false;


API.onKeyDown.connect( function ( sender, key ) {
	//if ( !freecamdata.freecamMode ) {
		if ( key.KeyCode == Keys.M ) {
			if ( !mapvotedata.menu.Visible ) {
				let tick = getTick();
				if ( mapvotedata.menucooldown <= tick ) {
					mapvotedata.menucooldown = tick + 3000;
					mp.events.callRemote( "onMapMenuOpen" );
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
						mp.events.callRemote( "onVoteForMap", mapvotedata.votingmaps[vote] );
						mapvotedata.lastselectedmap = mapvotedata.votingmaps[vote];
					}
				}
			}
		}
	//}
} );

API.onUpdate.connect( function () {
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
	mp.events.callRemote( "onMapVotingRequest", item.Text );
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
			if ( 1 in args ) {
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
	if ( mapvotedata.clickevent != null ) {
		mapvotedata.clickevent.disconnect();
		mapvotedata.clickevent = null;
	}
}*/
