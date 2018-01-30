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
        public readonly List<uint> teamColorsList = new List<uint>();
        internal readonly List<int> teamBlipColors = new List<int>();

        private List<int> teamsUID = new List<int> ();
        private static int sTeamsUIDCounter = 1;

        public virtual void AddTeam ( string name, PedHash hash, string colorstring = "s" ) {
            uint teamid = (uint) Teams.Count;
            Teams.Add ( name );
            teamSkins.Add ( hash );
            Players.Add ( new List<Client> () );
            alivePlayers.Add ( new List<Client> () );

            TeamColorStrings.Add ( colorstring );
            teamBlipColors.Add ( Colors.BlipColorByString[colorstring] );
            Color color = Colors.FontColor[colorstring];
            teamColorsList.Add ( (uint) color.Red );
            teamColorsList.Add ( (uint) color.Green );
            teamColorsList.Add ( (uint) color.Blue );

            teamsUID.Add ( sTeamsUIDCounter++ );
        }

        private void PreparePlayersist ( int amountteams ) {
            Players = new List<List<Client>> { new List<Client> () };

            for ( int i = 1; i < amountteams; ++i )
                Players.Add ( new List<Client> () );
        }

        public void MixTeams ( ) {
            List<List<Client>> oldplayerslist = new List<List<Client>> ( Players );
            int amountteams = oldplayerslist.Count;
            PreparePlayersist ( amountteams );

            for ( int i = 1; i < amountteams; i++ ) {
                foreach ( Client player in oldplayerslist[i] ) {
                    if ( player.Exists ) {
                        uint teamID = GetTeamIDWithFewestMember ( ref Players );
                        SetPlayerTeam ( player, teamID );
                    }
                }
            }
        }

        public uint GetTeamIDWithFewestMember ( ref List<List<Client>> newplayerlist ) {
            uint lastteamID = 1;
            int lastteamcount = newplayerlist[1].Count;
            for ( int k = 2; k < newplayerlist.Count; ++k ) {
                int count = newplayerlist[k].Count;
                if ( count < lastteamcount || count == lastteamcount && Utility.Rnd.Next ( 2 ) == 1 ) {             // 0 or 1
                    lastteamID = (uint) k;
                    lastteamcount = count;
                }
            }
            return lastteamID;
        }

        public string GetTeamName ( uint teamID ) {
            return Teams[(int) teamID];
        }

        private int GetTeamUID ( uint teamID ) {
            return teamsUID[(int)teamID];
        }

        public void SetPlayerTeam ( Client player, uint teamID, Character character = null ) {
            Players[(int) teamID].Add ( player );
            player.SetSkin ( teamSkins[(int) teamID] );
            if ( character.Team != (ushort) teamID ) {  // not old team
                if ( character == null )
                    character = player.GetChar ();
                character.Team = (ushort) teamID;
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerTeamChange", (int) teamID, GetTeamUID ( teamID ) );
            }
        }
    }
}
