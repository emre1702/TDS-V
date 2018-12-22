var alltimertable = [];
var puttimerintable = [];

mp.events.add( "render", function () {
	var tick = getTick();
	for ( let i = alltimertable.length - 1; i >= 0; --i )
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
		for ( var j = 0; j < puttimerintable.length; ++j ) {
			puttimerintable[j].putTimerInSorted();
		}
		puttimerintable = [];
	}

} );

class Timer {
    private func;
	private executeatms;
    private executeafterms;
    private executeamountleft;
    private args;
    private killit = false;

	constructor( func, executeafterms, executeamount, ...args ) {
		this.func = func;
		this.executeatms = executeafterms + getTick();
		this.executeafterms = executeafterms;
		this.executeamountleft = executeamount;
		this.args = args;
		puttimerintable[puttimerintable.length] = this;
		return this;
	}

	kill() {
        this.killit = true;
	}

	execute( notremove ) {
        this.func( ...this.args );

		if ( notremove == null ) {
			var index = alltimertable.indexOf( this );
			alltimertable.splice( index, 1 );
		}
		--this.executeamountleft;

		if ( this.executeamountleft !== 0 ) {
			this.executeatms += this.executeafterms;
			this.putTimerInSorted();
		}
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

/* Example: 
 * 	function aFunction ( text ) { 
 *		mp.gui.chat.push ( text );
 *	}	
 * new Timer ( aFunction, 1000, 5, "Hello" );
*/