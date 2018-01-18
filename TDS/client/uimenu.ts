let uimenudata = {
    lastbrowser: null as MpBrowser
}

function startUIMenu( uimenu: string ): MpBrowser {
    if ( uimenudata.lastbrowser != null )
        uimenudata.lastbrowser.destroy();
    uimenudata.lastbrowser = mp.browsers.new( "package://TDS-V/window/uimenu/index.html" );
    mp.events.add( "browserDomReady", ( thebrowser: MpBrowser ) => {
        if ( thebrowser == uimenudata.lastbrowser )
            uimenudata.lastbrowser.execute( "addScript ( \"" + uimenu + "\");" );
    } );
    return uimenudata.lastbrowser;
}