using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Lobbies
    {
        public Lobbies()
        {
            Playerbans = new HashSet<Playerbans>();
            Playerlobbystats = new HashSet<Playerlobbystats>();
            Teams = new HashSet<Teams>();
            WeaponsDamage = new HashSet<WeaponsDamage>();
        }

        public uint Id { get; set; }
        public uint? Owner { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public sbyte StartHealth { get; set; }
        public sbyte StartArmor { get; set; }
        public float DefaultSpawnX { get; set; }
        public float DefaultSpawnY { get; set; }
        public float DefaultSpawnZ { get; set; }
        public float AroundSpawnPoint { get; set; }
        public float DefaultSpawnRotation { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsOfficial { get; set; }
        public uint DisappearAfterDeathMs { get; set; }
        public uint SpawnAgainAfterDeathMs { get; set; }
        public DateTime? CreateTimestamp { get; set; }

        public Players OwnerNavigation { get; set; }
        public ICollection<Playerbans> Playerbans { get; set; }
        public ICollection<Playerlobbystats> Playerlobbystats { get; set; }
        public ICollection<Teams> Teams { get; set; }
        public ICollection<WeaponsDamage> WeaponsDamage { get; set; }
    }
}
