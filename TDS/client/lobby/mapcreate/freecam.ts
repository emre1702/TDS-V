// SOURCE: 
// https://github.com/TheVektor/freecam/blob/master/freecam/client/fc_client.js 

/// <reference path="../../types-ragemp/index.d.ts" />

/*let freecamdata = {
	CameraObject: null,
	cam: null,
	wdown: false,
	sdown: false,
	adown: false,
	ddown: false,
	shiftdown: false,
	altdown: false,
	freecamMode: false,
	toggleControl: true,
	lastPos: null,

	keydownevent: null,
	keyupevent: null,
	onupdateevent: null
}



function freecamKeyDown( sender, e ) {
	if ( freecamdata.freecamMode ) {
		if ( mapcreatordata.selected == null ) {
			if ( e.KeyCode === Keys.W )
				freecamdata.wdown = true;
			else if ( e.KeyCode === Keys.A )
				freecamdata.adown = true;
			else if ( e.KeyCode === Keys.S )
				freecamdata.sdown = true;
			else if ( e.KeyCode === Keys.D )
				freecamdata.ddown = true;

			else if ( e.KeyCode === Keys.RShiftKey || e.KeyCode === Keys.ShiftKey )
				freecamdata.shiftdown = true;
			else if ( e.KeyCode === Keys.Menu || e.KeyCode === Keys.RMenu )
				freecamdata.altdown = true;

			else if ( e.KeyCode === Keys.M ) {
				startMapCreatorMenu();
			}
		} else
			mapcreatorKeyDown( e );
	} 
	if ( e.KeyCode === Keys.F )
		toggleFreecam();

}

function freecamKeyUp( sender, e ) {
	if ( freecamdata.freecamMode ) {
		if ( mapcreatordata.selected == null ) {
			if ( e.KeyCode === Keys.W )
				freecamdata.wdown = false;
			else if ( e.KeyCode === Keys.A )
				freecamdata.adown = false;
			else if ( e.KeyCode === Keys.S )
				freecamdata.sdown = false;
			else if ( e.KeyCode === Keys.D )
				freecamdata.ddown = false;

			else if ( e.KeyCode === Keys.RShiftKey || e.KeyCode === Keys.ShiftKey )
				freecamdata.shiftdown = false;
			else if ( e.KeyCode === Keys.Menu || e.KeyCode === Keys.RMenu ) {
				freecamdata.altdown = false;
			}
		} else
			mapcreatorKeyUp( e );

	}
	
}


function toggleFreecam() {
	if ( freecamdata.freecamMode ) {
		if ( freecamdata.keydownevent != null ) {
			freecamdata.keydownevent.disconnect();
			freecamdata.keydownevent = null;
		}
		if ( freecamdata.keyupevent != null ) {
			freecamdata.keyupevent.disconnect();
			freecamdata.keyupevent = null;
		}
		if ( freecamdata.onupdateevent != null ) {
			freecamdata.onupdateevent.disconnect();
			freecamdata.onupdateevent = null;
		}
		API.setActiveCamera( null );
		freecamdata.freecamMode = false;
		API.callNative( "DISPLAY_RADAR", true );
		API.callNative( "SET_FOCUS_ENTITY", API.getLocalPlayer() );
	} else {
		API.attachCameraToEntity( freecamdata.cam, freecamdata.CameraObject, new Vector3( 0.0, 0.0, 0.0 ) ); //GTMP BUG: getting the camera position returns null, so i had to attach the camera to an object and move that instead
		API.setEntityCollisionless( freecamdata.CameraObject, true );
		API.setActiveCamera( freecamdata.cam );
		API.callNative( "DISPLAY_RADAR", false );
		freecamdata.onupdateevent = API.onUpdate.connect( freecamOnUpdate );
		freecamdata.keyupevent = API.onKeyUp.connect( freecamKeyUp );
		freecamdata.keydownevent = API.onKeyDown.connect( freecamKeyDown );
		stopMapCreatorMenu();
	}
}



function freecamOnUpdate () {
	if ( freecamdata.toggleControl ) {
		API.disableControlThisFrame( 16 );
		API.disableControlThisFrame( 17 );
		API.disableControlThisFrame( 26 );
		API.disableControlThisFrame( 24 );

		var camRot = API.getGameplayCamRot();
		var camDir = API.getGameplayCamDir();

		if ( freecamdata.cam != null ) {
			var to = null;
			var pos2 = null;
			var camPos = API.getEntityPosition( freecamdata.CameraObject );

			//API.sendChatMessage(camPos.X+" "+camPos.Y+" "+camPos.Z);
			var multiply = 1;
			if ( freecamdata.shiftdown )
				multiply = 3;
			if ( freecamdata.altdown )
				multiply = 0.5;

			if ( freecamdata.wdown ) {
				to = vector3Lerp( camPos, camPos.Add( camDir.Multiply( multiply ) ), 1.0 );
			}
			if ( freecamdata.sdown ) {
				if ( to != null ) camPos = to;
				to = vector3Lerp( camPos, camPos.Subtract( camDir.Multiply( multiply ) ), 1.0 );
			}
			if ( freecamdata.adown ) {
				if ( to != null ) camPos = to;
				pos2 = getPositionInFront( multiply, camPos, camRot.Z, 90 );
				to = vector3Lerp( camPos, pos2, 1.0 );
			}
			if ( freecamdata.ddown ) {
				if ( to != null ) camPos = to;
				pos2 = getPositionInFront( multiply, camPos, camRot.Z, -90 );
				to = vector3Lerp( camPos, pos2, 1.0 );
			}

			if ( to != null && freecamdata.CameraObject != null ) {
				mp.events.callRemote( "setFreecamObjectPositionTo", to );
				//API.setEntityPosition(CameraObject,to); //GTMP BUG: If the camera goes out from the streaming distace of the object's spawn position->System.NullReferenceException
			}

			API.setEntityRotation( API.getLocalPlayer(), new Vector3( 0.0, 0.0, camRot.Z ) );

			if ( camRot != API.getCameraRotation( freecamdata.cam ) ) {
				API.setCameraRotation( freecamdata.cam, camRot );
			}
			//By not a pope
			if ( freecamdata.lastPos != null && camPos != null && freecamdata.lastPos.DistanceTo( camPos ) > 100.0 )
				API.callNative( "_SET_FOCUS_AREA", camPos.X, camPos.Y, camPos.Z, freecamdata.lastPos.X, freecamdata.lastPos.Y, freecamdata.lastPos.Z );
			freecamdata.lastPos = camPos;
		}
	}
}


function startFreecam ( object ) {
	freecamdata.CameraObject = object;
	freecamdata.cam = API.createCamera( API.getEntityPosition( API.getLocalPlayer() ), new Vector3( 0.0, 0.0, 0.0 ) );
	API.attachCameraToEntity( freecamdata.cam, freecamdata.CameraObject, new Vector3( 0.0, 0.0, 0.0 ) ); //GTMP BUG: getting the camera position returns null, so i had to attach the camera to an object and move that instead
	API.setEntityCollisionless( freecamdata.CameraObject, true );
	API.setActiveCamera( freecamdata.cam );

	API.callNative( "DISPLAY_RADAR", false );
	freecamdata.freecamMode = true;
	freecamdata.onupdateevent = API.onUpdate.connect( freecamOnUpdate );
	freecamdata.keyupevent = API.onKeyUp.connect( freecamKeyUp );
	freecamdata.keydownevent = API.onKeyDown.connect( freecamKeyDown );
}


function stopFreecam() {
	if ( freecamdata.keydownevent != null ) {
		freecamdata.keydownevent.disconnect();
		freecamdata.keydownevent = null;
	}
	if ( freecamdata.keyupevent != null ) {
		freecamdata.keyupevent.disconnect();
		freecamdata.keyupevent = null;
	}
	if ( freecamdata.onupdateevent != null ) {
		freecamdata.onupdateevent.disconnect();
		freecamdata.onupdateevent = null;
	}
		
	freecamdata = {
		CameraObject: null,
		cam: null,
		wdown: false,
		sdown: false,
		adown: false,
		ddown: false,
		shiftdown: false,
		altdown: false,
		freecamMode: false,
		toggleControl: true,
		lastPos: null,

		keydownevent: null,
		keyupevent: null,
		onupdateevent: null
	}

	API.callNative( "DISPLAY_RADAR", true );
	API.callNative( "SET_FOCUS_ENTITY", API.getLocalPlayer() );
	stopMapCreatorMenu();
}


API.onServerEventTrigger.connect( function ( name, args ) {
	if ( name == "startFreecam" ) {
		startFreecam( args[0] );
	} else if ( name == "stopFreecam" ) {
		stopFreecam();
	}
	// if ( name == "toggleFreecamControls" ) {
	//	freecamdata.toggleControl = args[0];
//	} 
} ); */