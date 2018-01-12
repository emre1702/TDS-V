let mapDatas;
let language = "ENGLISH";
let normalMapsList;
let bombMapsList;
let mapInfo;
let lastMapName = "";

$( "#tabs" ).tabs( {
    collapsible: true,
    heightStyle: "fill"
} ); 

$( document ).ready( function () {
    normalMapsList = $( "#tabs-1" );
    bombMapsList = $( "#tabs-2" );
    mapInfo = $( "#map_info" );

    $( ".tab_list" ).each( function ( index, value ) {
        $( this ).selectable( {
            selected: function ( event, ui ) {
                $( ui.selected ).addClass( "ui-selected" ).siblings().removeClass( "ui-selected" );
                let mapname = ui.selected.innerHTML;
                for ( let i = 0; i < mapDatas.length; ++i ) {
                    if ( mapname === mapDatas[i].Name ) {
                        lastMapName = mapname; 
                        mapInfo.html ( mapDatas[i].Description[language] );
                        return;
                    }
                }
            },
            autoRefresh: false
        } );
    } );

    $( "button" ).click( function ( event ) {
        event.preventDefault();
        var type = $( this ).attr( "id" );
        switch ( type ) {
            case "choose_map_button":
                if ( lastMapName != "" ) {
                    mp.trigger( "onMapMenuVote", lastMapName );
                    alert( lastMapName );
                }
                break;
        }
    } );
} );

function openMapMenu( mylang, mapdatasjson ) {
    language = mylang;
    lastMapName = ""
    mapDatas = JSON.parse( mapdatasjson );
    for ( let i = 0; i < mapDatas.length; ++i ) {
        let element = $( "<div>" + mapDatas[i].Name + "</div>" );
        if ( mapDatas[i].Type === 0 )
            normalMapsList.append( element );
        else
            bombMapsList.append( element );
    }
    normalMapsList.selectable("refresh");
    bombMapsList.selectable( "refresh" );
}