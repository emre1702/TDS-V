using GTANetworkAPI;
using System.Linq;
using TDS.Entity;
using TDS.Enum;
using TDS.Interface;
using TDS.Manager.Utility;

namespace TDS.Instance.Player
{
    class Character
    {
        public Players Entity
        {
            get => this.GetEntity();
            set => this.fEntity = value;
        }
        public Client Player;
        public Lobby.Lobby CurrentLobby;
        public Playerlobbystats CurrentLobbyStats;
        public Teams Team
        {
            get => this.fTeam;
            set
            {
                this.fTeam = value;
                this.TeamChatColor = "{" + value.ColorR + "|" + value.ColorG + "|" + value.ColorB + "}"; 
            }
        }
        public uint Lifes = 0;
        public bool IsLobbyOwner
        {
            get => this.CurrentLobby.IsPlayerLobbyOwner(this);
        }
        public string TeamChatColor;
        public uint? MuteTime
        {
            get => this.Entity.Playerstats.MuteTime;
            set => this.Entity.Playerstats.MuteTime = value;
        }
        public bool IsPermamuted
        {
            get => this.Entity.Playerstats.MuteTime.HasValue && this.Entity.Playerstats.MuteTime.Value == 0;
        }
        public ILanguage Language
        {
            get => LangUtils.GetLang(this.Entity.Playersettings.Language);
        }
        public AdminLevel AdminLevel
        {
            get => AdminsManager.AdminLevels[this.Entity.AdminLvl];
        }
        public string AdminLevelName
        {
            get => this.AdminLevel.Names[(ELanguage)this.Entity.Playersettings.Language];
        }

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
            if (this.fEntity != null)
            {
                return this.fEntity;
            }
            else
            {
                using (var dbcontext = new TDSNewContext())
                {
                    Players entity = dbcontext.Players.FirstOrDefault(p => p.Scname == this.Player.SocialClubName);
                    if (entity != null)
                    {
                        fEntity = entity;
                    }
                    return entity;
                }
            }
        }
    }
}
