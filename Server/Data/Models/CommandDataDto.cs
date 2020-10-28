using System.Collections.Generic;
using TDS_Server.Database.Entity.Command;

namespace TDS_Server.Data.Models
{
    #nullable disable
    public class CommandDataDto
    {
        public Commands Entity { get; set; }
        public List<CommandMethodDataDto> MethodDatas { get; set; } = new List<CommandMethodDataDto>();
    }
}
