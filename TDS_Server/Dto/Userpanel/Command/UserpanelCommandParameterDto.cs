using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Dto.Userpanel.Command
{
    #nullable disable
    class UserpanelCommandParameterDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public object DefaultValue { get; set; }
    }
    #nullable restore
}
