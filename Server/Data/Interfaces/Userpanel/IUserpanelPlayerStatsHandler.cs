using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Shared.Data.Models.PlayerCommands;

namespace TDS.Server.Data.Interfaces.Userpanel
{
#nullable enable

    public interface IUserpanelPlayerCommandsHandler
    {
        Task<UserpanelPlayerCommandData?> GetData(ITDSPlayer player);

        Task<object?> Save(ITDSPlayer player, ArraySegment<object> args);
    }
}
