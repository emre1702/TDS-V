using TDS_Server.Data.Interfaces;

namespace TDS_Server.LobbySystem.Gamemodes
{
    partial class Gangwar
    {
        public ITeam AttackerTeam => Lobby.Teams[2];
        public ITeam OwnerTeam => Lobby.Teams[1];
    }
}
