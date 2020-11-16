using System.Collections.Generic;
using TDS.Server.Database.Entity.Command;

namespace TDS.Server.Data.Models
{
    #nullable disable
    public class CommandDataDto
    {
        public Commands Entity { get; set; }
        public List<CommandMethodDataDto> MethodDatas { get; } = new List<CommandMethodDataDto>();
    }
}
