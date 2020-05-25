using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolPlayersAPI : IPoolPlayersAPI
    {
        #region Private Fields

        private readonly List<IPlayer> _all = new List<IPlayer>();
        private readonly EntityConvertingHandler _entityConvertingHandler;

        private readonly List<IPlayer> _streamed = new List<IPlayer>();

        #endregion Private Fields

        #region Public Constructors

        public PoolPlayersAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods
    }
}
