using System.Collections.Generic;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Extensions;
using TDS.Client.Handler.Entities.GTA;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;

namespace TDS.Client.Handler.GangSystem
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