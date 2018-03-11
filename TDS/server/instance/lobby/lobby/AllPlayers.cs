using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

    partial class Lobby {

		public List<Client> Players = new List<Client> ();
        public List<List<Character>> TeamPlayers = new List<List<Character>>();
        public List<List<Character>> AlivePlayers = new List<List<Character>>();

        internal void FuncIterateAllPlayers ( Action<Character, int> func, int teamID = -1 ) {
            if ( teamID == -1 ) {
                for ( int i = 0; i < TeamPlayers.Count; i++ )
                    for ( int j = TeamPlayers[i].Count - 1; j >= 0; j-- ) {
                        Character character = TeamPlayers[i][j];
                        if ( character.Player.Exists ) {
                            if ( character.Lobby == this ) {
                                func ( character, i );
                            } else
                                TeamPlayers[i].RemoveAt ( j );
                        } else
                            TeamPlayers[i].RemoveAt ( j );
                    }
            } else
                for ( int j = TeamPlayers[teamID].Count - 1; j >= 0; j-- ) {
                    Character character = TeamPlayers[teamID][j];
                    if ( character.Player.Exists ) {
                        if ( character.Lobby == this ) {
                            func ( character, teamID );
                        } else
                            TeamPlayers[teamID].RemoveAt ( j );
                    } else
                        TeamPlayers[teamID].RemoveAt ( j );
                }
        }

        public void SendAllPlayerEvent ( string eventName, int teamindex = -1, params object[] args ) {
            if ( teamindex == -1 ) {
                FuncIterateAllPlayers ( ( character, teamID ) => { NAPI.ClientEvent.TriggerClientEvent ( character.Player, eventName, args ); } );
            } else
                FuncIterateAllPlayers ( ( character, teamID ) => { NAPI.ClientEvent.TriggerClientEvent ( character.Player, eventName, args ); }, teamindex );
        }

        public void SendAllPlayerLangNotification ( string langstr, int teamindex = -1, params string[] args ) {
            Dictionary<Language, string> texts = ServerLanguage.GetLangDictionary ( langstr, args );
            FuncIterateAllPlayers (
                ( character, teamID ) => {
                    NAPI.Notification.SendNotificationToPlayer ( character.Player, texts[character.Language] );
                }, teamindex );
        }

        public void SendAllPlayerLangMessage ( string langstr, int teamindex = -1, params string[] args ) {
            Dictionary<Language, string> texts = ServerLanguage.GetLangDictionary ( langstr, args );
            FuncIterateAllPlayers ( ( character, teamID ) => {
                NAPI.Chat.SendChatMessageToPlayer ( character.Player, texts[character.Language] );
            },teamindex );
        }

        public void SendAllPlayerChatMessage ( string message, int teamindex = -1 ) {
            FuncIterateAllPlayers ( ( character, teamID ) => {
                NAPI.Chat.SendChatMessageToPlayer ( character.Player, message, false );
            }, teamindex );
        }
    }
}
