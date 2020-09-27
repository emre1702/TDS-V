using TDS_Server.Data.Models.Map;

namespace TDS_Server.Data.Interfaces.LobbySystem.MapHandlers
{
#nullable enable

    public interface IRoundFightLobbyMapHandler
    {
        MapDto? CurrentMap { get; }
    }
}
