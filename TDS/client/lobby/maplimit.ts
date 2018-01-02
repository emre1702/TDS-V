/// <reference path="../types-ragemp/index.d.ts" />

let maplimitdata = {
    limit: [] as { x: number, y: number }[],
	outsidecounter: 11,
	checktimer: null,
	minX: 0,
	maxX: 0,
	minY: 0,
	maxY: 0,
	outsidetext: null,
}


function pointIsInPoly( p: MpVector3 ) {
	if ( p.x < maplimitdata.minX || p.x > maplimitdata.maxX || p.y < maplimitdata.minY || p.y > maplimitdata.maxY ) {
		return false;
	}

	var inside = false;
	var vs = maplimitdata.limit;
	for ( var i = 0, j = vs.length - 1; i < vs.length; j = i++ ) {
		var xi = vs[i].x, yi = vs[i].y;
		var xj = vs[j].x, yj = vs[j].y;

		var intersect = ( ( yi > p.y ) != ( yj > p.y ) )
			&& ( p.x < ( xj - xi ) * ( p.y - yi ) / ( yj - yi ) + xi );
		if ( intersect )
			inside = !inside;
	}

	return inside;
};


function checkMapLimit() {
	log( "checkMapLimit" );
	if ( maplimitdata.limit != null ) {
        var pos = mp.players.local.position;
		if ( !pointIsInPoly( pos ) ) {
			maplimitdata.outsidecounter--;
			if ( maplimitdata.outsidecounter == 10 && maplimitdata.outsidetext == null )
                maplimitdata.outsidetext = new cText( getLang( "round", "outside_map_limit" ).replace( "{1}", maplimitdata.outsidecounter ), 0.5, 0.5, 0, [255, 255, 255, 255], [1.2, 1.2], true, Alignment.CENTER, true );
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
}


function loadMapLimitData( data: { Item1, Item2, Item3 }[] ) {
    log( "loadMapLimitData" );
    maplimitdata.limit = [];
	for ( let j = 0; j < data.length; j++ ) {
		maplimitdata.limit[j] = { x: data[j].Item1, y: data[j].Item2 };
	}
	maplimitdata.outsidecounter = 11;
	if ( data.length > 0 ) {
		var minX = maplimitdata.limit[0].x;
		var maxX = maplimitdata.limit[0].x;
		var minY = maplimitdata.limit[0].y;
		var maxY = maplimitdata.limit[0].y;
		for ( let i = 1; i < data.length; i++ ) {
			var q = maplimitdata.limit[i];
			minX = Math.min( q.x, minX );
			maxX = Math.max( q.x, maxX );
			minY = Math.min( q.y, minY );
			maxY = Math.max( q.y, maxY );
		}
		maplimitdata.minX = minX;
		maplimitdata.maxX = maxX;
		maplimitdata.minY = minY;
		maplimitdata.maxY = maxY;
	}
}


function resetMapLimitCheck() {
	if ( maplimitdata.outsidetext != null ) {
		maplimitdata.outsidetext.remove();
		maplimitdata.outsidetext = null;
    }
    maplimitdata.outsidecounter = 11;
}


function startMapLimit() {
	log( "startMapLimit" );
	if ( maplimitdata.checktimer != null )
		maplimitdata.checktimer.kill();
	if ( 0 in maplimitdata.limit ) {
		maplimitdata.checktimer = new Timer( checkMapLimit, 1000, -1 );
	}
}


function stopMapLimitCheck() {
	log( "stopMapLimitCheck" );
	if ( maplimitdata.checktimer != null ) {
		maplimitdata.checktimer.kill();
		maplimitdata.checktimer = null;
	}
	maplimitdata.outsidecounter = 11;
	if ( maplimitdata.outsidetext != null ) {
		maplimitdata.outsidetext.remove();
		maplimitdata.outsidetext = null;
	}
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