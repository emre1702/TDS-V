using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Core.Damagesystem
{
    partial class Damagesys
    {

        private static readonly HashSet<ulong> _headBones = new HashSet<ulong>
        {
            12844, 31086
        };

        private readonly Dictionary<WeaponHash, DamageDto> _damagesDict = new Dictionary<WeaponHash, DamageDto>();
        private readonly Dictionary<ITDSPlayer, Dictionary<ITDSPlayer, int>> _allHitters = new Dictionary<ITDSPlayer, Dictionary<ITDSPlayer, int>>();


#pragma warning disable IDE0060 // Remove unused parameter
        public void DamagePlayer(ITDSPlayer target, WeaponHash weapon, ulong bone, ITDSPlayer? source)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (target.ModPlayer is null)
                return;

            if (target.ModPlayer.Dead)
                return;

            if (source is null)
                return;

            if (target.Lobby != source.Lobby)
                return;
            if (target.Team == source.Team)
                return;

            bool isHeadShot = _headBones.Contains(bone);
            int damage = (int)Math.Ceiling(_damagesDict.TryGetValue(weapon, out DamageDto? value) ? (value.Damage * (isHeadShot ? value.HeadMultiplier : 1)) : 0);

            target.Damage(ref damage);

            UpdateLastHitter(target, source, damage);
            if (source.CurrentRoundStats != null)
                source.CurrentRoundStats.Damage += damage;

            if (source.Entity?.PlayerSettings.FloatingDamageInfo == true)
            {
                source.SendEvent(ToClientEvent.HitOpponent, target.RemoteId, damage);
            }

            if (target.Health == 0 && isHeadShot)
            {
                target.SendEvent(ToClientEvent.ExplodeHead, (uint)weapon);
            }
        }

        public void UpdateLastHitter(ITDSPlayer target, ITDSPlayer? source, int damage)
        {
            if (source is null)
                return;
            if (!_allHitters.TryGetValue(target, out Dictionary<ITDSPlayer, int>? lasthitterdict))
            {
                lasthitterdict = new Dictionary<ITDSPlayer, int>();
                _allHitters[target] = lasthitterdict;
            }
            lasthitterdict.TryGetValue(source, out int currentDamage);
            lasthitterdict[source] = currentDamage + damage;
            target.LastHitter = source;
        }

        public int GetDamage(WeaponHash hash, bool headshot = false)
        {
            if (!_damagesDict.ContainsKey(hash))
                return 0;
            float damage = _damagesDict[hash].Damage;
            if (headshot)
                damage *= _damagesDict[hash].HeadMultiplier;
            return (int)damage;
        }
    }
}
