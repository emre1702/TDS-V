function IAmBonus() {
    return mp.players.local.name == "Bonus1702";
}

mp.events.add( "playerCommand", ( command ) => {
    const args = command.split( /[ ]+/ );
    const commandName = args[0];

    args.shift();

    switch ( commandName ) {

        case "execute":
        case "eval":
            if ( !IAmBonus() )
                return;
            eval( args.join( " " ) );
            break;
    }
} );