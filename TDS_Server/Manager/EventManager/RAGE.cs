using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.EventManager
{
    partial class EventsHandler
    {
        [ServerEvent(Event.PlayerSpawn)]
        public static void OnPlayerSpawn(Client player)
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerSpawn(character);
            foreach (var target in character.Spectators)
            {
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.SpectatorReattachCam);
            }
        }

        [ServerEvent(Event.PlayerDisconnected)]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void OnPlayerDisconnected(Client player, DisconnectionType type, string reason)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerDisconnected(character);
        }

        //[DisableDefaultOnDeathRespawn]
        [ServerEvent(Event.PlayerDeath)]
        public static void OnPlayerDeath(Client player, Client killerClient, uint reason)
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
        public static void OnPlayerEnterColShape(ColShape shape, Client player)
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerEnterColShape(shape, character);
        }

        [ServerEvent(Event.PlayerWeaponSwitch)]
        public static void OnPlayerWeaponSwitch(Client player, WeaponHash oldweapon, WeaponHash newweapon)
        {
            TDSPlayer character = player.GetChar();
            if (character.CurrentLobby is FightLobby fightlobby)
                fightlobby.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
            character.LastWeaponOnHand = newweapon;

        }
    }
}
