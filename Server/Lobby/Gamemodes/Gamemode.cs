using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Gamemodes
{
    public abstract class Gamemode : IGamemode
    {
        protected readonly InvitationsHandler InvitationsHandler;
        protected readonly LangHelper LangHelper;

        protected IBaseLobby Lobby;

        protected MapDto Map;

        protected readonly ISettingsHandler SettingsHandler;

        private static HashSet<WeaponHash> _allowedWeaponHashes = new HashSet<WeaponHash>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected Gamemode(ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            SettingsHandler = settingsHandler;
            LangHelper = langHelper;
            InvitationsHandler = invitationsHandler;
        }

        public void Init(IBaseLobby lobby, MapDto map)
        {
            Lobby = lobby;
            Map = map;
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
