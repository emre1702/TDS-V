using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class Languages
    {
        public Languages()
        {
            AdminLevelNames = new HashSet<AdminLevelNames>();
            CommandInfos = new HashSet<CommandInfos>();
            PlayerSettings = new HashSet<PlayerSettings>();
        }

        public short Id { get; set; }
        public string Language { get; set; }

        public virtual ICollection<AdminLevelNames> AdminLevelNames { get; set; }
        public virtual ICollection<CommandInfos> CommandInfos { get; set; }
        public virtual ICollection<PlayerSettings> PlayerSettings { get; set; }
    }
}
