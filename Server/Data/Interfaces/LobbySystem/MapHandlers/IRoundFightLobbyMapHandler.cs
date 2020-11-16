using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models.Map;

namespace TDS.Server.Data.Interfaces.LobbySystem.MapHandlers
{
#nullable enable

    public interface IRoundFightLobbyMapHandler : IBaseLobbyMapHandler
    {
        MapDto? CurrentMap { get; }
        List<MapDto> Maps { get; }

        MapDto? GetNextMap();

        string GetMapsJson();

        void SetMapList(IEnumerable<MapDto> maps, string? syncjson = null);
    }
}
