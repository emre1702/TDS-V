using MessagePack;
using System.Collections.Generic;

namespace TDS_Server.Dto
{
    [MessagePackObject]
    class LoadMapDialogGroupDto
    {
        [Key(0)]
        public string GroupName { get; set; } = string.Empty;
        [Key(1)]
        public List<string> Maps { get; set; } = new List<string>();
    }
}
