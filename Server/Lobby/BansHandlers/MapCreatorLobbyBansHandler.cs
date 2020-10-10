using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.BansHandlers
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
