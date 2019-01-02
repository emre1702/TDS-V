var fs = require( "fs" );
var html = fs.readFileSync( "main/main.html", "utf8" );
var javascript = fs.readFileSync( "main/main.js", "utf8" );
var inliner = require( "web-resource-inliner" );
var path = require( "path" );
var htmlminify = require( "html-minifier" ).minify;
let toes5 = require( "@babel/core" ).transform;
let uglify = require( "uglify-js" );
let cleancssrequire = require( "clean-css" );
console.log("test1");
let files = [
    { path: "choice/", html: "choice.html", js: ["choice.js"], css: ["choice/choice.css"], reserved: ["setLobbyChoiceLanguage"] },
    { path: "main/", html: "main.html", js: ["main.js", "mapmanager.js", "chat.js", "roundend.js"], 
        css: ["main/chat.css", "main/main.css", "main/mapmanager.css", "main/roundend.css"],
        reserved: ["setMoney", "playSound", "showBloodscreen", "addKillMessage", "alert", "openMapMenu", "closeMapMenu", "addVoteToMapVoting", "loadMapVotings", "clearMapVotings", "loadFavouriteMaps",
            "toggleCanVoteForMapWithNumpad", "loadOrderNames", "chatAPI", "loadUserName", "loadNamesForChat", "addNameForChat", "removeNameForChat", "showRoundEndReason", "hideRoundEndReason", "loadMyMapRatings"]
    },
    { path: "registerlogin/", html: "registerlogin.html", js: ["registerlogin.js"], css: [], reserved: ["loadLanguage", "setLoginPanelData"] },
    { path: "mapcreator/", html: "mapcreator.html", js: ["mapcreator.js"], css: ["mapcreator/mapcreator.css"],
        reserved: ["openMapCreatorMenu", "gotoPosition", "addCurrentPosition", "removePosition", "loadPositionFromClient", "loadLanguage", "sendMap", "checkMapName", "loadResultOfMapNameCheck",
        "toggleBombContents" ]
    }
];
console.log("test2");
let standalonejsfiles = [
    { path: "js/", js: ["copyclipboard.js", "dialog.js", "draggable.js", "starrating.js"],
        reserved: [["copyTextToClipboard"], ["showDialog", "closeDialog"], ["setElementDraggable"],
        ["SimpleStarRating", "attr", "disable", "enable", "setCurrentRating", "setDefaultRating", "onrate", "showRating", "showCurrentRating", "showDefaultRating", "clearRating", "starClick"]]
    },
    { path: "jquery-ui/", js: ["jquery-ui.autocomplete.js"], reserved: [ ["addAutocomplete"] ] } 
];
let standalonecssfiles = [{ path: "css/", css: ["dialog.css", "scrollbar.css", "style.css", "toggleswitch.css", "starrating.css"] }];
console.log("test3");
let minifysettings = {
    compress: {
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
        reserved: []
    },
    toplevel: true
};
console.log("test5");
let toes5settings = {
    comments: false,
    minified: true,
    presets: [
        ["@babel/preset-env", {}]
    ]
};
console.log("test6");
let htmlminifysettings = {
    caseSensitive: true,
    collapseWhitespace: true,
    collapseInlineTagWhitespace: true,
    collapseBooleanAttributes: true,
    removeRedundantAttributes: true,
    useShortDoctype: true,
    removeEmptyAttributes: true,
    removeOptionalTags: true,
    minifyCSS: true,
    minifyURLs: true,
    removeComments: true,
    removeAttributeQuotes: true,
    removeScriptTypeAttributes: true,
    removeStyleLinkTypeAttributes: true
};
let cssminifysettings = {
    level: 2,
    rebase: false
};
console.log("test7");

let cleancss = new cleancssrequire( cssminifysettings );
console.log("test8");
for ( let i = 0; i < files.length; ++i ) {
	console.log("test9");
    let javascript = "";
    for ( let j = 0; j < files[i].js.length; ++j ) {
        javascript += fs.readFileSync( path.resolve( files[i].path, files[i].js[j] ), "utf8" ) + "\n";
    }
	console.log("test10");
    javascript = toes5( javascript, toes5settings ).code;
	console.log("test10.1");
    minifysettings.mangle.reserved = files[i].reserved;
	console.log("test10.2");
    javascript = uglify.minify( javascript, minifysettings ).code;
	console.log("test11");
    fs.writeFile( path.resolve( files[i].path, "index.js" ), javascript, ( error ) => {
		console.log("test12");
        if ( error )
            console.log( error );
        else {
			console.log("test13");
            let html = fs.readFileSync( path.resolve( files[i].path, files[i].html ), "utf8" );
            html = htmlminify( html, htmlminifysettings );
			console.log("test14");
            fs.writeFile( path.resolve( files[i].path, "index.html" ), html, ( error ) => {
				console.log("test15");
                if ( error )
                    console.log( error );
                else {
					console.log("test16");
                    if ( "css" in files[i] && files[i].css.length > 0 ) {
                        cleancss.minify( files[i].css, ( error, minified ) => { 
                            if ( error )
                                console.log( error );
                            else 
                                fs.writeFile( path.resolve( files[i].path, "index.css" ), minified.styles, ( error ) => {
                                    if ( error )
                                        console.log( error );
                                } );
                        } );
                    }
                }
            } );
        }
    } );
}

for ( let i = 0; i < standalonejsfiles.length; ++i ) {
    for ( let j = 0; j < standalonejsfiles[i].js.length; ++j ) {
        let javascript = fs.readFileSync( path.resolve( standalonejsfiles[i].path, standalonejsfiles[i].js[j] ) );
        javascript = toes5( javascript, toes5settings ).code;
        minifysettings.mangle.reserved = standalonejsfiles[i].reserved[j];
        javascript = uglify.minify( javascript, minifysettings ).code;
        let newfilename = standalonejsfiles[i].js[j].slice( 0, -3 ) + ".min.js";
        fs.writeFile( path.resolve( standalonejsfiles[i].path, newfilename ), javascript, ( error ) => {
            if ( error )
                console.log( error );
        });
    }
}

for ( let i = 0; i < standalonecssfiles.length; ++i ) {
    for ( let j = 0; j < standalonecssfiles[i].css.length; ++j ) {
        let css = cleancss.minify( [standalonecssfiles[i].path + standalonecssfiles[i].css[j]], ( error, minified ) => {
            if ( error )
                console.log( error );
            else {
                let newfilename = standalonecssfiles[i].css[j].slice( 0, -4 ) + ".min.css";
                fs.writeFile( path.resolve( standalonecssfiles[i].path, newfilename ), minified.styles, ( error ) => {
                    if ( error )
                        console.log( error );
                });
            }
        } );
        
    }
}