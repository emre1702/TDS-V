using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS_Common.Enum.Userpanel;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Userpanel;

namespace TDS_Server.Manager.Userpanel
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
