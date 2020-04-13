using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    class PoolPedsAPI : IPoolPedsAPI
    {
        public IEnumerable<IPed> All
        {
            get
            {
                var list = new List<IPed>();
                foreach (var obj in RAGE.Elements.Entities.Peds.All)
                {
                    list.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return list;
            }
        }

        public IPed GetAtHandle(int handle)
        {
            var obj = RAGE.Elements.Entities.Peds.GetAtHandle(handle);
            return _entityConvertingHandler.GetEntity(obj);
        }

        public IPed GetAtRemote(ushort handleValue)
        {
            var obj = RAGE.Elements.Entities.Peds.GetAtRemote(handleValue);
            return _entityConvertingHandler.GetEntity(obj);
        }

        private readonly EntityConvertingHandler _entityConvertingHandler;

        public PoolPedsAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;
    }
}
