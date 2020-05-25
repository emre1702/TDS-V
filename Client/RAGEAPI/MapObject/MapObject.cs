using TDS_Client.Data.Interfaces.ModAPI.MapObject;

namespace TDS_Client.RAGEAPI.MapObject
{
    internal class MapObject : Entity.EntityBase, IMapObject
    {
        #region Private Fields

        private readonly RAGE.Elements.MapObject _instance;

        #endregion Private Fields

        #region Public Constructors

        public MapObject(RAGE.Elements.MapObject instance) : base(instance)
            => _instance = instance;

        #endregion Public Constructors
    }
}
