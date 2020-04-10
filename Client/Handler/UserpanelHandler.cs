using TDS_Client.Data.Enums;
using TDS_Client.Handler.Browser;

namespace TDS_Client.Handler
{
    public class UserpanelHandler
    {
        private bool _open;

        private readonly BrowserHandler _browserHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly SettingsHandler _settingsHandler;

        public UserpanelHandler(BrowserHandler browserHandler, CursorHandler cursorHandler, SettingsHandler settingsHandler)
        {
            _browserHandler = browserHandler;
            _cursorHandler = cursorHandler;
            _settingsHandler = settingsHandler;
        }

        public void Toggle(Key key)
        {
            if (key != Key.NoName)
            {
                if (_browserHandler.InInput)
                    return;
            }
            else 
                _open = true;

            _open = !_open;
            _cursorHandler.Visible = _open;
            _browserHandler.Angular.ToggleUserpanel(_open);

            if (!_open)
                _settingsHandler.RevertTempSettings();
        }

        public void Close()
        {
            Toggle(Key.NoName);
        }
    }
}
