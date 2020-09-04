using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSupportAdminHandler
    {
        #region Public Methods

        Task<string> GetData(ITDSPlayer player);

        #endregion Public Methods
    }
}
