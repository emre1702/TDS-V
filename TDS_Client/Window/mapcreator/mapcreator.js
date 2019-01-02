let mapcreatormenu = $( "#mapcreatormenu" );
let mapcreatortabs = [$( "#general_tabcontent" ), $( "#description_tabcontent" ), $( "#team_spawns_tabcontent" ), $( "#map_limit_tabcontent" ),
    $( "#map_points_tabcontent" ), $("#bomb_places_tabcontent" ), $( "#last_tabcontent" )];
let mapcreatortables = [$( "#team_spawns_table" ), $( "#map_limit_table" ), undefined, $("#bomb_places_table")];
let teamnumberteamspawns = $( "#team_number_teamspawns" );
let selectedrow = null;
let addpositionfor = 0;
let mapNames = null;
let langdata;
let sendbuttondisabled = true;

setElementDraggable( mapcreatormenu[0] );

function openMapCreatorMenu( index ) {
    for ( let i = 0; i < mapcreatortabs.length; ++i )
        mapcreatortabs[i].removeClass( "active" ).hide();

    mapcreatortabs[index].addClass( "active" ).show();

    if ( selectedrow !== null ) {
        selectedrow.removeAttr( "bgcolor" );
        selectedrow = null;
    }
}
openMapCreatorMenu( 0 );

function clickOnRow () {
    if ( selectedrow !== null )
        selectedrow.removeAttr( "bgcolor" );

    if ( selectedrow === null || selectedrow[0] !== $( this )[0] )
        selectedrow = $( this ).attr( "bgcolor", "#04074eC7" );
    else
        selectedrow = null;
}

function addCurrentPosition( number ) {
    if ( number === 0 )
        if ( !teamnumberteamspawns.val() )
            return;
    addpositionfor = number;
    mp.trigger( "requestCurrentPositionForMapCreator" );
}

function gotoPosition( number ) {
    if ( number !== 2 && selectedrow === null )
        return;

    let x, y, z;
    let rot = "";
    let row;
    switch ( number ) {
        case 0:
            row = selectedrow.find( "td" );
            x = row.eq( 1 ).text();
            y = row.eq( 2 ).text();
            z = row.eq( 3 ).text();
            rot = row.eq( 4 ).text();
            break;
        case 1:
        case 3:
            row = selectedrow.find( "td" );
            x = row.eq( 0 ).text();
            y = row.eq( 1 ).text();
            z = row.eq( 2 ).text();
            break;
        case 2:
            x = $( "#map_center_x" ).text().substr( 3 );
            y = $( "#map_center_y" ).text().substr( 3 );
            z = $( "#map_center_z" ).text().substr( 3 );
            break;
    }
    mp.trigger( "gotoPositionByMapCreator", parseFloat( x ), parseFloat( y ), parseFloat( z ), parseFloat ( rot ) );
}

function removePosition() {
    if ( selectedrow !== null ) {
        selectedrow.remove();
        disableSendButton();
    }
}

function loadPositionFromClient( x, y, z, rot ) {
    let row;
    switch ( addpositionfor ) {
        case 0:
            row = $( "<tr><td>" + teamnumberteamspawns.val() + "</td><td>" + x + "</td><td>" + y + "</td><td>+" + z + "</td><td>" + rot + "</td></tr>" );
            row.click( clickOnRow );
            mapcreatortables[addpositionfor].append( row );
            break;
        case 1:
        case 3:
            row = $( "<tr><td>" + x + "</td><td>" + y + "</td><td>+" + z + "</td></tr>" );
            row.click( clickOnRow );
            mapcreatortables[addpositionfor].append( row );
            break;
        case 2:
            $( "#map_center_x" ).text( "X: "+x );
            $( "#map_center_y" ).text( "Y: "+y );
            $( "#map_center_z" ).text( "Z: "+z );
            break;
    }
    disableSendButton();
}

function loadLanguage( lang ) {
    langdata = JSON.parse( lang );
    $( "[data-lang]" ).each( function () {
        $( this ).html( langdata[$( this ).attr( "data-lang" )] );
    } );
    $( "[data-lang-placeholder]" ).each( function () {
        $( this ).attr( "placeholder", langdata[$( this ).attr( "data-lang-placeholder" )] );
    } );
}

