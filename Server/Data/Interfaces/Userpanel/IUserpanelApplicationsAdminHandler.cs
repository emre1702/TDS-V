using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelApplicationsAdminHandler
    {
        Task<string> GetData(ITDSPlayer player);
        Task<object> SendInvitation(ITDSPlayer player, ArraySegment<object> args);
        Task<object> SendApplicationData(ITDSPlayer player, ArraySegment<object> args);
    }
}
