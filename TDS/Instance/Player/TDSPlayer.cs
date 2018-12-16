using GTANetworkAPI;
using GTANetworkMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Default;
using TDS.Dto;
using TDS.Entity;
using TDS.Enum;
using TDS.Interface;
using TDS.Manager.Logs;
using TDS.Manager.Utility;
using TDS_Common.Default;

namespace TDS.Instance.Player
{
    class TDSPlayer
    {
        public Players Entity
        {
            get => GetEntity();
            set
            {
                fEntity = value;
                NAPI.ClientEvent.TriggerClientEvent(Client, DToClientEvent.PlayerMoneyChange, fEntity.Playerstats.Money);
            }
        }
        public Client Client { get; }
        public Lobby.Lobby CurrentLobby { get; set; }
        public Playerlobbystats CurrentLobbyStats { get; set; }
        public Teams Team
        {
            get => fTeam;
            set
            {
                fTeam = value;
                TeamChatColor = "{" + value.ColorR + "|" + value.ColorG + "|" + value.ColorB + "}"; 
            }
        }
        public sbyte Lifes { get; set; } = 0;
        public bool IsLobbyOwner
        {
            get => CurrentLobby.IsPlayerLobbyOwner(this);
        }
        public string TeamChatColor { get; set; }
        public uint? MuteTime
        {
            get => Entity.Playerstats.MuteTime;
            set => Entity.Playerstats.MuteTime = value;
        }
        public bool IsPermamuted
        {
            get => Entity.Playerstats.MuteTime.HasValue && this.Entity.Playerstats.MuteTime.Value == 0;
        }
        public ILanguage Language
        {
            get => LangUtils.GetLang(LanguageEnum);
        }
        public ELanguage LanguageEnum
        {
            get => (ELanguage)Entity.Playersettings.Language;
            set => Entity.Playersettings.Language = (byte) value;
        }
        public AdminLevel AdminLevel
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
            get => (int)Entity.Playerstats.Money;
            set {
                Entity.Playerstats.Money = (uint)value;
                NAPI.ClientEvent.TriggerClientEvent(Client, DToClientEvent.PlayerMoneyChange, value);
            }
        }
        public TDSPlayer LastHitter { get; set; }
        public TDSPlayer Spectates { get; set; }
        public HashSet<TDSPlayer> Spectators { get; set; } = new HashSet<TDSPlayer>();

        private Players fEntity { get; set; }
        private Teams fTeam { get; set; }


        public TDSPlayer(Client client)
        {
            Client = client;
        }

        /// <summary>
        /// Get the EF Entity for a player.
        /// First look for it in playerEntities (caching) - if it's not there, search for it in the Database.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private Players GetEntity()
        {
            if (fEntity != null)
            {
                return fEntity;
            }
            else
            {
                using (var dbcontext = new TDSNewContext())
                {
                    Players entity = dbcontext.Players.FirstOrDefault(p => p.Scname == Client.SocialClubName);
                    if (entity != null)
                    {
                        fEntity = entity;
                    }
                    return entity;
                }
            }
        }

        #region Money
        public void GiveMoney(int money)
        {
            if (money >= 0 || Money > money * -1)
            {
                Money += money;
            }
            else
                Error.Log($"Should have went to minus money! Current: {Money} | Substracted money: {money}",
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
    }
}
