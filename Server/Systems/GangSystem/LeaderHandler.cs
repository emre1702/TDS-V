using GTANetworkAPI;
using MoreLinq;
using System;
using System.Linq;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.GangsSystem
{
    public class LeaderHandler : IGangLeaderHandler
    {
        private readonly IGang _gang;

        public LeaderHandler(IGang gang)
        {
            _gang = gang;
        }

        public void AppointNextSuitableLeader()
        {
            if (_gang.Entity.Members.Count == 0)
                return;

            var nextLeader = GetNextSuitableLeaderOnlyActive();
            if (nextLeader is null)
                nextLeader = GetNextSuitableLeaderAlsoInactive();
            if (nextLeader is null)
                nextLeader = _gang.Entity.Members.First();

            _gang.Entity.OwnerId = nextLeader.PlayerId;

            var onlinePlayer = _gang.Players.GetOnline(nextLeader.PlayerId);
            if (onlinePlayer is { })
                NAPI.Task.RunSafe(() =>
                    onlinePlayer?.SendNotification(onlinePlayer.Language.YOUVE_BECOME_GANG_LEADER));
        }

        private GangMembers? GetNextSuitableLeaderOnlyActive()
        {
            var activeMembers = _gang.Entity.Members.Where(m => (DateTime.UtcNow - m.Player.PlayerStats.LastLoginTimestamp).TotalDays < 5);
            var count = activeMembers.Count();

            if (count == 0)
                return null;

            var rankHighestMembers = activeMembers.MaxBy(m => m.Rank);
            count = rankHighestMembers.Count();

            if (count == 1)
                return rankHighestMembers.First();

            return rankHighestMembers.MaxBy(m => (DateTime.UtcNow - m.JoinTime).TotalSeconds).FirstOrDefault();
        }

        private GangMembers? GetNextSuitableLeaderAlsoInactive()
        {
            var rankHighestMembers = _gang.Entity.Members.MaxBy(m => m.Rank);
            var count = rankHighestMembers.Count();

            if (count == 1)
                return rankHighestMembers.First();

            return rankHighestMembers.MaxBy(m => (DateTime.UtcNow - m.JoinTime).TotalSeconds).FirstOrDefault();
        }
    }
}
