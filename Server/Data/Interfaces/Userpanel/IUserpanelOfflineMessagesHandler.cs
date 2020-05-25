using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelOfflineMessagesHandler
    {
        #region Public Methods

        Task<object> Answer(ITDSPlayer player, ArraySegment<object> args);

        Task<object> Delete(ITDSPlayer player, ArraySegment<object> args);

        Task<string> GetData(ITDSPlayer player);

        Task<object> Send(ITDSPlayer player, ArraySegment<object> args);

        #endregion Public Methods
    }
}
