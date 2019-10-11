using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Dto.TeamChoiceMenu
{
    class TeamChoiceMenuTeamData
    {
        public string Name { get; set; }
        public short Red { get; set; }
        public short Green { get; set; }
        public short Blue { get; set; }


        public TeamChoiceMenuTeamData(string name, short red, short green, short blue)
        {
            Name = name;
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}
