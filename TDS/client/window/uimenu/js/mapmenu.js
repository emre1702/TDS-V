let mainMenu = new PopupMenu( "Map-Manager", "Choose a type", [
    new MenuItem( "Normal", "Normal maps" ).Submenu( normalMapsMenu ),
    new MenuItem( "Bomb", "Bomb maps" ).Submenu( bombMapsMenu ),
    new MenuItem( 'Close' ).Style( 'red button' ).Click( () => { mp.trigger( "closeMapVotingMenu" ); } )
] );

let normalMapsMenu = new PopupMenu( "Normal-maps", "Choose a map", [
    new MenuItem( 'Back' ).Style( 'red button' ).Back()
] );

let bombMapsMenu = new PopupMenu( "Bomb-maps", "Choose a map", [
    new MenuItem( 'Back' ).Style( 'red button' ).Back()
] );

function chooseMap ( mapname ) {
    //let mapname = mainMenu.currentItem().submenu.currentItem().key;
    mp.trigger( "chooseMap", mapname );
}

function addNormalMaps ( jsonstring ) {
    let array = JSON.parse( jsonstring );
    for ( let i = 0; i < array.length; ++i ) {
        normalMapsMenu.items.push( new MenuItem( array[i] ).Click( chooseMap ( array[i] ) ) );
    }
}

function addBombMaps() {
    let array = JSON.parse( jsonstring );
    for ( let i = 0; i < array.length; ++i ) {
        bombMapsMenu.items.push( new MenuItem( array[i] ).Click( chooseMap( array[i] ) ) );
    }
}

var app = new Vue({
	el: '#container',
	data: {
        menu: mainMenu
	}
});
