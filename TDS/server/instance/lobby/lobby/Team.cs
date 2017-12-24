using GTANetworkAPI;
using System.Collections.Generic;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

    partial class Lobby {

        public List<string> Teams = new List<string> {
            "Spectator"
        };
        internal readonly List<PedHash> teamSkins = new List<PedHash> {
            (PedHash) ( 225514697 )
        };
        public Dictionary<uint, string> TeamColorStrings = new Dictionary<uint, string> {
            [0] = "s"
        };
        public readonly List<Color> teamColorsList = new List<Color> {
            new Color ( 255, 255, 255 )
        };
        internal readonly List<int> teamBlipColors = new List<int> {
            0
        };

        internal virtual void AddTeam ( string name, PedHash hash, string colorstring = "s" ) {
            uint teamid = (uint) Teams.Count;
            Teams.Add ( name );
            teamSkins.Add ( hash );
            Players.Add ( new List<Client> () );
            
            TeamColorStrings[teamid] = colorstring;
            teamBlipColors.Add ( Colors.BlipColorByString[colorstring] );
            Color color = Colors.FontColor[colorstring];
            teamColorsList.Add ( color );
        }

        public void MixTeams ( ) {
            List<List<Client>> newplayerslist = new List<List<Client>> {
                new List<Client> ()
            };
            for ( int i = 0; i < Players[0].Count; i++ ) {
                newplayerslist[0].Add ( Players[0][i] );
            }
            for ( int i = 1; i < Players.Count; i++ )
                newplayerslist.Add ( new List<Client> () );
            for ( uint i = 1; i < Players.Count; i++ ) {
                foreach ( Client player in Players[(int) i] ) {
                    if ( player.Exists ) {
                        int teamID = (int) GetTeamIDWithFewestMember ( newplayerslist );
                        newplayerslist[teamID].Add ( player );
                        player.SetSkin ( teamSkins[teamID] );
                    }
                }
            }
            Players = new List<List<Client>> ( newplayerslist );
        }

        public uint GetTeamIDWithFewestMember ( List<List<Client>> newplayerlist ) {
            uint lastteamID = 1;
            int lastteamcount = newplayerlist[1].Count;
            for ( uint k = 2; k < newplayerlist.Count; k++ ) {
                int count = newplayerlist[(int) k].Count;
                if ( count < lastteamcount || count == lastteamcount && Utility.Rnd.Next ( 1, 2 ) == 1 ) {
                    lastteamID = k;
                    lastteamcount = count;
                }
            }
            return lastteamID;
        }

        public string GetTeamName ( uint teamID ) {
            return Teams[(int) teamID];
        }

        public void SetPlayerTeam ( Client player, uint teamID, Character character = null ) {
            player.Team = (int) teamID;     // testit - need to remove this when creating own damage-system
            Players[(int) teamID].Add ( player );
            player.SetSkin ( teamSkins[(int) teamID] );
            if ( character == null )
                character = player.GetChar ();
            character.Team = teamID;
        }
    }
}
