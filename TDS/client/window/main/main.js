let mapMenuDiv;
let moneyText;
let bloodscreen;
let killmessagesBox;
let mapDatas;
let language = "ENGLISH";
let normalMapsList;
let bombMapsList;
let votingMapsList;
let mapVotingDiv;
let mapInfo;
let mapVoteButton;
let lastMapName = "";
let showingMapMenu = false;
let votings = [];
let votedMapName = "";
let votingInCooldown = false;

function setMoney( money ) {
    moneyText.text( "$" + money );
}

function playSound( soundname ) {
    $( "#audio_" + soundname ).trigger("play").volume = 0.05;
}

function showBloodscreen() {
    bloodscreen.stop().show(0).hide(2500);
}

function formatMsg( input ) {
    var start = '<span style="color: white;">';

    let replaced = input;
    if ( input.indexOf( "~" ) !== -1 ) {
        replaced = replaced.replace( /~r~/g, '</span><span style="color: rgb(222, 50, 50);">' )
            .replace( /~b~/g, '</span><span style="color: rgb(92, 180, 227);">' )
            .replace( /~g~/g, '</span><span style="color: rgb(113, 202, 113);">' )
            .replace( /~y~/g, '</span><span style="color: rgb(238, 198, 80);">' )
            //  .replace( /~p~/g, '</span><span style="color: rgb(131, 101, 224);">' )
            //  .replace( /~q~/g, '</span><span style="color: rgb(226, 79, 128);">' )
            .replace( /~o~/g, '</span><span style="color: rgb(253, 132, 85);">' )
            //  .replace( /~c~/g, '</span><span style="color: rgb(139, 139, 139);">' )
            //  .replace( /~m~/g, '</span><span style="color: rgb(99, 99, 99);">' )
            //  .replace( /~u~/g, '</span><span style="color: rgb(0, 0, 0);">' )
            .replace( /~s~/g, '</span><span style="color: rgb(220, 220, 220);">' )
            .replace( /~w~/g, '</span><span style="color: white;">' )		// white
            .replace( /~dr~/g, '</span><span style="color: rgb( 169, 25, 25 );">' )		// dark red
            .replace( /~n~/g, '<br>' );
    }

    return start + replaced + "</span>";
}

function removeThis( element ) {
    element.remove();
}

function addKillMessage( msg ) {
    let child = $( "<text>" + formatMsg( msg ) + "<br></text>" );
    killmessagesBox.append( child );
    child.delay ( 11000 ).fadeOut( 4000, child.remove );
}

function openMapMenu( mylang, mapdatasjson ) {
    mapMenuDiv.show( 1000 );
    showingMapMenu = true;
    language = mylang;
    lastMapName = "";
    votedMapName = "";
    normalMapsList.empty();
    bombMapsList.empty();
    votingMapsList.empty();
    mapDatas = JSON.parse( mapdatasjson );
    for ( let i = 0; i < mapDatas.length; ++i ) {
        let element = $( "<div>" + mapDatas[i].Name + "</div>" );
        if ( mapDatas[i].Type === 0 )
            normalMapsList.append( element );
        else
            bombMapsList.append( element );
    }
    for ( let i = 0; i < votings.length; ++i ) {
        votingMapsList.append( $( "<div>" + votings[i].name + "</div>" ) );
    }
    normalMapsList.selectable( "refresh" );
    bombMapsList.selectable( "refresh" );
    votingMapsList.selectable( "refresh" );
}

function closeMapMenu() {
    showingMapMenu = false;
    mapMenuDiv.hide(500);
}

function mapVotingsComparer( a, b ) {
    return a.votes > b.votes ? 1 : -1;
}

function sortMapVotings() {
    votings.sort( mapVotingsComparer );
}

function addMapToVoting( mapname ) {
    votings.push( { name: mapname, votes: 1 } );
    let element = $( "<div>" + votings.length + ". " + mapname + " (1)</div>" );
    mapVotingDiv.append( element );
    if ( showingMapMenu ) {
        votingMapsList.append( $( "<div>" + mapname + "</div>" ) );
        votingMapsList.selectable( "refresh" );
    }
    if ( votedMapName === mapname ) {
        element.addClass( "mapvoting_selected" );
        votedMapName = "";
    }
}

