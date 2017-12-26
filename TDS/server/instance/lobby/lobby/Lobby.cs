using GTANetworkAPI;
using GTANetworkInternals;
using System.Collections.Generic;
using TDS.server.enums;
using TDS.server.instance.lobby.interfaces;

namespace TDS.server.instance.lobby {

    public partial class Lobby : Script {

        private static readonly Dictionary<string, Lobby> sLobbiesByName = new Dictionary<string, Lobby> ();
        public static readonly Dictionary<int, Lobby> SLobbiesByIndex = new Dictionary<int, Lobby> ();
        private static readonly Dictionary<uint, Lobby> sDimensionsUsed = new Dictionary<uint, Lobby> ();

        public string Name;
        public int ID;
        public uint Dimension;
        public LobbyStatus status = LobbyStatus.NONE;

        public bool DeleteWhenEmpty = true;
        public bool IsOfficial = false;

        public Lobby ( ) { }

            
        public Lobby ( string name, int id = -1 ) {
            Name = name;
            if ( id == -1 ) {
                int theID = 0;
                while ( SLobbiesByIndex.ContainsKey ( theID ) )
                    theID++;
                ID = theID;
            } else {
                ID = id;
            }

            Dimension = GetFreeDimension();

            sLobbiesByName[name] = this;
            SLobbiesByIndex[ID] = this;
            sDimensionsUsed[Dimension] = this;

        }

        public virtual void Remove ( ) {
            sLobbiesByName.Remove ( Name );
            SLobbiesByIndex.Remove ( ID );
            sDimensionsUsed.Remove ( Dimension );

            FuncIterateAllPlayers ( ( player, teamID ) => {
                RemovePlayer ( player );
            } );
        }

        internal bool IsSomeoneInLobby ( ) {
            foreach ( List<Client> playerlist in Players ) {
                if ( playerlist.Count > 0 )
                    return true;
            }
            return false;
        }

        private bool IsPlayable ( ) {
            return ( this is IFight || this is IRound );
        }

        private uint GetFreeDimension ( ) {
            uint i = 1;
            while ( sDimensionsUsed.ContainsKey ( i ) )
                i++;
            return i;
        }

        public virtual void OnEntityEnterColShape ( ColShape shape, Client player ) { }

    }
}
