using TDS_Client.Data.Interfaces.ModAPI.Player;

namespace TDS_Client.RAGEAPI.Player
{
    class Player : Entity.Entity, IPlayer
    {
        private readonly RAGE.Elements.Player _instance;

        public Player(RAGE.Elements.Player instance) : base(instance)
            => _instance = instance;
    }
}
