using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Models.PlayerCommands;

namespace TDS_Server.Data.Interfaces.Userpanel
{
#nullable enable

    public interface IUserpanelPlayerCommandsHandler
    {
        #region Public Methods

        Task<UserpanelPlayerCommandData?> GetData(ITDSPlayer player);

        Task<object?> Save(ITDSPlayer player, ArraySegment<object> args);

        #endregion Public Methods
    }
}
