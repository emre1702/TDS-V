using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSupportAdminHandler
    {
        #region Public Methods

        Task<string> GetData(ITDSPlayer player);

        #endregion Public Methods
    }
}
