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

namespace TDS.Instance.Player
{
    class Character
    {
        public Players Entity
        {
            get => GetEntity();
            set
            {
                fEntity = value;
                NAPI.ClientEvent.TriggerClientEvent(Player, DCustomEvent.ClientMoneyChange, fEntity.Playerstats.Money);
            }
        }
        public Client Player;
        public Lobby.Lobby CurrentLobby;
        public Playerlobbystats CurrentLobbyStats;
        public Teams Team
        {
            get => fTeam;
            set
            {
                fTeam = value;
                TeamChatColor = "{" + value.ColorR + "|" + value.ColorG + "|" + value.ColorB + "}"; 
            }
        }
        public uint Lifes = 0;
        public bool IsLobbyOwner
        {
            get => CurrentLobby.IsPlayerLobbyOwner(this);
        }
        public string TeamChatColor;
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
        public RoundStatsDto CurrentRoundStats;
        public int Money
        {
            get => (int)Entity.Playerstats.Money;
            set {
                Entity.Playerstats.Money = (uint)value;
                NAPI.ClientEvent.TriggerClientEvent(Player, DCustomEvent.ClientMoneyChange, value);
            }
        }
        public Character LastHitter;
        public Character Spectates;
        public HashSet<Character> Spectators = new HashSet<Character>();

        private Players fEntity;
        private Teams fTeam;


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
                    Players entity = dbcontext.Players.FirstOrDefault(p => p.Scname == Player.SocialClubName);
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
                                Environment.StackTrace, Player);
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
            damage = Math.Min(Player.Armor + Player.Health, damage);

            int leftdmg = damage;
            if (Player.Armor > 0)
            {
                leftdmg -= Player.Armor;
                Player.Armor = leftdmg < 0 ? Math.Abs(leftdmg) : 0;
            }
            if (leftdmg > 0)
                Player.Health -= leftdmg;
        }
    }
}
