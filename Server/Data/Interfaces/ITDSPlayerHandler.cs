using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces
{
#nullable enable
    public interface ITDSPlayerHandler
    {
        int AmountLoggedInPlayers { get; }
        ICollection<ITDSPlayer> LoggedInPlayers { get; }
        ITDSPlayer? GetIfExists(int playerId);
        ITDSPlayer? GetIfLoggedIn(ushort remoteId);
        ITDSPlayer? FindTDSPlayer(string name);
    }
}
