using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.GamemodesHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Notifications;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Data.Interfaces.LobbySystem.Rankings;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.Interfaces.LobbySystem.Spectator;
using TDS_Server.Data.Interfaces.LobbySystem.Statistics;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IRoundFightLobby : IFightLobby
    {
        new IRoundFightLobbyDeathmatch Deathmatch { get; }
        new IRoundFightLobbyEventsHandler Events { get; }
        IRoundFightLobbyGamemodesHandler Gamemodes { get; }
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
