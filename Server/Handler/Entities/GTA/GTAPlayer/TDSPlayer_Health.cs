using GTANetworkAPI;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        private int _armor;
        private int _health;

        public override int Armor
        {
            get => _armor;
            set
            {
                _armor = value;
                NAPI.Player.SetPlayerArmor(this, value);
            }
        }

        public override int Health
        {
            get => _health;
            set
            {
                _health = value;
                NAPI.Player.SetPlayerHealth(this, value);
            }
        }

        public override void Spawn(Vector3 pos, float heading = 0)
        {
            DeathSpawnTimer?.Kill();
            DeathSpawnTimer = null;
            NAPI.Player.SpawnPlayer(this, pos, heading);
            if (LobbyNew is { })
                Dimension = LobbyNew.MapHandler.Dimension;
        }
    }
}
