using System;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.Userpanel
{
#nullable enable

    public interface IUserpanelPlayerWeaponStatsHandler
    {
        string? GetData(ITDSPlayer player);
    }
}