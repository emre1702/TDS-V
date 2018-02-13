var fs = require( "fs" );
var html = fs.readFileSync( "window/main/main.html", "utf8" );
var javascript = fs.readFileSync( "window/main/main.js", "utf8" );
var inliner = require( "web-resource-inliner" );
var path = require( "path" );
var htmlminify = require( "html-minifier" ).minify;
let toes5 = require( "babel-core" ).transform;
let uglify = require( "uglify-js" );
let cleancssrequire = require( "clean-css" );

let files = [
    { path: "window/choice/", html: "choice.html", js: ["choice.js"], css: ["window/choice/choice.css"], reserved: ["setLobbyChoiceLanguage"] },
    { path: "window/main/", html: "main.html", js: ["main.js", "mapmanager.js", "chat.js"], css: ["window/main/chat.css", "window/main/main.css", "window/main/mapmanager.css"],
        reserved: ["setMoney", "playSound", "showBloodscreen", "addKillMessage", "alert", "openMapMenu", "closeMapMenu", "addVoteToMapVoting", "loadMapVotings", "clearMapVotings", "loadFavouriteMaps",
            "toggleCanVoteForMapWithNumpad", "loadOrderNames", "chatAPI", "loadUserName" ]
    },
    { path: "window/registerlogin/", html: "registerlogin.html", js: ["registerlogin.js"], css: [], reserved: ["loadLanguage", "setLoginPanelData"] },
    { path: "window/mapcreator/", html: "mapcreator.html", js: ["mapcreator.js"], css: ["window/mapcreator/mapcreator.css"],
        reserved: ["openMapCreatorMenu", "gotoPosition", "addCurrentPosition", "removePosition", "loadPositionFromClient", "loadLanguage", "sendMap", "checkMapName", "loadResultOfMapNameCheck",
        "toggleBombContents" ]
    }
];
let standalonejsfiles = [
    { path: "window/js/", js: ["copyclipboard.js", "dialog.js", "draggable.js"],
        reserved: [ ["copyTextToClipboard"], ["showDialog", "closeDialog"], ["setElementDraggable"] ]
    },
    { path: "window/jquery-ui/", js: ["jquery-ui.autocomplete.js"], reserved: [ ["addAutocomplete"] ] } 
];
let standalonecssfiles = [{ path: "window/css/", css: ["dialog.css", "scrollbar.css", "style.css", "toggleswitch.css"] }];

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
let toes5settings = {
    comments: false,
    minified: true,
    presets: [
        ["env", {
            uglify: true
        }]
    ]
};
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
    level: 2
};

let cleancss = new cleancssrequire( cssminifysettings );

for ( let i = 0; i < files.length; ++i ) {
    let javascript = "";
    for ( let j = 0; j < files[i].js.length; ++j ) {
        javascript += fs.readFileSync( path.resolve( files[i].path, files[i].js[j] ), "utf8" ) + "\n";
    }
    javascript = toes5( javascript, toes5settings ).code;
    minifysettings.mangle.reserved = files[i].reserved;
    javascript = uglify.minify( javascript, minifysettings ).code;
    fs.writeFile( path.resolve( files[i].path, "index.js" ), javascript, ( error ) => {
        if ( error )
            console.log( error );
        else {
            let html = fs.readFileSync( path.resolve( files[i].path, files[i].html ), "utf8" );
            html = htmlminify( html, htmlminifysettings );
            fs.writeFile( path.resolve( files[i].path, "index.html" ), html, ( error ) => {
                if ( error )
                    console.log( error );
                else {
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