using GTANetworkAPI;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Default;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    public partial class TDSPlayer : ITDSPlayer
    {
        #region Private Fields

        private readonly AdminsHandler _adminsHandler;
        private readonly ChallengesHelper _challengesHandler;
        private readonly ChatHandler _chatHandler;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly GangsHandler _gangsHandler;
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly SpectateHandler _spectateHandler;
        private readonly WorkaroundsHandler _workaroundsHandler;
        private readonly ILoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public TDSPlayer(
            NetHandle netHandle,
            AdminsHandler adminsHandler,
            ChallengesHelper challengesHandler,
            LangHelper langHelper,
            ISettingsHandler settingsHandler,
            DataSyncHandler dataSyncHandler,
            SpectateHandler spectateHandler,
            GangsHandler gangsHandler,
            LobbiesHandler lobbiesHandler,
            ChatHandler chatHandler,
            EventsHandler eventsHandler,
            WorkaroundsHandler workaroundsHandler,
            DatabaseHandler databaseHandler,
            ILoggingHandler loggingHandler) : base(netHandle)
        {
            _adminsHandler = adminsHandler;
            _challengesHandler = challengesHandler;
            _langHelper = langHelper;
            _settingsHandler = settingsHandler;
            _dataSyncHandler = dataSyncHandler;
            _spectateHandler = spectateHandler;
            _gangsHandler = gangsHandler;
            _lobbiesHandler = lobbiesHandler;
            _chatHandler = chatHandler;
            _workaroundsHandler = workaroundsHandler;
            Database = databaseHandler;
            _loggingHandler = loggingHandler;

            Language = _langHelper.GetLang(TDS_Shared.Data.Enums.Language.English);

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
            eventsHandler.PlayerJoinedLobby += EventsHandler_PlayerJoinedLobby;
            eventsHandler.PlayerLeftLobby += EventsHandler_PlayerLeftLobby;
        }

        #endregion Public Constructors

        #region Public Properties

        public override string DisplayName => IsConsole ? "Console" : (AdminLevel.Level >= SharedConstants.ServerTeamSuffixMinAdminLevel
            ? SharedConstants.ServerTeamSuffix + (Entity is { } ? Entity.Name : Name) : (Entity is { } ? Entity.Name : Name));

        public override PedHash FreemodeSkin => Entity?.CharDatas.GeneralData.ElementAt(Entity.CharDatas.SyncedData.Slot).SyncedData.IsMale == true ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;

        public override ITDSVehicle? FreeroamVehicle { get; set; }

        public new int Id => Entity?.Id ?? 0;

        public override bool IsVip => Entity?.IsVip ?? false;

        public override bool LoggedIn => Entity?.PlayerStats?.LoggedIn == true;

        public override bool IsConsole { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Logout()
        {
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
