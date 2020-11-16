using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Handler.Helper;

namespace TDS.Server.GangsSystem
{
    public class Chat : IGangChat
    {
        private readonly LangHelper _langHelper;
        private readonly IGangPlayers _players;

        public Chat(LangHelper langHelper, IGangPlayers players)
            => (_langHelper, _players) = (langHelper, players);

        public void SendMessage(Func<ILanguage, string> langGetter)
        {
            var langDictionary = _langHelper.GetLangDictionary(langGetter);
            _players.DoInMain(player =>
                player.SendChatMessage(langDictionary[player.Language]));
        }

        public void SendNotification(Func<ILanguage, string> langGetter)
        {
            var langDictionary = _langHelper.GetLangDictionary(langGetter);
            _players.DoInMain(player =>
                player.SendNotification(langDictionary[player.Language]));
        }
    }
}
