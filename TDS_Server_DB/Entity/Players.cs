using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class Players
    {
        public Players()
        {
            Lobbies = new HashSet<Lobbies>();
            Maps = new HashSet<Maps>();
            OfflinemessagesSource = new HashSet<Offlinemessages>();
            OfflinemessagesTarget = new HashSet<Offlinemessages>();
            PlayerBansAdmin = new HashSet<PlayerBans>();
            PlayerBansPlayer = new HashSet<PlayerBans>();
            PlayerLobbyStats = new HashSet<PlayerLobbyStats>();
            PlayerMapFavourites = new HashSet<PlayerMapFavourites>();
            PlayerMapRatings = new HashSet<PlayerMapRatings>();
        }

        public int Id { get; set; }
        public string Scname { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public short AdminLvl { get; set; }
        public bool IsVip { get; set; }
        public short Donation { get; set; }
        public int? GangId { get; set; }
        public DateTime RegisterTimestamp { get; set; }

        public virtual AdminLevels AdminLvlNavigation { get; set; }
        public virtual Gangs Gang { get; set; }
        public virtual PlayerSettings PlayerSettings { get; set; }
        public virtual PlayerStats PlayerStats { get; set; }
        public virtual ICollection<Lobbies> Lobbies { get; set; }
        public virtual ICollection<Maps> Maps { get; set; }
        public virtual ICollection<Offlinemessages> OfflinemessagesSource { get; set; }
        public virtual ICollection<Offlinemessages> OfflinemessagesTarget { get; set; }
        public virtual ICollection<PlayerBans> PlayerBansAdmin { get; set; }
        public virtual ICollection<PlayerBans> PlayerBansPlayer { get; set; }
        public virtual ICollection<PlayerLobbyStats> PlayerLobbyStats { get; set; }
        public virtual ICollection<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual ICollection<PlayerMapRatings> PlayerMapRatings { get; set; }
    }
}
