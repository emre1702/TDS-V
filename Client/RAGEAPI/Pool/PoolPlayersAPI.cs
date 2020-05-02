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
        private readonly List<IPlayer> _all = new List<IPlayer>();
        public List<IPlayer> All
        {
            get
            {
                _all.Clear();
                foreach (var obj in RAGE.Elements.Entities.Players.All)
                {
                    _all.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return _all;
            }
        }

        private readonly List<IPlayer> _streamed = new List<IPlayer>();
        public List<IPlayer> Streamed
        {
            get
            {
                _streamed.Clear();
                foreach (var obj in RAGE.Elements.Entities.Players.Streamed)
                {
                    _streamed.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return _streamed;
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
