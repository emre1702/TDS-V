using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSettingsSpecialHandler
    {
        #region Public Methods

        string GetData(ITDSPlayer player);

        Task<object> SetData(ITDSPlayer player, ArraySegment<object> args);

        #endregion Public Methods
    }
}
