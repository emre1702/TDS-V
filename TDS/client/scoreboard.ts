/// <reference path="types-ragemp/index.d.ts" />

var scoreboardKeyJustPressed;
var scoreboardKeyJustReleased;

function createScoreboard() {
    let playertable = {} as {
        Name: string, PlayTime: string, Kills: number, Assists: number, Deaths: number, TeamOrLobby: string
    }[];
    let playertablelength = 0;
    let scroll = 0;
    let lastplayerlisttrigger = 0;
    let showing = false;
    let playerlistdata = {
		maxplayers: 25,
		completewidth: 0.4,
		scrollbarwidth: 0.02,
		columnheight: 0.025,
		columnwidth: {
			name: 0.3,
			playtime: 0.15,
			kills: 0.1,
			assists: 0.1,
			deaths: 0.1,
            team: 0.25,
            lobby: 0.25
		}, 
		titleheight: 0.03,
		//bottomheight: 0.03,
		bottomheight: 0,
        titlerectanglecolor: [20, 20, 20, 187] as [number, number, number, number],
        bottomrectanglecolor: [20, 20, 20, 187] as [number, number, number, number],
        rectanglecolor: [10, 10, 10, 187] as [number, number, number, number],
		titlefontcolor: [ 255, 255, 255, 255 ] as [ number, number, number, number],
        bottomfontcolor: [255, 255, 255, 255] as [number, number, number, number],
        fontcolor: [255, 255, 255, 255] as [number, number, number, number],
        scrollbarbackcolor: [30, 30, 30, 187] as [number, number, number, number],
        scrollbarcolor: [120, 120, 120, 187] as [number, number, number, number],
		titlefontscale: [ 1.0, 0.35 ] as [number, number],
        fontscale: [1.0, 0.28] as [number, number],
        bottomfontscale: [1.0, 0.35] as [number, number],
		titlefont: 0,
		font: 0,
        bottomfont: 0
	};
    let playerlisttitles = [ "name", "playtime", "kills", "assists", "deaths", "team", "lobby" ];
    let playerlisttitlesindex = { name: "Name", playtime: "PlayTime", kills: "Kills", assists: "Assists", deaths: "Deaths", team: "TeamOrLobby", lobby: "TeamOrLobby" };
    let otherlobbytable = [] as { Name: string, Amount: number }[];
    let teamnames = [];
    let inmainmenu = false;

    function drawPlayerList() {
        mp.game.controls.disableControlAction( 0, 16, true );
        mp.game.controls.disableControlAction( 0, 17, true );
        let language = getLang( "scoreboard" );
        let v = playerlistdata;
        let len = playertablelength;
		if ( len > v.maxplayers )
			len = v.maxplayers;
        let startX = 0.5 - v.completewidth/2;
        let startY = 0.5 - len*v.columnheight/2 + v.titleheight/2 - v.bottomheight/2;
        let titleStartY = startY - v.titleheight;
        let bottomStartY = startY + len*v.columnheight;

		// [[ TITEL ]] //
        // HINTERGRUND //
        drawRectangle( startX, titleStartY, v.completewidth, v.titleheight, v.titlerectanglecolor );
		// SCHRIFTEN //
        let lastwidthstitle = 0;
        let titleslength = playerlisttitles.length;
		for ( let i = 0; i < titleslength - 1; ++i ) {
            if ( inmainmenu && playerlisttitles[i] == "team" )
                drawText( language[playerlisttitles[i + 1]], startX + lastwidthstitle + v.columnwidth[playerlisttitles[i]] * v.completewidth / 2, titleStartY, v.titlefont, v.titlefontcolor, v.titlefontscale, true, Alignment.CENTER, true );
			else 
                drawText( language[playerlisttitles[i]], startX + lastwidthstitle + v.columnwidth[playerlisttitles[i]] * v.completewidth / 2, titleStartY, v.titlefont, v.titlefontcolor, v.titlefontscale, true, Alignment.CENTER, true );
            lastwidthstitle += v.columnwidth[playerlisttitles[i]]*v.completewidth;
		}

		// [[ INHALT ]] //
		// HINTERGRUND //
        drawRectangle ( startX, startY, v.completewidth, len*v.columnheight, v.rectanglecolor );
		// SCHRIFTEN //
		let notshowcounter = 0;
        for ( let i = 0 + scroll; i < len + scroll; ++i ) {
			if ( i in playertable ) {
                let lastwidths = 0;
				for ( let j = 0; j < titleslength - 1; ++j ) {
                    let index = playerlisttitlesindex[playerlisttitles[j]];
                    drawText( playertable[i][index], startX + lastwidths + v.columnwidth[playerlisttitles[j]] * v.completewidth / 2, startY + ( i - scroll ) * v.columnheight, v.font, v.fontcolor, v.fontscale, true, Alignment.CENTER, true );
                    lastwidths += v.columnwidth[playerlisttitles[j]] * v.completewidth;
				}
            } else {
                drawText( otherlobbytable[notshowcounter].Name + " (" + otherlobbytable[notshowcounter].Amount + ")", startX + v.completewidth / 2, startY + ( i - scroll ) * v.columnheight, v.font, v.fontcolor, v.fontscale, true, Alignment.CENTER, true );
				notshowcounter++;
			}	
		}
		// SCROLLBAR //
        if ( len < playertablelength ) {
            drawRectangle( startX + lastwidthstitle, titleStartY, v.scrollbarwidth, len * v.columnheight + v.titleheight, v.scrollbarbackcolor );
            let amountnotshown = playertablelength-len;
            let scrollbarheight = len * v.columnheight / ( amountnotshown + 1 );
            drawRectangle ( startX + lastwidthstitle, startY + scroll*scrollbarheight, v.scrollbarwidth, scrollbarheight, v.scrollbarcolor );
		}

		// [[ BOTTOM ]] //
		// HINTERGRUND //
        drawRectangle ( startX, bottomStartY, v.completewidth, v.bottomheight, v.bottomrectanglecolor );


        if ( playertablelength - playerlistdata.maxplayers > 0 ) {
            if ( mp.game.controls.isControlJustPressed( 0, 16 ) ) {
                let plus = Math.ceil(( playertablelength - playerlistdata.maxplayers ) / 10 );
                scroll = scroll + plus >= playertablelength - playerlistdata.maxplayers ? playertablelength - playerlistdata.maxplayers : scroll + plus;
            } else if ( mp.game.controls.isControlJustPressed( 0, 17 ) ) {
                let minus = Math.ceil(( playertablelength - playerlistdata.maxplayers ) / 10 );
                scroll = scroll - minus <= 0 ? 0 : scroll - minus;
            }
        } else
            scroll = 0;
    }

    scoreboardKeyJustPressed = function() {
        if ( /*!ischatopen &&*/ !showing ) {
            showing = true;
            scroll = 0;
            let tick = getTick();
            if ( tick - lastplayerlisttrigger >= 5000 ) {
                lastplayerlisttrigger = tick;
                playertablelength = 0;
                playertable = [];
                callRemoteCooldown( "onClientRequestPlayerListDatas" );
            } 
            mp.events.add( "render", drawPlayerList );
        }
    }

    scoreboardKeyJustReleased = function () {
        if ( showing ) {
            showing = false;
            mp.events.remove( "render", drawPlayerList );
        }
    }

    function sortArray(
            a: { Name: string, PlayTime: string, Kills: number, Assists: number, Deaths: number, TeamOrLobby: string },
            b: { Name: string, PlayTime: string, Kills: number, Assists: number, Deaths: number, TeamOrLobby: string } ) {
        if ( a.TeamOrLobby !== b.TeamOrLobby ) {
            if ( a.TeamOrLobby === "0" )
				return -1;
            else if ( b.TeamOrLobby === "0" )
				return 1;
			else
                return a.TeamOrLobby < b.TeamOrLobby ? -1 : 1;
        } else {
            if ( a.PlayTime === b.PlayTime )
                return a.Name < b.Name ? -1 : 1;
            else
                return a.PlayTime > b.PlayTime ? -1 : 1;
		}
	}

    function sortArrayMainmenu(
            a: { Name: string, PlayTime: string, Kills: number, Assists: number, Deaths: number, TeamOrLobby: string },
            b: { Name: string, PlayTime: string, Kills: number, Assists: number, Deaths: number, TeamOrLobby: string } ) {
        if ( a.TeamOrLobby !== b.TeamOrLobby ) {
            if ( a.PlayTime === "-" )
				return 1;
            else if ( b.PlayTime === "-" )
                return -1;
            else if ( a.TeamOrLobby === "mainmenu" )
				return 1;
            else if ( b.TeamOrLobby === "mainmenu" )
				return -1;
			else 
                return a.TeamOrLobby < b.TeamOrLobby ? -1 : 1;
        } else {
            return a.Name < b.Name ? -1 : 1;
		}
	}

    mp.events.add( "giveRequestedPlayerListDatas", ( playersdata, otherlobbiesdata ) => {
        log( "giveRequestedPlayerListDatas" );
        playertable = JSON.parse( playersdata );
        playertablelength = playertable.length;
        otherlobbytable = [];
        if ( otherlobbiesdata != undefined ) {
            otherlobbytable = JSON.parse( otherlobbiesdata );
            playertablelength += otherlobbytable.length;
            inmainmenu = false;
            playertable.sort( sortArray );
        } else {
            inmainmenu = true;
            playertable.sort( sortArrayMainmenu );
        }
    } );

}
createScoreboard();