using System.Collections.Generic;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler.GangSystem
{
    public class GangHousesHandler : ServiceBase
    {
        private readonly List<IBlip> _blips = new List<IBlip>();

        private readonly SettingsHandler _settingsHandler;
        private readonly Serializer _serializer;

        public GangHousesHandler(IModAPI modAPI, LoggingHandler loggingHandler, EventsHandler eventsHandler, SettingsHandler settingsHandler, Serializer serializer)
            : base(modAPI, loggingHandler)
        {
            _settingsHandler = settingsHandler;
            _serializer = serializer;

            eventsHandler.LobbyLeft += LobbyLeft;

            //Todo: Add house blips on request
            modAPI.Event.Add(ToClientEvent.CreateFreeGangHousesForLevel, CreateFreeGangHousesForLevel);

        }

        private void CreateFreeGangHousesForLevel(object[] args)
        {
            string json = (string)args[0];
            var gangHouseBlipDatas = _serializer.FromServer<List<GangHouseClientsideData>>(json);

            foreach (var blipData in gangHouseBlipDatas)
            {
                var blip = ModAPI.Blip.Create(Constants.GangHouseFreeBlipModel, blipData.Position,
                    string.Format(_settingsHandler.Language.GANG_LOBBY_FREE_HOUSE_DESCRIPTION, blipData.Level), shortRange: true,
                    alpha: 170, dimension: ModAPI.LocalPlayer.Dimension);
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
