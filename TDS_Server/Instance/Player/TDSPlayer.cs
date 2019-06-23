using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Utility;
using TDS_Server.Interface;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Instance.Player
{
    internal class TDSPlayer
    {
        public TDSNewContext DbContext { get; private set; }

        public Players? Entity
        {
            get => _entity;
            set
            {
                if (_entity != null)
                    DbContext.Entry(_entity).State = EntityState.Detached;
                _entity = value;
                if (_entity == null)
                    return;
                if (_langEnumBeforeLogin != ELanguage.English)
                    _entity.PlayerSettings.Language = _langEnumBeforeLogin;
                DbContext.Attach(value);
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
            get => Entity?.PlayerStats.MuteTime ?? 0;
            set
            {
                if (Entity != null)
                    Entity.PlayerStats.MuteTime = value;
            }
        }

        public bool IsMuted => Entity?.PlayerStats.MuteTime.HasValue ?? false;

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
        public TDSPlayer? Spectates { get; set; }
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
        public int KillingSpree { get; set; }
        public TDSPlayer? InPrivateChatWith { get; set; }
        public TDSPlayer? SentPrivateChatRequestTo { get; set; }
        public Vehicle? FreeroamVehicle { get; set; }

        private Players? _entity;
        private int _lastSaveTick;
        private ELanguage _langEnumBeforeLogin = ELanguage.English;
        private Team? _team;
        private Gang? _gang;
        private PlayerLobbyStats? _currentLobbyStats;

        public TDSPlayer(Client client)
        {
            Client = client;
            DbContext = new TDSNewContext();
        }

        #region Money

        public void GiveMoney(int money)
        {
            if (money >= 0 || Money > money * -1)
            {
                Money += money;
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

        public async Task SaveData()
        {
            if (Entity == null || !Entity.PlayerStats.LoggedIn)
                return;

            _lastSaveTick = Environment.TickCount;
            await DbContext.SaveChangesAsync();
        }

        public async void CheckSaveData()
        {
            if (Environment.TickCount - _lastSaveTick < SettingsManager.SavePlayerDataCooldownMinutes * 60 * 1000)
                return;

            await SaveData();
        }
    }
}