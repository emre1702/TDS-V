using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Players
    {
        public Players()
        {
            Lobbies = new HashSet<Lobbies>();
            OfflinemessagesSource = new HashSet<Offlinemessages>();
            OfflinemessagesTarget = new HashSet<Offlinemessages>();
            PlayerbansAdminNavigation = new HashSet<Playerbans>();
            PlayerbansIdNavigation = new HashSet<Playerbans>();
            Playerlobbystats = new HashSet<Playerlobbystats>();
            Playermapratings = new HashSet<Playermapratings>();
        }

        public uint Id { get; set; }
        public string Scname { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public sbyte AdminLvl { get; set; }
        public bool IsVip { get; set; }
        public sbyte Donation { get; set; }
        public DateTime RegisterTimestamp { get; set; }

        public virtual Playersettings Playersettings { get; set; }
        public virtual Playerstats Playerstats { get; set; }
        public virtual ICollection<Lobbies> Lobbies { get; set; }
        public virtual ICollection<Offlinemessages> OfflinemessagesSource { get; set; }
        public virtual ICollection<Offlinemessages> OfflinemessagesTarget { get; set; }
        public virtual ICollection<Playerbans> PlayerbansAdminNavigation { get; set; }
        public virtual ICollection<Playerbans> PlayerbansIdNavigation { get; set; }
        public virtual ICollection<Playerlobbystats> Playerlobbystats { get; set; }
        public virtual ICollection<Playermapratings> Playermapratings { get; set; }
    }
}
