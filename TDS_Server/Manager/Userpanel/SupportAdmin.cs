using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;

namespace TDS_Server.Manager.Userpanel
{
    static class SupportAdmin
    {
        public static Task<string?> GetData(TDSPlayer player)
        {
            if (player.Entity is null)
                return Task.FromResult((string?)null);

            return SupportRequest.GetSupportRequests(player);
        }
    }
}
