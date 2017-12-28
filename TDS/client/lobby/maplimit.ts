/// <reference path="../types-ragemp/index.d.ts" />

let maplimitdata = {
	limit: [],
	outsidecounter: 11,
	checktimer: null,
	minX: 0,
	maxX: 0,
	minY: 0,
	maxY: 0,
	outsidetext: null,
}


function pointIsInPoly( p ) {
	if ( p.X < maplimitdata.minX || p.X > maplimitdata.maxX || p.Y < maplimitdata.minY || p.Y > maplimitdata.maxY ) {
		return false;
	}

	var inside = false;
	var vs = maplimitdata.limit;
	for ( var i = 0, j = vs.length - 1; i < vs.length; j = i++ ) {
		var xi = vs[i].X, yi = vs[i].Y;
		var xj = vs[j].X, yj = vs[j].Y;

		var intersect = ( ( yi > p.Y ) != ( yj > p.Y ) )
			&& ( p.X < ( xj - xi ) * ( p.Y - yi ) / ( yj - yi ) + xi );
		if ( intersect )
			inside = !inside;
	}

	return inside;
};


function checkMapLimit() {
	log( "checkMapLimit start" );
	if ( maplimitdata.limit != null ) {
		var pos = mp.players.local.position;
		if ( !pointIsInPoly( pos ) ) {
			maplimitdata.outsidecounter--;
			if ( maplimitdata.outsidecounter == 10 && maplimitdata.outsidetext == null )
				maplimitdata.outsidetext = new cText( getLang( "round", "outside_map_limit" ).replace( "{1}", maplimitdata.outsidecounter ), res.x / 2, res.y / 2, 1, { r: 255, g: 255, b: 255, a: 255 }, 1.2, 1.2, true );
			else if ( maplimitdata.outsidecounter > 0 )
				maplimitdata.outsidetext.setText( getLang( "round", "outside_map_limit" ).replace( "{1}", maplimitdata.outsidecounter ) );
			else if ( maplimitdata.outsidecounter == 0 ) {
				mp.events.callRemote( "onPlayerWasTooLongOutsideMap" );
				maplimitdata.checktimer.kill();
				maplimitdata.checktimer = null;
				maplimitdata.outsidetext.remove();
				maplimitdata.outsidetext = null;
			}
		} else
			resetMapLimitCheck();
	} else
		resetMapLimitCheck();
	log( "checkMapLimit start" );
}


function loadMapLimitData( data ) {
	log( "loadMapLimitData start" );
	maplimitdata.limit = [];
	for ( let j = 0; j < data.length; j++ ) {
		maplimitdata.limit[j] = { X: Number.parseFloat( data[j].Item1 ), Y: Number.parseFloat( data[j].Item2 ) };
	}
	maplimitdata.outsidecounter = 11;
	if ( data.length > 0 ) {
		var minX = maplimitdata.limit[0].X;
		var maxX = maplimitdata.limit[0].X;
		var minY = maplimitdata.limit[0].Y;
		var maxY = maplimitdata.limit[0].Y;
		for ( let i = 1; i < data.length; i++ ) {
			var q = maplimitdata.limit[i];
			minX = Math.min( q.X, minX );
			maxX = Math.max( q.X, maxX );
			minY = Math.min( q.Y, minY );
			maxY = Math.max( q.Y, maxY );
		}
		maplimitdata.minX = minX;
		maplimitdata.maxX = maxX;
		maplimitdata.minY = minY;
		maplimitdata.maxY = maxY;
	}
	log( "loadMapLimitData end" );
}


function resetMapLimitCheck() {
	if ( maplimitdata.outsidetext != null ) {
		maplimitdata.outsidecounter = 11;
		maplimitdata.outsidetext.remove();
		maplimitdata.outsidetext = null;
	}
}


function startMapLimit() {
	log( "startMapLimit start" );
	if ( maplimitdata.checktimer != null )
		maplimitdata.checktimer.kill();
	if ( maplimitdata.limit[0] != undefined ) {
		maplimitdata.checktimer = new Timer( checkMapLimit, 1000, -1 );
	}
	log( "startMapLimit end" );
}


function stopMapLimitCheck() {
	log( "stopMapLimitCheck start" );
	if ( maplimitdata.checktimer != null ) {
		maplimitdata.checktimer.kill();
		maplimitdata.checktimer = null;
	}
	maplimitdata.outsidecounter = 11;
	if ( maplimitdata.outsidetext != null ) {
		maplimitdata.outsidetext.remove();
		maplimitdata.outsidetext = null;
	}
	log( "stopMapLimitCheck end" );
}


function emptyMapLimit() {
	maplimitdata.limit = [];
}
			









/* function pointIsInPoly( p ) {
	var isInside = false;

	if ( p.X < lobbydata.minX || p.X > lobbydata.maxX || p.Y < lobbydata.minY || p.Y > lobbydata.maxY ) {
		return false;
	}

	var polygon = lobbydata.maplimit;
	for ( var i = 0, j = polygon.length - 1; i < polygon.length; j = i++ ) {
		if ( ( polygon[i].Y > p.Y ) != ( polygon[j].Y > p.Y ) &&
			p.X < ( polygon[j].X - polygon[i].X ) * ( p.Y - polygon[i].Y ) / ( polygon[j].Y - polygon[i].Y ) + polygon[i].X ) {
			isInside = !isInside;
		}
	}

	return isInside;
} */