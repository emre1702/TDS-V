using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

    partial class Lobby {

        public List<List<Client>> Players = new List<List<Client>>();
        public List<List<Client>> alivePlayers = new List<List<Client>>();

        internal void FuncIterateAllPlayers ( Action<Client, int> func, int teamID = -1 ) {
            if ( teamID == -1 ) {
                for ( int i = 0; i < Players.Count; i++ )
                    for ( int j = Players[i].Count - 1; j >= 0; j-- ) {
                        Client player = Players[i][j];
                        if ( player.Exists ) {
                            if ( player.GetChar ().Lobby == this ) {
                                func ( player, i );
                            } else
                                Players[i].RemoveAt ( j );
                        } else
                            Players[i].RemoveAt ( j );
                    }
            } else
                for ( int j = Players[teamID].Count - 1; j >= 0; j-- ) {
                    Client player = Players[teamID][j];
                    if ( player.Exists ) {
                        if ( player.GetChar ().Lobby == this ) {
                            func ( player, teamID );
                        } else
                            Players[teamID].RemoveAt ( j );
                    } else
                        Players[teamID].RemoveAt ( j );
                }
        }

        public void SendAllPlayerEvent ( string eventName, int teamindex = -1, params object[] args ) {
            if ( teamindex == -1 ) {
                FuncIterateAllPlayers ( ( player, teamID ) => { NAPI.ClientEvent.TriggerClientEvent ( player, eventName, args ); } );
            } else
                FuncIterateAllPlayers ( ( player, teamID ) => { NAPI.ClientEvent.TriggerClientEvent (player, eventName, args ); }, teamindex );
        }

        public void SendAllPlayerLangNotification ( string langstr, int teamindex = -1, params string[] args ) {
            Dictionary<Language, string> texts = ServerLanguage.GetLangDictionary ( langstr, args );
            FuncIterateAllPlayers (
                ( player, teamID ) => {
                    NAPI.Notification.SendNotificationToPlayer ( player, texts[player.GetChar ().Language] );
                }, teamindex );
        }

        public void SendAllPlayerLangMessage ( string langstr, int teamindex = -1, params string[] args ) {
            Dictionary<Language, string> texts = ServerLanguage.GetLangDictionary ( langstr, args );
            FuncIterateAllPlayers ( ( player, teamID ) => {
                player.SendChatMessage ( texts[player.GetChar ().Language] );
            },teamindex );
        }

        public void SendAllPlayerChatMessage ( string message, int teamindex = -1 ) {
            FuncIterateAllPlayers ( ( player, teamID ) => {
                player.SendChatMessage ( message );
            }, teamindex );
        }
    }
}
