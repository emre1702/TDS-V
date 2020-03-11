using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Entity.Player
{
    public partial class TDSPlayer : DatabaseEntityWrapper, ITDSPlayer
    {
        public IPlayer? ModPlayer { get; }

        public bool LoggedIn => Entity?.PlayerStats?.LoggedIn == true;

        public IVehicle? FreeroamVehicle { get; set; }


        public List<PlayerRelations> PlayerRelationsTarget { get; private set; } = new List<PlayerRelations>();
        public List<PlayerRelations> PlayerRelationsPlayer { get; private set; } = new List<PlayerRelations>();

        public HashSet<int> BlockingPlayerIds => PlayerRelationsTarget.Where(r => r.Relation == PlayerRelation.Block).Select(r => r.PlayerId).ToHashSet();
        public PedHash FreemodeSkin => Entity?.PlayerClothes.IsMale == true ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;
        public string DisplayName => ModPlayer is null ? "Console" : (AdminLevel.Level >= Constants.ServerTeamSuffixMinAdminLevel ? Constants.ServerTeamSuffix + ModPlayer.Name : ModPlayer.Name);

        public bool IsCrouched { get; set; }
        public bool IsConsole { get; set; }


        public TDSPlayer(IPlayer? modPlayer)
        {
            ModPlayer = modPlayer;
            SetPlayer(this);
        }

        public bool HasRelationTo(TDSPlayer target, PlayerRelation relation)
        {
            return Entity?.PlayerRelationsPlayer.Any(p => p.TargetId == target.Entity?.Id && p.Relation == relation) == true;
        }

        public async void Logout()
        {
            await RemoveDBContext().ConfigureAwait(false);
        }
    }
}
