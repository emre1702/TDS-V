using GTANetworkAPI;
using System.Linq;
using TDS.Entity;

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
        public int TeamID;

        private Players fEntity;

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
