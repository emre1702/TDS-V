using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Events;
using static TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers.IFightLobbyEventsHandler;

namespace TDS_Server.LobbySystem.EventsHandlers
{
    public class FightLobbyEventsHandler : BaseLobbyEventsHandler, IFightLobbyEventsHandler
    {
        public event PlayerDiedDelegate? PlayerDied;

        public event PlayerWeaponSwitchDelegate? PlayerWeaponSwitch;

        public FightLobbyEventsHandler(IBaseLobby lobby, EventsHandler eventsHandler, ILoggingHandler logging)
            : base(lobby, eventsHandler, logging)
        {
        }

        public void TriggerPlayerDied(ITDSPlayer player, ITDSPlayer killer, uint weapon, int hadLifes)
        {
            PlayerDied?.Invoke(player, killer, weapon, hadLifes);
        }

        public void TriggerPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            PlayerWeaponSwitch?.Invoke(player, oldWeapon, newWeapon);
        }
    }
}
