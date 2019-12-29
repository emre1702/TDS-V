using GTANetworkAPI;
using System;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Player
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
            damage = Math.Min(Client!.Armor + Client!.Health, damage);

            int leftdmg = damage;
            if (Client.Armor > 0)
            {
                leftdmg -= Client.Armor;
                Client.Armor = leftdmg < 0 ? Math.Abs(leftdmg) : 0;
            }
            if (leftdmg > 0)
                Client.Health -= leftdmg;
        }

        public void AddHPArmor(int healtharmor)
        {
            #region HP

            if (Client!.Health + healtharmor <= 100)
            {
                Client.Health += healtharmor;
                healtharmor = 0;
            }
            else if (Client.Health != 100)
            {
                healtharmor -= 100 - Client.Health;
                Client.Health = 100;
            }

            #endregion HP

            #region Armor

            if (healtharmor > 0)
            {
                Client.Armor = Client.Armor + healtharmor <= 100 ? Client.Armor + healtharmor : 100;
            }

            #endregion Armor
        }
    }
}
