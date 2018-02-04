mp.events.add( 'browserDomReady', ( browser: MpBrowser ) => {

    if ( browser === loginpanel.loginbrowser )
        browser.execute( "setLoginPanelData ( `" + loginpanel.name + "`, " + loginpanel.isregistered + ", `" + JSON.stringify( getLang( "loginregister" ) ) + "` );" );

    else if ( browser === mainbrowserdata.browser )
        loadOrderNamesInBrowser( JSON.stringify( getLang( "orders" ) ) );

    else if ( browser === lobbychoicedata.browser )
        lobbychoicedata.browser.execute( "setLobbyChoiceLanguage (`" + JSON.stringify( getLang( "lobby_choice" ) ) + "`)" );

    else if ( browser === mapcreatordata.browser )
        mapcreatordata.browser.execute( "loadLanguage (`" + JSON.stringify( getLang( "mapcreator_menu" ) ) + "`);" );
} );