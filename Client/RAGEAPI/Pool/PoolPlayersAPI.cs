using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    class PoolPlayersAPI : IPoolPlayersAPI
    {
        public IEnumerable<IPlayer> All
        {
            get
            {
                var list = new List<IPlayer>();
                foreach (var obj in RAGE.Elements.Entities.Players.All)
                {
                    list.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return list;
            }
        }

        public IPlayer GetAtHandle(int handle)
        {
            var obj = RAGE.Elements.Entities.Players.GetAtHandle(handle);
            return _entityConvertingHandler.GetEntity(obj);
        }

        public IPlayer GetAtRemote(ushort handleValue)
        {
            var obj = RAGE.Elements.Entities.Players.GetAtRemote(handleValue);
            return _entityConvertingHandler.GetEntity(obj);
        }

        private readonly EntityConvertingHandler _entityConvertingHandler;

        public PoolPlayersAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;
    }
}