function refreshMapVotingNames() {
    let children = mapVotingDiv.children();
    for ( let i = 0; i < children.length; ++i ) {
        if ( i in votings ) {
            children.eq( i ).text( ( i + 1 ) + ". " + mapname + " (" + votes[i].votings + ")" );
        } else
            votings.splice( i );
    }
}

function removeVoteFromMap( mapname ) {
    for ( let i = 0; i < votings.length; ++i ) {
        if ( votings[i].name === mapname ) {
            if ( --votings[i].votes <= 0 ) {
                votings.splice( i, 1 );
                mapVotingDiv.children().eq( i ).remove();
            }
            break;
        }
    }
    refreshMapVotingNames();
}

function addVoteToMapVoting( mapname, oldmapname ) {
    if ( votedMapName === mapname )
        $( ".mapvoting_selected" ).removeClass( "mapvoting_selected" );
    if ( typeof oldmapname !== "undefined" )
        removeVoteFromMap( oldmapname );
    let foundmap = false;
    for ( let i = 0; i < votings.length && !foundmap; ++i ) {
        if ( votings[i].name === mapname ) {
            ++votings[i].votes;
            let element = mapVotingDiv.children().eq( i );
            element.text( ( i + 1 ) + ". " + mapname + "(" + votings[i].votes + ")" );
            if ( votedMapName === mapname ) {
                element.addClass( "mapvoting_selected" );
                votedMapName = "";
            }
            foundmap = true;
        }
    }
    if ( !foundmap )
        addMapToVoting( mapname );
    sortMapVotings();
}

function loadMapVotings( votingsjson ) {
    let mapsvotesdict = JSON.parse( mapsvotesjson );
    votings = [];
    for ( let key in mapsvotesdict ) {
        votings.push( { name: key, votes: mapsvotesdict[key] } );
    }
    sortMapVotings();
}

function clearMapVotings() {
    votings = [];
}

function setMapVotingCooldown() {
    votingInCooldown = true;
    mapVoteButton.disabled = true;
    setTimeout( () => { votingInCooldown = false; mapVoteButton.disabled = false; }, 1500 );
}


$( "body" ).keydown( function ( event ) {
    let key = event.which;
    // map-voting //
    if ( key >= 0x61 && key <= 0x69 ) {
        if ( votingInCooldown )
            return;
        event.preventDefault();
        let index = 9 - ( 0x69 - key ) - 1;   // -1 because of indexing starting at 0
        if ( votings.length > index ) {
            votedMapName = votings[index].name;
            mp.trigger( "onMapMenuVote", votedMapName );
            setMapVotingCooldown();
        }
    }
} );


$( document ).ready( () => {
    mapMenuDiv = $( "#mapmenu" );
    mapVotingDiv = $( "#mapvoting" );
    moneyText = $( "#money" );
    bloodscreen = $( "#bloodscreen" );
    killmessagesBox = $( "#kill_messages_box" );
    normalMapsList = $( "#tabs-1" );
    bombMapsList = $( "#tabs-2" );
    votingMapsList = $( "#tabs-3" );
    mapInfo = $( "#map_info" );
    mapVoteButton = $( "#choose_map_button" );

    $( "#tabs" ).tabs( {
        collapsible: true,
        heightStyle: "fill"
    } );

    $( ".tab_list" ).each( function ( index, value ) {
        $( this ).selectable( {
            selected: function ( event, ui ) {
                $( ui.selected ).addClass( "ui-selected" ).siblings().removeClass( "ui-selected" );
                let mapname = ui.selected.innerHTML;
                for ( let i = 0; i < mapDatas.length; ++i ) {
                    if ( mapname === mapDatas[i].Name ) {
                        lastMapName = mapname;
                        mapInfo.html( mapDatas[i].Description[language] );
                        return;
                    }
                }
            },
            autoRefresh: false
        } );
    } );

    mapVoteButton.click( function ( event ) {
        event.preventDefault();
        if ( votingInCooldown )
            return;
        if ( lastMapName !== "" ) {
            votedMapName = lastMapName;
            mp.trigger( "onMapMenuVote", votedMapName );
            setMapVotingCooldown();
        }
    } );
} );