function sendMap() {
    let obj = {
        Name: $( "#map_name_input" ).val(),
        Type: typeof $( "#normal_radio_switch" ).attr( "checked" ) !== "undefined" ? "normal" : "bomb",
        Descriptions: {
            English: $( "#english_description" ).attr( "placeholder" ),
            German: $( "#german_description" ).attr( "placeholder" )
        },
        MinPlayers: parseInt( $( "#min_players" ).val() ),
        MaxPlayers: parseInt( $( "#max_players" ).val() ),
        MapSpawns: [],
        MapLimitPositions: [],
        MapCenter: {},
        BombPlaces: []
    };

    let english = $( "#english_description" ).val();
    if ( english !== "" )
        obj.Descriptions.English = english;

    let german = $( "#german_description" ).val();
    if ( german !== "" )
        obj.Descriptions.German = german;

    $( "#team_spawns_table tr:not(:first-child)" ).each(function() {
        let row = $( this ).find( "td" );
        let team = parseFloat( row.eq( 0 ).text() );
        let x = parseFloat( row.eq( 1 ).text() );
        let y = parseFloat( row.eq( 2 ).text() );
        let z = parseFloat( row.eq( 3 ).text() );
        let rot = parseFloat( row.eq( 4 ).text() );
        obj.MapSpawns.push( { Team: team, X: x, Y: y, Z: z, Rot: rot } );
    } );

    $( "#map_limit_table tr:not(:first-child)" ).each( function() {
        let row = $( this ).find( "td" );
        let x = parseFloat( row.eq( 0 ).text() );
        let y = parseFloat( row.eq( 1 ).text() );
        let z = parseFloat( row.eq( 2 ).text() );
        obj.MapLimitPositions.push( { X: x, Y: y, Z: z } );
    } );

    obj.MapCenter.X = parseFloat( $( "#map_center_x" ).text().substr( 3 ) );
    obj.MapCenter.X = isNaN( obj.MapCenter.X ) ? -9999 : obj.MapCenter.X;
    obj.MapCenter.Y = parseFloat( $( "#map_center_y" ).text().substr( 3 ) );
    obj.MapCenter.Y = isNaN( obj.MapCenter.Y ) ? -9999 : obj.MapCenter.Y;
    obj.MapCenter.Z = parseFloat( $( "#map_center_z" ).text().substr( 3 ) );
    obj.MapCenter.Z = isNaN( obj.MapCenter.Z ) ? -9999 : obj.MapCenter.Z;

    if ( obj.Type === "bomb" ) {
        $( "#bomb_places_table tr:not(:first-child)" ).each( function () {
            let row = $( this ).find( "td" );
            let x = parseFloat( row.eq( 0 ).text() );
            let y = parseFloat( row.eq( 1 ).text() );
            let z = parseFloat( row.eq( 2 ).text() );
            obj.BombPlaces.push( { X: x, Y: y, Z: z } );
        } );
    }

    mp.trigger( "sendMapFromCreator", JSON.stringify( obj ) );
}

function disableSendButton() {
    if ( !sendbuttondisabled ) {
        $( "#send_map_button" ).attr( "disabled", "" );
        sendbuttondisabled = true;
    }
}

function checkMap() {
    let name = $( "#map_name_input" ).val();
    if ( name.length < 6 ) {
        showDialog( langdata["map_name_atleast"].replace( "{1}", "6" ) );
        return false;
    }

    let minplayers = parseInt( $( "#min_players" ).val() );
    if ( isNaN( minplayers ) || minplayers < 0 || minplayers > 99 ) {
        showDialog( langdata["map_min_players_min_max"].replace( "{1}", "0" ).replace( "{2}", "99" ) );
        return false;
    }

    let maxplayers = parseInt( $( "#max_players" ).val() );
    if ( isNaN( maxplayers ) || maxplayers < 2 || maxplayers > 999 ) {
        showDialog( langdata["map_max_players_min_max"].replace( "{1}", "2" ).replace( "{2}", "999" ) );
        return false;
    } 

    let mapspawnrows = $( "#team_spawns_table tr:not(:first-child)" );
    if ( mapspawnrows.length < 4 ) {     
        showDialog( langdata["map_team_spawns_min_per_team"].replace( "{1}", "4" ) );
        return false;
    }

    let maplimitrows = $( "#map_limit_table tr:not(:first-child)" );
    if ( maplimitrows.length !== 0 && ( maplimitrows.length < 3 || maplimitrows.length > 20 ) ) {
        showDialog( langdata["map_limit_min_max"].replace( "{1}", "3" ).replace ( "{2}", 20 ) );
        return false;
    }

    // bomb mode //
    if ( typeof $( "#bomb_radio_switch" ).attr( "checked" ) !== "undefined" ) {
        let bombplacesrows = $( "#bomb_places_table tr:not(:first-child)" );
        if ( bombplacesrows.length === 0 ) {
            showDialog( langdata["bomb_places_min"].replace( "{1}", 1 ) );
            return false;
        }
    }

    sendbuttondisabled = false;
    $( "#send_map_button" ).removeAttr( "disabled" );
    return true;
}

function checkMapName() {
    mp.trigger( "checkMapName", $("#map_name_input").val() );
}

function loadResultOfMapNameCheck( alreadyinuse ) {
    if ( !alreadyinuse )
        checkMap();
    else {
        alert( langdata['map_name_already_used'] );
        showDialog( langdata["map_name_already_used"] );
    }
}

function toggleBombContents( bool ) {
    if ( bool )
        $( ".bomb_content" ).show();
    else
        $( ".bomb_content" ).hide();
}

$( document ).keyup( function ( e ) {
    if ( e.which === 0x2E ) {       // delete
        if ( selectedrow === null )
            return;
        selectedrow.remove();
        selectedrow = null;
    }
} );