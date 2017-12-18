"use strict";
let drawdrawings = [];
mp.events.add("render", () => {
    let tick = getTick();
    for (var i = 0; i < drawdrawings.length; i++) {
        let c = drawdrawings[i];
        if (c.activated) {
            switch (c.type) {
                case "rectangle":
                    mp.game.graphics.drawRect(c.x, c.y, c.width, c.height, c.r, c.g, c.b, c.a);
                    break;
                case "text":
                    let alpha = c.a;
                    if (c.enda != null) {
                        alpha = getBlendValue(tick, c.a, c.enda, c.endastarttick, c.endaendtick);
                    }
                    let scale = c.scale;
                    if (c.endscale != null) {
                        scale = getBlendValue(tick, c.scale, c.endscale, c.endscalestarttick, c.endscaleendtick);
                    }
                    mp.game.graphics.drawText(c.text, c.font, c.color, c.scaleX, c.scaleY, c.outline, c.x, c.y);
                    break;
            }
        }
    }
});
function getBlendValue(tick, start, end, starttick, endtick) {
    let progress = (tick - starttick) / (endtick - starttick);
    if (progress > 1)
        progress = 1;
    return start + progress * (end - start);
}
function removeClassDraw() {
    let index = drawdrawings.indexOf(this);
    if (index != -1) {
        drawdrawings.splice(index, 1);
        return true;
    }
    else
        return false;
}
function blendClassDrawTextAlpha(enda, mstime) {
    this.enda = enda;
    this.endastarttick = getTick();
    this.endaendtick = this.endastarttick + mstime;
}
function blendClassDrawTextScale(endscale, mstime) {
    this.endscale = endscale;
    this.endscalestarttick = getTick();
    this.endscaleendtick = this.endscalestarttick + mstime;
}
function getClassDrawText() {
    return this.text;
}
function setClassDrawText(text) {
    this.text = text;
}
function cLine(start, end, r, g, b, a) {
    this.type = "line";
    this.activated = true;
    this.start = start;
    this.end = end;
    this.r = r;
    this.g = g;
    this.b = b;
    this.a = a;
    this.remove = removeClassDraw;
    drawdrawings.push(this);
}
function cRectangle(xpos, ypos, wsize, hsize, r, g, b, a) {
    this.type = "rectangle";
    this.activated = true;
    this.x = xpos;
    this.y = ypos;
    this.width = wsize;
    this.height = hsize;
    this.r = r;
    this.g = g;
    this.b = b;
    this.a = a;
    this.remove = removeClassDraw;
    drawdrawings.push(this);
}
function cText(text, x, y, fontid, color, scaleX, scaleY, outline) {
    this.type = "text";
    this.activated = true;
    this.text = text;
    this.x = x;
    this.y = y;
    this.color = color;
    this.scaleX = scaleX;
    this.scaleY = scaleY;
    this.outline = outline;
    this.remove = removeClassDraw;
    this.blendTextAlpha = blendClassDrawTextAlpha;
    this.blendTextScale = blendClassDrawTextScale;
    this.getText = getClassDrawText;
    this.setText = setClassDrawText;
    drawdrawings.push(this);
}
function cEditBox(defaulttext, xpos, ypos, wsize, hsize, r = 20, g = 20, b = 20, a = 187, scale = 1.0, textr = 255, textg = 255, textb = 255, texta = 255, font = 0, justify = 0, shadow = false, outline = false, wordwrap = 0) {
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
    this.wordwrap = wordwrap == 0 ? Math.floor(wsize) : wordwrap;
    this.remove = removeClassDraw;
    drawdrawings.push(this);
}
