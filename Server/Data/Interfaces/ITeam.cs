using System;
using System.Collections.Generic;

namespace TDS_Server.Data.Interfaces
{
    public interface ITeam : IEquatable<ITeam>
    {
        HashSet<ITDSPlayer> Players { get; }
        string ChatColor { get; }
    }
}
