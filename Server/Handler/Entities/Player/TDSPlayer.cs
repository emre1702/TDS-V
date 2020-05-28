using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Entities.Player
{
    public partial class TDSPlayer : DatabaseEntityWrapper, ITDSPlayer
    {
        #region Private Fields

        private readonly AdminsHandler _adminsHandler;
        private readonly ChallengesHelper _challengesHandler;
        private readonly ChatHandler _chatHandler;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly GangsHandler _gangsHandler;
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly IModAPI _modAPI;
        private readonly ISettingsHandler _settingsHandler;
        private readonly SpectateHandler _spectateHandler;

        #endregion Private Fields

        #region Public Constructors

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
            ChatHandler chatHandler,
            EventsHandler eventsHandler) : base(dbContext, loggingHandler)
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

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
            eventsHandler.PlayerJoinedLobby += EventsHandler_PlayerJoinedLobby;
            eventsHandler.PlayerLeftLobby += EventsHandler_PlayerLeftLobby;
        }

        #endregion Public Constructors

        #region Public Properties

        public string DisplayName => ModPlayer is null ? "Console" : (AdminLevel.Level >= SharedConstants.ServerTeamSuffixMinAdminLevel
            ? SharedConstants.ServerTeamSuffix + (Entity is { } ? Entity.Name : ModPlayer.Name) : (Entity is { } ? Entity.Name : ModPlayer.Name));

        public PedHash FreemodeSkin => Entity?.CharDatas.GeneralData.IsMale == true ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;

        public IVehicle? FreeroamVehicle { get; set; }

        public string IPAddress => ModPlayer?.Address ?? "-";

        public bool IsConsole { get; set; }

        public bool IsCrouched { get; set; }

        public IPlayer? ModPlayer { get; set; }

        public bool TryingToLoginRegister { get; set; }

        public int Id => Entity?.Id ?? 0;

        public bool IsVip => Entity?.IsVip ?? false;

        public bool LoggedIn => Entity?.PlayerStats?.LoggedIn == true;

        public ushort RemoteId => ModPlayer?.RemoteId ?? 0;

        public string Serial => ModPlayer?.Serial ?? "-";

        public ulong SocialClubId => ModPlayer?.SocialClubId ?? 0;

        public int VehicleSeat => ModPlayer?.VehicleSeat ?? -1;

        #endregion Public Properties

        #region Public Methods

        public bool Equals(ITDSPlayer? other)
        {
            return Id == other?.Id;
        }

        public void Logout()
        {
        }

        public void Spawn(Position3D position, float rotation)
        {
            ModPlayer?.Spawn(position, rotation);
        }

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_PlayerJoinedLobby(ITDSPlayer player, ILobby lobby)
        {
            CheckFriendPlayerJoinedLobby(player, lobby);
        }

        private void EventsHandler_PlayerLeftLobby(ITDSPlayer player, ILobby lobby)
        {
            CheckFriendPlayerLeftLobby(player, lobby);
        }

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            CheckPlayerOnlineIsFriend(player);
        }

        private void EventsHandler_PlayerLoggedOut(ITDSPlayer player)
        {
            RemovePlayerFromOnlineFriend(player);
        }

        #endregion Private Methods
    }
}
