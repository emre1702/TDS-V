using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Languages
    {
        public Languages()
        {
            AdminLevelNames = new HashSet<AdminLevelNames>();
            CommandsInfo = new HashSet<CommandsInfo>();
            PlayerSettings = new HashSet<PlayerSettings>();
        }

        public byte Id { get; set; }
        public string Language { get; set; }

        public virtual ICollection<AdminLevelNames> AdminLevelNames { get; set; }
        public virtual ICollection<CommandsInfo> CommandsInfo { get; set; }
        public virtual ICollection<PlayerSettings> PlayerSettings { get; set; }
    }
}
