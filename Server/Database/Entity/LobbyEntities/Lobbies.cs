using System;
using System.Collections.Generic;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.LobbyEntities
{
    public partial class Lobbies
    {
        #region Public Constructors

        public Lobbies()
        {
            LobbyMaps = new HashSet<LobbyMaps>();
            LobbyWeapons = new HashSet<LobbyWeapons>();
            PlayerBans = new HashSet<PlayerBans>();
            PlayerLobbyStats = new HashSet<PlayerLobbyStats>();
            Teams = new HashSet<Teams>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual ICollection<LobbyArmsRaceWeapons> ArmsRaceWeapons { get; set; }
        public float AroundSpawnPoint { get; set; }
        public DateTime CreateTimestamp { get; set; }
        public float DefaultSpawnRotation { get; set; }
        public float DefaultSpawnX { get; set; }
        public float DefaultSpawnY { get; set; }
        public float DefaultSpawnZ { get; set; }
        public virtual LobbyFightSettings FightSettings { get; set; }
        public int Id { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsTemporary { get; set; }
        public virtual ICollection<LobbyKillingspreeRewards> LobbyKillingspreeRewards { get; set; }
        public virtual ICollection<LobbyMaps> LobbyMaps { get; set; }
        public virtual LobbyMapSettings LobbyMapSettings { get; set; }
        public virtual LobbyRewards LobbyRewards { get; set; }
        public virtual LobbyRoundSettings LobbyRoundSettings { get; set; }
        public virtual ICollection<LobbyWeapons> LobbyWeapons { get; set; }
        public string Name { get; set; }
        public virtual Players Owner { get; set; }
        public int OwnerId { get; set; }
        public string Password { get; set; }
        public virtual ICollection<PlayerBans> PlayerBans { get; set; }
        public virtual ICollection<PlayerLobbyStats> PlayerLobbyStats { get; set; }
        public virtual ICollection<Teams> Teams { get; set; }
        public LobbyType Type { get; set; }

        #endregion Public Properties
    }
}
