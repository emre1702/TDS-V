let dialog = null;

function showDialog( content ) {
    if ( dialog !== null )
        dialog.remove();

    dialog = document.createElement( "div" );
    dialog.className = "customdialog";
    let closespan = document.createElement( "span" );
    closespan.className = "customdialog-close";
    closespan.innerHTML = "&times;";
    let contentelement = document.createElement( "div" );
    contentelement.className = "customdialog-content";
    contentelement.innerHTML = content;

    dialog.appendChild( contentelement );
    dialog.appendChild( closespan );

    closespan.onclick = function() {
        closeDialog();
    };

    document.body.appendChild( dialog );
}

function closeDialog() {
    dialog.remove();
    dialog = null;
}