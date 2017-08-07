var False = false;
var True = true;

function updateScroll() {
	var body = $("#chat-body");
	if (body.scrollTop() >= body[0].scrollHeight - 400) {
		body.scrollTop(body[0].scrollHeight);
	}		
}

function formatMsg(input) {
	var start = '<span style="color: white;">';
	var output = start;
	
	var pass = input.replace( "~r~", '</span><span style="color: rgb(222, 50, 50);">');
	pass = pass.replace( "~b~", '</span><span style="color: rgb(92, 180, 227);">');
	pass = pass.replace( "~g~", '</span><span style="color: rgb(113, 202, 113);">');
	//pass = pass.replace( "~y~", '</span><span style="color: rgb(238, 198, 80);">' );
	//pass = pass.replace( "~p~", '</span><span style="color: rgb(131, 101, 224);">' );
	//pass = pass.replace( "~q~", '</span><span style="color: rgb(226, 79, 128);">' );
	pass = pass.replace( "~o~", '</span><span style="color: rgb(253, 132, 85);">' );
	//pass = pass.replace( "~c~", '</span><span style="color: rgb(139, 139, 139);">' );
	//pass = pass.replace( "~m~", '</span><span style="color: rgb(99, 99, 99);">' );
	//pass = pass.replace( "~u~", '</span><span style="color: rgb(0, 0, 0);">' );
	pass = pass.replace( "~s~", '</span><span style="color: rgb(220, 220, 220);">' );
	pass = pass.replace( "~w~", '</span><span style="color: white;">' );

	return output + pass + "</span>";
}

function addMessage(msg) {
	var child = $("<p>" + formatMsg(msg) + "</p>");
	child.hide();
	$("#chat-body").append(child);
	child.fadeIn();

	updateScroll();
}

function addColoredMessage(msg, r,g,b) {
	var child = $('<p style="color: rgb(' + r + ', ' + g + ', ' + b + ');">' + formatMsg(msg) + '</p>');
	child.hide();
	$("#chat-body").append(child);
	child.fadeIn();

	updateScroll();
}

function setFocus(focus) {
	var mainInput = $("#main-input");
	if (focus) {		
		mainInput.show();
		mainInput.val("");
		mainInput.focus();
	} else {
		mainInput.hide();
		mainInput.val("");
	}
}

function onKeyUp(event) {
	if (event.keyCode == 13) {
		var m = $("#main-input").val();
		if (m)		
		{
			try
			{
				resourceCall("commitMessage", m+"");
			}
			catch(err) {
				$("body").text(err);
			}
		}
		setFocus(false);	
	}
}
/*
window.setInterval(function () {
	addMessage($("#chat-body").scrollTop() + " / " + $("#chat-body")[0].scrollHeight);
}, 500);
*/