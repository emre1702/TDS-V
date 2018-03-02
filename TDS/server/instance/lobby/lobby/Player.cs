﻿using GTANetworkAPI;
using TDS.server.extend;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {

    partial class Lobby {

        public uint Armor = 100;
        public uint Health = 100;
        

        public void OnPlayerDisconnected ( Character character, DisconnectionType type, string reason ) {
            character.Lobby.RemovePlayer ( character );
        }

        public virtual void OnPlayerSpawn ( Character character ) {
            Lobby lobby = character.Lobby;
            Client player = character.Player;

            NAPI.Player.SetPlayerHealth ( player, (int) Health );
            NAPI.Player.SetPlayerArmor ( player, (int) Armor );
            NAPI.Player.FreezePlayer ( player, true );

            if ( character.Lifes == 0 ) {
                player.Position = SpawnPoint.Around ( AroundSpawnPoint ); 
                player.Freeze ( true );
                player.Rotation = SpawnRotation;
            }
        }

        public virtual void AddPlayer ( Character character, bool spectator = false ) {
            Client player = character.Player;
            player.Freeze ( true );
            character.Lobby.RemovePlayerDerived ( character );
            character.Lobby = this;
            character.Spectating = null;
            player.StopSpectating ();
            player.Dimension = Dimension;

            if ( !IsOfficial ) {
                character.TempStats = new LobbyDeathmatchStats ();
                character.CurrentStats = character.TempStats;
            }

            player.Position = SpawnPoint.Around ( AroundSpawnPoint );

            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerJoinLobby", ID );

            if ( spectator )
                AddPlayerAsSpectator ( character );
        }

        private void AddPlayerAsSpectator ( Character character ) {
            SetPlayerTeam ( character, 0 );
            character.Lifes = 0;
        }

        public void RemovePlayerDerived ( Character character ) {
            if ( this is Arena lobby )
                lobby.RemovePlayer ( character );
            else
                RemovePlayer ( character );
        }

        public virtual void RemovePlayer ( Character character ) {
            int teamID = character.Team;
            SendAllPlayerEvent ( "onClientPlayerLeaveLobby", -1, character.Player.Value );
            character.IsLobbyOwner = false;
            character.CurrentStats = character.ArenaStats;

            Players[teamID].Remove ( character );

            if ( character.Player.Exists )
                character.Player.Transparency = 255;

            if ( DeleteWhenEmpty && !IsSomeoneInLobby() ) {
                Remove ();
            }
        }

        public void SendTeamOrder ( Character character, string ordershort ) {
            string teamfontcolor = character.Lobby.TeamColorStrings[character.Team] ?? "w";
            string beforemessage = "[TEAM] #" + teamfontcolor + "#" + character.Player.SocialClubName + "#r#: ";
            SendAllPlayerLangMessage ( ordershort, character.Team, beforemessage );
        }

        public void SetPlayerLobbyOwner ( Character character ) {
            character.IsLobbyOwner = true;
        }
    }
}
