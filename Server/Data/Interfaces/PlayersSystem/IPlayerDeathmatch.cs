using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    #nullable enable
    public interface IPlayerDeathmatch
    {
        short KillingSpree { get; set; }
        ITDSPlayer? LastHitter { get; set; }
        WeaponHash? LastHitterWeapon { get; set; }
        DateTime? LastKillAt { get; set; }
        WeaponHash LastWeaponOnHand { get; set; }
        short ShortTimeKillingSpree { get; }

        void Init(ITDSPlayer player, IPlayerEvents events);
    }
}
