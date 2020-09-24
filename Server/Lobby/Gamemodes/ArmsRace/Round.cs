using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.LobbySystem.Gamemodes
{
    partial class ArmsRace
    {
        public override bool CanJoinDuringRound(ITDSPlayer player, ITeam team) => true;

        public override void StartRoundCountdown()
        {
            Lobby.AmountLifes = short.MaxValue;
        }

        public override void StopRound()
        {
            Lobby.AmountLifes = (Lobby.Entity.FightSettings?.AmountLifes ?? 0);
        }

        private bool CheckRoundEnd(ITDSPlayer killer)
        {
            if (!GetNextWeapon(killer, out WeaponHash? weaponHash))
                return false;

            // WeaponHash == null => Round end
            if (weaponHash.HasValue)
                return false;

            Lobby.CurrentRoundEndBecauseOfPlayer = killer;
            Lobby.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.PlayerWon);
            return true;
        }
    }
}
