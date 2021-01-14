using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.MapHandlers;

namespace TDS.Server.LobbySystem.MapHandlers
{
#nullable enable

    public interface IMapCreatorMapHandler : IBaseLobbyMapHandler
    {
        void SyncLocation(ITDSPlayer player, string json);
    }
}