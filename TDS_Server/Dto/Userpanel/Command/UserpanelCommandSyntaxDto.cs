using System.Collections.Generic;

namespace TDS_Server.Dto.Userpanel.Command
{
    class UserpanelCommandSyntaxDto
    {
        public List<UserpanelCommandParameterDto> Parameters { get; set; } = new List<UserpanelCommandParameterDto>();
    }
}
