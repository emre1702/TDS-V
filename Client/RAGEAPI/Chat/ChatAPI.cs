using TDS_Client.Data.Interfaces.ModAPI.Chat;

namespace TDS_Client.RAGEAPI.Chat
{
    internal class ChatAPI : IChatAPI
    {
        #region Public Properties

        public bool SafeMode
        {
            get => RAGE.Chat.SafeMode;
            set => RAGE.Chat.SafeMode = value;
        }

        #endregion Public Properties

        #region Public Methods

        public void Output(string msg)
        {
            RAGE.Chat.Output(msg);
        }

        public void Show(bool show)
        {
            RAGE.Chat.Show(show);
        }

        #endregion Public Methods
    }
}
