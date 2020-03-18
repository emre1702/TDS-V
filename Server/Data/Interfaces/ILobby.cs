using System;
using System.Collections.Generic;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces
{
    public interface ILobby : IEquatable<ILobby>
    {
        int Id { get; }
        uint Dimension { get; }
        HashSet<ITDSPlayer> Players { get; }
        bool IsOfficial { get; }
        bool SavePlayerLobbyStats { get; }
        int StartTotalHP { get; }
        LobbyType Type { get; }

        bool IsPlayerLobbyOwner(ITDSPlayer player);
        void RemovePlayer(ITDSPlayer player);
    }
}
