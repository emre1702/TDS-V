using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Data.Abstracts.Entities.GTA
{
#nullable enable

    public abstract class ITDSPlayer : Player, IEquatable<ITDSPlayer>
    {
        public abstract AdminLevelDto AdminLevel { get; }
        public abstract string AdminLevelName { get; }
        public virtual new int Armor { get => base.Armor; set => base.Armor = value; }
        public abstract RoundStatsDto? CurrentRoundStats { get; set; }

        public new WeaponHash CurrentWeapon
        {
            get => base.CurrentWeapon;
            set => NAPI.Player.SetPlayerCurrentWeapon(this, value);
        }

        public abstract IDatabaseEntityWrapper Database { get; }
        public abstract string DisplayName { get; }
        public abstract Players? Entity { get; set; }
        public abstract PedHash FreemodeSkin { get; }
        public abstract ITDSVehicle? FreeroamVehicle { get; set; }
        public abstract IGang Gang { get; set; }
        public abstract GangRanks? GangRank { get; set; }
        public virtual new int Health { get => base.Health; set => base.Health = value; }
        public new int Id => Entity?.Id ?? 0;
        public abstract ITDSPlayer? InPrivateChatWith { get; set; }
        public string IPAddress => Address;
        public abstract bool IsConsole { get; set; }
        public abstract bool IsCrouched { get; set; }
        public abstract bool IsGangOwner { get; }
        public abstract bool IsInGang { get; }
        public abstract bool IsLobbyOwner { get; }

        public abstract bool IsMuted { get; }
        public abstract bool IsPermamuted { get; }
        public abstract bool IsVip { get; }
        public abstract bool IsVoiceMuted { get; }
        public abstract short KillingSpree { get; set; }
        public abstract ILanguage Language { get; set; }
        public abstract Language LanguageEnum { get; set; }
        public abstract ITDSPlayer? LastHitter { get; set; }
        public abstract DateTime? LastKillAt { get; set; }
        public abstract WeaponHash LastWeaponOnHand { get; set; }
        public short Lifes { get; set; } = 0;
        public ILobby? Lobby { get; set; }
        public IBaseLobby? LobbyNew { get; set; }
        public abstract PlayerLobbyStats? LobbyStats { get; set; }
        public abstract bool LoggedIn { get; }
        public abstract int Money { get; set; }
        public abstract int? MuteTime { get; set; }
        public abstract int PlayMinutes { get; set; }
        public abstract ILobby? PreviousLobby { get; set; }
        public ushort RemoteId => Handle.Value;
        public abstract ITDSPlayer? SentPrivateChatRequestTo { get; set; }
        public abstract short ShortTimeKillingSpree { get; }
        public ITDSPlayer? Spectates { get; set; }
        public abstract ITeam? Team { get; }
        public abstract int TeamIndex { get; }
        public abstract PlayerTotalStats? TotalStats { get; }
        public bool TryingToLoginRegister { get; set; }
        public new ITDSVehicle? Vehicle => base.Vehicle as ITDSVehicle;
        public abstract int? VoiceMuteTime { get; set; }
        public abstract Dictionary<WeaponHash, Dictionary<PedBodyPart, PlayerWeaponBodypartStats>>? WeaponBodyPartsStats { get; set; }
        public abstract Dictionary<WeaponHash, PlayerWeaponStats>? WeaponStats { get; set; }
        public TDSTimer? DeathSpawnTimer { get; set; }

        public ITDSPlayer(NetHandle netHandle) : base(netHandle)
        {
        }

        public abstract void AddHPArmor(int healtharmor);

        public abstract void AddToChallenge(ChallengeType challengeType, int amount = 1, bool setTheValue = false);

        public abstract void AddSpectator(ITDSPlayer spectator);

        public abstract void AddWeaponShot(WeaponHash weaponHash, PedBodyPart? pedBodyPart, int? damage, bool killed);

        public abstract void AttachTo(ITDSPlayer player, PedBone bone, Vector3? positionOffset, Vector3? rotationOffset);

        public abstract void ChangeMuteTime(ITDSPlayer target, int minutes, string reason);

        public abstract void ChangeVoiceMuteTime(ITDSPlayer player, int minutes, string reason);

        public abstract void CheckPlayerOnlineIsFriend(ITDSPlayer otherPlayer, bool outputInfo = true);

        public abstract void CheckReduceMapBoughtCounter();

        public abstract void CheckSaveData();

        public abstract void ClosePrivateChat(bool v);

        public abstract void Damage(ref int damage, out bool killed);

        public abstract void Detach();

        public abstract void Freeze(bool toggle);

        public abstract DateTime GetLocalDateTime(DateTime dateTime);

        public abstract string GetLocalDateTimeString(DateTime createTime);

        public abstract PlayerRelation GetRelationTo(ITDSPlayer target);

        public abstract List<ITDSPlayer> GetSpectators();

        public abstract void GiveMoney(uint money);

        public abstract void GiveMoney(int money);

        public abstract bool HasRelationTo(ITDSPlayer target, PlayerRelation block);

        public abstract bool HasSpectators();

        public abstract void InitChallengesDict();

        public abstract void LoadTimezone();

        public abstract void OnPlayerWeaponSwitch(WeaponHash previousWeapon, WeaponHash newWeapon);

        public abstract void RemovePlayerFromOnlineFriend(ITDSPlayer otherPlayer, bool outputInfo = true);

        public abstract void RemoveSpectator(ITDSPlayer spectator);

        public abstract void ResetVoiceToAndFrom();

        public abstract ValueTask SaveData(bool force = false);

        public abstract void TriggerBrowserEvent(string eventName, params object[] args);

        public virtual new void SendChatMessage(string msg)
            => base.SendChatMessage(msg);

        public virtual new void SendNotification(string msg, bool flashing = false)
            => base.SendNotification(msg, flashing);

        public abstract void SetCollisionsless(bool toggle);

        public abstract void SetEntityInvincible(ITDSVehicle vehicle, bool invincible);

        public abstract void SetInvincible(bool toggle);

        public abstract void SetInvisible(bool toggle);

        public abstract void SetLobby(IBaseLobby lobby);

        public abstract void SetSpectates(ITDSPlayer? target);

        public abstract Task SetPlayerLobbyStats(PlayerLobbyStats? playerLobbyStats);

        public abstract void SetRelation(ITDSPlayer target, PlayerRelation relation);

        public abstract void SetTeam(ITeam? team, bool forceIsNew);

        public virtual void SetVoiceTo(ITDSPlayer target, bool toggle)
        {
            if (toggle)
                base.EnableVoiceTo(target);
            else
                base.DisableVoiceTo(target);
        }

        public abstract void Spawn(Vector3 pos, float heading = 0);

        protected new void DisableVoiceTo(Player _)
        {
        }

        protected new void EnableVoiceTo(Player _)
        {
        }

        public bool Equals(ITDSPlayer? other)
        {
            return Id == other?.Id;
        }
    }
}
