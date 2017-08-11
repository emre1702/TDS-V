/// <reference path="../types-gt-mp/index.d.ts" />

let spectateevent = null;
let countdownsounds = [
	"go.wav",
	"1.wav",
	"2.wav",
	"3.wav"
];
let lobbydata = {
	mapinfo: null,
	countdowntimer: null,
	countdowntext: null,
	countdowntime: 0,
	isspectator: true,
	maplimit: [],
	maplimitchecktimer: null,
	outsidemapcounter: 11,
	minX: 0,
	maxX: 0,
	minY: 0,
	maxY: 0,
	outsidemaptext: null,
	teammateblips: [],
	updateteammateblipposevent: null
}

API.onResourceStart.connect( function () {
	for ( var i = 0; i < countdownsounds.length; i++ ) {
		API.preloadAudio( soundspath + countdownsounds[i] );
	}
} );

function countdownFunc( counter ) {
	counter--;
	if ( counter > 0 ) {
		lobbydata.countdowntext.setText( counter.toString() );
		lobbydata.countdowntext.blendTextScale( 6, 1000 );
		lobbydata.countdowntimer = new Timer( countdownFunc, 1000, 1, counter );
		if ( countdownsounds[counter] != null ) {
			API.setAudioVolume( 0.3 );
			var audio = API.startAudio( soundspath + countdownsounds[counter], false );
		}
	} 
}

function countdownRemoveText() {
	if ( lobbydata.countdowntext != null ) { 
		lobbydata.countdowntext.remove();
		lobbydata.countdowntext = null;
	}
	lobbydata.countdowntimer = null;

}



