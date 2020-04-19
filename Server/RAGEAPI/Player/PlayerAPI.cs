using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGEAPI.Player
{
    class PlayerAPI : IPlayerAPI
    {
        private readonly EntityConvertingHandler _entityConvertingHandler;

        internal PlayerAPI(EntityConvertingHandler entityConvertingHandler) 
            => _entityConvertingHandler = entityConvertingHandler;

        public void SetHealth(IPlayer player, int health)
        {
            if (!((player as Player)?._instance is { } instance))
                return;
            NAPI.Player.SetPlayerHealth(instance, health);
        }


    }
}
