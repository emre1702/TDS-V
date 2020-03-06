using GTANetworkAPI;
using System;
using TDS_Common.Enum.Challenge;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.PlayerInstance
{
    partial class TDSPlayer
    {
        private short _killingSpree;
        private short _shortTimeKillingSpree;

        public sbyte Lifes { get; set; } = 0;
        public TDSPlayer? LastHitter { get; set; }
        public DateTime? LastKillAt { get; set; }
        public WeaponHash LastWeaponOnHand { get; set; } = WeaponHash.Unarmed;

        public short KillingSpree
        {
            get => _killingSpree;
            set
            {
                if (_killingSpree + 1 == value)
                {
                    ++_shortTimeKillingSpree;
                }
                _killingSpree = value;
                if (CurrentLobby?.IsOfficial == true)
                    AddToChallenge(EChallengeType.Killstreak, _killingSpree, true);
            }
        }
        public short ShortTimeKillingSpree
        {
            get
            {
                if (LastKillAt is null)
                    return _shortTimeKillingSpree;

                var timeSpanSinceLastKill = DateTime.UtcNow - LastKillAt.Value;
                if (timeSpanSinceLastKill.TotalSeconds <= SettingsManager.KillingSpreeMaxSecondsUntilNextKill)
                {
                    return _shortTimeKillingSpree;
                }
                _shortTimeKillingSpree = 1;
                return 1;
            }
        }


        public void Damage(ref int damage)
        {
            if (damage == 0)
                return;
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

        public void AddHPArmor(int healtharmor)
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
    }
}
