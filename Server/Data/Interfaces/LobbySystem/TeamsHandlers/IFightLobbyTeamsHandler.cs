using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
    public interface IFightLobbyTeamsHandler : IBaseLobbyTeamsHandler
    {
        void SendTeamOrder(ITDSPlayer player, TeamOrder teamOrder);
    }
}
