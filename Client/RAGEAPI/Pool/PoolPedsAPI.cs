using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Interfaces.ModAPI.Pool;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolPedsAPI : IPoolPedsAPI
    {
        #region Public Constructors

        public PoolPedsAPI()
        {
            RAGE.Elements.Entities.Peds.CreateEntity = (ushort id, ushort remoteId) => new Ped.Ped(id, remoteId);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IPed> All
            => RAGE.Elements.Entities.Peds.All.OfType<IPed>().ToList();

        public List<IPed> Streamed
            => RAGE.Elements.Entities.Peds.Streamed.OfType<IPed>().ToList();

        #endregion Public Properties

        #region Public Methods

        public IPed GetAt(ushort id)
            => RAGE.Elements.Entities.Peds.GetAt(id) as IPed;

        public IPed GetAtHandle(int handle)
             => RAGE.Elements.Entities.Peds.GetAtHandle(handle) as IPed;

        public IPed GetAtRemote(ushort handleValue)
            => RAGE.Elements.Entities.Peds.GetAtRemote(handleValue) as IPed;

        #endregion Public Methods
    }
}
