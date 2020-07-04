using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class WeaponStatsHandler : ServiceBase
    {
        #region Private Fields

        private readonly RemoteEventsSender _remoteEventsSender;

        #endregion Private Fields

        #region Public Constructors

        public WeaponStatsHandler(IModAPI modAPI, LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender)
            : base(modAPI, loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;

            modAPI.Event.WeaponShot.Add(new EventMethodData<WeaponShotDelegate>(WeaponShot));
        }

        #endregion Public Constructors

        #region Private Methods

        private void WeaponShot(Position3D pos, IPlayer target, CancelEventArgs cancelEventArgs)
        {
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.WeaponShot);
        }

        #endregion Private Methods
    }
}
