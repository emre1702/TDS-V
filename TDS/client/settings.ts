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