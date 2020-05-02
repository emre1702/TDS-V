using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.MapObject;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    class PoolObjectsAPI : IPoolObjectsAPI
    {
        private readonly List<IMapObject> _all = new List<IMapObject>();
        public List<IMapObject> All
        {
            get
            {
                _all.Clear();
                foreach (var obj in RAGE.Elements.Entities.Objects.All)
                {
                    _all.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return _all;
            }
        }

        private readonly List<IMapObject> _streamed = new List<IMapObject>();
        public List<IMapObject> Streamed
        {
            get
            {
                _streamed.Clear();
                foreach (var obj in RAGE.Elements.Entities.Objects.Streamed)
                {
                    _streamed.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return _streamed;
            }
        }

        public IMapObject GetAtHandle(int handle)
        {
            var obj = RAGE.Elements.Entities.Objects.GetAtHandle(handle);
            return _entityConvertingHandler.GetEntity(obj);
        }

        public IMapObject GetAtRemote(ushort handleValue)
        {
            var obj = RAGE.Elements.Entities.Objects.GetAtRemote(handleValue);
            return _entityConvertingHandler.GetEntity(obj);
        }

        private readonly EntityConvertingHandler _entityConvertingHandler;

        public PoolObjectsAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;
    }
}
