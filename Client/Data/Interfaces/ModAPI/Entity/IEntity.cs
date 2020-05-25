using System;
using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Entity
{
    public interface IEntity : IEquatable<IEntity>
    {
        #region Public Properties

        uint Dimension { get; set; }
        bool Exists { get; }
        int Handle { get; }

        // Summary: Local (client-side) entity ID.
        ushort Id { get; }

        bool IsLocal { get; }
        bool IsNull { get; }

        // Summary: Entity model.
        uint Model { get; set; }

        Position3D Position { get; set; }
        ushort RemoteId { get; }
        EntityType Type { get; }

        #endregion Public Properties

        #region Public Methods

        void Destroy();

        #endregion Public Methods
    }
}
