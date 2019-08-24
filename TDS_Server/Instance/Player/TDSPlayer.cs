using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Utility;
using TDS_Server.Interface;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Sync;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Instance.Player
{
    internal class TDSPlayer
    {
        public TDSNewContext DbContext 
        { 
            get
            {
                if (_dbContext == null)
                    _dbContext = new TDSNewContext();
                return _dbContext;
            } 
            set => _dbContext = value;
        }

        public Players? Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                if (_entity == null)
                    return;
                if (_langEnumBeforeLogin != ELanguage.English)
                    _entity.PlayerSettings.Language = _langEnumBeforeLogin;
                PlayerRelationsPlayer = _entity.PlayerRelationsPlayer.ToList();
                PlayerRelationsTarget = _entity.PlayerRelationsTarget.ToList();
                NAPI.ClientEvent.TriggerClientEvent(Client, DToClientEvent.PlayerMoneyChange, _entity.PlayerStats.Money);
            }
        }

        public Client Client { get; }
        public Lobby.Lobby? CurrentLobby { get; set; }
        public Lobby.Lobby? PreviousLobby { get; set; }
        public PlayerLobbyStats? CurrentLobbyStats
        {
            get => _currentLobbyStats;
            set
            {
                if (_currentLobbyStats != null)
                    DbContext.Entry(_currentLobbyStats).State = EntityState.Detached;
                _currentLobbyStats = value;
                if (value != null)
                    DbContext.Attach(_currentLobbyStats);
            }
        }

        public Team? Team
        {
            get => _team;
            set
            {
                if (value != _team)
                {
                    _team?.RemovePlayer(this);
                    value?.AddPlayer(this);
                    NAPI.ClientEvent.TriggerClientEvent(Client, DToClientEvent.PlayerTeamChange, value?.Entity.Name ?? "-");

                    if (CurrentLobby != null && CurrentLobby is Arena)
                        TeamChoiceMenuSync.PlayerChangedTeam(this, value, _team);

                    _team = value;
                }
            }
        }

        public Gang Gang
        {
            get
            {
                if (_gang == null)
                {
                    _gang = Gang.None;
                }
                return _gang;
            }
            set => _gang = value;
        }

        public sbyte Lifes { get; set; } = 0;
        public bool IsLobbyOwner => CurrentLobby?.IsPlayerLobbyOwner(this) ?? false;

        public int? MuteTime
        {
            get => Entity?.PlayerStats.MuteTime;
            set
            {
                if (Entity != null)
                    Entity.PlayerStats.MuteTime = value;
            }
        }

        public int? VoiceMuteTime
        {
            get => Entity?.PlayerStats.VoiceMuteTime;
            set
            {
                if (Entity != null)
                    Entity.PlayerStats.VoiceMuteTime = value;
            }
        }

        public bool IsMuted => Entity?.PlayerStats.MuteTime.HasValue ?? false;
        public bool IsVoiceMuted => Entity?.PlayerStats.VoiceMuteTime.HasValue ?? false;

        public bool IsPermamuted
        {
            get
            {
                if (Entity == null)
                    return false;
                return Entity.PlayerStats.MuteTime.HasValue && Entity.PlayerStats.MuteTime.Value == 0;
            }
        }

        public ILanguage Language => LangUtils.GetLang(LanguageEnum);

        public ELanguage LanguageEnum
        {
            get
            {
                if (Entity == null || Entity.PlayerSettings == null)
                    return _langEnumBeforeLogin;
                return Entity.PlayerSettings.Language;
            }
            set
            {
                if (Entity == null || Entity.PlayerSettings == null)
                    _langEnumBeforeLogin = value;
                else
                    Entity.PlayerSettings.Language = value;
            }
        }

        public AdminLevelDto AdminLevel
        {
            get
            {
                if (Entity == null)
                    return AdminsManager.AdminLevels[0];
                return AdminsManager.AdminLevels[Entity.AdminLvl];
            }
        }

        public string AdminLevelName => AdminLevel.Names[LanguageEnum];
        public RoundStatsDto? CurrentRoundStats { get; set; }

        public int Money
        {
            get => Entity?.PlayerStats.Money ?? 0;
            set
            {
                if (Entity == null)
                    return;
                Entity.PlayerStats.Money = value;
                NAPI.ClientEvent.TriggerClientEvent(Client, DToClientEvent.PlayerMoneyChange, value);
            }
        }

        public TDSPlayer? LastHitter { get; set; }
        public TDSPlayer? Spectates 
        { 
            get => _spectates;
            set 
            {
                SpectateSystem.SetPlayerToSpectatePlayer(this, value);
                _spectates = value;
            }
        }
        public HashSet<TDSPlayer> Spectators { get; set; } = new HashSet<TDSPlayer>();
        public bool LoggedIn => Entity != null && Entity.PlayerStats != null ? Entity.PlayerStats.LoggedIn : false;

        public int PlayMinutes
        {
            get => Entity?.PlayerStats.PlayTime ?? 0;
            set
            {
                if (Entity == null)
                    return;
                Entity.PlayerStats.PlayTime = value;
            }
        }

        public bool ChatLoaded { get; set; }
        public short KillingSpree
        {
            get => _killingSpree;
            set
            {
                if (_killingSpree + 1 == value)
                {
                    ++_shortTimeKillingSpree;
                }
                _killingSpree = value;
            }
        }
        public short ShortTimeKillingSpree
        {
            get
            {
                if (LastKillAt == null)
                    return _shortTimeKillingSpree;

                var timeSpanSinceLastKill = DateTime.UtcNow - LastKillAt.Value;
                if (timeSpanSinceLastKill.TotalSeconds <= SettingsManager.KillingSpreeMaxSecondsUntilNextKill)
                {
                    return _shortTimeKillingSpree;
                }
                _shortTimeKillingSpree = 1;
                return 1;
            }
        }
        public TDSPlayer? InPrivateChatWith { get; set; }
        public TDSPlayer? SentPrivateChatRequestTo { get; set; }
        public Vehicle? FreeroamVehicle { get; set; }
        public DateTime? LastKillAt { get; set; }
        public PlayerTotalStats? TotalStats => Entity?.PlayerTotalStats;
        public List<PlayerRelations> PlayerRelationsTarget { get; private set; } = new List<PlayerRelations>();
        public List<PlayerRelations> PlayerRelationsPlayer { get; private set; } = new List<PlayerRelations>();
        public WeaponHash LastWeaponOnHand { get; set; } = WeaponHash.Unarmed;

        public HashSet<int> BlockingPlayerIds => PlayerRelationsTarget.Where(r => r.Relation == EPlayerRelation.Block).Select(r => r.PlayerId).ToHashSet();

        private Players? _entity;
        private int _lastSaveTick;
        private ELanguage _langEnumBeforeLogin = ELanguage.English;
        private Team? _team;
        private Gang? _gang;
        private PlayerLobbyStats? _currentLobbyStats;
        private short _killingSpree;
        private short _shortTimeKillingSpree;
        private TDSNewContext? _dbContext;
        private TDSPlayer? _spectates;

        private readonly SemaphoreSlim _semaphoreSlime = new SemaphoreSlim(1);

        public TDSPlayer(Client client)
        {
            Client = client;
        }

        #region Money

        public void GiveMoney(int money)
        {
            if (money >= 0 || Money > money * -1)
            {
                Money += money;
                if (TotalStats != null)
                    TotalStats.Money += money;
            }
            else
                ErrorLogsManager.Log($"Should have went to minus money! Current: {Money} | Substracted money: {money}",
                                Environment.StackTrace, Client);
        }

        public void GiveMoney(uint money)
        {
            GiveMoney((int)money);
        }

        #endregion Money

        public void Damage(ref int damage)
        {
            if (damage == 0)
                return;
            damage = Math.Min(Client.Armor + Client.Health, damage);

            int leftdmg = damage;
            if (Client.Armor > 0)
            {
                leftdmg -= Client.Armor;
                Client.Armor = leftdmg < 0 ? Math.Abs(leftdmg) : 0;
            }
            if (leftdmg > 0)
                Client.Health -= leftdmg;
        }

        public void AddHPArmor(int healtharmor)
        {
            #region HP

            if (Client.Health + healtharmor <= 100)
            {
                Client.Health += healtharmor;
                healtharmor = 0;
            }
            else if (Client.Health != 100)
            {
                healtharmor -= 100 - Client.Health;
                Client.Health = 100;
            }

            #endregion HP

            #region Armor

            if (healtharmor > 0)
            {
                Client.Armor = Client.Armor + healtharmor <= 100 ? Client.Armor + healtharmor : 100;
            }

            #endregion Armor
        }

        public void ClosePrivateChat(bool disconnected)
        {
            if (InPrivateChatWith == null && SentPrivateChatRequestTo == null)
                return;

            if (InPrivateChatWith != null)
            {
                if (disconnected)
                {
                    InPrivateChatWith.Client.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_DISCONNECTED);
                }
                else
                {
                    Client.SendNotification(Language.PRIVATE_CHAT_CLOSED_YOU);
                    InPrivateChatWith.Client.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_CLOSED_PARTNER);
                }
                InPrivateChatWith.InPrivateChatWith = null;
                InPrivateChatWith = null;
            }
            else if (SentPrivateChatRequestTo != null)
            {
                if (!disconnected)
                {
                    Client.SendNotification(Language.PRIVATE_CHAT_REQUEST_CLOSED_YOU);
                }
                SentPrivateChatRequestTo.Client.SendNotification(
                    SentPrivateChatRequestTo.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER.Formatted(Client.Name)
                );
                SentPrivateChatRequestTo = null;
            }
        }

        public bool HasRelationTo(TDSPlayer target, EPlayerRelation relation)
        {
            return Entity?.PlayerRelationsPlayer.Any(p => p.TargetId == target.Entity?.Id && p.Relation == relation) == true;
        }

        public async Task SaveData()
        {
            if (Entity == null || !Entity.PlayerStats.LoggedIn)
                return;

            _lastSaveTick = Environment.TickCount;
            await _semaphoreSlime.WaitAsync();
            try
            {
                if (CurrentLobbyStats != null && LobbyManager.GetLobby(CurrentLobbyStats.LobbyId) == null)
                {
                    DbContext.Entry(CurrentLobbyStats).State = EntityState.Detached;
                    CurrentLobbyStats = null;
                }
                await DbContext.SaveChangesAsync();
            }
            finally
            {
                _semaphoreSlime.Release();
            }
        }

        public async void CheckSaveData()
        {
            if (Environment.TickCount - _lastSaveTick < SettingsManager.SavePlayerDataCooldownMinutes * 60 * 1000)
                return;

            await SaveData();
        }

        public void Logout()
        {
            DbContext.Dispose();
            _dbContext = null;
        }

        public void InitDbContext()
        {
            _dbContext = new TDSNewContext();
        }
    }
}