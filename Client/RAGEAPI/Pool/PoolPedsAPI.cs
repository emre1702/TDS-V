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
        private readonly List<IPed> _all = new List<IPed>();
        public List<IPed> All
        {
            get
            {
                _all.Clear();
                foreach (var obj in RAGE.Elements.Entities.Peds.All)
                {
                    _all.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return _all;
            }
        }

        private readonly List<IPed> _streamed = new List<IPed>();
        public List<IPed> Streamed
        {
            get
            {
                _streamed.Clear();
                foreach (var obj in RAGE.Elements.Entities.Peds.Streamed)
                {
                    _streamed.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return _streamed;
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
