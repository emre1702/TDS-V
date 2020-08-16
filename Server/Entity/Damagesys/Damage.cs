using AltV.Net.Data;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Entity.Damagesys
{
    partial class DamageSystem
    {
        #region Private Fields

        private readonly Dictionary<ITDSPlayer, Dictionary<ITDSPlayer, int>> _allHitters = new Dictionary<ITDSPlayer, Dictionary<ITDSPlayer, int>>();
        private readonly Dictionary<WeaponHash, DamageDto> _damagesDict = new Dictionary<WeaponHash, DamageDto>();

        #endregion Private Fields

        #region Public Methods

        public void DamagePlayer(ITDSPlayer target, WeaponHash weapon, BodyPart bodyPart, ITDSPlayer? source)
        {
            if (!target.LoggedIn)
                return;

            if (target.IsDead)
                return;

            if (source is null)
                return;

            if (target.Lobby != source.Lobby)
                return;
            if (target.Team == source.Team)
                return;

            bool isHeadShot = bodyPart == BodyPart.Head;
            int damage = (int)Math.Ceiling(_damagesDict.TryGetValue(weapon, out DamageDto? value) ? (value.Damage * (isHeadShot ? value.HeadMultiplier : 1)) : 0);

            target.Damage(ref damage, out bool killed);
            source.AddWeaponShot(weapon, bodyPart, damage, killed);

            UpdateLastHitter(target, source, damage);
            if (source.CurrentRoundStats != null)
                source.CurrentRoundStats.Damage += damage;

            if (source.Entity?.PlayerSettings.FloatingDamageInfo == true)
            {
                source.SendEvent(ToClientEvent.HitOpponent, target, damage);
            }

            if (target.Health == 0 && isHeadShot)
            {
                target.SendEvent(ToClientEvent.ExplodeHead, (uint)weapon);
            }
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

        #endregion Public Methods
    }
}
