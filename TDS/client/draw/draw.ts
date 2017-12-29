/// <reference path="../types-ragemp/index.d.ts" />

let drawdrawings = [];

mp.game.ui.setTextWrap( 0.0, 1.0 );

function drawText( text: string, x: number, y: number, fontid: number, color: [number, number, number, number], scale: [number, number], outline: boolean, alignment: number ) {
	mp.game.ui.setTextJustification( this.alignment );

	mp.game.graphics.drawText( text, [x, y], { font: fontid, color: color, scale: scale, outline: outline } );
}

mp.events.add( "render", () => {
	let tick = getTick();
	for ( var i = 0; i < drawdrawings.length; i++ ) {
		let c = drawdrawings[i];
		if ( c.activated ) {

			switch ( c.constructor.name ) {

				//case "line":
				//	mp.game.graphics.drawline( c.start, c.end, c.a, c.r, c.g, c.b );
				//	break;

				case "cRectangle":
					mp.game.graphics.drawRect( c.x, c.y, c.width, c.height, c.r, c.g, c.b, c.a );
					break;

				case "cText":
					// Alpha-blend //
					let alpha = c[3];
					if ( c.enda != null ) {
						alpha = getBlendValue( tick, c[3], c.enda, c.endastarttick, c.endaendtick );
					} 

					// scale-blend //
					let scale = c.scale[0];
					if ( c.endscale != null ) {
						scale = getBlendValue( tick, c.scale[0], c.endscale, c.endscalestarttick, c.endscaleendtick );
					} 

					// draw //
					drawText ( c.text, c.x, c.y, c.font, [c.color[0], c.color[1], c.color[2], alpha], [scale, scale], c.outline, c.alignment );
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

function blendClassDrawTextAlpha( enda, mstime ) {
	this.enda = enda;
	this.endastarttick = getTick();
	this.endaendtick = this.endastarttick + mstime;
}

function blendClassDrawTextScale( endscale, mstime ) {
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


function cLine( start, end, r, g, b, a ) {
	this.type = "line";
	this.activated = true;
	this.start = start;
	this.end = end;
	this.r = r;
	this.g = g;
	this.b = b;
	this.a = a;
	this.remove = removeClassDraw;
	drawdrawings.push( this );
}

class cRectangle {
	activated = true;
	x: number;
	y: number;
	width: number;
	height: number;
	color: [number, number, number, number];

	remove = removeClassDraw;

	constructor ( xpos, ypos, wsize, hsize, color ) {
		this.x = xpos;
		this.y = ypos;
		this.width = wsize;
		this.height = hsize;
		this.color = color;

		drawdrawings.push( this );
	}
}

class cText {
	activated = true;
	text: string;
	x: number;
	y: number;
	color: [number, number, number, number];
	scale: [number, number];
	outline: boolean;
	alignment: number;

	remove = removeClassDraw;
	blendTextAlpha = blendClassDrawTextAlpha;
	blendTextScale = blendClassDrawTextScale;
	getText = getClassDrawText;
	setText = setClassDrawText;

	constructor ( text: string, x: number, y: number, fontid: number, color: [number, number, number, number], scale: [number, number], outline: boolean, alignment: number ) {
		this.text = text;
		this.x = x;
		this.y = y;
		this.color = color;
		this.scale = scale;
		this.outline = outline;
		this.alignment = alignment;

		// workaround //
		if ( alignment == 0 )  // left
			this.text += "            ";
		else if ( alignment == 1 ) // middle
			this.text = "      " + text + "      ";
		else
			this.text = "            " + text;
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
	
