using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolPedsAPI : IPoolPedsAPI
    {
        #region Private Fields

        private readonly List<IPed> _all = new List<IPed>();
        private readonly EntityConvertingHandler _entityConvertingHandler;

        private readonly List<IPed> _streamed = new List<IPed>();

        #endregion Private Fields

        #region Public Constructors

        public PoolPedsAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods
    }
}
