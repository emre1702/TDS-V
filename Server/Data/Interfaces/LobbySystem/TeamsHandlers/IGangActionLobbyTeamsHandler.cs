using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers
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
