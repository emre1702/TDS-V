using GTANetworkAPI;
using System.Collections.Generic;
using TDS.server.enums;
using TDS.server.instance.lobby.interfaces;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {

    public partial class Lobby {

        private static readonly Dictionary<string, Lobby> lobbiesByName = new Dictionary<string, Lobby> ();
        public static readonly Dictionary<int, Lobby> LobbiesByIndex = new Dictionary<int, Lobby> ();
        private static readonly Dictionary<uint, Lobby> dimensionsUsed = new Dictionary<uint, Lobby> ();

        public string Name;
        public int ID;
        public uint Dimension;
        public LobbyStatus status = LobbyStatus.NONE;

        public bool DeleteWhenEmpty = true;
        public bool IsOfficial = false;

        public Vector3 SpawnPoint = new Vector3 ( 0, 0, 900 );
        public float AroundSpawnPoint = 3.0f;
        public Vector3 SpawnRotation = new Vector3();


        public Lobby ( string name, int id = -1 ) {
            Name = name;
            if ( id == -1 ) {
                int theID = 0;
                while ( LobbiesByIndex.ContainsKey ( theID ) )
                    theID++;
                ID = theID;
            } else {
                ID = id;
            }

            Dimension = GetFreeDimension();

            lobbiesByName[name] = this;
            LobbiesByIndex[ID] = this;
            dimensionsUsed[Dimension] = this;

            AddTeam ( "Spectator", (PedHash) ( 225514697 ) );
        }

        public void SetDefaultSpawnPoint ( Vector3 spawnpoint, float around = 0 ) {
            SpawnPoint = spawnpoint;
            AroundSpawnPoint = around;
        }

        public void SetDefaultSpawnRotation ( Vector3 spawnrotation ) {
            SpawnRotation = spawnrotation;
        }

        public virtual void Remove ( ) {
            lobbiesByName.Remove ( Name );
            LobbiesByIndex.Remove ( ID );
            dimensionsUsed.Remove ( Dimension );

            FuncIterateAllPlayers ( ( character, teamID ) => {
                RemovePlayer ( character );
            } );
        }

        internal bool IsSomeoneInLobby ( ) {
            foreach ( List<Character> playerlist in TeamPlayers ) {
                if ( playerlist.Count > 0 )
                    return true;
            }
            return false;
        }

        private uint GetFreeDimension ( ) {
            uint i = 1;
            while ( dimensionsUsed.ContainsKey ( i ) )
                ++i;
            return i;
        }

        public virtual void OnPlayerEnterColShape ( ColShape shape, Character character ) { }

    }
}
