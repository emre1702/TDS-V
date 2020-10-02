using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.Data.Interfaces.LobbySystem.MapVotings
{
#nullable enable

    public interface IArenaMapVoting
    {
        void BuyMap(ITDSPlayer player, int mapId);

        string? GetJson();

        MapDto? GetVotedMap();

        void VoteForMap(ITDSPlayer player, int mapId);
    }
}
