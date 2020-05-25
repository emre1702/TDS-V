//using GTANetworkAPI;
//using TDS_Common.Default;
//using TDS_Server.Instance.LobbyInstances;
//using TDS_Server.Instance.PlayerInstance;
//using TDS_Server.Manager.PlayerManager;
//using TDS_Server.Manager.Utility;

//namespace TDS_Server.Core.Manager.EventManager
//{
//    partial class EventsHandler
//    {
//        [ServerEvent(Event.PlayerSpawn)]
//        public static void OnPlayerSpawn(Player player)
//        {
//            TDSPlayer character = player.GetChar();
//            character.Lobby?.OnPlayerSpawn(character);
//            foreach (var target in character.Spectators)
//            {
//                NAPI.ClientEvent.TriggerClientEvent(target.Player, ToClientEvent.SpectatorReattachCam);
//            }
//        }

//        [ServerEvent(Event.PlayerDisconnected)]
//#pragma warning disable IDE0060 // Remove unused parameter
//        public static async void OnPlayerDisconnected(Player client, DisconnectionType type, string reason)
//#pragma warning restore IDE0060 // Remove unused parameter
//        {
//            TDSPlayer player = client.GetChar();
//            if (player.Entity is null)
//                return;
//            if (!player.LoggedIn)
//                return;

// player.Lobby?.OnPlayerDisconnected(player);

// player.Entity.PlayerStats.LoggedIn = false; player.ClosePrivateChat(true);

// CustomEventManager.SetPlayerLoggedOut(player);

// await player.SaveData(true).ConfigureAwait(true); player.Logout();

// LangUtils.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_OUT, player.DisplayName)); }

// //[DisableDefaultOnDeathRespawn] [ServerEvent(Event.PlayerDeath)] public static void
// OnPlayerDeath(Player player, Player killerClient, uint reason) { TDSPlayer character =
// player.GetChar(); if (!character.LoggedIn) return; if (character.Lobby is null) return; TDSPlayer
// killer; if (character.Lobby is FightLobby fightLobby) killer =
// fightLobby.DmgSys.GetKiller(character, killerClient); else killer = killerClient?.GetChar() ??
// character; character.Lobby?.OnPlayerDeath(character, killer, reason); }

// [ServerEvent(Event.PlayerEnterColshape)] public static void OnPlayerEnterColShape(ColShape shape,
// Player player) { TDSPlayer character = player.GetChar();
// character.Lobby?.OnPlayerEnterColShape(shape, character); }

// [ServerEvent(Event.PlayerWeaponSwitch)] public static void OnPlayerWeaponSwitch(Player player,
// WeaponHash oldweapon, WeaponHash newweapon) { TDSPlayer character = player.GetChar(); if
// (character.Lobby is FightLobby fightlobby) fightlobby.OnPlayerWeaponSwitch(character, oldweapon,
// newweapon); character.LastWeaponOnHand = newweapon;

//        }
//    }
//}
