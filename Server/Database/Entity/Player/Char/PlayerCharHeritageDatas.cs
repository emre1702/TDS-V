﻿using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharHeritageDatas : CharCreateHeritageData
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public virtual PlayerCharDatas CharDatas { get; set; }
    }
}