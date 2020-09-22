﻿using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Deathmatch
{
    public interface IBaseLobbyDeathmatch
    {
        void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon);

        void OnPlayerSpawned(ITDSPlayer player);
    }
}
