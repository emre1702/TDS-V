using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Data.Interfaces.LobbySystem.Spectator;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Weapons;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
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
