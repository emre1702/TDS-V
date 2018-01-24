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
                        int teamID = (int) GetTeamIDWithFewestMember ( ref newplayerslist );
                        newplayerslist[teamID].Add ( player );
                        player.SetSkin ( teamSkins[teamID] );
                    }
                }
            }
            Players = new List<List<Client>> ( newplayerslist );
        }

        public uint GetTeamIDWithFewestMember ( ref List<List<Client>> newplayerlist ) {
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

        private int GetTeamUID ( uint teamID ) {
            return teamsUID[(int)teamID];
        }

        public void SetPlayerTeam ( Client player, uint teamID, Character character = null ) {
            player.Team = (int) teamID;     //TODO testit - need to remove this when creating own damage-system
            Players[(int) teamID].Add ( player );
            player.SetSkin ( teamSkins[(int) teamID] );
            if ( character == null )
                character = player.GetChar ();
            character.Team = (ushort) teamID;
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerTeamChange", (int) teamID, GetTeamUID ( teamID ) );
        }
    }
}
