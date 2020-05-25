using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.GameModes
{
    public abstract class GameMode
    {
        #region Protected Fields

        protected readonly InvitationsHandler InvitationsHandler;
        protected readonly LangHelper LangHelper;
        protected readonly Arena Lobby;
        protected readonly MapDto Map;
        protected readonly IModAPI ModAPI;
        protected readonly Serializer Serializer;
        protected readonly ISettingsHandler SettingsHandler;

        #endregion Protected Fields

        #region Protected Constructors

        protected GameMode(Arena lobby, MapDto map, IModAPI modAPI, Serializer serializer, ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
        {
            Lobby = lobby;
            Map = map;
            ModAPI = modAPI;
            Serializer = serializer;
            SettingsHandler = settingsHandler;
            LangHelper = langHelper;
            InvitationsHandler = invitationsHandler;
        }

        #endregion Protected Constructors

        #region Public Properties

        public ITeam? WinnerTeam { get; set; }

        #endregion Public Properties

        #region Public Methods

        public virtual void AddPlayer(ITDSPlayer player, uint? teamIndex)
        {
        }

        public virtual bool CanEndRound(RoundEndReason newPlayer)
        {
            return true;
        }

        public virtual bool CanJoinDuringRound(ITDSPlayer player, ITeam team)
        {
            return false;
        }

        public virtual bool CanJoinLobby(ITDSPlayer player, uint? teamIndex)
        {
            return true;
        }

        public virtual bool IsWeaponAllowed(WeaponHash weaponHash) => true;

        public virtual void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer)
        {
        }

        public virtual void OnPlayerEnterColshape(IColShape shape, ITDSPlayer player)
        {
        }

        public virtual void OnPlayerWeaponSwitch(ITDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon)
        {
        }

        public virtual void RemovePlayer(ITDSPlayer player)
        {
        }

        public virtual void RemovePlayerFromAlive(ITDSPlayer player)
        {
        }

        public virtual void SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
        }

        public virtual void StartMapChoose()
        {
        }

        public virtual void StartMapClear()
        {
        }

        public virtual void StartRound()
        {
        }

        public virtual void StartRoundCountdown()
        {
        }

        public virtual void StopRound()
        {
        }

        #endregion Public Methods
    }
}
