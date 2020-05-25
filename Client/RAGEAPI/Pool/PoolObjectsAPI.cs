using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.MapObject;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolObjectsAPI : IPoolObjectsAPI
    {
        #region Private Fields

        private readonly List<IMapObject> _all = new List<IMapObject>();
        private readonly EntityConvertingHandler _entityConvertingHandler;

        private readonly List<IMapObject> _streamed = new List<IMapObject>();

        #endregion Private Fields

        #region Public Constructors

        public PoolObjectsAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods
    }
}
