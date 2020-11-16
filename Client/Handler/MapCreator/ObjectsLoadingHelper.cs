namespace TDS.Client.Handler.MapCreator
{
    public class ObjectsLoadingHelper : ServiceBase
    {
        #region Private Fields

        private readonly SettingsHandler _settingsHandler;
        private readonly UtilsHandler _utilsHandler;

        #endregion Private Fields

        #region Public Constructors

        public ObjectsLoadingHelper(LoggingHandler loggingHandler, UtilsHandler utilsHandler, SettingsHandler settingsHandler)
            : base(loggingHandler)
        {
            _utilsHandler = utilsHandler;
            _settingsHandler = settingsHandler;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool LoadObjectHash(uint hash)
        {
            if (!RAGE.Game.Streaming.IsModelInCdimage(hash) || !RAGE.Game.Streaming.IsModelValid(hash))
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

        #endregion Public Methods

        #region Private Methods

        private bool LoadObjectModel(uint hash)
        {
            //RAGE.Game.Utils.Settimera(0);
            RAGE.Game.Streaming.RequestModel(hash);
            /*while (!RAGE.Game.Streaming.HasModelLoaded(hash))
            {
                RAGE.Game.Utils.Wait(0);
                RAGE.Game.Ui.HideHudAndRadarThisFrame();
                RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                RAGE.Game.Ui.AddTextComponentSubstringPlayerName("Loading...");
                RAGE.Game.Ui.SetTextScale(0.45f);
                RAGE.Game.Ui.SetTextColour(255, 255, 255, 255);
                RAGE.Game.Ui.SetTextCentre(true);
                RAGE.Game.Ui.SetTextJustification(0);
                RAGE.Game.Ui.SetTextFont(0);
                RAGE.Game.Ui.SetTextDropShadow();
                RAGE.Game.Ui.EndTextCommandDisplayText(0.5f, 0.9f);
                if (RAGE.Game.Utils.Timera() > 1000)
                    return false;
            }*/
            return true;
        }

        #endregion Private Methods
    }
}
