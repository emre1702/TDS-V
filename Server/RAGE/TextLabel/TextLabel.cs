using TDS_Server.Data.Interfaces.ModAPI.TextLabel;

namespace TDS_Server.RAGE.TextLabel
{
    class TextLabel : ITextLabel
    {
        private GTANetworkAPI.TextLabel _instance;

        public TextLabel(GTANetworkAPI.TextLabel textLabel)
        {
            _instance = textLabel;
        }

        public void Delete()
        {
            _instance.Delete();
        }
    }
}
