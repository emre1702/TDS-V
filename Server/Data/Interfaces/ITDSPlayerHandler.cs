using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces
{
#nullable enable
    public interface ITDSPlayerHandler
    {
        int AmountLoggedInPlayers { get; }
        ICollection<ITDSPlayer> LoggedInPlayers { get; }
        ITDSPlayer? GetPlayer(int playerId);
        ITDSPlayer? GetIfLoggedIn(ushort remoteId);
        ITDSPlayer? FindTDSPlayer(string name);
    }
}
