using System.Collections.Generic;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Data.Interfaces.Handlers
{
    #nullable enable
    public interface ITDSPlayerHandler
    {
        List<ITDSPlayer> LoggedInPlayers { get; }
        ITDSPlayer? FindTDSPlayer(string name);
        ITDSPlayer? Get(int playerId);
    }
}
