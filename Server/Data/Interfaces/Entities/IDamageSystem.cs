using AltV.Net.Data;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.Entities
{
    #nullable enable
    public interface IDamageSystem
    {
        bool DamageDealtThisRound { get; }

        void UpdateLastHitter(ITDSPlayer target, ITDSPlayer? source, int damagee);
        void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon);
        void DamagePlayer(ITDSPlayer target, WeaponHash weapon, BodyPart bodyPart, ITDSPlayer source);
        void Clear();
        void CheckLastHitter(ITDSPlayer player, out ITDSPlayer? killer);
    }
}
