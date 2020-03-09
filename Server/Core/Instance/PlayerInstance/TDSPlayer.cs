using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.PlayerManager;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Core.Instance.PlayerInstance
{
    public partial class TDSPlayer : EntityWrapperClass
    {
        public Player? Player { get; }

        public bool LoggedIn => Entity?.PlayerStats?.LoggedIn == true;

        public Vehicle? FreeroamVehicle { get; set; }


        public List<PlayerRelations> PlayerRelationsTarget { get; private set; } = new List<PlayerRelations>();
        public List<PlayerRelations> PlayerRelationsPlayer { get; private set; } = new List<PlayerRelations>();

        public HashSet<int> BlockingPlayerIds => PlayerRelationsTarget.Where(r => r.Relation == EPlayerRelation.Block).Select(r => r.PlayerId).ToHashSet();
        public PedHash FreemodeSkin => Entity?.PlayerClothes.IsMale == true ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;
        public string DisplayName => Player is null ? "Console" : (AdminLevel.Level >= Constants.ServerTeamSuffixMinAdminLevel ? Constants.ServerTeamSuffix + Player.Name : Player.Name);

        public bool IsCrouched { get; set; }
        public bool IsConsole { get; set; }


        public TDSPlayer(Player? client)
        {
            Player = client;
            SetPlayer(this);
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
