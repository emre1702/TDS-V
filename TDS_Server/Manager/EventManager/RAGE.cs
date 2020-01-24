﻿using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.PlayerManager;

namespace TDS_Server.Manager.EventManager
{
    partial class EventsHandler
    {
        [ServerEvent(Event.PlayerSpawn)]
        public static void OnPlayerSpawn(Player player)
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerSpawn(character);
            foreach (var target in character.Spectators)
            {
                NAPI.ClientEvent.TriggerClientEvent(target.Player, DToClientEvent.SpectatorReattachCam);
            }
        }

        [ServerEvent(Event.PlayerDisconnected)]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerDisconnected(character);
        }

        //[DisableDefaultOnDeathRespawn]
        [ServerEvent(Event.PlayerDeath)]
        public static void OnPlayerDeath(Player player, Player killerClient, uint reason)
        {
            TDSPlayer character = player.GetChar();
            if (!character.LoggedIn)
                return;
            if (character.CurrentLobby is null)
                return;
            TDSPlayer killer;
            if (character.CurrentLobby is FightLobby fightLobby)
                killer = fightLobby.DmgSys.GetKiller(character, killerClient);
            else 
                killer = killerClient?.GetChar() ?? character;
            character.CurrentLobby?.OnPlayerDeath(character, killer, reason);
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        public static void OnPlayerEnterColShape(ColShape shape, Player player)
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerEnterColShape(shape, character);
        }

        [ServerEvent(Event.PlayerWeaponSwitch)]
        public static void OnPlayerWeaponSwitch(Player player, WeaponHash oldweapon, WeaponHash newweapon)
        {
            TDSPlayer character = player.GetChar();
            if (character.CurrentLobby is FightLobby fightlobby)
                fightlobby.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
            character.LastWeaponOnHand = newweapon;

        }
    }
}
