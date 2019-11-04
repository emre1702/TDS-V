using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class GangLobby
    {
        public async override Task<bool> AddPlayer(TDSPlayer character, uint? teamindex)
        {
            if (!await base.AddPlayer(character, teamindex))
                return false;

            Workaround.FreezePlayer(character.Client, false);

            return true;
        }
    }
}