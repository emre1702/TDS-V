"use strict"

let files = [
   /* "language/language.js",
    "loginregister/connect.js",
    "loginregister/registerlogin.js", */
    "timer.js",
    "draw/draw.js",
];
let code = "";
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
        warnings: true,
        properties: true,
        dead_code: true,
        conditionals: true,
        booleans: true,
        loops: true,
        unused: true,
        toplevel: true,
        if_return: true,
        join_vars: true,
        collapse_vars: true,
        reduce_vars: true,
    },
    toplevel: true
} );

if ( result.warnings != undefined ) 
    console.log ( "warnings "+JSON.stringify ( result.warnings ) );

if ( result.error == undefined ) {
    fs.writeFile ( "script.js", warning+"\n"+result.code, function ( err ) {
        if ( err ) {
            console.log ( "Write "+err );
        } else 
            console.log ( "Erledigt" );
    } );
} else 
    console.log ( "minifiy "+JSON.stringify ( result.error ) );


