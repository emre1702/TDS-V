namespace TDS_Server.Dto
{
    class RoundStatsDto
    {
        public uint Kills { get; set; }
        public uint Assists { get; set; }
        public uint Damage { get; set; }

        public RoundStatsDto()
        {
            Clear();
        }

        public void Clear()
        {
            Kills = 0;
            Assists = 0;
            Damage = 0;
        }
    }
}
