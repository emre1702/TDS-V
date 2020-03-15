using System;
using System.Collections.Generic;

namespace TDS_Server.Data.Interfaces
{
    public interface ILobby : IEquatable<ILobby>
    {
        int Id { get; }
        uint Dimension { get; }
        HashSet<ITDSPlayer> Players { get; }
        bool IsOfficial { get; }

        bool IsPlayerLobbyOwner(ITDSPlayer player);
    }
}
