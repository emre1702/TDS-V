using GTANetworkAPI;
using TDS.server.extend;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {

    partial class Lobby {

        public uint Armor = 100;
        public uint Health = 100;
        

        public void OnPlayerDisconnected ( Client player, byte type, string reason ) {
            player.GetChar ().Lobby.RemovePlayer ( player );
        }

        public virtual void OnPlayerSpawn ( Client player ) {
            Character character = player.GetChar ();
            Lobby lobby = character.Lobby;

            NAPI.Player.SetPlayerHealth ( player, (int) Health );
            NAPI.Player.SetPlayerArmor ( player, (int) Armor );
            NAPI.Player.FreezePlayer ( player, true );

            if ( character.Lifes == 0 ) {
                player.Position = SpawnPoint.Around ( AroundSpawnPoint ); 
                player.Freeze ( true );

                if ( SpawnRotation != null )
                    player.Rotation = SpawnRotation;
            }
        }

        public virtual void OnClientEventTrigger ( Client player, string eventName, params dynamic[] args ) { }


        public virtual void AddPlayer ( Client player, bool spectator = false ) {
            NAPI.Util.ConsoleOutput ( "Lobby AddPlayer " + player.Name );
            player.Freeze ( true );
            Character character = player.GetChar ();
            character.Lobby.RemovePlayerDerived ( player );
            character.Lobby = this;
            NAPI.Util.ConsoleOutput ( "Current Lobby: " + character.Lobby.Name + " - " + player.GetChar().Lobby.Name );
            character.Spectating = null;
            player.StopSpectating ();
            player.Dimension = Dimension;

            player.Position = SpawnPoint.Around ( AroundSpawnPoint );

            if ( spectator )
                AddPlayerAsSpectator ( player );
        }

        private void AddPlayerAsSpectator ( Client player ) {
            SetPlayerTeam ( player, 0 );
            Character character = player.GetChar ();
            character.Lifes = 0;
        }

        public void RemovePlayerDerived ( Client player ) {
            if ( this is Arena lobby )
                lobby.RemovePlayer ( player );
            else
                RemovePlayer ( player );
        }

        public virtual void RemovePlayer ( Client player ) {
            Character character = player.GetChar ();
            uint teamID = character.Team;
            SendAllPlayerEvent ( "onClientPlayerLeaveLobby", -1, player.Value );

            Players[(int) teamID].Remove ( player );

            if ( player.Exists ) 
                player.Transparency = 255;

            NAPI.Util.ConsoleOutput ( "RemovePlayer Lobby " + player.Name );

            //MainMenu.Join ( player );       // TODO
        }
    }
}
