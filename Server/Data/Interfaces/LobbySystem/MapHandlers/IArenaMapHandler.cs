using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Data.Models.Map.Creator;

namespace TDS.Server.LobbySystem.MapHandlers
{
#nullable enable

    public interface IArenaMapHandler : IRoundFightLobbyMapHandler
    {
        Position4DDto? GetMapRandomSpawnData(ITeam? team);
    }
}
