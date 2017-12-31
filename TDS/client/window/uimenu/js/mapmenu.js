let mainMenu = new PopupMenu( "Map-Manager", "Choose a type", [
    new MenuItem( "Normal", "Normal maps" ).Submenu( normalMapsMenu ),
    new MenuItem( "Bomb", "Bomb maps" ).Submenu( bombMapsMenu )
] );

let normalMapsMenu = new PopupMenu( "Normal-maps", "Choose a map", [] );
let bombMapsMenu = new PopupMenu( "Bomb-maps", "Choose a map", [] );

var app = new Vue({
	el: '#container',
	data: {
        menu: mainMenu
	}
});
