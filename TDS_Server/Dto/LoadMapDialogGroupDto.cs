using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Dto
{
    class LoadMapDialogGroupDto
    {
        public string GroupName { get; set; } = string.Empty;
        public List<string> Maps { get; set; } = new List<string>();
    }
}
