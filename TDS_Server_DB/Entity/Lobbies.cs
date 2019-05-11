﻿using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
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

        public int Id { get; set; }
        public int Owner { get; set; }
        public short Type { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public short StartHealth { get; set; }
        public short StartArmor { get; set; }
        public short? AmountLifes { get; set; }
        public float DefaultSpawnX { get; set; }
        public float DefaultSpawnY { get; set; }
        public float DefaultSpawnZ { get; set; }
        public float AroundSpawnPoint { get; set; }
        public float DefaultSpawnRotation { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsOfficial { get; set; }
        public int SpawnAgainAfterDeathMs { get; set; }
        public DateTime CreateTimestamp { get; set; }
        public int DieAfterOutsideMapLimitTime { get; set; }

        public virtual Players OwnerNavigation { get; set; }
        public virtual LobbyTypes TypeNavigation { get; set; }
        public virtual LobbyRewards LobbyRewards { get; set; }
        public virtual LobbyRoundSettings LobbyRoundSettings { get; set; }
        public virtual ICollection<LobbyMaps> LobbyMaps { get; set; }
        public virtual ICollection<LobbyWeapons> LobbyWeapons { get; set; }
        public virtual ICollection<PlayerBans> PlayerBans { get; set; }
        public virtual ICollection<PlayerLobbyStats> PlayerLobbyStats { get; set; }
        public virtual ICollection<Teams> Teams { get; set; }
    }
}
