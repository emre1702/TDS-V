using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces
{
#nullable enable

    public interface ITDSPlayer : ITDSPedBase, IDatabaseEntityWrapper, IEquatable<ITDSPlayer>
    {
        #region Public Properties

        AdminLevelDto AdminLevel { get; }
        string AdminLevelName { get; }
        int Armor { get; set; }
        RoundStatsDto? CurrentRoundStats { get; set; }
        string DisplayName { get; }
        Players? Entity { get; }
        PedHash FreemodeSkin { get; }
        IVehicle? FreeroamVehicle { get; set; }
        IGang Gang { get; set; }
        GangRanks? GangRank { get; set; }
        int Health { get; set; }
        int Id { get; }
        ITDSPlayer? InPrivateChatWith { get; set; }
        string IPAddress { get; }
        bool IsConsole { get; set; }
        bool IsCrouched { get; set; }
        bool IsLobbyOwner { get; }
        bool IsMuted { get; }
        bool IsPermamuted { get; }
        bool IsVip { get; }
        bool IsVoiceMuted { get; }
        short KillingSpree { get; set; }
        ILanguage Language { get; }
        Language LanguageEnum { get; set; }
        ITDSPlayer? LastHitter { get; set; }
        DateTime? LastKillAt { get; set; }
        WeaponHash LastWeaponOnHand { get; set; }
        sbyte Lifes { get; set; }
        ILobby? Lobby { get; set; }
        PlayerLobbyStats? LobbyStats { get; }
        bool LoggedIn { get; }
        IPlayer? ModPlayer { get; set; }
        int Money { get; set; }
        int? MuteTime { get; set; }
        int PlayMinutes { get; set; }
        ILobby? PreviousLobby { get; set; }
        ushort RemoteId { get; }
        ITDSPlayer? SentPrivateChatRequestTo { get; set; }
        short ShortTimeKillingSpree { get; }
        ulong SocialClubId { get; }
        ITDSPlayer? Spectates { get; set; }
        HashSet<ITDSPlayer> Spectators { get; }
        ITeam? Team { get; }
        int TeamIndex { get; }

        bool TryingToLoginRegister { get; set; }

        int? VoiceMuteTime { get; set; }

        #endregion Public Properties

        #region Public Methods

        void AddHPArmor(int healtharmor);

        void AddToChallenge(ChallengeType challengeType, int amount = 1, bool setTheValue = false);

        void ChangeMuteTime(ITDSPlayer target, int minutes, string reason);

        void ChangeVoiceMuteTime(ITDSPlayer player, int minutes, string reason);

        void CheckReduceMapBoughtCounter();

        void CheckSaveData();

        void ClosePrivateChat(bool v);

        void Damage(ref int damage);

        string GetLocalDateTimeString(DateTime createTime);

        PlayerRelation GetRelationTo(ITDSPlayer target);

        void GiveMoney(uint money);

        void GiveMoney(int money);

        bool HasRelationTo(ITDSPlayer target, PlayerRelation block);

        void LoadTimezone();

        void ResetVoiceToAndFrom();

        ValueTask SaveData(bool force = false);

        void SendBrowserEvent(string eventName, params object[] args);

        void SendEvent(string eventName, params object[] args);

        void SendMessage(string msg);

        void SendNotification(string msg, bool flashing = false);

        void SetEntityInvincible(IVehicle vehicle, bool invincible);

        Task SetPlayerLobbyStats(PlayerLobbyStats? playerLobbyStats);

        void SetRelation(ITDSPlayer target, PlayerRelation relation);

        void SetTeam(ITeam? team, bool forceIsNew);

        void SetVoiceTo(ITDSPlayer target, bool v);

        void Spawn(Position3D position, float rotation);

        #endregion Public Methods
    }
}
