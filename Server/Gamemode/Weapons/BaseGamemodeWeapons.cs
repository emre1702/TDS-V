using GTANetworkAPI;
using TDS_Server.Data.Interfaces.GamemodesSystem.Weapons;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;

namespace TDS_Server.GamemodesSystem.Weapons
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
