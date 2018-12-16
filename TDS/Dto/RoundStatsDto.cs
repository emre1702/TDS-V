﻿namespace TDS.Dto
{
    class RoundStatsDto
    {
#warning Add roundstats to player after the round
        public uint Kills { get; set; }
        public uint Assists { get; set; }
        public uint Damage { get; set; }
        public uint Deaths { get; set; }

        public RoundStatsDto()
        {
            Clear();
        }

        public void Clear()
        {
            Kills = 0;
            Assists = 0;
            Damage = 0;
            Deaths = 0;
        }
    }
}
