using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.MapCreator
{
    public class ObjectsLoadingHelper : ServiceBase
    {
        private readonly UtilsHandler _utilsHandler;
        private readonly SettingsHandler _settingsHandler;

        public ObjectsLoadingHelper(IModAPI modAPI, LoggingHandler loggingHandler, UtilsHandler utilsHandler, SettingsHandler settingsHandler)
            : base(modAPI, loggingHandler)
        {
            _utilsHandler = utilsHandler;
            _settingsHandler = settingsHandler;
        }

        public bool LoadObjectHash(uint hash)
        {
            if (!ModAPI.Streaming.IsModelInCdimage(hash) || !ModAPI.Streaming.IsModelValid(hash))
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
            //ModAPI.Utils.Settimera(0);
            ModAPI.Streaming.RequestModel(hash);
            /*while (!ModAPI.Streaming.HasModelLoaded(hash))
            {
                ModAPI.Utils.Wait(0);
                ModAPI.Ui.HideHudAndRadarThisFrame();
                ModAPI.Ui.BeginTextCommandDisplayText("STRING");
                ModAPI.Ui.AddTextComponentSubstringPlayerName("Loading...");
                ModAPI.Ui.SetTextScale(0.45f);
                ModAPI.Ui.SetTextColour(255, 255, 255, 255);
                ModAPI.Ui.SetTextCentre(true);
                ModAPI.Ui.SetTextJustification(0);
                ModAPI.Ui.SetTextFont(0);
                ModAPI.Ui.SetTextDropShadow();
                ModAPI.Ui.EndTextCommandDisplayText(0.5f, 0.9f);
                if (ModAPI.Utils.Timera() > 1000)
                    return false;
            }*/
            return true;
        }
    }
}
