using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.Data.Interfaces.Entities.Gamemodes
{
    public interface IGamemode
    {
        void Init(IRoundFightLobby lobby, MapDto map);

        ITeam WinnerTeam { get; }
        bool HandlesGivingWeapons { get; }

        bool CanEndRound(RoundEndReason newPlayer);

        void StartMapClear();

        void StartMapChoose();

        bool IsWeaponAllowed(WeaponHash hash);

        void StartRoundCountdown();

        void StartRound();

        void StopRound();

        void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer);

        void RemovePlayerFromAlive(ITDSPlayer player);

        void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon);

        bool CanJoinLobby(ITDSPlayer player, uint? teamindex);

        void AddPlayer(ITDSPlayer player, uint? teamindex);

        void RemovePlayer(ITDSPlayer player);

        bool CanJoinDuringRound(ITDSPlayer player, ITeam team);

        void RespawnPlayer(ITDSPlayer player);

        void SendPlayerRoundInfoOnJoin(ITDSPlayer player);

        void OnPlayerEnterColshape(ITDSColShape shape, ITDSPlayer player);
    }
}
