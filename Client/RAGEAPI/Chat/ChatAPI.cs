using TDS_Client.Data.Interfaces.ModAPI.Chat;

namespace TDS_Client.RAGEAPI.Chat
{
    class ChatAPI : IChatAPI
    {
        public bool SafeMode 
        { 
            get => RAGE.Chat.SafeMode;
            set => RAGE.Chat.SafeMode = value; 
        }

        public void Output(string msg)
        {
            RAGE.Chat.Output(msg);
        }

        public void Show(bool show)
        {
            RAGE.Chat.Show(show);
        }
    }
}
