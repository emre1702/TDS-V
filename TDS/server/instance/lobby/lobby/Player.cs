﻿using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {

    partial class Lobby {

        public uint Armor = 100;
        public uint Health = 100;

		public HashSet<uint> playerBlacklist = new HashSet<uint> ();
        

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

        public virtual bool AddPlayer ( Character character, bool spectator = false ) {
			if ( !IsPlayerAllowedToJoin ( character ) )
				return false;

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

			Players.Add ( player );
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerJoinLobby", ID );

			FuncIterateAllPlayers ( ( thechar, teamID ) => {
				if ( thechar != character )
					NAPI.ClientEvent.TriggerClientEvent ( thechar.Player, "joinPlayerSameLobby", player );
			} );
			NAPI.ClientEvent.TriggerClientEvent ( player, "syncPlayersSameLobby", JsonConvert.SerializeObject ( Players ) );

			if ( spectator )
                AddPlayerAsSpectator ( character );

			return true;
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
			Client player = character.Player;
            SendAllPlayerEvent ( "onClientPlayerLeaveLobby", -1, player.Value );
            character.IsLobbyOwner = false;
            character.CurrentStats = character.ArenaStats;

            TeamPlayers[teamID].Remove ( character );
			Players.Remove ( player );

			if ( player.Exists )
				player.Transparency = 255;

            if ( !IsSomeoneInLobby() ) {
				if ( DeleteWhenEmpty )
					Remove ();
            } else {
				FuncIterateAllPlayers ( ( thechar, teamid ) => {
					if ( thechar != character )
						NAPI.ClientEvent.TriggerClientEvent ( thechar.Player, "leavePlayerSameLobby", player );
				} );
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

		public bool IsPlayerAllowedToJoin ( Character character ) {
			return !playerBlacklist.Contains ( character.UID );
		}
    }
}
