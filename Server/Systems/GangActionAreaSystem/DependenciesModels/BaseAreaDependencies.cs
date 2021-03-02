using TDS.Server.Data.Interfaces.GangActionAreaSystem.Action;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Database;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Events;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.GangsHandler;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.LobbyHandlers;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.MapHandlers;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Notifications;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.StartRequirements;

namespace TDS.Server.GangActionAreaSystem.DependenciesModels
{
    internal class BaseAreaDependencies
    {
        internal IBaseGangActionAreaAction? Action { get; set;}
        internal IBaseGangActionAreaDatabase? DatabaseHandler { get; set;}
        internal IBaseGangActionAreaEvents? Events { get; set;}
        internal IBaseGangActionAreaGangsHandler? GangsHandler { get; set;}
        internal IBaseGangActionAreaLobbyHandler? LobbyHandler { get; set;}
        internal IBaseGangActionAreaMapHandler? MapHandler { get; set;}
        internal IBaseGangActionAreaNotifications? Notifications { get; set;}
        internal IBaseGangActionAreaStartRequirements? StartRequirements { get; set;}
    }
}
