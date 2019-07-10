namespace TDS_Server.Dto
{
    class CustomLobbyData
    {
        public int? LobbyId;
        public string Name = "";
        public string? OwnerName;
        public string Password = "";
        public short StartHealth;
        public short StartArmor;
        public short AmountLifes;

        public bool MixTeamsAfterRound;

        public int BombDetonateTimeMs;
        public int BombDefuseTimeMs;
        public int BombPlantTimeMs;
        public int RoundTime;
        public int CountdownTime;

        public int SpawnAgainAfterDeathMs;
        public int DieAfterOutsideMapLimitTime;
    }
}
