using System.Threading.Tasks;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Lobby;

namespace TDS_Server.Instance.Lobby
{
    partial class GangLobby : FightLobby
    {
        public GangLobby(Lobbies lobbyEntity) : base(lobbyEntity)
        {

        }

        public async Task<GangLobby> Init(TDSNewContext dbContext)
        {
            await LoadGangwarAreas(dbContext);
            return this;
        }
    }
}