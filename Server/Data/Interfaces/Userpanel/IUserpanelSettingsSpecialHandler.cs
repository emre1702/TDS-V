using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSettingsSpecialHandler
    {
        string GetData(ITDSPlayer player);
        Task<object> SetData(ITDSPlayer player, ArraySegment<object> args);
    }
}
