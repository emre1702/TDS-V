/*var False = false;
var True = true;
let amountchilds = 0;

function updateScroll() {
	var body = $( "#chat-body" );
	body.animate( { scrollTop: body.prop ( "scrollHeight" ) }, "slow" );	
}

function formatMsg(input) {
	var start = '<span style="color: white;">';
	var output = start;

	var pass = input.replace( /~r~/g, '</span><span style="color: rgb(222, 50, 50);">' );
	pass = pass.replace( /~b~/g, '</span><span style="color: rgb(92, 180, 227);">');
	pass = pass.replace( /~g~/g, '</span><span style="color: rgb(113, 202, 113);">');
	pass = pass.replace( /~y~/g, '</span><span style="color: rgb(238, 198, 80);">' );
	//pass = pass.replace( /~p~/g, '</span><span style="color: rgb(131, 101, 224);">' );
	//pass = pass.replace( /~q~/g, '</span><span style="color: rgb(226, 79, 128);">' );
	pass = pass.replace( /~o~/g, '</span><span style="color: rgb(253, 132, 85);">' );
	//pass = pass.replace( /~c~/g, '</span><span style="color: rgb(139, 139, 139);">' );
	//pass = pass.replace( /~m~/g, '</span><span style="color: rgb(99, 99, 99);">' );
	//pass = pass.replace( /~u~/g, '</span><span style="color: rgb(0, 0, 0);">' );
	pass = pass.replace( /~s~/g, '</span><span style="color: rgb(220, 220, 220);">' );
	pass = pass.replace( /~w~/g, '</span><span style="color: white;">' );		// white
	pass = pass.replace( /~dr~/g, '</span><span style="color: rgb( 169, 25, 25 );">' );		// dark red
	pass = pass.replace( /~n~/g, '<br>' );

	return output + pass + "</span>";
}

function addMessage(msg) {
	var child = $( "<text>" + formatMsg( msg ) + "<br></text>");
	child.hide();
	var chatbody = $( "#chat-body" );
	chatbody.append( child );
	amountchilds++;
	child.fadeIn();

	if ( amountchilds >= 40 ) {
		amountchilds--;
		chatbody.find( "text:first" ).remove();
	}

	updateScroll();
}

function addColoredMessage(msg, r,g,b) {
	var child = $( '<text style="color: rgb(' + r + ', ' + g + ', ' + b + ');">' + formatMsg( msg ) + '<br></text>');
	child.hide();
	var chatbody = $( "#chat-body" );
	chatbody.append( child );
	amountchilds++;
	child.fadeIn();

	if ( amountchilds >= 40 ) {
		amountchilds--;
		chatbody.find( "text:first" ).remove();
	}

	updateScroll();
}

function setFocus( focus, fromscript = false, cmd = "" ) {
	var mainInput = $("#main-input");
	if ( focus ) {
		mainInput.fadeIn();
		mainInput.val( cmd );
		setTimeout( function () {
			mainInput.focus();
		}, 100 );
		
	} else {
		mainInput.hide();
		mainInput.val( "" );
		if ( !fromscript )
			mp.trigger( "onFocusChange", false, true );
	}
}

function onKeyUp(event) {
	if (event.keyCode == 13) {
		var m = $("#main-input").val();
		if (m)		
		{
			try
			{
				mp.trigger("commitMessage", m+"");
			}
			catch(err) {
				$("body").text(err);
			}
		} 
			
		setFocus(false);	
	}
}

mp.trigger( "onChatLoad" );


window.setInterval(function () {
	addMessage($("#chat-body").scrollTop() + " / " + $("#chat-body")[0].scrollHeight);
}, 500);
*/