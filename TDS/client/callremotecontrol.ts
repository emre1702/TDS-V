let callremotedata = {
    cooldowns: {
        "onPlayerLanguageChange": 300,
        "checkMapName": 2000,
        "sendMapFromCreator": 10000,
        "joinLobby": 1000,
        "joinMapCreatorLobby": 1000,
        "onPlayerWasTooLongOutsideMap": 5000,
        "onMapsListRequest": 4000,
        "onMapVotingRequest": 1000,
        "spectateNext": 200,
        "onPlayerTryLogin": 1000,
        "onPlayerTryRegister": 1000,
        "onPlayerChatLoad": 10000,
        "onPlayerGiveOrder": 3000,
        "onClientRequestPlayerListData": 4500,
    },
    lastused: {}
}

function callRemote( eventname: string, ...args: any[] ) {
    mp.events.callRemote( eventname, args );
}

function callRemoteCooldown( eventname: string, ...args: any[] ) {
    if ( !( eventname in callremotedata.cooldowns ) ) {
        mp.events.callRemote( eventname, args );
        return;
    }

    let currenttick = getTick();
    let incooldown = eventname in callremotedata.lastused && currenttick - callremotedata.lastused[eventname] < callremotedata.cooldowns[eventname];
    if ( !incooldown ) {
        callremotedata.lastused[eventname] = currenttick;
        mp.events.callRemote( eventname, args );
        return;
    }
}