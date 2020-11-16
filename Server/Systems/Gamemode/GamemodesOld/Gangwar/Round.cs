/*using TDS.Server.Data.Enums;
using TDS.Server.Handler.Entities.Utility;

namespace TDS.Server.LobbySystem.Gamemodes
{
    partial class Gangwar
    {
        public override bool CanEndRound(RoundEndReason endReason)
        {
            return endReason switch
            {
                RoundEndReason.BombDefused => false,
                RoundEndReason.BombExploded => false,
                RoundEndReason.PlayerWon => false,
                RoundEndReason.Death => !Lobby.IsGangActionLobby,     // will handle this manually if gang action lobby
                RoundEndReason.Empty => !Lobby.IsGangActionLobby,     // will handle this manually if gang action lobby
                RoundEndReason.NewPlayer => !Lobby.IsGangActionLobby,

                RoundEndReason.TargetEmpty => true,
                RoundEndReason.Time => true,
                RoundEndReason.Command => true,

                _ => true
            };
        }
    }
}
*/
