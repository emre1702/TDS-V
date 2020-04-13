using TDS_Client.Data.Interfaces.ModAPI.MapObject;

namespace TDS_Client.RAGEAPI.MapObject
{
    class MapObject : Entity.EntityBase, IMapObject
    {
        private readonly RAGE.Elements.MapObject _instance;

        public MapObject(RAGE.Elements.MapObject instance) : base(instance)
            => _instance = instance;

        public uint Model
        {
            get => _instance.Model;
            set => _instance.Model = value;
        }


    }
}
