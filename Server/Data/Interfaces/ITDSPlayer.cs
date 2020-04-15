using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces
{
#nullable enable

    public interface ITDSPlayer : ITDSEntity, IEquatable<ITDSPlayer>
    {
        ITDSPlayer? InPrivateChatWith { get; set; }
        AdminLevelDto AdminLevel { get; }
        RoundStatsDto? CurrentRoundStats { get; set; }
        string DisplayName { get; }
        Players? Entity { get; }
        int Id { get; }
        bool IsVip { get; }
        ILanguage Language { get; }
        ILobby? Lobby { get; set; }
        IPlayer? ModPlayer { get; set; }
        ushort RemoteId { get; }
        ITeam? Team { set; get; }
        bool IsPermamuted { get; }
        bool IsMuted { get; }
        int? MuteTime { get; set; }

        void LoadTimezone();

        HashSet<int> BlockingPlayerIds { get; }
        string AdminLevelName { get; }
        bool LoggedIn { get; }
        ulong SocialClubId { get; }
        GangRanks? GangRank { get; set; }
        IGang Gang { get; set; }
        ITDSPlayer? Spectates { get; set; }

        bool HandleBan(PlayerBans? ban);

        sbyte Lifes { get; set; }
        HashSet<ITDSPlayer> Spectators { get; }
        int Health { get; set; }
        int Armor { get; set; }
        WeaponHash LastWeaponOnHand { get; set; }
        DateTime? LastKillAt { get; set; }
        short KillingSpree { get; set; }

        string GetLocalDateTimeString(DateTime createTime);

        PedHash FreemodeSkin { get; }
        bool IsVoiceMuted { get; }
        int TeamIndex { get; }
        ITDSPlayer? LastHitter { get; set; }
        PlayerLobbyStats? LobbyStats { get; }
        bool IsLobbyOwner { get; }
        int Money { get; set; }
        ILobby? PreviousLobby { get; set; }
        short ShortTimeKillingSpree { get; }
        IVehicle? FreeroamVehicle { get; set; }
        int PlayMinutes { get; set; }
        int? VoiceMuteTime { get; set; }
        Language LanguageEnum { get; set; }
        bool IsConsole { get; set; }
        List<PlayerRelations> PlayerRelationsPlayer { get; }
        List<PlayerRelations> PlayerRelationsTarget { get; }
        bool IsCrouched { get; set; }

        void SendBrowserEvent(string eventName, params object[] args);
        void SendEvent(string eventName, params object[] args);
        void SendMessage(string msg);
        void SendNotification(string msg, bool flashing = false);
        void GiveMoney(uint money);
        void GiveMoney(int money);
        void AddToChallenge(ChallengeType challengeType, int amount = 1, bool setTheValue = false);
        void Damage(ref int damage);
        bool HasRelationTo(ITDSPlayer target, PlayerRelation block);
        void SetVoiceTo(ITDSPlayer target, bool v);
        void Spawn(Position3D position, float rotation);
        Task ExecuteForDB(Action<TDSDbContext> p);
        void SetEntityInvincible(IVehicle vehicle, bool invincible);
        Task ExecuteForDBAsync(Func<TDSDbContext, Task> p);
        Task<T> ExecuteForDBAsync<T>(Func<TDSDbContext, Task<T>> action);
        void CheckSaveData();
        void CheckReduceMapBoughtCounter();
        ValueTask SaveData(bool force = false);
        void ClosePrivateChat(bool v);
        void ChangeVoiceMuteTime(ITDSPlayer player, int minutes, string reason);
        void ChangeMuteTime(ITDSPlayer target, int minutes, string reason);
        Task SetPlayerLobbyStats(PlayerLobbyStats? playerLobbyStats);
    }
}
