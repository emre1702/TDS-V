using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Languages
    {
        public Languages()
        {
            Adminlevelnames = new HashSet<Adminlevelnames>();
            CommandsInfo = new HashSet<CommandsInfo>();
            Playersettings = new HashSet<Playersettings>();
        }

        public byte Id { get; set; }
        public string Language { get; set; }

        public virtual ICollection<Adminlevelnames> Adminlevelnames { get; set; }
        public virtual ICollection<CommandsInfo> CommandsInfo { get; set; }
        public virtual ICollection<Playersettings> Playersettings { get; set; }
    }
}
