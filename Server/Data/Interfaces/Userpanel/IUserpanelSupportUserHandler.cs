using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSupportUserHandler
    {
        #region Public Methods

        Task<string> GetData(ITDSPlayer player);

        #endregion Public Methods
    }
}
