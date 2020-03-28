using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        HashSet<ITDSPlayer> Players { get; }
        bool IsOfficial { get; }
        bool SavePlayerLobbyStats { get; }
        int StartTotalHP { get; }
        LobbyType Type { get; }
        Lobbies Entity { get; }
        string OwnerName { get; }

        bool IsPlayerLobbyOwner(ITDSPlayer player);
        void RemovePlayer(ITDSPlayer player);
        void UnbanPlayer(ITDSPlayer player, ITDSPlayer target, string reason);
        void SendAllPlayerLangMessage(Func<ILanguage, string> langGetter, ITeam? targetTeam = null);
        void BanPlayer(ITDSPlayer player, ITDSPlayer target, DateTime? length, string reason);
        void UnbanPlayer(ITDSPlayer player, Players dbTarget, string reason);
        void BanPlayer(ITDSPlayer player, Players dbTarget, DateTime? length, string reason, string? serial = null);
        Task<bool> AddPlayer(ITDSPlayer iTDSPlayer, uint? teamIndex);
        void SendTeamOrder(ITDSPlayer player, TeamOrder teamOrder);
    }
}
