using System.Collections.Generic;
using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.Gamemodes
{
    public abstract class Gamemode : IGamemode
    {
        #region Protected Fields

        protected readonly InvitationsHandler InvitationsHandler;
        protected readonly LangHelper LangHelper;

        protected readonly IArena Lobby;

        protected readonly MapDto Map;

        protected readonly Serializer Serializer;

        protected readonly ISettingsHandler SettingsHandler;

        #endregion Protected Fields

        #region Private Fields

        private static HashSet<WeaponHash> _allowedWeaponHashes = new HashSet<WeaponHash>();

        #endregion Private Fields

        #region Protected Constructors

        protected Gamemode(IArena lobby, MapDto map, Serializer serializer, ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
        {
            Lobby = lobby;
            Map = map;
            Serializer = serializer;
            SettingsHandler = settingsHandler;
            LangHelper = langHelper;
            InvitationsHandler = invitationsHandler;
        }

        #endregion Protected Constructors

        #region Public Properties

        public virtual bool HandlesGivingWeapons => false;

        public ITeam? WinnerTeam { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static HashSet<WeaponHash> GetAllowedWeapons() => _allowedWeaponHashes;

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }

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

        public virtual void GivePlayerWeapons(ITDSPlayer player)
        {
        }

        public virtual bool IsWeaponAllowed(WeaponHash weaponHash) => _allowedWeaponHashes.Contains(weaponHash);

        public virtual void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer)
        {
        }

        public virtual void OnPlayerEnterColshape(ITDSColShape shape, ITDSPlayer player)
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

        public virtual void RespawnPlayer(ITDSPlayer player)
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
