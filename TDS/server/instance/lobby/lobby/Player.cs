﻿using GTANetworkAPI;
using TDS.server.extend;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {

    partial class Lobby {

        public uint Armor = 100;
        public uint Health = 100;

        public Vector3 spawnPoint;
        public Vector3 spawnRotation;
        

        public void OnPlayerDisconnected ( Client player, byte type, string reason ) {
            player.GetChar ().Lobby.RemovePlayer ( player );
        }

        public virtual void OnPlayerSpawn ( Client player ) {
            Character character = player.GetChar ();
            Lobby lobby = character.Lobby;

            NAPI.Player.SetPlayerHealth ( player, (int) Health );
            NAPI.Player.SetPlayerArmor ( player, (int) Armor );
            NAPI.Player.FreezePlayer ( player, true );

            if ( spawnPoint != null ) 
                player.Position = spawnPoint;

            if ( spawnRotation != null )
                player.Rotation = spawnRotation;
        }

        public virtual void OnClientEventTrigger ( Client player, string eventName, params dynamic[] args ) { }


        public virtual void AddPlayer ( Client player, bool spectator = false ) {
            player.Freeze ( true );
            Character character = player.GetChar ();

            character.Lobby = this;
            character.Spectating = null;
            player.StopSpectating ();
            player.Dimension = Dimension;

            player.Position = spawnPoint.Around ( 5 );

            if ( !spectator )
                AddPlayerAsSpectator ( player );
        }

        private void AddPlayerAsSpectator ( Client player ) {
            Players[0].Add ( player );
            Character character = player.GetChar ();
            character.Team = 0;
            character.Lifes = 0;
        }

        public virtual void RemovePlayer ( Client player ) {
            Character character = player.GetChar ();
            uint teamID = character.Team;
            SendAllPlayerEvent ( "onClientPlayerLeaveLobby", -1, player );

            Players[(int) teamID].Remove ( player );

            if ( player.Exists ) {
                player.Transparency = 255;
            }

            //MainMenu.Join ( player );       // TODO
        }
    }
}
