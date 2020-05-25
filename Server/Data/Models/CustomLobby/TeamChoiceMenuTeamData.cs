﻿using Newtonsoft.Json;

namespace TDS_Server.Data.Models.CustomLobby
{
    public class TeamChoiceMenuTeamData
    {
        #region Public Constructors

        public TeamChoiceMenuTeamData(string name, short red, short green, short blue)
        {
            Name = name;
            Red = red;
            Green = green;
            Blue = blue;
        }

        #endregion Public Constructors

        #region Public Properties

        [JsonProperty("3")]
        public short Blue { get; set; }

        [JsonProperty("2")]
        public short Green { get; set; }

        [JsonProperty("0")]
        public string Name { get; set; }

        [JsonProperty("1")]
        public short Red { get; set; }

        #endregion Public Properties
    }
}
