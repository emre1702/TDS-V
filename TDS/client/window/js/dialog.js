let dialog = null;

function showDialog( content ) {
    if ( dialog !== null )
        dialog.remove();

    dialog = document.createElement( "div" );
    dialog.className = "customdialog";
    let closespan = document.createElement( "span" );
    closespan.className = "customdialog-close";
    let contentelement = document.createElement( "p" );
    contentelement.className = "customdialog-content";
    let contentnode = document.createTextNode ( content );
    contentelement.appendChild ( contentnode );

    dialog.appendChild ( closespan );
    dialog.appendChild ( contentelement );

    closespan.onclick = function() {
        closeDialog();
    };
}

function closeDialog() {
    dialog.remove();
    dialog = null;
}