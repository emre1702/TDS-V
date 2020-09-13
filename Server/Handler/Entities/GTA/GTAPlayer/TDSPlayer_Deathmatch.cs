﻿using GTANetworkAPI;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        #region Private Fields

        private short _killingSpree;
        private short _shortTimeKillingSpree;

        #endregion Private Fields

        #region Public Properties

        public override short KillingSpree
        {
            get => _killingSpree;
            set
            {
                if (_killingSpree + 1 == value)
                {
                    ++_shortTimeKillingSpree;
                }
                _killingSpree = value;
                if (Lobby?.IsOfficial == true)
                    AddToChallenge(ChallengeType.Killstreak, _killingSpree, true);
            }
        }

        public override bool IsCrouched { get; set; }
        public override ITDSPlayer? LastHitter { get; set; }
        public override DateTime? LastKillAt { get; set; }
        public override WeaponHash LastWeaponOnHand { get; set; } = WeaponHash.Unarmed;
        public override short Lifes { get; set; } = 0;

        public override short ShortTimeKillingSpree
        {
            get
            {
                if (LastKillAt is null)
                    return _shortTimeKillingSpree;

                var timeSpanSinceLastKill = DateTime.UtcNow - LastKillAt.Value;
                if (timeSpanSinceLastKill.TotalSeconds <= _settingsHandler.ServerSettings.KillingSpreeMaxSecondsUntilNextKill)
                {
                    return _shortTimeKillingSpree;
                }
                _shortTimeKillingSpree = 1;
                return 1;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void AddHPArmor(int healtharmor)
        {
            #region HP

            if (_health + healtharmor <= 100)
            {
                Health += healtharmor;
                healtharmor = 0;
            }
            else if (_health != 100)
            {
                healtharmor -= 100 - _health;
                Health = 100;
            }

            #endregion HP

            #region Armor

            if (healtharmor > 0)
            {
                Armor = _armor + healtharmor <= 100 ? _armor + healtharmor : 100;
            }

            #endregion Armor
        }

        public override void Damage(ref int damage, out bool killed)
        {
            if (damage == 0)
            {
                killed = false;
                return;
            }

            killed = _armor + _health <= damage;
            damage = Math.Min(_armor + _health, damage);

            int leftdmg = damage;
            if (_armor > 0)
            {
                leftdmg -= _armor;
                Armor = leftdmg < 0 ? Math.Abs(leftdmg) : 0;
            }
            if (leftdmg > 0)
                Health -= leftdmg;
        }

        public override void OnPlayerWeaponSwitch(WeaponHash previousWeapon, WeaponHash newWeapon)
        {
            LastWeaponOnHand = newWeapon;
        }

        #endregion Public Methods
    }
}