using System.Threading.Tasks;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Core.Manager.Userpanel
{
    static class SupportUser
    {
        public static Task<string?> GetData(TDSPlayer player)
        {
            if (player.Entity is null)
                return Task.FromResult((string?)null);

            return SupportRequest.GetSupportRequests(player);
        }
    }
}
