using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Entity
{
    internal class Entity : IEntity
    {
        #region Private Fields

        private readonly RAGE.Elements.GameEntity _instance;

        #endregion Private Fields

        #region Public Constructors

        public Entity(RAGE.Elements.GameEntity instance)
            => _instance = instance;

        #endregion Public Constructors

        #region Public Properties

        public uint Dimension
        {
            get => _instance.Dimension;
            set => _instance.Dimension = value;
        }

        public bool Exists => _instance.Exists;
        public int Handle => _instance.Handle;
        public ushort Id => _instance.Id;
        public bool IsLocal => _instance.IsLocal;
        public bool IsNull => _instance.IsNull;

        public uint Model
        {
            get => _instance.Model;
            set => _instance.Model = value;
        }

        public virtual Position3D Position
        {
            get => _instance.Position.ToPosition3D();
            set => _instance.Position = value.ToVector3();
        }

        public ushort RemoteId => _instance.RemoteId;
        /** <summary>Like Null but also checks if the entity spawned</summary> */
        public EntityType Type => (EntityType)_instance.Type;

        #endregion Public Properties

        #region Public Methods

        public void Destroy()
        {
            _instance.Destroy();
        }

        public bool Equals(IEntity other)
        {
            return Handle == other?.Handle;
        }

        #endregion Public Methods
    }
}
