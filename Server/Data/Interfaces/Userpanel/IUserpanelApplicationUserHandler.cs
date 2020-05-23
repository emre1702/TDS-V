using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelApplicationUserHandler
    {
        Task<string> GetData(ITDSPlayer player);
        Task<object> CreateApplication(ITDSPlayer player, ArraySegment<object> args);
        Task<object> AcceptInvitation(ITDSPlayer player, ArraySegment<object> args);
        Task<object> RejectInvitation(ITDSPlayer player, ArraySegment<object> args);
    }
}
