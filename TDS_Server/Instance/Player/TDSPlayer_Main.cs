using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Enums;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Utility;
using TDS_Server.Interfaces;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Player;
using TimeZoneConverter;

namespace TDS_Server.Instance.Player
{
    public partial class TDSPlayer : EntityWrapperClass
    {
        public Client Client { get; }

        public bool LoggedIn => Entity?.PlayerStats?.LoggedIn == true;

        public Vehicle? FreeroamVehicle { get; set; }


        public List<PlayerRelations> PlayerRelationsTarget { get; private set; } = new List<PlayerRelations>();
        public List<PlayerRelations> PlayerRelationsPlayer { get; private set; } = new List<PlayerRelations>();

        public HashSet<int> BlockingPlayerIds => PlayerRelationsTarget.Where(r => r.Relation == EPlayerRelation.Block).Select(r => r.PlayerId).ToHashSet();
        public PedHash FreemodeSkin => Entity?.PlayerClothes.IsMale == true ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;
        public string DisplayName => AdminLevel.Level >= Constants.ServerTeamSuffixMinAdminLevel ? Constants.ServerTeamSuffix + Client.Name : Client.Name;

        public bool IsCrouched { get; set; }


        public TDSPlayer(Client client)
        {
            Client = client;
        }

        public bool HasRelationTo(TDSPlayer target, EPlayerRelation relation)
        {
            return Entity?.PlayerRelationsPlayer.Any(p => p.TargetId == target.Entity?.Id && p.Relation == relation) == true;
        }

        public async void Logout()
        {
            await RemoveDBContext().ConfigureAwait(false);
        }
    }
}