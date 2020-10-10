using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.Data.Interfaces.LobbySystem.MapHandlers
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
