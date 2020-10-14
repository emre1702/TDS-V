using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
#nullable enable

    public interface IPlayerWeaponStats
    {
        void AddWeaponDamage(WeaponHash weaponHash, PedBodyPart pedBodyPart, int damage, bool killed);

        void AddWeaponShot(WeaponHash weaponHash);

        void Init(ITDSPlayer player, IPlayerEvents events);

        PlayerWeaponStats? GetWeaponStats(WeaponHash weaponHash);

        PlayerWeaponBodypartStats? GetWeaponBodyPartStats(WeaponHash weaponHash, PedBodyPart pedBodyPart);

        void DoForBodyPartStats(WeaponHash weaponHash, Action<Dictionary<PedBodyPart, PlayerWeaponBodypartStats>> bodyStats);

        List<string> GetWeaponHashesUsedSoFar();
    }
}
