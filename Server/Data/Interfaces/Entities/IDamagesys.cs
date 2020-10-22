using System.Collections.Generic;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.Entities
{
#nullable enable

    public interface IDamagesys
    {
        bool DamageDealtThisRound { get; }

        void Clear();

        void DamagePlayer(ITDSPlayer target, WeaponHash weapon, PedBodyPart pedBodyPart, ITDSPlayer? source);

        int GetDamage(WeaponHash hash, bool headshot = false);

        ITDSPlayer GetKiller(ITDSPlayer player, ITDSPlayer? possiblekiller);

        void Init(IEnumerable<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards);

        void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon);

        void RewardLastHitter(ITDSPlayer player, out ITDSPlayer? killer);

        void UpdateLastHitter(ITDSPlayer target, ITDSPlayer? source, int damage);

        Dictionary<WeaponHash, DamageDto> GetDamages();

        void SetDamage(WeaponHash weapon, DamageDto damageDto);
    }
}
