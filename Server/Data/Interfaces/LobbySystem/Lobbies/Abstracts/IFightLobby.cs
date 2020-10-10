using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Data.Interfaces.LobbySystem.Spectator;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IFightLobby : IBaseLobby
    {
        new IFightLobbyDeathmatch Deathmatch { get; }
        new IFightLobbyEventsHandler Events { get; }
        new IFightLobbyPlayers Players { get; }
        IFightLobbySpectator Spectator { get; }
        new IFightLobbyTeamsHandler Teams { get; }
        IFightLobbyWeapons Weapons { get; set; }
    }
}
