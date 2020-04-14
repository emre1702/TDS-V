using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces
{
#nullable enable
    public interface ILobby : IEquatable<ILobby>
    {
        int Id { get; }
        uint Dimension { get; }
        ConcurrentDictionary<int, ITDSPlayer> Players { get; }
        bool IsOfficial { get; }
        bool SavePlayerLobbyStats { get; }
        int StartTotalHP { get; }
        LobbyType Type { get; }
        Lobbies Entity { get; }
        string OwnerName { get; }

        bool IsPlayerLobbyOwner(ITDSPlayer player);
        Task RemovePlayer(ITDSPlayer player);
        void UnbanPlayer(ITDSPlayer player, ITDSPlayer target, string reason);
        void SendAllPlayerLangMessage(Func<ILanguage, string> langGetter, ITeam? targetTeam = null);
        Task BanPlayer(ITDSPlayer player, ITDSPlayer target, DateTime? length, string reason);
        void UnbanPlayer(ITDSPlayer player, Players dbTarget, string reason);
        Task BanPlayer(ITDSPlayer player, Players dbTarget, DateTime? length, string reason, string? serial = null);
        Task<bool> AddPlayer(ITDSPlayer iTDSPlayer, uint? teamIndex);
        void SendTeamOrder(ITDSPlayer player, TeamOrder teamOrder);
        void OnPlayerLoggedOut(ITDSPlayer tdsPlayer);
        void OnPlayerSpawn(ITDSPlayer player);
        void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon, bool spawnPlayer = true);
        void OnPlayerEnterColshape(IColShape colshape, ITDSPlayer player);
    }
}
