using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.BansHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.BansHandlers
{
    public class CharCreateLobbyBansHandler : BaseLobbyBansHandler, ICharCreateLobbyBansHandler
    {
        public CharCreateLobbyBansHandler(IBaseLobby lobby, LangHelper langHelper)
            : base(lobby, langHelper)
        {
        }

        public override ValueTask<bool> CheckIsBanned(ITDSPlayer player)
            => new ValueTask<bool>(false);
    }
}
