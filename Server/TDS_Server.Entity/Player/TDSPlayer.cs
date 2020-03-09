using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.Player
{
    public partial class TDSPlayer : DatabaseEntityWrapper
    {
        public IPlayer? Player { get; }

        public bool LoggedIn => Entity?.PlayerStats?.LoggedIn == true;

        public IVehicle? FreeroamVehicle { get; set; }


        public List<PlayerRelations> PlayerRelationsTarget { get; private set; } = new List<PlayerRelations>();
        public List<PlayerRelations> PlayerRelationsPlayer { get; private set; } = new List<PlayerRelations>();

        public HashSet<int> BlockingPlayerIds => PlayerRelationsTarget.Where(r => r.Relation == PlayerRelation.Block).Select(r => r.PlayerId).ToHashSet();
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
