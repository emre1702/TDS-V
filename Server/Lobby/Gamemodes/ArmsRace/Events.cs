using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.LobbySystem.Gamemodes
{
    partial class ArmsRace
    {
        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer)
        {
            if (killer == player)
                return;
            if (CheckRoundEnd(killer))
                return;

            GiveNextWeapon(killer);
        }
    }
}
