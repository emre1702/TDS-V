using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Notifications;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Data.Interfaces.LobbySystem.Rankings;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS.Server.Data.Interfaces.LobbySystem.Spectator;
using TDS.Server.Data.Interfaces.LobbySystem.Statistics;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Weapons;
using TDS.Server.Data.Models.Map;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IRoundFightLobby : IFightLobby
    {
        new IRoundFightLobbyDeathmatch Deathmatch { get; }
        new IRoundFightLobbyEventsHandler Events { get; }
        new IRoundFightLobbyMapHandler MapHandler { get; }
        new IRoundFightLobbyNotifications Notifications { get; }
        new IRoundFightLobbyPlayers Players { get; }
        IRoundFightLobbyRanking Ranking { get; }
        IRoundFightLobbyRoundsHandler Rounds { get; }
        new IRoundFightLobbySpectator Spectator { get; }
        IRoundFightLobbyStatistics Statistics { get; }
        new IRoundFightLobbyTeamsHandler Teams { get; }
        new IRoundFightLobbyWeapons Weapons { get; }

        MapDto CurrentMap => MapHandler.CurrentMap;

        IRoundEndReason CurrentRoundEndReason => Rounds.RoundStates.CurrentRoundEndReason;
    }
}
