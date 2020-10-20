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
            : (Admin.Level.Level >= SharedConstants.ServerTeamSuffixMinAdminLevel
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

        public void SendAlert(string msg) => Chat.SendAlert(msg);

        public void TriggerBrowserEvent(params object[] eventNameAndArgs) => Sync.TriggerBrowserEvent(eventNameAndArgs);

        public virtual new int Armor { get => base.Armor; set => base.Armor = value; }
        public RoundStatsDto? CurrentRoundStats { get; set; }

        public ITDSVehicle? FreeroamVehicle { get; set; }

        public virtual new int Health { get => base.Health; set => base.Health = value; }

        public bool IsCrouched { get; set; }

        public PlayerLobbyStats? LobbyStats { get; set; }

        public ushort RemoteId => Handle.Value;
        public ITDSPlayer? Spectates { get; set; }
        public bool TryingToLoginRegister { get; set; }
        public new ITDSVehicle? Vehicle => base.Vehicle as ITDSVehicle;
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

        public abstract void Spawn(Vector3 pos, float heading = 0);

        public abstract void SetInvincible(bool toggle);

        public bool Equals(ITDSPlayer? other)
        {
            return Id == other?.Id;
        }
    }
}
