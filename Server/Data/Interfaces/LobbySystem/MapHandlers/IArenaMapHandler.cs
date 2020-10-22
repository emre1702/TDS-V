﻿using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Data.Models.Map.Creator;

namespace TDS_Server.LobbySystem.MapHandlers
{
#nullable enable

    public interface IArenaMapHandler : IRoundFightLobbyMapHandler
    {
        Position4DDto? GetMapRandomSpawnData(ITeam? team);
    }
}