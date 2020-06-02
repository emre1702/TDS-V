using System;
using System.Collections.Generic;
using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Entity
{
    public interface IEntity : IEquatable<IEntity>
    {
        #region Public Properties

        uint Dimension { get; set; }

        // Summary: Local (client-side) entity ID.
        ushort Id { get; }

        bool IsLocal { get; }
        bool IsNull { get; }

        // Summary: Entity model.
        uint Model { get; set; }

        ushort RemoteId { get; }
        EntityType Type { get; }

        #endregion Public Properties

        #region Public Methods

        void Destroy();

        Dictionary<string, object>.KeyCollection GetData();

        T GetData<T>(string key);

        object GetSharedData(string key);

        object GetSharedData(ulong key);

        bool HasData(string key);

        void ResetData();

        bool ResetData(string key);

        void SetData<T>(string key, T value);

        #endregion Public Methods
    }
}
