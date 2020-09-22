using System;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Players;

namespace TDS_Server.LobbySystem.Chats
{
    public class BaseLobbyChat
    {
        private readonly Action<Action<ITDSPlayer>> _doForPlayersActionProvider;
        private readonly LangHelper _langHelper;

        public BaseLobbyChat(Action<Action<ITDSPlayer>> doForPlayersActionProvider, LangHelper langHelper)
            => (_doForPlayersActionProvider, _langHelper) = (doForPlayersActionProvider, langHelper);

        public void Send(string msg, ITeam? targetTeam = null)
        {
            if (targetTeam is null)
            {
                _doForPlayersActionProvider(player =>
                {
                    player.SendChatMessage(msg);
                });
            }
            else
            {
                targetTeam.FuncIterate(player =>
                {
                    player.SendChatMessage(msg);
                });
            }
        }

        public void Send(string msg, HashSet<int> blockingPlayerIds, ITeam? targetTeam = null)
        {
            if (targetTeam is null)
            {
                _doForPlayersActionProvider(player =>
                {
                    if (blockingPlayerIds.Contains(player.Entity?.Id ?? 0))
                        return;
                    player.SendChatMessage(msg);
                });
            }
            else
            {
                targetTeam.FuncIterate(player =>
                {
                    if (blockingPlayerIds.Contains(player.Entity?.Id ?? 0))
                        return;
                    player.SendChatMessage(msg);
                });
            }
        }

        public void Send(Func<ILanguage, string> langGetter, ITeam? targetTeam = null)
        {
            Dictionary<ILanguage, string> texts = _langHelper.GetLangDictionary(langGetter);
            if (targetTeam is null)
                _doForPlayersActionProvider(player =>
                {
                    player.SendChatMessage(texts[player.Language]);
                });
            else
                targetTeam.FuncIterate(player =>
                {
                    player.SendChatMessage(texts[player.Language]);
                });
        }

        public void Send(Dictionary<ILanguage, string> texts, ITeam? targetTeam = null)
        {
            if (targetTeam is null)
                _doForPlayersActionProvider(player =>
                {
                    player.SendChatMessage(texts[player.Language]);
                });
            else
                targetTeam.FuncIterate(player =>
                {
                    player.SendChatMessage(texts[player.Language]);
                });
        }
    }
}
