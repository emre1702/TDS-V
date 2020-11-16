using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelOfflineMessagesHandler
    {
        #region Public Methods

        Task<object> Answer(ITDSPlayer player, ArraySegment<object> args);

        Task<object> Delete(ITDSPlayer _, ArraySegment<object> args);

        Task<string> GetData(ITDSPlayer player);

        Task<object> Send(ITDSPlayer player, ArraySegment<object> args);

        #endregion Public Methods
    }
}
