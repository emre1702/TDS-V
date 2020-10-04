using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Core;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Specials
{
#nullable enable

    public interface IBombGamemodeSpecials
    {
        ITDSObject? Bomb { get; set; }
        TDSTimer? BombDetonateTimer { get; }
        TDSTimer? BombPlantDefuseTimer { get; }

        void BombToBack(ITDSPlayer player);

        void BombToHand(ITDSPlayer player);

        void DetonateBomb();

        void DropBomb();

        void GiveBombToRandomTerrorist();

        bool StartBombDefusing(ITDSPlayer player);

        bool StartBombPlanting(ITDSPlayer player);

        void StopBombDefusing(ITDSPlayer player);

        void StopBombPlanting(ITDSPlayer player);

        void TakeBomb(ITDSPlayer player);

        void ToggleBombAtHand(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon);
    }
}
