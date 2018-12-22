using GTANetworkAPI;
using System.Collections.Generic;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

    partial class Lobby {

        public List<string> Teams = new List<string> ();
        internal readonly List<PedHash> teamSkins = new List<PedHash>();
        public List<string> TeamColorStrings = new List<string>();
        public readonly List<Color> teamColorsList = new List<Color> ();
        internal readonly List<int> teamBlipColors = new List<int>();

        private List<int> teamsUID = new List<int> ();
        private static int sTeamsUIDCounter = 1;

        public virtual void AddTeam ( string name, PedHash hash, string colorstring = "s" ) {
            Teams.Add ( name );
            teamSkins.Add ( hash );
            TeamPlayers.Add ( new List<Character> () );
            AlivePlayers.Add ( new List<Character> () );

            TeamColorStrings.Add ( colorstring );
            teamBlipColors.Add ( Colors.BlipColorByString[colorstring] );
            Color color = Colors.FontColor[colorstring];
            teamColorsList.Add ( color );

            teamsUID.Add ( sTeamsUIDCounter++ );
        }

        private void PreparePlayersist ( int amountteams ) {
            TeamPlayers = new List<List<Character>> { new List<Character> () };

            for ( int i = 1; i < amountteams; ++i )
                TeamPlayers.Add ( new List<Character> () );
        }

        public void MixTeams ( ) {
            List<List<Character>> oldplayerslist = new List<List<Character>> ( TeamPlayers );
            int amountteams = oldplayerslist.Count;
            PreparePlayersist ( amountteams );

            for ( int i = 1; i < amountteams; i++ ) {
                foreach ( Character character in oldplayerslist[i] ) {
                    if ( character.Player.Exists ) {
                        int teamID = GetTeamIDWithFewestMember ( ref TeamPlayers );
                        SetPlayerTeam ( character, teamID );
                    }
                }
            }
        }

        public int GetTeamIDWithFewestMember ( ref List<List<Character>> newplayerlist ) {
            int lastteamID = 1;
            int lastteamcount = newplayerlist[1].Count;
            for ( int k = 2; k < newplayerlist.Count; ++k ) {
                int count = newplayerlist[k].Count;
                if ( count < lastteamcount || count == lastteamcount && Utility.Rnd.Next ( 2 ) == 1 ) {             // 0 or 1
                    lastteamID = k;
                    lastteamcount = count;
                }
            }
            return lastteamID;
        }

        public string GetTeamName ( int teamID ) {
            return Teams[teamID];
        }

        private int GetTeamUID ( int teamID ) {
            return teamsUID[teamID];
        }

        public void SetPlayerTeam ( Character character, int teamID ) {
            TeamPlayers[teamID].Add ( character );
            character.Player.SetSkin ( teamSkins[teamID] );
            if ( character.Team != teamID ) {  // not old team
                character.Team = teamID;
                NAPI.ClientEvent.TriggerClientEvent ( character.Player, "onClientPlayerTeamChange", teamID, GetTeamUID ( teamID ) );
            }
        }

        internal int GetTeamAmountWithPlayers ( ) {
            int amount = 0;
            for ( int i = 0; i < TeamPlayers.Count; ++i ) {
                if ( TeamPlayers[i].Count > 0 )
                    ++amount;
            }
            return amount;
        }
    }
}
