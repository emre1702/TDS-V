using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler.Helper;

namespace TDS.Server.LobbySystem.BansHandlers
{
    public class DamageTestLobbyBansHandler : BaseLobbyBansHandler
    {
        public DamageTestLobbyBansHandler(IDamageTestLobby lobby, LangHelper langHelper)
            : base(lobby, langHelper)
        {
        }

        public override ValueTask<bool> CheckIsBanned(ITDSPlayer player)
            => new ValueTask<bool>(false);
    }
}
