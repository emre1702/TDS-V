using GTANetworkAPI;
using GTANetworkMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Dto;
using TDS_Server.Entity;
using TDS_Server.Interface;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Enum;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.Player
{
    class TDSPlayer
    {

        public Players Entity
        {
            get => fEntity;
            set
            {
                fEntity = value;
                if (fLangEnumBeforeLogin != ELanguage.English)
                    fEntity.PlayerSettings.Language = (byte)fLangEnumBeforeLogin;
                NAPI.ClientEvent.TriggerClientEvent(Client, DToClientEvent.PlayerMoneyChange, fEntity.PlayerStats.Money);
            }
        }
        public Client Client { get; }
        public Lobby.Lobby CurrentLobby { get; set; }
        public Lobby.Lobby PreviousLobby { get; set; }
        public PlayerLobbyStats CurrentLobbyStats { get; set; }
        public Team Team
        {
            get => _team;
            set
            {
                if (value != _team)
                {
                    _team?.RemovePlayer(this);
                    value?.AddPlayer(this);
                }
                _team = value;
            }
        }
        public sbyte Lifes { get; set; } = 0;
        public bool IsLobbyOwner
        {
            get => CurrentLobby.IsPlayerLobbyOwner(this);
        }
        public uint? MuteTime
        {
            get => Entity.PlayerStats.MuteTime;
            set => Entity.PlayerStats.MuteTime = value;
        }
        public bool IsMuted
        {
            get => Entity.PlayerStats.MuteTime.HasValue;
        }
        public bool IsPermamuted
        {
            get => Entity.PlayerStats.MuteTime.HasValue && Entity.PlayerStats.MuteTime.Value == 0;
        }
        public ILanguage Language
        {
            get => LangUtils.GetLang(LanguageEnum);
        }
        public ELanguage LanguageEnum
        {
            get
            {
                if (Entity == null || Entity.PlayerSettings == null)
                    return fLangEnumBeforeLogin;
                return (ELanguage)Entity.PlayerSettings.Language;
            }
            set
            {
                if (Entity == null || Entity.PlayerSettings == null)
                    fLangEnumBeforeLogin = value;
                else
                    Entity.PlayerSettings.Language = (byte)value;
                SaveSettings();
            }
                
        }
        public AdminLevelDto AdminLevel
        {
            get => AdminsManager.AdminLevels[Entity.AdminLvl];
        }
        public string AdminLevelName
        {
            get => AdminLevel.Names[LanguageEnum];
        }
        public RoundStatsDto CurrentRoundStats { get; set; }
        public int Money
        {
            get => (int)Entity.PlayerStats.Money;
            set {
                Entity.PlayerStats.Money = (uint)value;
                NAPI.ClientEvent.TriggerClientEvent(Client, DToClientEvent.PlayerMoneyChange, value);
            }
        }
        public TDSPlayer LastHitter { get; set; }
        public TDSPlayer Spectates { get; set; }
        public HashSet<TDSPlayer> Spectators { get; set; } = new HashSet<TDSPlayer>();
        public bool LoggedIn => Entity != null && Entity.PlayerStats != null ? Entity.PlayerStats.LoggedIn : false;
        public uint PlayMinutes
        {
            get => Entity.PlayerStats.PlayTime;
            set => Entity.PlayerStats.PlayTime = value;
        }
        public bool ChatLoaded { get; set; }
        public int KillingSpree { get; set; }

        private Players fEntity;
        private int LastSaveTick;
        private ELanguage fLangEnumBeforeLogin = ELanguage.English;
        private Team _team;

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
            }
            else
                ErrorLogsManager.Log($"Should have went to minus money! Current: {Money} | Substracted money: {money}",
                                Environment.StackTrace, Client);
        }

        public void GiveMoney(uint money)
        {
            GiveMoney((int)money);
        }
        #endregion

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

        public void SetTeam(Team team, bool withOtherThings)
        {
            if (withOtherThings)
                this.Team = team;
            else
                this._team = team;
        }

        public async void SaveData()
        {
            using (TDSNewContext dbContext = new TDSNewContext())
            {
                await SaveData(dbContext);
            }
        }

        public Task<int> SaveData(TDSNewContext dbcontext)
        {
            if (Entity == null || !Entity.PlayerStats.LoggedIn)
                return System.Threading.Tasks.Task.FromResult(0);

            dbcontext.Players.Attach(Entity);
            dbcontext.Entry(Entity).State = EntityState.Modified;

            if (Entity.PlayerLobbyStats.Count > 0)
            {
                dbcontext.PlayerLobbyStats.AttachRange(Entity.PlayerLobbyStats);
                dbcontext.Entry(Entity.PlayerLobbyStats).State = EntityState.Modified;
            }
            
            dbcontext.PlayerStats.Attach(Entity.PlayerStats);
            dbcontext.Entry(Entity.PlayerStats).State = EntityState.Modified;

            return dbcontext.SaveChangesAsync();
        }

        public Task<int> CheckSaveData(TDSNewContext dbcontext)
        {
            if (Environment.TickCount - LastSaveTick < SettingsManager.SavePlayerDataCooldownMinutes * 60 * 1000)
                return System.Threading.Tasks.Task.FromResult(0);
            LastSaveTick = Environment.TickCount;
            return SaveData(dbcontext);
        }

        private async void SaveSettings()
        {
            using (TDSNewContext dbContext = new TDSNewContext())
            {
                dbContext.PlayerSettings.Attach(Entity.PlayerSettings);
                dbContext.Entry(Entity.PlayerSettings).State = EntityState.Modified;
                await SaveData(dbContext);
            }
        }
    }
}
