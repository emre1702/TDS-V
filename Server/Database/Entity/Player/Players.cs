using System;
using System.Collections.Generic;
using TDS.Server.Database.Entity.Admin;
using TDS.Server.Database.Entity.Challenge;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Database.Entity.Player.Character.Body;
using TDS.Server.Database.Entity.Player.Settings;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Database.Entity.Userpanel;

namespace TDS.Server.Database.Entity.Player
{
    public partial class Players
    {
        public Players()
        {
            AdminMembers = new List<Players>();
            ApplicationInvitations = new List<ApplicationInvitations>();
            ApplicationQuestions = new List<ApplicationQuestions>();
            Challenges = new List<PlayerChallenges>();
            Commands = new List<PlayerCommands>();
            CreatedHouses = new List<GangHouses>();
            Lobbies = new List<Lobbies>();
            Maps = new List<Maps>();
            OfflinemessagesSource = new List<Offlinemessages>();
            OfflinemessagesTarget = new List<Offlinemessages>();
            PlayerBansAdmin = new List<PlayerBans>();
            PlayerBansPlayer = new List<PlayerBans>();
            PlayerLobbyStats = new List<PlayerLobbyStats>();
            PlayerMapFavourites = new List<PlayerMapFavourites>();
            PlayerMapRatings = new List<PlayerMapRatings>();
            PlayerRelationsPlayer = new List<PlayerRelations>();
            PlayerRelationsTarget = new List<PlayerRelations>();
            SupportRequestMessages = new List<SupportRequestMessages>();
            WeaponBodypartStats = new List<PlayerWeaponBodypartStats>();
            WeaponStats = new List<PlayerWeaponStats>();
        }

        public int Id { get; set; }
        public bool IsVip { get; set; }
        public int? AdminLeaderId { get; set; }
        public short AdminLvl { get; set; }
        public short Donation { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime RegisterTimestamp { get; set; }
        public ulong SCId { get; set; }
        public string SCName { get; set; }
        public ulong? DiscordUserId { get; set; }

        public virtual Players AdminLeader { get; set; }
        public virtual AdminLevels AdminLvlNavigation { get; set; }
        public virtual ICollection<Players> AdminMembers { get; set; }
        public virtual Applications Application { get; set; }
        public virtual ICollection<ApplicationInvitations> ApplicationInvitations { get; set; }
        public virtual ICollection<ApplicationQuestions> ApplicationQuestions { get; set; }
        public virtual ICollection<PlayerChallenges> Challenges { get; set; }
        public virtual PlayerBodyDatas BodyDatas { get; set; }
        public virtual ICollection<PlayerCommands> Commands { get; set; }
        public virtual ICollection<GangHouses> CreatedHouses { get; set; }
        public virtual GangMembers GangMemberNavigation { get; set; }
        public virtual PlayerKillInfoSettings KillInfoSettings { get; set; }
        public virtual ICollection<Lobbies> Lobbies { get; set; }
        public virtual ICollection<Maps> Maps { get; set; }
        public virtual ICollection<Offlinemessages> OfflinemessagesSource { get; set; }
        public virtual ICollection<Offlinemessages> OfflinemessagesTarget { get; set; }
        public virtual Gangs OwnedGang { get; set; }
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
        public virtual ICollection<SupportRequestMessages> SupportRequestMessages { get; set; }
        public virtual SupportRequests SupportRequests { get; set; }
        public virtual PlayerThemeSettings ThemeSettings { get; set; }
        public virtual ICollection<PlayerWeaponBodypartStats> WeaponBodypartStats { get; set; }
        public virtual ICollection<PlayerWeaponStats> WeaponStats { get; set; }

        public string Discriminator => $"{Name} ({SCName})";
    }
}