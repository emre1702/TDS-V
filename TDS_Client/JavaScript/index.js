mp.keys.bind(0x7B, false, function() {
    var date = new Date;
    date = date.toISOString().slice(0, 10).replace(/-/g, "-") + "-" + date.getHours() + "-" + date.getMinutes() + "-" + date.getSeconds() + ".jpg";
	mp.gui.chat.push("Screenshot taken.");
    mp.gui.takeScreenshot(date, 0, 100, 0);
});

mp.events.add("drawMarker", (type, posx, posy, posz, dirx, diry, dirz, rotx, roty, rotz, scalex, scaley, scalez, r, g, b, a, updown, face, p19, rotate, textDict, textName, drawents) =>
{
	mp.game.graphics.drawMarker(type, posx, posy, posz, dirx, diry, dirz, rotx, roty, rotz, scalex, scaley, scalez, r, g, b, a, updown, face, p19, rotate, textDict, textName, drawents);
});