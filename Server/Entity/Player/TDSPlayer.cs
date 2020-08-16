using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Database.Entity;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Entity.Player
{
    public partial class TDSPlayer : AltV.Net.Elements.Entities.Player, IDatabaseEntityWrapper, ITDSPlayer
    {
        #region Private Fields

        private readonly AdminsHandler _adminsHandler;
        private readonly ChallengesHelper _challengesHandler;
        private readonly ChatHandler _chatHandler;
        private readonly GangsHandler _gangsHandler;
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly SpectateHandler _spectateHandler;
        private readonly ILoggingHandler _loggingHandler;
        private string _name;

        #endregion Private Fields

        #region Public Constructors

        public TDSPlayer(
            IntPtr entityPointer, ushort id,
            TDSDbContext dbContext,
            ILoggingHandler loggingHandler,
            AdminsHandler adminsHandler,
            ChallengesHelper challengesHandler,
            LangHelper langHelper,
            ISettingsHandler settingsHandler,
            SpectateHandler spectateHandler,
            GangsHandler gangsHandler,
            LobbiesHandler lobbiesHandler,
            ChatHandler chatHandler,
            EventsHandler eventsHandler) : base(entityPointer, id)
        {
            _dbContext = dbContext;
            _loggingHandler = loggingHandler;
            _adminsHandler = adminsHandler;
            _challengesHandler = challengesHandler;
            _langHelper = langHelper;
            _settingsHandler = settingsHandler;
            _spectateHandler = spectateHandler;
            _gangsHandler = gangsHandler;
            _lobbiesHandler = lobbiesHandler;
            _chatHandler = chatHandler;
            _name = base.Name;

            Language = _langHelper.GetLang(TDS_Shared.Data.Enums.Language.English);

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
            eventsHandler.PlayerJoinedLobby += EventsHandler_PlayerJoinedLobby;
            eventsHandler.PlayerLeftLobby += EventsHandler_PlayerLeftLobby;
        }

        #endregion Public Constructors

        #region Public Properties

        public string DisplayName => AdminLevel.Level >= SharedConstants.ServerTeamSuffixMinAdminLevel
            ? SharedConstants.ServerTeamSuffix + (Entity is { } ? Entity.Name : Name) : (Entity is { } ? Entity.Name : Name);

        public PedHash FreemodeSkin => Entity?.CharDatas.GeneralData.ElementAt(Entity.CharDatas.Slot).IsMale == true ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;

        public ITDSVehicle? FreeroamVehicle { get; set; }

        public new ITDSVehicle? Vehicle => base.Vehicle as ITDSVehicle;

        public bool IsConsole { get; set; }

        public bool IsCrouched { get; set; }

        public bool TryingToLoginRegister { get; set; }

        public new int Id => Entity?.Id ?? 0;

        public bool IsVip => Entity?.IsVip ?? false;

        public bool LoggedIn => Entity?.PlayerStats?.LoggedIn == true;

        public ushort AltVId => base.Id;

        public new string Name
        {
            get => _name;
            set
            {
                _name = value;
                SetSyncedMetaData(PlayerDataKey.Name.ToString(), value);
            }
        }

        public int Transparency { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion Public Properties

        #region Public Methods

        public bool Equals(ITDSPlayer? other)
        {
            return Id == other?.Id;
        }

        public void Logout()
        {
        }

        public void Spawn(Position position, float rotation)
        {
            //Todo: Add more here?
            base.Spawn(position, 0);
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

        public void SetClientMetaData(string key, object value)
        {
            SendEvent(ToClientEvent.SetPlayerData, key, value);
        }

        #endregion Private Methods
    }
}
