using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.GamemodesHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Notifications;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.Interfaces.LobbySystem.Spectator;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IRoundFightLobby : IFightLobby
    {
        new IFightLobbyDeathmatch Deathmatch { get; }
        new IRoundFightLobbyEventsHandler Events { get; }
        IRoundFightLobbyGamemodesHandler Gamemodes { get; set; }
        new IRoundFightLobbyMapHandler MapHandler { get; }
        new IRoundFightLobbyNotifications Notifications { get; }
        IRoundFightLobbyRoundsHandler Rounds { get; set; }
        IRoundFightLobbySpectator Spectator { get; set; }
        IRoundFightLobbyWeapons Weapons { get; }

        MapDto CurrentMap => MapHandler.CurrentMap;

        IRoundEndReason CurrentRoundEndReason => Rounds.RoundStates.CurrentRoundEndReason;
    }
}
