/// <reference path="types-ragemp/index.d.ts" />

var alltimertable = [];
var puttimerintable = [];

mp.events.add( "render", function () {
	var tick = getTick();
	for ( let i = alltimertable.length - 1; i >= 0; i-- )
		if ( !alltimertable[i].killit ) {
			if ( alltimertable[i].executeatms <= tick ) {
				var timer = alltimertable[i];
				alltimertable.splice( i, 1 );	
				timer.execute( true );
			} else
				break;
		} else 
			alltimertable.splice( i, 1 );
	if ( puttimerintable.length > 0 ) {
		for ( var j = 0; j < puttimerintable.length; j++ ) {
			puttimerintable[j].putTimerInSorted();
		}
		puttimerintable = [];
	}

} );


class Timer {
	private func;
	executeatms;
	private executeafterms;
	private executeamountleft;
	private args;
	private killit;

	constructor( func, executeafterms, executeamount, ...args ) {
		this.func = func;
		this.executeatms = executeafterms + getTick();
		this.executeafterms = executeafterms;
		this.executeamountleft = executeamount;
		this.args = args;
		this.killit = false;
		puttimerintable[puttimerintable.length] = this;
		return this;
	}

	kill() {
		this.killit = true;
	}

	execute( notremove ) {
		log( "timer execute start" );
		var argslength = this.args.length;
		switch ( argslength ) {
			case 0:
				this.func();
				break;
			case 1:
				this.func( this.args[0] );
				break;
			case 2:
				this.func( this.args[0], this.args[1] );
				break;
			case 3:
				this.func( this.args[0], this.args[1], this.args[2] );
				break;
			case 4:
				this.func( this.args[0], this.args[1], this.args[2], this.args[3] );
				break;
			case 5:
				this.func( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4] );
				break;
			case 6:
				this.func( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5] );
				break;
			case 7:
				this.func( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6] );
				break;
			case 8:
				this.func( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6], this.args[7] );
				break;
			case 9:
				this.func( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6], this.args[7], this.args[8] );
				break;
			case 10:
				this.func( this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6], this.args[7], this.args[8], this.args[9] );
				break;
		}
		if ( notremove == null ) {
			var index = alltimertable.indexOf( this );
			alltimertable.splice( index, 1 );
		}
		this.executeamountleft--;

		if ( this.executeamountleft !== 0 ) {
			this.executeatms += this.executeafterms;
			this.putTimerInSorted();
		}
		log( "timer execute end" );
	}

	private putTimerInSorted () {
		for ( let i = alltimertable.length - 1; i >= 0; i-- )
			if ( alltimertable[i].executeatms > this.executeatms ) {
				alltimertable.splice( i + 1, 0, this );
				return;
			}
		alltimertable.splice( 0, 0, this );
	}
}

/* Beispiel-Nutzung: 
 * 	function eineFunktion ( text ) { 
 *		API.sendChatMessage ( text );
 *	}	
 * resource.timer.setTimer ( eineFunktion, 1000, 5, "Hallo" );
*/