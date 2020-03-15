using TDS_Server.Data.Enums;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.GameModes
{
    public abstract class GameMode
    {
        protected Arena Lobby;
        protected MapDto Map;

        public Team? WinnerTeam { get; set; }

        protected GameMode(Arena lobby, MapDto map)
        {
            Lobby = lobby;
            Map = map;
        }

        public virtual void StartRoundCountdown() { }

        public virtual void StartRound() { }

        public virtual void StartMapChoose() { }

        public virtual void StartMapClear() { }

        public virtual void StopRound() { }


        public virtual void SendPlayerRoundInfoOnJoin(TDSPlayer player) { }


        public virtual bool CanJoinLobby(TDSPlayer player, uint? teamIndex) { return true; }
        public virtual void AddPlayer(TDSPlayer player, uint? teamIndex) { }
        public virtual void RemovePlayer(TDSPlayer player) { }
        public virtual void RemovePlayerFromAlive(TDSPlayer player) { }

        public virtual bool IsWeaponAllowed(WeaponHash weaponHash) => true;

        public virtual void OnPlayerEnterColShape(IColShape shape, TDSPlayer player) { }
        public virtual void OnPlayerDeath(TDSPlayer player, TDSPlayer killer) { }
        public virtual void OnPlayerWeaponSwitch(TDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon) { }


        public virtual bool CanJoinDuringRound(TDSPlayer player, Team team) { return false; }

        public virtual bool CanEndRound(RoundEndReason newPlayer) { return true; }
    }
}
