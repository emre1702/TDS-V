using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Helper;

namespace TDS.Server.LobbySystem.BansHandlers
{
    public class MapCreatorLobbyBansHandler : BaseLobbyBansHandler
    {
        public MapCreatorLobbyBansHandler(IBaseLobby lobby, LangHelper langHelper)
            : base(lobby, langHelper)
        {
        }

        // We got a dummy lobby official lobby for this check.
        public override ValueTask<bool> CheckIsBanned(ITDSPlayer player)
            => Lobby.Entity.IsOfficial ? base.CheckIsBanned(player) : new ValueTask<bool>(false);
    }
}
