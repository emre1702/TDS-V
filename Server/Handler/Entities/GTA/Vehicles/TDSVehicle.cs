using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.GTA.Vehicles
{
    public partial class TDSVehicle : ITDSVehicle
    {
        private readonly WorkaroundsHandler _workaroundsHandler;

        public ILobby? Lobby { get; set; }

        public TDSVehicle(NetHandle netHandle,
            WorkaroundsHandler workaroundsHandler) : base(netHandle)
        {
            _workaroundsHandler = workaroundsHandler;
        }
    }
}
