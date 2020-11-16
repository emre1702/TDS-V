using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSupportAdminHandler
    {
        #region Public Methods

        Task<string> GetData(ITDSPlayer player);

        #endregion Public Methods
    }
}
