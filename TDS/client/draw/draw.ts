/// <reference path="../types-ragemp/index.d.ts" />

let drawdrawings = [];

function getStringWidth( text: string, scale: [number, number], font: number ) {
    mp.game.ui.setTextEntryForWidth( "STRING" );
    mp.game.ui.addTextComponentSubstringPlayerName( text );
    mp.game.ui.setTextFont( font );
    mp.game.ui.setTextScale( scale[0], scale[1] );
    return mp.game.ui.getTextScreenWidth( true );
}

function drawText( text: string, x: number, y: number, font: number, color: [number, number, number, number], scale: [number, number], outline: boolean, alignment: number, relative: boolean ) {
    /*mp.game.ui.setTextJustification( alignment );
    if ( alignment == Alignment.RIGHT )
        mp.game.ui.setTextWrap ( 0, x );

    mp.game.ui.setTextEntry( "CELL_EMAIL_BCON" );
    mp.game.ui.addTextComponentSubstringPlayerName ( text );
    mp.game.ui.setTextFont( font );
    mp.game.ui.setTextScale( scale[0], scale[1] );
    mp.game.ui.setTextColour( color[0], color[1], color[2], color[3] );

    //mp.game.graphics.drawText( text, [relative ? x : x / res.x, relative ? y : y / res.y], { font: font, color: color, scale: scale, outline: outline } );
    mp.game.invoke( "0x6C188BE134E074AA", text );
    mp.game.ui.drawText( relative ? x : x / res.x, relative ? y : y / res.y )

    //mp.gui.chat.push( text + " - " + ( relative ? x : x / res.x ) + " - " + ( relative ? y : y / res.y ) + " - " + font );
    //mp.gui.chat.push( color + " - " + scale + " - " + outline ); */

    let xpos = relative ? x : x / res.x;
    let ypos = relative ? y : y / res.y;

    if ( alignment == Alignment.LEFT )
        xpos += getStringWidth( text, scale, font ) / 2;
    else if ( alignment == Alignment.RIGHT )
        xpos -= getStringWidth( text, scale, font ) / 2;

    mp.game.graphics.drawText( text, [xpos, ypos], { font, color, scale, outline } );
}

function drawRectangle( x: number, y: number, width: number, length: number, color: [number, number, number, number], alignment = Alignment.LEFT, relative = true ) {
    let xpos = relative ? x : x / res.x;
    let ypos = relative ? y : y / res.y;
    let sizex = relative ? width : width / res.x;
    let sizey = relative ? length : length / res.y;

    if ( alignment == Alignment.LEFT )
        xpos += sizex / 2;
    else if ( alignment == Alignment.RIGHT )
        xpos -= sizex / 2; 
    ypos += sizey / 2;

    mp.game.graphics.drawRect( xpos, ypos, sizex, sizey, color[0], color[1], color[2], color[3] );
}

mp.events.add( "render", () => {
	let tick = getTick();
	for ( var i = 0; i < drawdrawings.length; i++ ) {
		let c = drawdrawings[i];
		if ( c.activated ) {

			switch ( c.type ) {

				//case "line":
				//	mp.game.graphics.drawline( c.start, c.end, c.a, c.r, c.g, c.b );
				//	break;

				case "cRectangle":
                    drawRectangle( c.position[0], c.position[1], c.size[0], c.size[1], c.color, c.alignment, c.relative );
					break;

                case "cText":
                    // Alpha-blend //
                    let alpha = c.color[3];
                    if ( c.enda != null ) {
                        alpha = getBlendValue( tick, c.color[3], c.enda, c.endastarttick, c.endaendtick );
                    }

                    // scale-blend //
                    let scale = [ c.scale[0], c.scale[1] ] as [number, number];
					if ( c.endscale != null ) {
                        scale[0] = getBlendValue( tick, c.scale[0], c.endscale[0], c.endscalestarttick, c.endscaleendtick );
                        scale[1] = getBlendValue( tick, c.scale[1], c.endscale[1], c.endscalestarttick, c.endscaleendtick );
					} 

					// draw //
					drawText ( c.text, c.position[0], c.position[1], c.font, [c.color[0], c.color[1], c.color[2], alpha], scale, c.outline, c.alignment, c.relative );
					break;

				//case "editbox":
					//API.drawRectangle( c.x, c.y, c.width, c.height, c.r, c.g, c.b, c.a );
					//API.drawText( c.text, c.x + 1, c.y, c.scale, c.textr, c.textg, c.textb, c.texta, c.font, c.justify, c.shadow, c.outline, c.wordwrap );
					//break;
			}
		}
	}
} );

