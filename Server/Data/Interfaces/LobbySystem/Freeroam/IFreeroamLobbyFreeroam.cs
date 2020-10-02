using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.LobbySystem.Freeroam
{
    public interface IFreeroamLobbyFreeroam
    {
        void GiveVehicle(ITDSPlayer player, FreeroamVehicleType vehType);

        void SetPosition(ITDSPlayer player, float x, float y, float z, float rot);
    }
}
