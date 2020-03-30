using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.Player
{
    public partial class TDSPlayer : DatabaseEntityWrapper, ITDSPlayer
    {
        public int Id => Entity?.Id ?? 0;
        public ushort RemoteId => ModPlayer?.RemoteId ?? 0;
        public ulong SocialClubId => ModPlayer?.SocialClubId ?? 0;
        public IPlayer? ModPlayer { get; set; }
        public string IPAddress => ModPlayer?.IPAddress ?? "-";
        public string Serial => ModPlayer?.Serial ?? "-";

        public bool LoggedIn => Entity?.PlayerStats?.LoggedIn == true;

        public IVehicle? FreeroamVehicle { get; set; }
        public int VehicleSeat => ModPlayer?.VehicleSeat ?? -1;


        public List<PlayerRelations> PlayerRelationsTarget { get; private set; } = new List<PlayerRelations>();
        public List<PlayerRelations> PlayerRelationsPlayer { get; private set; } = new List<PlayerRelations>();

        public HashSet<int> BlockingPlayerIds => PlayerRelationsTarget.Where(r => r.Relation == PlayerRelation.Block).Select(r => r.PlayerId).ToHashSet();
        public PedHash FreemodeSkin => Entity?.PlayerClothes.IsMale == true ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;
        public string DisplayName => ModPlayer is null ? "Console" : (AdminLevel.Level >= SharedConstants.ServerTeamSuffixMinAdminLevel ? SharedConstants.ServerTeamSuffix + ModPlayer.Name : ModPlayer.Name);
        public bool IsVip => Entity?.IsVip ?? false;

        public bool IsCrouched { get; set; }
        public bool IsConsole { get; set; }

        private readonly AdminsHandler _adminsHandler;
        private readonly ChallengesHelper _challengesHandler;
        private readonly LangHelper _langHelper;
        private readonly IModAPI _modAPI;
        private readonly ISettingsHandler _settingsHandler;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly SpectateHandler _spectateHandler;
        private readonly GangsHandler _gangsHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ChatHandler _chatHandler;

        public TDSPlayer(
            TDSDbContext dbContext,
            ILoggingHandler loggingHandler,
            AdminsHandler adminsHandler,
            ChallengesHelper challengesHandler,
            LangHelper langHelper,
            IModAPI modAPI,
            ISettingsHandler settingsHandler,
            DataSyncHandler dataSyncHandler,
            SpectateHandler spectateHandler,
            GangsHandler gangsHandler,
            LobbiesHandler lobbiesHandler,
            ChatHandler chatHandler) : base(dbContext, loggingHandler)
        {
            _adminsHandler = adminsHandler;
            _challengesHandler = challengesHandler;
            _langHelper = langHelper;
            _modAPI = modAPI;
            _settingsHandler = settingsHandler;
            _dataSyncHandler = dataSyncHandler;
            _spectateHandler = spectateHandler;
            _gangsHandler = gangsHandler;
            _lobbiesHandler = lobbiesHandler;
            _chatHandler = chatHandler;

            Language = _langHelper.GetLang(TDS_Shared.Data.Enums.Language.English);
        }

        public bool HasRelationTo(ITDSPlayer target, PlayerRelation relation)
        {
            return Entity?.PlayerRelationsPlayer.Any(p => p.TargetId == target.Entity?.Id && p.Relation == relation) == true;
        }

        public void Logout()
        {

        }

        public bool Equals(ITDSPlayer? other)
        {
            return Id == other?.Id;
        }

        public void Spawn(Position3D position, float rotation)
        {
            ModPlayer?.Spawn(position, rotation);
        }


    }
}
