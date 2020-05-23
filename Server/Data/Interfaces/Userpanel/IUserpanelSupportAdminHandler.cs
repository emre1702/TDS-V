using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSupportAdminHandler
    {
        Task<string> GetData(ITDSPlayer player);
    }
}
