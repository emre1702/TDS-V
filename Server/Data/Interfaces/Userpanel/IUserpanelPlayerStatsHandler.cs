using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    #nullable enable
    public interface IUserpanelPlayerCommandsHandler
    {
        Task<string?> GetData(ITDSPlayer player);
        Task<object?> Save(ITDSPlayer player, ArraySegment<object> args);
    }
}
