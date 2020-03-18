using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces.ModAPI.Marker;

namespace TDS_Server.RAGE.Marker
{
    class Marker : IMarker
    {
        private readonly GTANetworkAPI.Marker _instance;

        public Marker(GTANetworkAPI.Marker instance)
            => _instance = instance;

        public ushort Id => _instance.Id;

        public void Delete()
        {
            _instance.Delete();
        }

        public bool Equals(IMarker? other)
        {
            return _instance.Id == other?.Id;
        }
    }
}
