/// <reference path="../types-ragemp/index.d.ts" />

let roundinfo = {
	amountinteams: [],
	aliveinteams: [],
	roundtime: 0,
	starttick: 0,
	teamnames: [],
	teamcolors: [],
	killinfo: [],
	drawclasses: {
		text: {
			time: null as cText,
			teams: [] as [cText]
		},
		rect: {
			time: null as cRectangle,
			teams: [] as [cRectangle]
		}
	},
	drawdata: {
		time: {
			text: {
				ypos: 0.005,
				scale: [0.5, 0.5] as [number, number],
				color: [255, 255, 255, 255] as [number, number, number, number]
			},
			rectangle: {
				xpos: 0.47,
				ypos: 0,
				width: 0.06,
				height: 0.05,
				color: [20, 20, 20, 180] as [number, number, number, number]
			}
		},
		team: {
			text: {
				ypos: -0.002,
				scale: [0.41, 0.41] as [number, number],
				color: [255, 255, 255, 255] as [number, number, number, number]
			},
			rectangle: {
				ypos: 0,
				width: 0.13,
				height: 0.06,
				a: 180
			}
		},
		kills: {
			showtick: 15000,
			fadeaftertick: 11000,
			xpos: 0.99,
			ypos: 0.45,
			scale: [0.3, 0.3] as [number, number],
			height: 0.04
		}
			
	}
}
	// roundinfo.roundtimetext = new cText( minutes + ":" + ( seconds >= 10 ? seconds : "0" + seconds ), res.Width / 2, res.Height * 0.02, 0, 255, 255, 255, 255, 0, 1, true );

function refreshRoundInfo() {
	// Time //
	let tick = getTick();
	let fullseconds = Math.ceil( ( roundinfo.roundtime - ( tick - roundinfo.starttick ) ) / 1000 )
	let minutes = Math.floor( fullseconds / 60 );
	let seconds = fullseconds % 60;
	roundinfo.drawclasses.text.time.setText( minutes + ":" + ( seconds >= 10 ? seconds : "0" + seconds ) );
}


function setRoundTimeLeft( lefttime ) {
	roundinfo.starttick = getTick() - ( roundinfo.roundtime - lefttime ); 
}


mp.events.add ( "render", function () {
	// Kill-Info //
	let length = roundinfo.killinfo.length;
	if ( length > 0 ) {
		let tick = getTick();
		for ( let i = length - 1; i >= 0; i-- ) {
			let tickwasted = tick - roundinfo.killinfo[i].starttick;
			let data = roundinfo.drawdata.kills;
			if ( tickwasted < data.showtick ) {
				let alpha = tickwasted <= data.fadeaftertick ? 255 : Math.ceil( ( data.showtick - tickwasted ) / ( data.showtick - data.fadeaftertick ) * 255 );
				let counter = length - i - 1;
				drawText( roundinfo.killinfo[i].killstr, data.xpos, data.ypos + counter * data.height, 1, [255, 255, 255, alpha], data.scale, true, 2 );
			} else {
				roundinfo.killinfo.splice( 0, i + 1 );
				break;
			}
		}
	}
} );


function removeRoundInfo() {
	roundinfo.amountinteams = [];
	roundinfo.aliveinteams = [];

	mp.events.remove( "render", refreshRoundInfo );
}

function roundStartedRoundInfo( wastedticks ) {
	roundinfo.starttick = getTick();
	if ( wastedticks != null )
		roundinfo.starttick -= wastedticks;

	let tdata = roundinfo.drawdata.time.text;
	let trdata = roundinfo.drawdata.time.rectangle;

	roundinfo.drawclasses.rect.time = new cRectangle ( trdata.xpos, trdata.ypos, trdata.width, trdata.height, trdata.color );
	roundinfo.drawclasses.text.time = new cText ( "0:00", trdata.xpos + trdata.width / 2, tdata.ypos, 1, tdata.color, tdata.scale, true, 1 );

	let teamdata = roundinfo.drawdata.team.text;
	let teamrdata = roundinfo.drawdata.team.rectangle;
	let leftteamamount = Math.ceil( roundinfo.teamnames.length / 2 );
	for ( let i = 0; i < leftteamamount; i++ ) {
		let startx = trdata.xpos - teamrdata.width * ( i + 1 );
		roundinfo.drawclasses.text.teams[i] = new cText( roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], startx + teamrdata.width / 2, teamdata.ypos, 1, teamdata.color, teamdata.scale, true, 1 );
		roundinfo.drawclasses.rect.teams[i] = new cRectangle( startx, teamrdata.ypos, teamrdata.width, teamrdata.height, [roundinfo.teamcolors[0 + i * 3], roundinfo.teamcolors[1 + i * 3], roundinfo.teamcolors[2 + i * 3], teamrdata.a] );
	}
	for ( let j = 0; j < roundinfo.teamnames.length - leftteamamount; j++ ) {
		let startx = trdata.xpos + trdata.width + teamrdata.width * j;
		let i = leftteamamount + j;
		roundinfo.drawclasses.text.teams[i] = new cText( roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], startx + teamrdata.width / 2, teamdata.ypos, 1, teamdata.color, teamdata.scale, true, 1 );
		roundinfo.drawclasses.rect.teams[i] = new cRectangle( startx, teamrdata.ypos, teamrdata.width, teamrdata.height, [roundinfo.teamcolors[0 + i * 3], roundinfo.teamcolors[1 + i * 3], roundinfo.teamcolors[2 + i * 3], teamrdata.a] );
	}

	mp.events.add( "render", refreshRoundInfo );
}

function addTeamInfos ( teamnames, teamcolors ) {
	for ( let i = 1; i < teamnames.length; i++ ) {
		roundinfo.teamnames[i - 1] = teamnames[i];
	}
	for ( let i = 3; i < teamcolors.length; i++ ) {
		roundinfo.teamcolors[i - 3] = teamcolors[i];
	}
}

function refreshRoundInfoTeamData() {
	for ( let i = 0; i < roundinfo.drawclasses.text.teams.length; ++i ) {
		roundinfo.drawclasses.text.teams[i].setText( roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i] );
	}
}

// use teamID - 1, because spectator (ID 0) isn't included
function playerDeathRoundInfo( teamID, killstr ) {
	roundinfo.aliveinteams[teamID-1]--;
	roundinfo.drawclasses.text.teams[teamID-1].setText( roundinfo.teamnames[teamID-1] + "\n" + roundinfo.aliveinteams[teamID-1] + "/" + roundinfo.amountinteams[teamID-1] );
	roundinfo.killinfo.push( { "killstr": killstr, "starttick": getTick() } );
}

mp.events.add( "onClientPlayerAmountInFightSync", ( amountinteam, isroundstarted, amountaliveinteam ) => {
	log( "onClientPlayerAmountInFightSync" );
	roundinfo.amountinteams = [];
	roundinfo.aliveinteams = [];
	amountinteam = JSON.parse( amountinteam );
	if ( isroundstarted )
		amountaliveinteam = JSON.parse( amountaliveinteam );
	for ( let i = 0; i < amountinteam.length; i++ ) {
		roundinfo.amountinteams[i] = Number.parseInt( amountinteam[i] );
		if ( !isroundstarted )
			roundinfo.aliveinteams[i] = Number.parseInt ( amountinteam[i] );
		else 
			roundinfo.aliveinteams[i] = Number.parseInt ( amountaliveinteam[i] );
	}
	refreshRoundInfoTeamData();
} );