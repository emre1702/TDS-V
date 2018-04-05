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
    hitsound: true,
    bloodscreen: true
}

mp.events.add( "onPlayerSettingChange", ( setting, value ) => {
    switch ( setting as PlayerSetting ) {
        case PlayerSetting.LANGUAGE:
            settingsdata.language = value;
            mp.storage.data.language = value;
            callRemoteCooldown( "onPlayerLanguageChange", value );
            if ( mainbrowserdata.angularloaded ) {
                loadOrderNamesInBrowser( JSON.stringify( getLang( "orders" ) ) );
                mainbrowserdata.angular.call( `syncLanguage('${settingsdata.language}');` );
            }
            break;

        case PlayerSetting.HITSOUND:
            settingsdata.hitsound = value;
            mp.storage.data.hitsound = value;
            break;

        case PlayerSetting.BLOODSCREEN:
            settingsdata.bloodscreen = value;
            mp.storage.data.blodscreen = value;
            break;
    } 
} );


function loadSettings() {
    let savedlang = mp.storage.data.language;
    let savedhitsound = mp.storage.data.hitsound;
    let savedbloodscreen = mp.storage.data.bloodscreen;

    if ( typeof savedlang !== "undefined" )
        settingsdata.language = savedlang;
    else {
        let langnumber = mp.game.invoke( "E7A981467BC975BA", 0 );
        if ( langnumber == 2 )
            settingsdata.language = "" + Language.GERMAN;
    }

    if ( typeof savedhitsound !== "undefined" )
        settingsdata.hitsound = savedhitsound;

    if ( typeof savedbloodscreen !== "undefined" )
        settingsdata.bloodscreen = savedbloodscreen;
}

function getLanguage() {
    return settingsdata.language;
}