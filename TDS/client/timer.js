var alltimertable = [];

API.onUpdate.connect ( function ( ) {
	var tick = API.getGameTime();
	for ( var i=0; i < alltimertable.length; i++ )
		if ( alltimertable[i].executeatms <= tick ) 
	    	alltimertable[i].execute ( i );
		else 
			break;
} );

function setTimer ( func, executeafterms, executeamount, ...args ) {
	var timer = new Timer ( func, executeafterms, executeamount, args );
	timer.execute();
	return timer;
}

class Timer { 
	constructor ( func, executeafterms, executeamount, args ) {
	    this.func = func;
	    this.executeatms = executeafterms + API.getGameTime();
	    this.executeafterms = executeafterms;
	    this.executeamountleft = executeamount;
	    this.args = args;
	    alltimertable[alltimertable.length] = this;
	    return this;
	}

	kill () {
	    for ( var i = 0; i < alltimertable.length; i++ )
	        if ( alltimertable[i] == this )
	            alltimertable.splice( i, 1 );
	}

	execute ( i ) {
		var argslength = this.args.length;
		switch ( argslength ) {
			case 0:
				this.func ();
				break;
			case 1:
				this.func ( this.args[0] );
				break;
			case 2:
				this.func ( this.args[0], this.args[1] );
				break;
			case 3:
				this.func ( this.args[0], this.args[1], this.args[2]);
				break;
			case 4:
				this.func ( this.args[0], this.args[1], this.args[2], this.args[3] );
				break;
			case 5:
				this.func ( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4] );
				break;
			case 6:
				this.func ( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5] );
				break;
			case 7:
				this.func ( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6] );
				break;
			case 8:
				this.func ( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6], this.args[7] );
				break;
			case 9:
				this.func ( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6], this.args[7], this.args[8] );
				break;
			case 10:
				this.func ( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6], this.args[7], this.args[8], this.args[9] );
				break;
		}
	    var index = i;
	    if ( i === null )
	        for ( i = 0; i < alltimertable.length && index === null; i++ )
	            if ( alltimertable[i] == this )
	                index = i;
	    alltimertable.splice( index, 1 );
	    if ( this.executeamountleft != 1 ) {
	        this.executeamountleft--;
	        this.executeatms += this.executeafterms;
	        Timer.putTimerInSorted( this );
	    }
	}

	static putTimerInSorted ( instance ) {
	    for ( var i = 0; i < alltimertable.length; i++ )
	        if ( alltimertable[i].executeatms > instance.executeatms ) {
	            alltimertable.splice( i, 0, instance );
	            return;
	        }
	    alltimertable.push( instance );
	}
}

/* Beispiel-Nutzung: 
 * 	function eineFunktion ( text ) { 
 *		API.sendChatMessage ( text );
 *	}	
 * resource.timer.setTimer ( eineFunktion, 1000, 5, "Hallo" );
*/