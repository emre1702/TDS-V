using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;

namespace TDS.Server.Data.Interfaces.GangActionAreaSystem.LobbyHandlers
{
#nullable enable
    public interface IBaseGangActionAreaLobbyHandler
    {
        IGangActionLobby? InLobby { get; }

        void Init(IBaseGangActionArea area);
        Task<IGangActionLobby> SetInGangActionLobby(ITDSPlayer attacker);
    }
}