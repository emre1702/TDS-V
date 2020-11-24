using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Action;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Database;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Events;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.GangsHandler;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.LobbyHandlers;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.MapHandlers;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Notifications;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.StartRequirements;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;

namespace TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas
{
#nullable enable
    public interface IBaseGangActionArea
    {
        IBaseGangActionAreaAction Action { get; }
        IBaseGangActionAreaDatabase DatabaseHandler { get; }
        IBaseGangActionAreaEvents Events { get; }
        IBaseGangActionAreaGangsHandler GangsHandler { get; }
        IBaseGangActionAreaLobbyHandler LobbyHandler { get; }
        IBaseGangActionAreaMapHandler MapHandler { get; }
        IBaseGangActionAreaNotifications Notifications { get; }
        IBaseGangActionAreaStartRequirements StartRequirements { get; }

        IGangActionLobby? InLobby => LobbyHandler.InLobby;
        IGang? Attacker => GangsHandler.Attacker;
        IGang? Owner => GangsHandler.Owner;
        GangActionType Type { get; }
        IDatabaseHandler Database => DatabaseHandler.Database;
    }
}