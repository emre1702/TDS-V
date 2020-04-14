using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Entity
{
    class Entity : IEntity
    {
        private readonly RAGE.Elements.GameEntity _instance;

        public Entity(RAGE.Elements.GameEntity instance) 
            => _instance = instance;

        public int Handle => _instance.Handle;
        public ushort RemoteId => _instance.RemoteId;

        public virtual Position3D Position
        {
            get => _instance.Position.ToPosition3D();
            set => _instance.Position = value.ToVector3();
        }
        public uint Dimension
        {
            get => _instance.Dimension;
            set => _instance.Dimension = value;
        }
        public bool IsNull => _instance.IsNull;

        /** <summary>Like Null but also checks if the entity spawned</summary> */
        public bool Exists => _instance.Exists;

        public EntityType Type => (EntityType)_instance.Type;

        public void Destroy()
        {
            _instance.Destroy();
        }

        public bool Equals(IEntity other)
        {
            return Handle == other?.Handle;
        }
    }
}