function getBlendValue( tick, start, end, starttick, endtick ) {
	let progress = ( tick - starttick ) / ( endtick - starttick );
	if ( progress > 1 )
		progress = 1;
	return start + progress * ( end - start );
}


function removeClassDraw() {
	let index = drawdrawings.indexOf( this );
	if ( index != -1 ) {
		drawdrawings.splice( index, 1 );
		return true;
	} else
		return false;
}

function blendClassDrawTextAlpha( enda: number, mstime ) {
	this.enda = enda;
	this.endastarttick = getTick();
	this.endaendtick = this.endastarttick + mstime;
}

function blendClassDrawTextScale( endscale: [number, number], mstime ) {
	this.endscale = endscale;
	this.endscalestarttick = getTick();
	this.endscaleendtick = this.endscalestarttick + mstime;
}

function getClassDrawText() {
	return this.text;
}

function setClassDrawText ( text ) {
	this.text = text;
}


class cLine {
	type = "cLine";
	activated = true;

	start: [number, number];
	end: [number, number];
	color: [number, number, number, number];

	remove = removeClassDraw;

	constructor( start, end, r, g, b, a ) {
		this.type = "line";
		this.activated = true;
		this.start = start;
		this.end = end;
		this.color = [r, g, b, a];
		this.remove = removeClassDraw;
		drawdrawings.push( this );
	}
}

class cRectangle {
	type = "cRectangle";
	activated = true;

	position: [number, number];
	size: [number, number];
    color: [number, number, number, number];
    alignment: Alignment;
    relative: boolean;

	remove = removeClassDraw;

    constructor( xpos, ypos, width, height, color, alignment = Alignment.LEFT, relative = true ) {
		this.position = [xpos, ypos];
		this.size = [width, height];
        this.color = color;
        this.alignment = alignment;
        this.relative = relative;

		drawdrawings.push( this );
	}
}

class cText {
	type = "cText";
	activated = true;

	text: string;
    position: [number, number];
    font: number;
	color: [number, number, number, number];
	scale: [number, number];
	outline: boolean;
    alignment: Alignment;
    relative: boolean;

    endscale = null as [number, number];
    endscalestarttick: number;
    endscaleendtick: number;
    enda = null as number;
    endastarttick: number;
    endaendtick: number;

	remove = removeClassDraw;
	blendTextAlpha = blendClassDrawTextAlpha;
	blendTextScale = blendClassDrawTextScale;
	getText = getClassDrawText;
	setText = setClassDrawText;

	constructor ( text: string, x: number, y: number, font: number, color: [number, number, number, number], scale: [number, number], outline: boolean, alignment: Alignment, relative: boolean ) {
		this.text = text;
        this.position = [x, y];
        this.font = font;
		this.color = color;
		this.scale = scale;
		this.outline = outline;
        this.alignment = alignment;
        this.relative = relative;

		// workaround //
		if ( alignment == 0 )  // middle
            this.text = "        " + text + "        ";
		else if ( alignment == 1 ) // left  
            this.text += "                ";
		else
			this.text = "                " + text;
		/////////////////

		drawdrawings.push( this );
	}
}

function cEditBox( defaulttext, xpos, ypos, wsize, hsize, r = 20, g = 20, b = 20, a = 187, scale = 1.0, textr = 255, textg = 255, textb = 255, texta = 255, font = 0, justify = 0, shadow = false, outline = false, wordwrap = 0 ) {
	this.type = "editbox";
	this.activated = true;
	this.text = defaulttext;
	this.x = xpos;
	this.y = ypos;
	this.width = wsize;
	this.height = hsize;
	this.r = r;
	this.g = g;
	this.b = b;
	this.scale = scale;
	this.textr = textr;
	this.textg = textg;
	this.textb = textb;
	this.texta = texta;
	this.font = font;
	this.justify = justify;
	this.shadow = shadow;
	this.outline = outline;
	this.wordwrap = wordwrap == 0 ? Math.floor( wsize ) : wordwrap;
	this.remove = removeClassDraw;
	drawdrawings.push( this );
}
	
