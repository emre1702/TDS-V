let mapcreatormenu = $( "#mapcreatormenu" );
let mapcreatortabs = [$( "#general_tabcontent" ), $( "#description_tabcontent" ), $( "#team_spawns_tabcontent" ), $( "#map_limit_tabcontent" ), $( "#map_points_tabcontent" )];
let mapcreatortables = [$( "#team_spawn_table" ), $( "#map_limit_table" )];
let teamnumberteamspawns = $( "#team_number_teamspawns" );
let selectedrow = null;
let addpositionfor = 0;

setElementDraggable( mapcreatormenu[0] );

function openMapCreatorMenu( index ) {
    for ( let i = 0; i < mapcreatortabs.length; ++i )
        mapcreatortabs[i].removeClass( "active" ).hide();

    mapcreatortabs[index].addClass( "active" ).show();

    if ( selectedrow !== null )
        selectedrow.removeAttr( "bgcolor" );
    selectedrow = null;
}
openMapCreatorMenu( 0 );

$( "tr:not(:first-child)" ).click( function () {
    if ( selectedrow !== null )
        selectedrow.removeAttr( "bgcolor" );

    if ( selectedrow === null || selectedrow[0] !== $( this )[0] )
        selectedrow = $( this ).attr( "bgcolor", "#04074eC7" );
} );

function AddCurrentPosition( number ) {
    addpositionfor = number;
    mp.trigger( "requestCurrentPositionForMapCreator" );
}

function GotoPosition( number ) {
    if ( number != 2 && selectedrow === null )
        return;

    let x, y, z;
    switch ( number ) {
        case 0:
            x = selectedrow.find( "td:eq(1)" ).text();
            y = selectedrow.find( "td:eq(2)" ).text();
            z = selectedrow.find( "td:eq(3)" ).text();
            break;
        case 1:
            x = selectedrow.find( "td:eq(0)" ).text();
            y = selectedrow.find( "td:eq(1)" ).text();
            z = selectedrow.find( "td:eq(2)" ).text();
            break;
        case 2:
            x = $( "#map_center_x" ).text().substr( 3 );
            y = $( "#map_center_y" ).text().substr( 3 );
            z = $( "#map_center_z" ).text().substr( 3 );
            break;
    }
    mp.trigger( "gotoPositionByMapCreator", Number.parseFloat( x ), Number.parseFloat( y ), Number.parseFloat( z ) );
}

function RemovePosition() {
    if ( selectedrow !== null )
        selectedrow.remove();
}

function LoadPositionFromClient( x, y, z ) {
    let row;
    switch ( addpositionfor ) {
        case 0:
            row = $( "<tr><td>" + teamnumberteamspawns.val() + "</td><td>" + x + "</td><td>" + y + "</td><td>+" + z + "</td></tr>" );
            mapcreatortables[addpositionfor].append( row );
            break;
        case 1:
            row = $( "<tr><td>" + x + "</td><td>" + y + "</td><td>+" + z + "</td></tr>" );
            mapcreatortables[addpositionfor].append( row );
            break;
        case 2:
            $( "#map_center_x" ).text( "X: "+x );
            $( "#map_center_y" ).text( "Y: "+y );
            $( "#map_center_z" ).text( "Z: "+z );
            break;
    }
    
}

$( document ).keyup( function ( e ) {
    if ( e.which === 0x2E ) {       // delete
        if ( selectedrow === null )
            return;
        selectedrow.remove();
        selectedrow = null;
    }
} );