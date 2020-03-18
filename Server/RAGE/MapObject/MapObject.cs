using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Server.Data.Models.GTA;

namespace TDS_Server.RAGE.MapObject
{
    class MapObject : IMapObject
    {
        private readonly Object _instance;

        public MapObject(Object instance) 
            => _instance = instance;

        public Position3D Position 
        { 
            get => new Position3D(_instance.Position.X, _instance.Position.Y, _instance.Position.Z); 
            set => _instance.Position = new Vector3(value.X, value.Y, value.Z); 
        }

        public void Delete()
        {
            _instance.Delete();
        }

        public void Freeze(bool toggle, ILobby lobby)
        {
            throw new System.NotImplementedException();
        }

        public void SetCollisionsless(bool toggle, ILobby lobby)
        {
            throw new System.NotImplementedException();
        }
    }
}
