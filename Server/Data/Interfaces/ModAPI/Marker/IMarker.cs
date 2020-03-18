using System;

namespace TDS_Server.Data.Interfaces.ModAPI.Marker
{
    public interface IMarker : IEquatable<IMarker>
    {
        ushort Id { get; }

        void Delete();
    }
}
