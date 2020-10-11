using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.PlayersSystem;

namespace TDS_Server.PlayersSystem
{
    public class HealthAndArmor : IPlayerHealthAndArmor
    {
        private int _armor;
        private int _health;

        public int Armor
        {
            get => _armor;
            set
            {
                _armor = value;
                NAPI.Player.SetPlayerArmor(_player, value);
            }
        }

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                NAPI.Player.SetPlayerHealth(_player, value);
            }
        }

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public void Add(int effectiveHp, out int effectiveHpAdded)
        {
            if (effectiveHp == 0)
            {
                effectiveHpAdded = 0;
                return;
            }

            if (effectiveHp < 0)
            {
                Add(effectiveHp, out effectiveHpAdded);
                effectiveHpAdded *= -1;
                return;
            }

            //Todo: Add max hp & armor (instead of 100)
            var addToHealth = _health + effectiveHp <= 100 ? effectiveHp : _health;
            var addToArmor = _armor + (effectiveHp - addToHealth) >= 0 ? (effectiveHp - addToHealth) : _armor;

            effectiveHpAdded = addToHealth + addToArmor;

            if (addToArmor > 0)
                Armor += addToArmor;
            if (addToHealth > 0)
                Health += addToHealth;
        }

        public void Remove(int effectiveHp, out int effectiveHpRemoved, out bool killed)
        {
            if (effectiveHp == 0)
            {
                effectiveHpRemoved = 0;
                killed = false;
                return;
            }

            if (effectiveHp > 0)
            {
                Add(effectiveHp, out effectiveHpRemoved);
                effectiveHpRemoved *= -1;
                killed = false;
                return;
            }

            var removeFromArmor = _armor - effectiveHp >= 0 ? effectiveHp : _armor;
            var removeFromHealth = _health - (effectiveHp - removeFromArmor) >= 0 ? (effectiveHp - removeFromArmor) : _health;
            effectiveHpRemoved = removeFromArmor + removeFromHealth;

            if (removeFromArmor > 0)
                Armor -= removeFromArmor;
            if (removeFromHealth > 0)
                Health -= removeFromHealth;

            killed = Health <= 0;
        }
    }
}
