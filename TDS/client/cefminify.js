var fs = require( "fs" );
var html = fs.readFileSync( "window/main/main.html", "utf8" );
var javascript = fs.readFileSync( "window/main/main.js", "utf8" );
var inliner = require( "web-resource-inliner" );
var path = require( "path" );
var htmlminify = require( "html-minifier" ).minify;
let toes5 = require( "babel-core" ).transform;
let uglify = require( "uglify-js" );

let files = [
    { path: "window/chat/", html: "chat.html", js: ["chat.js"], reserved: ["chatAPI"] },
    { path: "window/choice/", html: "choice.html", js: ["choice.js"], reserved: ["setLobbyChoiceLanguage"] },
    { path: "window/main/", html: "main.html", js: ["main.js"], 
        reserved: ["setMoney", "playSound", "showBloodscreen", "addKillMessage", "alert", "openMapMenu", "closeMapMenu", "addVoteToMapVoting", "loadMapVotings", "clearMapVotings", "loadFavouriteMaps",
            "toggleCanVoteForMapWithNumpad", "loadOrderNames", "loadUserName" ]
    },
    { path: "window/registerlogin/", html: "registerlogin.html", js: ["registerlogin.js"], reserved: ["loadLanguage", "setLoginPanelData" ] }
];
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

for ( let i = 0; i < files.length; ++i ) {
    let javascript = "";
    for ( let j = 0; j < files[i].js.length; ++j ) {
        javascript += fs.readFileSync( path.resolve( files[i].path, files[i].js[j] ), "utf8" ) + "\n";
    }
    javascript = toes5( javascript, toes5settings ).code;
    minifysettings.mangle.reserved = files[i].reserved;
    javascript = uglify.minify( javascript, minifysettings ).code;
    fs.writeFile( path.resolve( files[i].path, "index.js" ), javascript, ( error ) => {
        let html = fs.readFileSync( path.resolve( files[i].path, files[i].html ), "utf8" );
        html = htmlminify( html, htmlminifysettings );
        fs.writeFileSync( path.resolve( files[i].path, "index.html" ), html, ( error ) => {
            console.log( error );
        } );
    } );
}