/* /// <reference path="types-ragemp/index.d.ts" />

let settings = { language: "english" };

function loadSettings() {
	if ( API.doesSettingExist( "TDS_settings" ) ) {
		settings = JSON.parse( API.getSetting( "TDS_settings" ) )
		if ( settings.language != "english" && languagelist[settings.language] != undefined ) {
			languagesetting = settings.language;
			mp.events.callRemote( "onPlayerLanguageChange", settings.language );
		}
	} else
		API.setSetting( "TDS_settings", JSON.stringify( settings ) );
}

function changeSetting ( index, value ) {
	settings[index] = value;
	API.setSetting( "TDS_settings", JSON.stringify( settings ) );
}
*/

var settingsdata = {
    language: "ENGLISH",
    hitsound: true
}

mp.events.add( "onPlayerSettingChange", ( setting, value ) => {
    switch ( setting as PlayerSetting ) {
        case PlayerSetting.LANGUAGE:
            settingsdata.language = value;
            mp.storage.data.language = value;
            callRemoteCooldown( "onPlayerLanguageChange", value );
            loadOrderNamesInBrowser( JSON.stringify( getLang( "orders" ) ) );
            break;

        case PlayerSetting.HITSOUND:
            mp.storage.data.hitsound = value;
            break;
    } 
} );


function loadSettings() {
    let savedlang = mp.storage.data.language;
    let savedhitsound = mp.storage.data.hitsound;

    if ( typeof savedlang !== "undefined" )
        settingsdata.language = savedlang;
    else {
        let langnumber = mp.game.invoke( "E7A981467BC975BA", 0 );
        if ( langnumber == 2 )
            settingsdata.language = "" + Language.GERMAN;
    }

    if ( typeof savedhitsound !== "undefined" )
        settingsdata.hitsound = savedhitsound;
}

function getLanguage() {
    return settingsdata.language;
}