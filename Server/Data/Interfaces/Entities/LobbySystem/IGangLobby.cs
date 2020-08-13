using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities.Gang;

namespace TDS_Server.Data.Interfaces.Entities.LobbySystem
{
    public interface IGangLobby : ILobby
    {
        Task LoadGangVehicles(IGang gang);
    }
}
