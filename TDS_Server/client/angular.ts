class Angular {
    browser: BrowserMp;

    constructor() {}

    load( path ) {
        if ( this.browser !== undefined )
            this.browser.destroy();
        this.browser = mp.browsers.new( path );
    }

    listen( eventName, callback ) {
        mp.events.add( eventName, ( responseId, ...args ) => {
            let response = callback( ...args );

            if ( responseId !== -1 ) {
                if ( typeof response === "object" ) {
                    response = JSON.stringify( response );
                    response = response.replace( /\\"/g, '\\\\"' );
                }

                this.browser.execute( "RageJS.sendFuncResponseToRAGE(" + responseId + ",'" + response + "');" );
            }
        } );
    }

    destroy() {
        if ( this.browser ) {
            this.browser.destroy();
            this.browser = undefined;
        }
    }

    /**
     * Only for calling functions in Angular-browser, which are added with "listen"
     * @param str
     */
    call( str: string ) {
        if ( this.browser ) {
            this.browser.execute( "RageAngular."+str );
        }
    }

    execute( str: string ) {
        if ( this.browser )
            this.browser.execute( str );
        
    }

    isActive() {
        return ( !!this.browser);
    }
}