using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;

namespace TDS_Server.RAGE.MapObject
{
    class MapObject : Entity.Entity, IMapObject
    {
        private readonly Object _instance;

        public MapObject(Object instance) : base(instance)
            => _instance = instance;
    }
}
