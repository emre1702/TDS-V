"use strict";

let files = [
	"globals.js",
	"logging.js",
	"chat.js",
	"money.js",
	"utility.js",
	"timer.js",
	"scoreboard.js",
	"draw/draw.js",
	"damagesys/damagesys.js",
	"damagesys/hitsound.js",
    "language/language.js",
	"registerlogin/registerlogin.js",

	"lobby/blips.js",
	"lobby/bomb.js",
	"lobby/camera.js",
	"lobby/choice.js",
	"lobby/countdown.js",
	"lobby/maplimit.js",
	"lobby/mapvoting.js",
	"lobby/round.js",
	"lobby/roundinfo.js",
	"lobby/settings.js",
	"lobby/spectate.js",
	"lobby/lobbyevents.js",
	"lobby/roundevents.js"
];
let code = "";
let htmlcode = "";
let warning = "/* Copyright by Bonus!\n * Stealing is not allowed! */";

let fs = require("fs");
let toes5 = require ( "babel-core" ).transform;

for ( let i = 0; i < files.length; i++ ) {
    code += fs.readFileSync ( files[i], "utf-8" );  
}
code = toes5 ( code, { 
    comments: false,
    minified: true,
    presets: ["es2015"]
} ).code;

let uglify = require("uglify-js");
let result = uglify.minify ( code, {
    compress : {
        sequences: true,
        warnings: true,
        properties: true,
        dead_code: true,
        unused: false,
        conditionals: true,
        booleans: true,
        loops: true,
        toplevel: true,
        if_return: true,
        join_vars: true,
        collapse_vars: true,
        reduce_vars: true
    },
    mangle: {
		reserved: ["loginFunc", "registerFunc", "getLoginPanelData", "changeLanguage", "getLobbyChoiceLanguage", "joinArena", "commitMessage", "onFocusChange", "onChatLoad" ] 
    },
    toplevel: true
} );

if ( result.warnings !== undefined ) 
    console.log ( "warnings "+JSON.stringify ( result.warnings ) );

if ( result.error === undefined ) {
    fs.writeFile ( "script.js", warning+"\n"+result.code, function ( err ) {
        if ( err ) {
            console.log ( "Write "+err );
        } else 
            console.log ( "Erledigt" );
    } );
} else 
    console.log ( "minifiy "+JSON.stringify ( result.error ) );


