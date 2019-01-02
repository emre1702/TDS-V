function addAutocomplete( elem, values, selecteventfunc, closeevent ) {
    // don't navigate away from the field on tab when selecting an item
    elem.bind( "keydown", function ( event ) {
        if ( event.keyCode === $.ui.keyCode.TAB && $( this ).data( "autocomplete" ).menu.active ) {
            event.preventDefault();
        }
    } ).autocomplete( {
        minLength: 1,
        source: function ( request, response ) {
            var term = request.term,
                results = [];
            let searchchar = term.lastIndexOf( "#" ) > term.lastIndexOf( "@" ) ? "#" : "@";
            if ( term.indexOf( searchchar ) >= 0 ) {
                term = request.term.split( new RegExp( searchchar + "\\s*" ) ).pop();
                if ( term.length > 0 ) {
                    results = $.ui.autocomplete.filter(
                        values, term );
                }
            } 
            response( results );
        },
        focus: function () {
            // prevent value inserted on focus
            return false;
        },
        select: function ( event, ui ) {
            var terms;
            let text = elem.val();
            let hashtagused = text.lastIndexOf( "#" ) > text.lastIndexOf( "@" );
            if ( hashtagused ) {
                terms = text.split( /#\s*/ );
            } else {
                terms = text.split( /@\s*/ );
                ui.item.value = "@" + ui.item.value + ":";
            }
            // remove the current input
            terms.pop();
            // add the selected item
            terms.push( ui.item.value );
            // add placeholder to get the comma-and-space at the end
            terms.push( "" );
            elem.val( terms.join( "" ) );
            if ( typeof selecteventfunc !== "undefined" )
                selecteventfunc();
            return false;
        },
        close: closeevent,
        autoFocus: true
        /*,
        _renderItem: function ( ul, item ) {
            return $( "<li>" )
                .data( "item.autocomplete", item )
                .append( "<a>@" + item.label + ":&nbsp;</a>" )
                .appendTo( ul );
        } */
    } );
}