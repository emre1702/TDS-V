using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.Entities
{
#nullable enable

    public interface ITDSPlayer : ITDSPedBase, IDatabaseEntityWrapper, IPlayer, IEquatable<ITDSPlayer>
    {

        AdminLevelDto AdminLevel { get; }
        string AdminLevelName { get; }
        new int Armor { get; set; }
        RoundStatsDto? CurrentRoundStats { get; set; }
        new WeaponHash CurrentWeapon { get; set; }
        string DisplayName { get; }
        Players? Entity { get; set; }
        PedHash FreemodeSkin { get; }

        new ITDSVehicle Vehicle { get; }
        IVehicle? FreeroamVehicle { get; set; }
        IGang Gang { get; set; }
        GangRanks? GangRank { get; set; }
        new int Health { get; set; }
        new int Id { get; }
        int AltVId { get; }
        ITDSPlayer? InPrivateChatWith { get; set; }
        bool IsConsole { get; set; }
        bool IsCrouched { get; set; }
        bool IsGangOwner { get; }
        bool IsInGang { get; }
        bool IsLobbyOwner { get; }
        bool IsMuted { get; }
        bool IsPermamuted { get; }
        bool IsVip { get; }
        bool IsVoiceMuted { get; }

        void SetCollisionsless(bool toggle, ILobby lobby);

        short KillingSpree { get; set; }
        ILanguage Language { get; }
        Language LanguageEnum { get; set; }
        ITDSPlayer? LastHitter { get; set; }
        DateTime? LastKillAt { get; set; }
        WeaponHash LastWeaponOnHand { get; set; }
        short Lifes { get; set; }
        ILobby? Lobby { get; set; }
        PlayerLobbyStats? LobbyStats { get; }
        bool LoggedIn { get; set; }
        int Money { get; set; }
        int? MuteTime { get; set; }
        int PlayMinutes { get; set; }
        ILobby? PreviousLobby { get; set; }

        void Freeze(bool toggle);

        ITDSPlayer? SentPrivateChatRequestTo { get; set; }
        short ShortTimeKillingSpree { get; }
        ITDSPlayer? Spectates { get; set; }
        HashSet<ITDSPlayer> Spectators { get; }
        ITeam? Team { get; }
        int TeamIndex { get; }
        bool TryingToLoginRegister { get; set; }
        int? VoiceMuteTime { get; set; }
        Dictionary<WeaponHash, Dictionary<PedBodyPart, PlayerWeaponBodypartStats>>? WeaponBodyPartsStats { get; }

        void SetSkin(PedHash pedHash);

        Dictionary<WeaponHash, PlayerWeaponStats>? WeaponStats { get; }
        int Transparency { get; set; }

        void AddHPArmor(int healtharmor);

        void AddToChallenge(ChallengeType challengeType, int amount = 1, bool setTheValue = false);

        void AddWeaponShot(WeaponHash weaponHash, PedBodyPart? pedBodyPart, int? damage, bool killed);

        void ChangeMuteTime(ITDSPlayer target, int minutes, string reason);

        void ChangeVoiceMuteTime(ITDSPlayer player, int minutes, string reason);

        void CheckPlayerOnlineIsFriend(ITDSPlayer otherPlayer, bool outputInfo = true);

        void CheckReduceMapBoughtCounter();

        void CheckSaveData();

        void ClosePrivateChat(bool v);

        void Damage(ref int damage, out bool killed);

        string GetLocalDateTimeString(DateTime createTime);
        void SetIntoVehicle(ITDSVehicle vehicle, sbyte seat = -2);
        PlayerRelation GetRelationTo(ITDSPlayer target);

        void GiveMoney(uint money);

        void GiveMoney(int money);

        bool HasRelationTo(ITDSPlayer target, PlayerRelation block);

        void LoadTimezone();

        void OnPlayerWeaponSwitch(WeaponHash previousWeapon, WeaponHash newWeapon);

        void RemovePlayerFromOnlineFriend(ITDSPlayer otherPlayer, bool outputInfo = true);

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

        void Spawn(Position position, float rotation);
        void Kill(string? reason = null);
        void SetClothes(int slot, int drawable, int texture);
    }
}
