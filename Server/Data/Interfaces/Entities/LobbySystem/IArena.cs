using System.Collections.Generic;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.Data.Interfaces.Entities.LobbySystem
{
    #nullable enable
    public interface IArena : IFightLobby
    {
        IGamemode CurrentGameMode { get; }
        IGangwarArea? GangwarArea { get; set; }
        ITDSPlayer CurrentRoundEndBecauseOfPlayer { get; set; }

        void BuyMap(ITDSPlayer player, int mapId);
        void ChooseTeam(ITDSPlayer player, int index);
        void SendMapsForVoting(ITDSPlayer player);
        void MapVote(ITDSPlayer player, int mapId);
        void SetMapList(List<MapDto> mapsList);
        void SetRoundStatus(RoundStatus status, RoundEndReason reason);
    }
}
