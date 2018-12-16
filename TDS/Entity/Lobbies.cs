using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Lobbies
    {
        public Lobbies()
        {
            LobbyWeapons = new HashSet<LobbyWeapons>();
            Playerbans = new HashSet<Playerbans>();
            Playerlobbystats = new HashSet<Playerlobbystats>();
            Teams = new HashSet<Teams>();
        }

        public uint Id { get; set; }
        public uint? Owner { get; set; }
        public byte Type { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public sbyte StartHealth { get; set; }
        public sbyte StartArmor { get; set; }
        public sbyte? AmountLifes { get; set; }
        public float DefaultSpawnX { get; set; }
        public float DefaultSpawnY { get; set; }
        public float DefaultSpawnZ { get; set; }
        public float AroundSpawnPoint { get; set; }
        public float DefaultSpawnRotation { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsOfficial { get; set; }
        public uint? SpawnAgainAfterDeathMs { get; set; }
        public uint? MoneyPerKill { get; set; }
        public uint? MoneyPerAssist { get; set; }
        public uint? MoneyPerDamage { get; set; }
        public uint? DurationRound { get; set; }
        public uint? BombDetonateTimeMs { get; set; }
        public uint? BombDefuseTimeMs { get; set; }
        public uint? BombPlantTimeMs { get; set; }
        public bool? MixTeamsAfterRound { get; set; }
        public DateTime? CreateTimestamp { get; set; }

        public virtual Players OwnerNavigation { get; set; }
        public virtual ICollection<LobbyWeapons> LobbyWeapons { get; set; }
        public virtual ICollection<Playerbans> Playerbans { get; set; }
        public virtual ICollection<Playerlobbystats> Playerlobbystats { get; set; }
        public virtual ICollection<Teams> Teams { get; set; }
    }
}
