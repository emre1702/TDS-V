using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Dto.Map.Creator
{
    public class MapCreatorObject
    {
        #nullable disable
        public string ObjectName { get; set; }
        public MapCreatorPosition Position { get; set; }
        #nullable restore
    }
}
