using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    public abstract class Gamemode
    {
        protected readonly InvitationsHandler InvitationsHandler;
        protected readonly LangHelper LangHelper;

        protected readonly Arena Lobby;

        protected readonly MapDto Map;

        protected readonly ISettingsHandler SettingsHandler;

        private static HashSet<WeaponHash> _allowedWeaponHashes = new HashSet<WeaponHash>();

        protected Gamemode(Arena lobby, MapDto map, ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
        {
            Lobby = lobby;
            Map = map;

            SettingsHandler = settingsHandler;
            LangHelper = langHelper;
            InvitationsHandler = invitationsHandler;
        }

        public virtual bool HandlesGivingWeapons => false;

        public ITeam? WinnerTeam { get; set; }

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
    }
}