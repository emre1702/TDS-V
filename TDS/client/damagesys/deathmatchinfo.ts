﻿let deathmatchinfodata = {
    damage: 0,
    kills: 0,
    assists: 0
}

mp.events.add( "onClientPlayerHitOpponent", ( damage ) => {
    playHitsound();

    //deathmatchinfodata.damage += damage;
} );

/*function restartDeathmatchInfo() {
    deathmatchinfodata.damage = 0;
    deathmatchinfodata.kills = 0;
    deathmatchinfodata.assists = 0;
}*/
