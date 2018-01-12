let mapDatas;
let language = 0;
let normalMapsList;
let bombMapsList;
let mapInfo;

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
                let mapname = ui.selected.value;
                for ( let i = 0; i < mapDatas.length; ++i ) {
                    if ( mapname === mapDatas[i].Name ) {
                        mapInfo.value = mapDatas[i].Description[language];
                        return;
                    }
                }
            },
            autoRefresh: false
        } );
    } );
} );

function openMapMenu( mylang, mapdatasjson ) {
    language = mylang;
    mapDatas = JSON.parse( mapdatasjson );
    for ( let i = 0; i < mapDatas.length; ++i ) {
        let element = $( "<div>" + mapDatas[i].Name + "</div>" );
        if ( mapDatas[i].Type === 0 )
            normalMapsList.append( element );
        else
            bombMapsList.append( element );
    }
    normalMapsList.refresh();
    bombMapsList.refresh();
}