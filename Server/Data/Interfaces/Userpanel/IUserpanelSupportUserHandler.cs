using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSupportUserHandler
    {
        Task<string> GetData(ITDSPlayer player);
    }
}
