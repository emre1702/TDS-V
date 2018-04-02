function RageMP() {
    this.Loaded = false;

    this.Init = function () {
        this.Loaded = true;
    };
	
    this.sendFuncResponseToRAGE = function ( id, response ) {
        if ( !this.Loaded ) return;
        if ( id === -1 ) return;
        if ( typeof response !== "string" && typeof response !== "number" ) return;

        // Be carefully here, if you want to sent a string, don't put the string in braces
        RAGE.Client.response( id,
            ( typeof response === "string" && response[0] === "{" && response.endsWith( "}" ) ) ? JSON.parse( response ) : response
        );
    };

    this.callClient = function ( func, data ) {
        if ( typeof mp !== "undefined" )  // so it doesn't output errors if testing without using Rage
            mp.trigger( func, data.catchResponse ? data.catchResponse.id : -1, ...data.Arguments );
    };
}

var RageJS = new RageMP();
