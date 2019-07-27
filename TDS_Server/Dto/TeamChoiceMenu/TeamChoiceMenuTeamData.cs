﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Dto.TeamChoiceMenu
{
    class TeamChoiceMenuTeamData
    {
        //public int Index { get; set; }
        public string Name { get; set; }
        public short Red { get; set; }
        public short Green { get; set; }
        public short Blue { get; set; }

        public IEnumerable<string> PlayerNames { get; set; }

        public TeamChoiceMenuTeamData(string name, short red, short green, short blue, IEnumerable<string> playerNames)
        {
            //Index = index;
            Name = name;
            Red = red;
            Green = green;
            Blue = blue;
            PlayerNames = playerNames;
        }
    }
}
