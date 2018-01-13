using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

    partial class Lobby {

        public List<List<NetHandle>> Players = new List<List<NetHandle>>();
        public List<List<NetHandle>> alivePlayers = new List<List<NetHandle>>();

        internal void FuncIterateAllPlayers ( Action<NetHandle, int> func, int teamID = -1 ) {
            if ( teamID == -1 ) {
                for ( int i = 0; i < Players.Count; i++ )
                    for ( int j = Players[i].Count - 1; j >= 0; j-- ) {
                        Client player = NAPI.Player.GetPlayerFromHandle ( Players[i][j] );
                        if ( player.Exists ) {
                            if ( player.GetChar ().Lobby == this ) {
                                func ( player.Handle, i );
                            } else
                                Players[i].RemoveAt ( j );
                        } else
                            Players[i].RemoveAt ( j );
                    }
            } else
                for ( int j = Players[teamID].Count - 1; j >= 0; j-- ) {
                    Client player = NAPI.Player.GetPlayerFromHandle ( Players[teamID][j] );
                    if ( player.Exists ) {
                        if ( player.GetChar ().Lobby == this ) {
                            func ( player.Handle, teamID );
                        } else
                            Players[teamID].RemoveAt ( j );
                    } else
                        Players[teamID].RemoveAt ( j );
                }
        }

        public void SendAllPlayerEvent ( string eventName, int teamindex = -1, params object[] args ) {
            if ( teamindex == -1 ) {
                FuncIterateAllPlayers ( ( player, teamID ) => { NAPI.ClientEvent.TriggerClientEvent ( NAPI.Player.GetPlayerFromHandle ( player ), eventName, args ); } );
            } else
                FuncIterateAllPlayers ( ( player, teamID ) => { NAPI.ClientEvent.TriggerClientEvent ( NAPI.Player.GetPlayerFromHandle ( player ), eventName, args ); }, teamindex );
        }

        public void SendAllPlayerLangNotification ( string langstr, int teamindex = -1, params string[] args ) {
            Dictionary<Language, string> texts = ServerLanguage.GetLangDictionary ( langstr, args );
            FuncIterateAllPlayers (
                ( playerhandle, teamID ) => {
                    Client player = NAPI.Player.GetPlayerFromHandle ( playerhandle );
                    NAPI.Notification.SendNotificationToPlayer ( player, texts[player.GetChar ().Language] );
                }, teamindex );
        }

        public void SendAllPlayerLangMessage ( string langstr, int teamindex = -1, params string[] args ) {
            Dictionary<Language, string> texts = ServerLanguage.GetLangDictionary ( langstr, args );
            FuncIterateAllPlayers ( ( playerhandle, teamID ) => {
                Client player = NAPI.Player.GetPlayerFromHandle ( playerhandle );
                player.SendChatMessage ( texts[player.GetChar ().Language] );
            },teamindex );
        }

        public void SendAllPlayerChatMessage ( string message, int teamindex = -1 ) {
            FuncIterateAllPlayers ( ( playerhandle, teamID ) => {
                Client player = NAPI.Player.GetPlayerFromHandle ( playerhandle );
                player.SendChatMessage ( message );
            }, teamindex );
        }
    }
}
