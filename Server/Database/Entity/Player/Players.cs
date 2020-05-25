using System;
using System.Collections.Generic;
using TDS_Server.Database.Entity.Admin;
using TDS_Server.Database.Entity.Challenge;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Player.Char;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Database.Entity.Userpanel;

namespace TDS_Server.Database.Entity.Player
{
    public partial class Players
    {
        #region Public Constructors

        public Players()
        {
            Lobbies = new HashSet<Lobbies>();
            Maps = new HashSet<Maps>();
            OfflinemessagesSource = new HashSet<Offlinemessages>();
            OfflinemessagesTarget = new HashSet<Offlinemessages>();
            PlayerBansAdmin = new HashSet<PlayerBans>();
            PlayerBansPlayer = new HashSet<PlayerBans>();
            PlayerLobbyStats = new HashSet<PlayerLobbyStats>();
            PlayerMapFavourites = new HashSet<PlayerMapFavourites>();
            PlayerMapRatings = new HashSet<PlayerMapRatings>();
            PlayerRelationsPlayer = new HashSet<PlayerRelations>();
            PlayerRelationsTarget = new HashSet<PlayerRelations>();
            SupportRequestMessages = new HashSet<SupportRequestMessages>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual Players AdminLeader { get; set; }
        public int? AdminLeaderId { get; set; }
        public short AdminLvl { get; set; }
        public virtual AdminLevels AdminLvlNavigation { get; set; }
        public virtual ICollection<Players> AdminMembers { get; set; }
        public virtual Applications Application { get; set; }
        public virtual ICollection<ApplicationInvitations> ApplicationInvitations { get; set; }
        public virtual ICollection<ApplicationQuestions> ApplicationQuestions { get; set; }
        public virtual ICollection<PlayerChallenges> Challenges { get; set; }
        public virtual PlayerCharDatas CharDatas { get; set; }
        public virtual ICollection<GangHouses> CreatedHouses { get; set; }
        public short Donation { get; set; }
        public string Email { get; set; }
        public virtual GangMembers GangMemberNavigation { get; set; }
        public int Id { get; set; }
        public bool IsVip { get; set; }
        public virtual ICollection<Lobbies> Lobbies { get; set; }
        public virtual ICollection<Maps> Maps { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Offlinemessages> OfflinemessagesSource { get; set; }
        public virtual ICollection<Offlinemessages> OfflinemessagesTarget { get; set; }
        public virtual Gangs OwnedGang { get; set; }
        public string Password { get; set; }
        public virtual ICollection<PlayerBans> PlayerBansAdmin { get; set; }
        public virtual ICollection<PlayerBans> PlayerBansPlayer { get; set; }
        public virtual PlayerClothes PlayerClothes { get; set; }
        public virtual ICollection<PlayerLobbyStats> PlayerLobbyStats { get; set; }
        public virtual ICollection<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual ICollection<PlayerMapRatings> PlayerMapRatings { get; set; }
        public virtual ICollection<PlayerRelations> PlayerRelationsPlayer { get; set; }
        public virtual ICollection<PlayerRelations> PlayerRelationsTarget { get; set; }
        public virtual PlayerSettings PlayerSettings { get; set; }
        public virtual PlayerStats PlayerStats { get; set; }
        public virtual PlayerTotalStats PlayerTotalStats { get; set; }
        public DateTime RegisterTimestamp { get; set; }
        public ulong SCId { get; set; }
        public string SCName { get; set; }
        public virtual ICollection<SupportRequestMessages> SupportRequestMessages { get; set; }
        public virtual SupportRequests SupportRequests { get; set; }

        #endregion Public Properties

        #region Public Methods

        public string GetDiscriminator()
        {
            return $"{Name} ({SCName})";
        }

        #endregion Public Methods
    }
}
