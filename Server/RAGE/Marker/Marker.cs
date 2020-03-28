using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces.ModAPI.Marker;

namespace TDS_Server.RAGE.Marker
{
    class Marker : Entity.Entity, IMarker
    {
        private readonly GTANetworkAPI.Marker _instance;

        public Marker(GTANetworkAPI.Marker instance) : base(instance)
            => _instance = instance;

    }
}
