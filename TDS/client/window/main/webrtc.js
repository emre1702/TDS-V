let connection = new RTCMultiConnection();

connection.socketURL = 'https://tds-v.com:8546/';

connection.socketMessageEvent = 'audio-conference-demo';

connection.session = {
    audio: true,
    video: false
};

connection.mediaConstraints = {
    audio: true,
    video: false
};

connection.sdpConstraints.mandatory = {
    OfferToReceiveAudio: true,
    OfferToReceiveVideo: false
};

connection.audiosContainer = document.getElementById( "audios_container" );

connection.onstream = function ( event ) {
    var width = parseInt( connection.audiosContainer.clientWidth / 2 ) - 20;
    var mediaElement = getHTMLMediaElement( event.mediaElement, {
        title: event.userid,
        buttons: ['full-screen'],
        width: width,
        showOnMouseEnter: false
    } );

    connection.audiosContainer.appendChild( mediaElement );

    mediaElement.media.play();
    mediaElement.media.volume = 1.0;

    mediaElement.id = event.streamid;
};

connection.onstreamended = function ( event ) {
    var mediaElement = document.getElementById( event.streamid );
    if ( mediaElement ) {
        //mediaElement.parentNode.removeChild( mediaElement );
    }
};

( function () {
    connection.openOrJoin( "1", function ( isRoomExist, roomid ) {
        alert( "open or join worked" );
        if ( !isRoomExist ) {
            //showRoomURL( roomid );
        }
    } );
} )();