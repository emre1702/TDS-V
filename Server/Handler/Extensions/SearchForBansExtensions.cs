using System.Linq;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Handler.Extensions
{
    public static class SearchForBansExtensions
    {
        public static IQueryable<PlayerBans> WherePlayerAndLobby(this IQueryable<PlayerBans> playerBans, int playerId, int lobbyId)
           => playerBans.Where(ban => ban.PlayerId == playerId && ban.LobbyId == lobbyId);

        public static IQueryable<PlayerBans> WhereAllConditions(this IQueryable<PlayerBans> playerBans, int lobbyId,
                   int? playerId = null, string? ip = null, string? serial = null, string? socialClubName = null, ulong? socialClubId = null,
                   bool? preventConnection = null)
            => playerBans.Where(b => b.LobbyId == lobbyId
                        && (playerId == null || b.PlayerId == playerId)
                        && (ip == null || b.IP == ip)
                        && (serial == null || b.Serial == serial)
                        && (socialClubName == null || b.SCName == socialClubName)
                        && (socialClubId == null || b.SCId == socialClubId)
                        && (preventConnection == null || b.PreventConnection == preventConnection));

        public static IQueryable<PlayerBans> WhereOneCondition(this IQueryable<PlayerBans> playerBans, int lobbyId,
                  int? playerId = null, string? ip = null, string? serial = null, string? socialClubName = null, ulong? socialClubId = null,
                  bool? preventConnection = null)
          => playerBans.Where(b => b.LobbyId == lobbyId && (
                      (playerId == null || b.PlayerId == playerId)
                      || (ip == null || b.IP == ip)
                      || (serial == null || b.Serial == serial)
                      || (socialClubName == null || b.SCName == socialClubName)
                      || (socialClubId == null || b.SCId == socialClubId))
                      && (preventConnection == null || b.PreventConnection == preventConnection));
    }
}
