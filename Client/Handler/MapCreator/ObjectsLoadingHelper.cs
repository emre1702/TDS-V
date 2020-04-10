using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.MapCreator
{
    public class ObjectsLoadingHelper
    {
        private readonly IModAPI _modAPI;
        private readonly UtilsHandler _utilsHandler;
        private readonly SettingsHandler _settingsHandler;

        public ObjectsLoadingHelper(IModAPI modAPI, UtilsHandler utilsHandler, SettingsHandler settingsHandler)
        {
            _modAPI = modAPI;
            _utilsHandler = utilsHandler;
            _settingsHandler = settingsHandler;
        }

        public bool LoadObjectHash(uint hash)
        {
            if (!_modAPI.Streaming.IsModelInCdimage(hash) || !_modAPI.Streaming.IsModelValid(hash))
            {
                _utilsHandler.Notify(_settingsHandler.Language.OBJECT_MODEL_INVALID);
                return false;
            }
            if (!LoadObjectModel(hash))
            {
                _utilsHandler.Notify(_settingsHandler.Language.COULD_NOT_LOAD_OBJECT);
                return false;
            }
            return true;
        }

        private bool LoadObjectModel(uint hash)
        {
            _modAPI.Utils.Settimera(0);
            _modAPI.Streaming.RequestModel(hash);
            while (!_modAPI.Streaming.HasModelLoaded(hash))
            {
                _modAPI.Misc.Wait(0);
                _modAPI.Ui.HideHudAndRadarThisFrame();
                _modAPI.Ui.BeginTextCommandDisplayText("STRING");
                _modAPI.Ui.AddTextComponentSubstringPlayerName("Loading...");
                _modAPI.Ui.SetTextScale(1.0f, 0.45f);
                _modAPI.Ui.SetTextColour(255, 255, 255, 255);
                _modAPI.Ui.SetTextCentre(true);
                _modAPI.Ui.SetTextJustification(0);
                _modAPI.Ui.SetTextFont(0);
                _modAPI.Ui.SetTextDropShadow();
                _modAPI.Ui.EndTextCommandDisplayText(0.5f, 0.9f, 0);
                if (_modAPI.Utils.Timera() > 1000)
                    return false;
            }
            return true;
        }
    }
}
