using System;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.Userpanel
{
#nullable enable

    public interface IUserpanelPlayerWeaponStatsHandler
    {
        #region Public Methods

        string? GetData(ITDSPlayer player);

        object? GetPlayerWeaponStats(ITDSPlayer player, ref ArraySegment<object> args);

        #endregion Public Methods
    }
}
