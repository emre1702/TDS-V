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
                player.Rotation = SpawnRotation;
            }
        }

        public virtual void AddPlayer ( Client player, bool spectator = false ) {
            player.Freeze ( true );
            Character character = player.GetChar ();
            character.Lobby.RemovePlayerDerived ( player );
            character.Lobby = this;
            character.Spectating = null;
            player.StopSpectating ();
            player.Dimension = Dimension;

            player.Position = SpawnPoint.Around ( AroundSpawnPoint );

            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerJoinLobby", ID );

            if ( spectator )
                AddPlayerAsSpectator ( player );
        }

        private void AddPlayerAsSpectator ( Client player ) {
            Character character = player.GetChar ();
            SetPlayerTeam ( player, 0, character );
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
            character.IsLobbyOwner = false;

            Players[(int) teamID].Remove ( player );

            if ( player.Exists ) 
                player.Transparency = 255;

            if ( DeleteWhenEmpty && !IsSomeoneInLobby() ) {
                Remove ();
            }
        }

        public void SendTeamOrder ( Client player, string ordershort ) {
            Character character = player.GetChar ();
            string teamfontcolor = character.Lobby.TeamColorStrings[character.Team] ?? "w";
            string beforemessage = "[TEAM] #" + teamfontcolor + "#" + player.SocialClubName + "#r#: ";
            SendAllPlayerLangMessage ( ordershort, player.GetChar ().Team, beforemessage );
        }

        public void SetPlayerLobbyOwner ( Client player ) {
            player.GetChar ().IsLobbyOwner = true;
        }
    }
}
