using System.Collections.Generic;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Extensions;
using TDS_Client.Handler.Entities.GTA;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler.GangSystem
{
    public class GangHousesHandler : ServiceBase
    {
        #region Private Fields

        private readonly List<ITDSBlip> _blips = new List<ITDSBlip>();

        private readonly SettingsHandler _settingsHandler;

        #endregion Private Fields

        public GangHousesHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler, SettingsHandler settingsHandler)
            : base(loggingHandler)
        {
            _settingsHandler = settingsHandler;

            eventsHandler.LobbyLeft += LobbyLeft;

            //Todo: Add house blips on request
            RAGE.Events.Add(ToClientEvent.CreateFreeGangHousesForLevel, CreateFreeGangHousesForLevel);
        }

        private void CreateFreeGangHousesForLevel(object[] args)
        {
            string json = (string)args[0];
            var gangHouseBlipDatas = Serializer.FromServer<List<GangHouseClientsideData>>(json);

            foreach (var blipData in gangHouseBlipDatas)
            {
                var blip = new TDSBlip(Constants.GangHouseFreeBlipModel, blipData.Position.ToVector3(),
                    string.Format(_settingsHandler.Language.GANG_LOBBY_FREE_HOUSE_DESCRIPTION, blipData.Level), shortRange: true,
                    alpha: 170, dimension: RAGE.Elements.Player.LocalPlayer.Dimension);
                _blips.Add(blip);
            }
        }

        private void LobbyLeft(SyncedLobbySettings syncedLobbySettings)
        {
            if (syncedLobbySettings.Type != LobbyType.GangLobby)
                return;

            foreach (var blip in _blips)
            {
                blip.Destroy();
            }
            _blips.Clear();
        }
    }
}