let orderdata = {
    activated: false,
    orders: ["order_attack", "order_stay_back", "order_spread_out", "order_go_to_bomb"],
    lastordertick: 0,
    cooldown: 3000
}

function toggleOrderMode() {
    orderdata.activated = !orderdata.activated;
    toggleCanVoteForMapWithNumpadInBrowser( !orderdata.activated );
}

// numpad 0 to activate
mp.keys.bind( 0x60, false, toggleOrderMode );

for ( let i = 0; i < orderdata.orders.length && i < 9; ++i ) {
    mp.keys.bind( 0x61 + i, false, () => {
        let currenttick = getTick();
        if ( currenttick - orderdata.lastordertick >= orderdata.cooldown ) {
            mp.events.callRemote( "onPlayerGiveOrder", orderdata.orders[i] );
            orderdata.lastordertick = currenttick;
            toggleOrderMode();
        }
    } );
}