using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Spectator
{
    public interface IFightLobbySpectator
    {
        void SetPlayerInSpectateMode(ITDSPlayer player);

        Task EnsurePlayerSpectatesAnyone(ITDSPlayer player);

        ValueTask SpectateNext(ITDSPlayer player, bool forward);

        Task SpectateOtherAllTeams(ITDSPlayer player, bool spectateNext = true);

        void SpectateOtherSameTeam(ITDSPlayer player, bool spectateNext = true, bool ignoreSource = false);
    }
}
