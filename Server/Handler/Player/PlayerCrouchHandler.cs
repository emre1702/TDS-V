using AltV.Net;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Player
{
    public class PlayerCrouchHandler
    {
        #region Constructors

        public PlayerCrouchHandler(EventsHandler eventsHandler)
        {
            eventsHandler.PlayerLeftLobby += EventsHandler_PlayerLeftLobby;

            Alt.OnClient<ITDSPlayer>(ToServerEvent.ToggleCrouch, OnToggleCrouch);
        }

        #endregion Constructors

        #region Methods

        public void OnToggleCrouch(ITDSPlayer player)
        {
            player.IsCrouched = !player.IsCrouched;
            player.SetStreamSyncedMetaData(PlayerDataKey.Crouched.ToString(), player.IsCrouched);
        }

        private void EventsHandler_PlayerLeftLobby(ITDSPlayer player, ILobby lobby)
        {
            if (player.IsCrouched)
                OnToggleCrouch(player);
        }

        #endregion Methods
    }
}
