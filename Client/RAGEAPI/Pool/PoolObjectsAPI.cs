using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.MapObject;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    class PoolObjectsAPI : IPoolObjectsAPI
    {
        public IEnumerable<IMapObject> All
        {
            get
            {
                var list = new List<IMapObject>();
                foreach (var obj in RAGE.Elements.Entities.Objects.All)
                {
                    list.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return list;
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
