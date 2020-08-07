using System.Collections.Generic;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.Data.Interfaces
{
    #nullable enable
    public interface ITDSPlayerHandler
    {
        int AmountLoggedInPlayers { get; }
        ICollection<ITDSPlayer> LoggedInPlayers { get; }
        ITDSPlayer Get(IPlayer modPlayer);
        ITDSPlayer? GetIfExists(int playerId);
        ITDSPlayer? GetIfLoggedIn(IPlayer modPlayer);
        ITDSPlayer? GetIfLoggedIn(ushort remoteId);
        ITDSPlayer GetNotLoggedIn(IPlayer modPlayer);
        ITDSPlayer? FindTDSPlayer(string name);
    }
}
