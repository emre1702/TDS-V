using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Core;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Data.Abstracts.Entities.GTA
{
#nullable enable

    public abstract class ITDSPlayer : Player, IEquatable<ITDSPlayer>
    {
        public abstract IPlayerAdmin Admin { get; }
        public abstract IPlayerChallengesHandler Challenges { get; }
        public abstract IPlayerChat Chat { get; }
        public abstract IPlayerDatabaseHandler DatabaseHandler { get; }
        public abstract IPlayerDeathmatch Deathmatch { get; }
        public abstract IPlayerEvents Events { get; }
        public abstract IPlayerGangHandler GangHandler { get; }
        public abstract IPlayerHealthAndArmor HealthAndArmor { get; }
        public abstract IPlayerLanguageHandler LanguageHandler { get; }
        public abstract IPlayerLobbyHandler LobbyHandler { get; }
        public abstract IPlayerMapsVoting MapsVoting { get; }
        public abstract IPlayerMoneyHandler MoneyHandler { get; }
        public abstract IPlayerMuteHandler MuteHandler { get; }
        public abstract IPlayerPlayTime PlayTime { get; }
        public abstract IPlayerRelations Relations { get; }
        public abstract IPlayerSpectateHandler SpectateHandler { get; }
        public abstract IPlayerSync Sync { get; }
        public abstract IPlayerTeamHandler TeamHandler { get; }
        public abstract IPlayerTimezone Timezone { get; }
        public abstract IPlayerVoice Voice { get; }
        public abstract IPlayerWeaponStats WeaponStats { get; }

        public IDatabaseHandler Database => DatabaseHandler.Database;
        public ILanguage Language => LanguageHandler.Data;
        public IBaseLobby? Lobby => LobbyHandler.Current;
        public Players? Entity => DatabaseHandler.Entity;

        public new int Id => Entity?.Id ?? 0;

        public new WeaponHash CurrentWeapon
        {
            get => base.CurrentWeapon;
            set => NAPI.Player.SetPlayerCurrentWeapon(this, value);
        }

        public string DisplayName => IsConsole
            ? "Console"
            : (AdminLevel.Level >= SharedConstants.ServerTeamSuffixMinAdminLevel
                ? SharedConstants.ServerTeamSuffix + (Entity is { } ? Entity.Name : Name)
                : (Entity is { } ? Entity.Name : Name));

        public PedHash FreemodeSkin => Entity?.CharDatas.GeneralData.ElementAt(Entity.CharDatas.SyncedData.Slot).SyncedData.IsMale == true
            ? PedHash.FreemodeMale01
            : PedHash.FreemodeFemale01;

        public bool IsConsole { get; set; }
        public bool IsVip => Entity?.IsVip ?? false;
        public bool LoggedIn => Entity?.PlayerStats?.LoggedIn == true;
        public int Money { get => MoneyHandler.Money; set => MoneyHandler.Money = value; }

        public short Lifes { get; set; } = 0;
        public bool IsLobbyOwner => Lobby?.Players.IsLobbyOwner(this) == true;

        public ITeam? Team => TeamHandler.Team;
        public int TeamIndex => Team?.Entity.Index ?? 0;

        public IGang Gang { get => GangHandler.Gang; set => GangHandler.Gang = value; }
        public GangRanks? GangRank { get; set; }
        public bool IsGangOwner => Gang.Entity.OwnerId == Entity?.Id;
        public bool IsInGang => Gang.Entity.Id > 0;

        public ITDSPlayer? InPrivateChatWith { get; set; }
        public ITDSPlayer? SentPrivateChatRequestTo { get; set; }

        public new void SendChatMessage(string msg) => Chat.SendChatMessage(msg);

        public new void SendNotification(string msg, bool flashing = false) => Chat.SendNotification(msg, flashing);

        public void TriggerBrowserEvent(params object[] eventNameAndArgs) => Sync.TriggerBrowserEvent(eventNameAndArgs);

        public abstract AdminLevelDto AdminLevel { get; }
        public abstract string AdminLevelName { get; }
        public virtual new int Armor { get => base.Armor; set => base.Armor = value; }
        public RoundStatsDto? CurrentRoundStats { get; set; }

        public abstract ITDSVehicle? FreeroamVehicle { get; set; }

        public virtual new int Health { get => base.Health; set => base.Health = value; }

        public abstract bool IsCrouched { get; set; }

        public abstract bool IsMuted { get; }
        public abstract bool IsPermamuted { get; }
        public abstract bool IsVoiceMuted { get; }
        public abstract short KillingSpree { get; set; }

        public abstract ITDSPlayer? LastHitter { get; set; }
        public abstract DateTime? LastKillAt { get; set; }
        public abstract WeaponHash LastWeaponOnHand { get; set; }

        public PlayerLobbyStats? LobbyStats { get; set; }

        public abstract int? MuteTime { get; set; }
        public abstract int PlayMinutes { get; set; }
        public IBaseLobby? PreviousLobby { get; set; }
        public ushort RemoteId => Handle.Value;
        public abstract short ShortTimeKillingSpree { get; }
        public ITDSPlayer? Spectates { get; set; }
        public abstract PlayerTotalStats? TotalStats { get; }
        public bool TryingToLoginRegister { get; set; }
        public new ITDSVehicle? Vehicle => base.Vehicle as ITDSVehicle;
        public abstract int? VoiceMuteTime { get; set; }
        public TDSTimer? DeathSpawnTimer { get; set; }

        public ITDSPlayer(NetHandle netHandle) : base(netHandle)
        {
        }

        public abstract void SetEntityInvincible(ITDSVehicle vehicle, bool invincible);

        public abstract void AttachTo(ITDSPlayer player, PedBone bone, Vector3? positionOffset, Vector3? rotationOffset);

        public abstract void Detach();

        public abstract void Freeze(bool toggle);

        public abstract void SetCollisionsless(bool toggle);

        public abstract void SetInvisible(bool toggle);

        public abstract void AddHPArmor(int healtharmor);

        public abstract void AddToChallenge(ChallengeType challengeType, int amount = 1, bool setTheValue = false);

        public abstract void AddSpectator(ITDSPlayer spectator);

        public abstract void ChangeMuteTime(ITDSPlayer target, int minutes, string reason);

        public abstract void ChangeVoiceMuteTime(ITDSPlayer player, int minutes, string reason);

        public abstract void CheckPlayerOnlineIsFriend(ITDSPlayer otherPlayer, bool outputInfo = true);

        public abstract void CheckReduceMapBoughtCounter();

        public abstract void CheckSaveData();

        public abstract void ClosePrivateChat(bool v);

        public abstract void Damage(ref int damage, out bool killed);

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

        public abstract void RemovePlayerFromOnlineFriend(ITDSPlayer otherPlayer, bool outputInfo = true);

        public abstract void RemoveSpectator(ITDSPlayer spectator);

        public abstract void ResetVoiceToAndFrom();

        public abstract ValueTask SaveData(bool force = false);

        public abstract void TriggerBrowserEvent(string eventName, params object[] args);

        public abstract void SetBoughtMap(int price);

        public abstract void SetInvincible(bool toggle);

        public abstract void SetLobby(Interfaces.LobbySystem.Lobbies.Abstracts.IBaseLobby lobby);

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
