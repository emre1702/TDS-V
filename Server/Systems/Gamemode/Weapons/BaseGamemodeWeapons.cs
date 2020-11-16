using GTANetworkAPI;
using TDS.Server.Data.Interfaces.GamemodesSystem.Weapons;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;

namespace TDS.Server.GamemodesSystem.Weapons
{
    public class BaseGamemodeWeapons : IBaseGamemodeWeapons
    {
        public virtual bool HandlesGivingWeapons => false;

        internal virtual void AddEvents(IRoundFightLobbyEventsHandler events)
        {
        }

        internal virtual void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
        }

        public virtual bool IsWeaponAllowed(WeaponHash weaponHash) => true;
    }
}
