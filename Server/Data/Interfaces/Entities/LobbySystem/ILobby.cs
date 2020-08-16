using AltV.Net.Elements.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.Entities.LobbySystem
{
#nullable enable

    public interface ILobby : IDatabaseEntityWrapper, IEquatable<ILobby>
    {
        #region Public Properties

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
        string CreatorName { get; }

        #endregion Public Properties

        #region Public Methods

        Task<bool> AddPlayer(ITDSPlayer iTDSPlayer, uint? teamIndex);

        Task AddToDB();

        Task<PlayerBans?> BanPlayer(ITDSPlayer player, ITDSPlayer target, TimeSpan? length, string reason);

        Task<PlayerBans?> BanPlayer(ITDSPlayer player, Players dbTarget, TimeSpan? length, string reason, ulong? hwid = null, ulong? hwidEx = null);

        void FuncIterateAllPlayers(Action<ITDSPlayer, ITeam?> func);

        Task<bool> IsPlayerBaned(ITDSPlayer player);

        bool IsPlayerLobbyOwner(ITDSPlayer player);

        void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon, bool spawnPlayer = true);

        void OnPlayerEnterColShape(ITDSColShape colshape, ITDSPlayer player);

        void OnPlayerLoggedOut(ITDSPlayer tdsPlayer);

        void OnPlayerSpawn(ITDSPlayer player);

        void PlaySound(string soundName);

        Task RemovePlayer(ITDSPlayer player);

        void SendAllPlayerChatMessage(string msg, ITeam? targetTeam = null);

        void SendAllPlayerChatMessage(string msg, HashSet<int> blockingPlayerIds, ITeam? targetTeam = null);

        void SendAllPlayerLangMessage(Func<ILanguage, string> langGetter, ITeam? targetTeam = null);

        void SendAllPlayerLangMessage(Dictionary<ILanguage, string> texts, ITeam? targetTeam = null);

        void SendAllPlayerLangNotification(Func<ILanguage, string> langgetter, ITeam? targetTeam = null, bool flashing = false);

        void SendEvent(string eventName, params object[] args);

        void SendNative(NativeHash native, params object[] args);

        void SendTeamOrder(ITDSPlayer player, TeamOrder teamOrder);

        void UnbanPlayer(ITDSPlayer player, ITDSPlayer target, string reason);

        void UnbanPlayer(ITDSPlayer player, Players dbTarget, string reason);

        #endregion Public Methods
    }
}