function pressSpectateKey( sender, e ) {
	if ( e.KeyCode == Keys.Left || e.KeyCode == Keys.A ) {
		API.triggerServerEvent( "spectateNext", false );
	} else if ( e.KeyCode == Keys.Right || e.KeyCode == Keys.D ) {
		API.triggerServerEvent( "spectateNext", true );
	}
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

function pointIsInPoly( p ) {
	if ( p.X < lobbydata.minX || p.X > lobbydata.maxX || p.Y < lobbydata.minY || p.Y > lobbydata.maxY ) {
		return false;
	}

	var inside = false;
	var vs = lobbydata.maplimit;
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
	if ( lobbydata.maplimit != null ) {
		var pos = API.getEntityPosition( API.getLocalPlayer() );
		if ( !pointIsInPoly( pos ) ) {
			lobbydata.outsidemapcounter--;
			if ( lobbydata.outsidemapcounter == 10 && lobbydata.outsidemaptext == null )
				lobbydata.outsidemaptext = new cText( getLang( "round", "outside_map_limit" ).replace( "{1}", lobbydata.outsidemapcounter ), res.Width / 2, res.Height / 2, 1.2, 255, 255, 255, 255, 0, 1 );
			else if ( lobbydata.outsidemapcounter > 0 )
				lobbydata.outsidemaptext.setText( getLang( "round", "outside_map_limit" ).replace( "{1}", lobbydata.outsidemapcounter ) );
			else if ( lobbydata.outsidemapcounter == 0 ) {
				API.triggerServerEvent( "onPlayerWasTooLongOutsideMap" );
				lobbydata.maplimitchecktimer.kill();
				lobbydata.maplimitchecktimer = null;
				lobbydata.outsidemaptext.remove();
				lobbydata.outsidemaptext = null;
			}
		} else if ( lobbydata.outsidemaptext != null ) {
			lobbydata.outsidemapcounter = 11;
			lobbydata.outsidemaptext.remove();
			lobbydata.outsidemaptext = null;
		}
	} else if ( lobbydata.outsidemaptext != null ) {
		lobbydata.outsidemapcounter = 11;
		lobbydata.outsidemaptext.remove();
		lobbydata.outsidemaptext = null;
	}
}

function removeLobbyTextsTimer ( removemapinfo ) {
	if ( spectateevent != null ) {
		spectateevent.disconnect();
		spectateevent = null;
	}
	if ( lobbydata.maplimitchecktimer != null ) {
		lobbydata.maplimitchecktimer.kill();
		lobbydata.maplimitchecktimer = null;
	}
	lobbydata.outsidemapcounter = 11;
	if ( lobbydata.outsidemaptext != null ) {
		lobbydata.outsidemaptext.remove();
		lobbydata.outsidemaptext = null;
	}
	countdownRemoveText();
	if ( removemapinfo ) {
		if ( lobbydata.mapinfo != null ) {
			lobbydata.mapinfo.remove();
			lobbydata.mapinfo = null;
		}
	}
}

function updateTeamBlipPositions() {
	for ( var playername in lobbydata.teammateblips ) {
		var player = API.getPlayerByName( playername );
		if ( !player.IsNull ) {
			let pos = API.getEntityPosition( player );
			API.setBlipPosition( lobbydata.teammateblips[playername], pos );
		}
	}
}

API.onServerEventTrigger.connect( function ( eventName, args ) {
	switch ( eventName ) {
		case "sendClientMapData":
			lobbydata.maplimit = [];
			for ( let j = 0; j < args[0].Count; j++ ) {
				lobbydata.maplimit[j] = args[0][j];
			}
			lobbydata.outsidemapcounter = 11;
			if ( args[0].Count > 0 ) {
				var minX = lobbydata.maplimit[0].X;
				var maxX = lobbydata.maplimit[0].X;
				var minY = lobbydata.maplimit[0].Y;
				var maxY = lobbydata.maplimit[0].Y;
				for ( let i = 1; i < args[0].Count; i++ ) {
					var q = lobbydata.maplimit[i];
					minX = Math.min( q.X, minX );
					maxX = Math.max( q.X, maxX );
					minY = Math.min( q.Y, minY );
					maxY = Math.max( q.Y, maxY );
				}
				lobbydata.minX = minX;
				lobbydata.maxX = maxX;
				lobbydata.minY = minY;
				lobbydata.maxY = maxY;
			}
			break;

		case "onClientCountdownStart":
			lobbydata.countdowntext = new cText( lobbydata.countdowntime.toString(), res.Width / 2, res.Height * 0.2, 2.0, 255, 255, 255, 255, 0, 1, true );
			countdownFunc( lobbydata.countdowntime + 1 );
			if ( lobbydata.isspectator && spectateevent == null )
				spectateevent = API.onKeyDown.connect( pressSpectateKey );
			lobbydata.mapinfo.setText( args[0] );
			break;

		case "onClientRoundStart":
			if ( lobbydata.countdowntext == null ) {
				lobbydata.countdowntext = new cText( "GO", res.Width / 2, res.Height * 0.2, 2.0, 255, 255, 255, 255, 0, 1, true );
			} else
				lobbydata.countdowntext.setText( "GO" );
			if ( lobbydata.countdowntimer != null )
				lobbydata.countdowntimer.kill();
			API.startAudio( soundspath + countdownsounds[0], false );
			API.setAudioVolume( 0.3 );
			lobbydata.countdowntimer = new Timer( countdownRemoveText, 2000, 1 );
			
			
			if ( lobbydata.maplimitchecktimer != null )
				lobbydata.maplimitchecktimer.kill();
			if ( !args[0] /* is not spectator */ ) {
				if ( lobbydata.maplimit[0] != undefined ) {
					lobbydata.maplimitchecktimer = new Timer( checkMapLimit, 1000, -1 );
				}
				lobbydata.teammateblips = [];
				let localplayer = API.getLocalPlayer();
				for ( let i = 0; i < args[1].Count; i++ ) {
					if ( !localplayer.Equals( args[1][i] ) ) {
						let blip = API.createBlip( API.getEntityPosition( args[1][i] ) );
						API.setBlipSprite( blip, 0 );
						lobbydata.teammateblips[API.getPlayerName( args[1][i] )] = blip;
					}
				}
			}

			lobbydata.updateteammateblipposevent = API.onUpdate.connect( updateTeamBlipPositions );
			break;

		case "onClientRoundEnd": 
			lobbydata.maplimit = [];
			removeLobbyTextsTimer( false );
			countdownRemoveText();
			if ( lobbydata.updateteammateblipposevent != null ) {
				lobbydata.updateteammateblipposevent.disconnect();
				lobbydata.updateteammateblipposevent = null;
			}
			for ( var playername in lobbydata.teammateblips ) {
				API.deleteEntity( lobbydata.teammateblips[playername] );
			}
			lobbydata.teammateblips = [];
			break;

		case "onClientSpectateMode":
			lobbydata.isspectator = true;
			if ( spectateevent == null )
				spectateevent = API.onKeyDown.connect( pressSpectateKey );
			break;

		case "onClientPlayerJoinLobby":
			lobbydata.isspectator = args[0];
			lobbydata.countdowntime = args[1];
			lobbydata.mapinfo = new cText( args[3], res.Width*0.5, res.Height * 0.95, 0.5, 255, 255, 255, 255, 0, 2, true );
			break;

		case "onClientPlayerLeaveLobby":
			removeLobbyTextsTimer( true );
			break;

		case "onClientPlayerDeath":
			if ( API.getLocalPlayer().Equals( args[0] ) ) {
				if ( lobbydata.maplimitchecktimer != null ) {
					lobbydata.maplimitchecktimer.kill();
					lobbydata.maplimitchecktimer = null;
				}
				if ( lobbydata.outsidemaptext != null ) {
					lobbydata.outsidemapcounter = 11;
					lobbydata.outsidemaptext.remove();
					lobbydata.outsidemaptext = null;
				}
			} else if ( lobbydata.teammateblips[API.getPlayerName( args[0] )] != undefined ) {
				var name = API.getPlayerName( args[0] );
				API.deleteEntity( lobbydata.teammateblips[name] );
				lobbydata.teammateblips[name] = undefined;
			}
			break;

		case "onClientPlayerQuit":
			var name = API.getPlayerName( args[0] );
			if ( lobbydata.teammateblips[name] != undefined ) {
				API.deleteEntity( lobbydata.teammateblips[name] );
				lobbydata.teammateblips[name] = undefined;
			}
			break;
	}
} );