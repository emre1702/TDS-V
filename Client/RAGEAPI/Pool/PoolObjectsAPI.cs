using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Interfaces.RAGE.Game.MapObject;
using TDS_Client.Data.Interfaces.RAGE.Game.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolObjectsAPI : IPoolObjectsAPI
    {
        #region Public Constructors

        public PoolObjectsAPI()
        {
            RAGE.Elements.Entities.Objects.CreateEntity = (ushort id, ushort remoteId) => new MapObject.MapObject(id, remoteId);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IMapObject> All
            => RAGE.Elements.Entities.Objects.All.OfType<IMapObject>().ToList();

        public List<IMapObject> Streamed
            => RAGE.Elements.Entities.Objects.Streamed.OfType<IMapObject>().ToList();

        #endregion Public Properties

        #region Public Methods

        public IMapObject GetAt(ushort id)
            => RAGE.Elements.Entities.Objects.GetAt(id) as IMapObject;

        public IMapObject GetAtHandle(int handle)
             => RAGE.Elements.Entities.Objects.GetAtHandle(handle) as IMapObject;

        public IMapObject GetAtRemote(ushort handleValue)
            => RAGE.Elements.Entities.Objects.GetAtRemote(handleValue) as IMapObject;

        #endregion Public Methods
    }
}