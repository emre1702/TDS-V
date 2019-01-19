using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Lobbies
    {
        public Lobbies()
        {
            LobbyMaps = new HashSet<LobbyMaps>();
            LobbyWeapons = new HashSet<LobbyWeapons>();
            PlayerBans = new HashSet<PlayerBans>();
            PlayerLobbyStats = new HashSet<PlayerLobbyStats>();
            Teams = new HashSet<Teams>();
        }

        public uint Id { get; set; }
        public uint? Owner { get; set; }
        public byte Type { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public sbyte StartHealth { get; set; }
        public sbyte StartArmor { get; set; }
        public byte? AmountLifes { get; set; }
        public float DefaultSpawnX { get; set; }
        public float DefaultSpawnY { get; set; }
        public float DefaultSpawnZ { get; set; }
        public float AroundSpawnPoint { get; set; }
        public float DefaultSpawnRotation { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsOfficial { get; set; }
        public uint? SpawnAgainAfterDeathMs { get; set; }
        public float? MoneyPerKill { get; set; }
        public float? MoneyPerAssist { get; set; }
        public float? MoneyPerDamage { get; set; }
        public uint? RoundTime { get; set; }
        public uint? CountdownTime { get; set; }
        public uint? BombDetonateTimeMs { get; set; }
        public uint? BombDefuseTimeMs { get; set; }
        public uint? BombPlantTimeMs { get; set; }
        public bool? MixTeamsAfterRound { get; set; }
        public uint? DieAfterOutsideMapLimitTime { get; set; }
        public DateTime? CreateTimestamp { get; set; }

        public virtual Players OwnerNavigation { get; set; }
        public virtual LobbyTypes TypeNavigation { get; set; }
        public virtual ICollection<LobbyMaps> LobbyMaps { get; set; }
        public virtual ICollection<LobbyWeapons> LobbyWeapons { get; set; }
        public virtual ICollection<PlayerBans> PlayerBans { get; set; }
        public virtual ICollection<PlayerLobbyStats> PlayerLobbyStats { get; set; }
        public virtual ICollection<Teams> Teams { get; set; }
    }
}
