using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
    public interface IGangActionLobbyTeamsHandler : IRoundFightLobbyTeamsHandler
    {
        ITeam Attacker { get; }
        ITeam Owner { get; }

        int AttackerAmount { get; }
        int OwnerAmount { get; }

        bool HasTeamFreePlace(bool isAttacker);

        bool HasBeenInLobby(ITDSPlayer player, int teamIndex);
    }
}
