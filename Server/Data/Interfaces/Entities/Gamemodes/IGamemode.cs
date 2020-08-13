using TDS_Server.Data.Enums;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.Entities.Gamemodes
{
    public interface IGamemode
    {
        ITeam WinnerTeam { get; }

        bool CanEndRound(RoundEndReason newPlayer);
        void StartMapClear();
        void StartMapChoose();
        bool IsWeaponAllowed(WeaponHash hash);
        void StartRoundCountdown();
        void StartRound();
        void StopRound();
    }
}
