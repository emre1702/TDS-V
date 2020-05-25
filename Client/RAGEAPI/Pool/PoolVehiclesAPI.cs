using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolVehiclesAPI : IPoolVehiclesAPI
    {
        #region Private Fields

        private readonly List<IVehicle> _all = new List<IVehicle>();
        private readonly EntityConvertingHandler _entityConvertingHandler;

        private readonly List<IVehicle> _streamed = new List<IVehicle>();

        #endregion Private Fields

        #region Public Constructors

        public PoolVehiclesAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;

        #endregion Public Constructors

        #region Public Properties

        public List<IVehicle> All
        {
            get
            {
                _all.Clear();
                foreach (var obj in RAGE.Elements.Entities.Vehicles.All)
                {
                    _all.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return _all;
            }
        }

        public List<IVehicle> Streamed
        {
            get
            {
                _streamed.Clear();
                foreach (var obj in RAGE.Elements.Entities.Vehicles.Streamed)
                {
                    _streamed.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return _streamed;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public IVehicle GetAtHandle(int handle)
        {
            var obj = RAGE.Elements.Entities.Vehicles.GetAtHandle(handle);
            return _entityConvertingHandler.GetEntity(obj);
        }

        public IVehicle GetAtRemote(ushort handleValue)
        {
            var obj = RAGE.Elements.Entities.Vehicles.GetAtRemote(handleValue);
            return _entityConvertingHandler.GetEntity(obj);
        }

        #endregion Public Methods
    }
}
