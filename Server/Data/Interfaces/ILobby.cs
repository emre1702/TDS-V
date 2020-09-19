using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces
{
#nullable enable

    public interface ILobby : IDatabaseEntityWrapper, IEquatable<ILobby>
    {
        uint Dimension { get; }
        Lobbies Entity { get; }
        int Id { get; }
        bool IsGangActionLobby { get; }
        bool IsOfficial { get; }
        string Name { get; }
        string OwnerName { get; }
        ConcurrentDictionary<int, ITDSPlayer> Players { get; }
        bool SavePlayerLobbyStats { get; }
        int StartTotalHP { get; }
        List<ITeam> Teams { get; set; }
        LobbyType Type { get; }

        Task<bool> AddPlayer(ITDSPlayer iTDSPlayer, uint? teamIndex);

        Task AddToDB();

        Task<PlayerBans?> BanPlayer(ITDSPlayer player, ITDSPlayer target, TimeSpan? length, string reason);

        Task<PlayerBans?> BanPlayer(ITDSPlayer player, Players dbTarget, TimeSpan? length, string reason, string? serial = null);

        void FuncIterateAllPlayers(Action<ITDSPlayer, ITeam?> func);

        Task<bool> IsPlayerBaned(ITDSPlayer player);

        bool IsPlayerLobbyOwner(ITDSPlayer player);

        void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon, bool spawnPlayer = true);

        void OnPlayerEnterColshape(ITDSColShape colshape, ITDSPlayer player);

        void OnPlayerSpawn(ITDSPlayer player);

        void PlaySound(string soundName);

        Task RemovePlayer(ITDSPlayer player);

        void SendMessage(string msg, ITeam? targetTeam = null);

        void SendMessage(string msg, HashSet<int> blockingPlayerIds, ITeam? targetTeam = null);

        void SendMessage(Func<ILanguage, string> langGetter, ITeam? targetTeam = null);

        void SendMessage(Dictionary<ILanguage, string> texts, ITeam? targetTeam = null);

        void SendNotification(Func<ILanguage, string> langgetter, ITeam? targetTeam = null, bool flashing = false);

        void SendTeamOrder(ITDSPlayer player, TeamOrder teamOrder);

        void UnbanPlayer(ITDSPlayer player, ITDSPlayer target, string reason);

        void UnbanPlayer(ITDSPlayer player, Players dbTarget, string reason);

        void TriggerEvent(string eventName, params object[] args);
    }
}
