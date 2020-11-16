using System;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.Userpanel
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
