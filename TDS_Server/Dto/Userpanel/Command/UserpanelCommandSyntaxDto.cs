using MessagePack;
using System.Collections.Generic;

namespace TDS_Server.Dto.Userpanel.Command
{
    [MessagePackObject]
    class UserpanelCommandSyntaxDto
    {
        [Key(0)]
        public List<UserpanelCommandParameterDto> Parameters { get; set; } = new List<UserpanelCommandParameterDto>();
    }
}
