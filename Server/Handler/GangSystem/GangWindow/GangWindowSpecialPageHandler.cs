using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Handler.Events;

namespace TDS.Server.Handler.GangSystem.GangWindow
{
    public class GangWindowSpecialPageHandler
    {
        private readonly Dictionary<IGang, Dictionary<GangWindowOnlyOneEditorPage, ITDSPlayer>> _playerInWindow
            = new Dictionary<IGang, Dictionary<GangWindowOnlyOneEditorPage, ITDSPlayer>>();

        public GangWindowSpecialPageHandler(EventsHandler eventsHandler)
        {
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
            eventsHandler.PlayerLeftGang += RemovePlayerFromPossiblePage;
        }

        private void RemovePlayerFromPossiblePage(ITDSPlayer player, IGang gang)
        {
            if (!_playerInWindow.TryGetValue(gang, out Dictionary<GangWindowOnlyOneEditorPage, ITDSPlayer>? dict))
                return;

            if (!dict.ContainsValue(player))
                return;

            var key = dict.FirstOrDefault(e => e.Value == player).Key;
            if (key is { })
                dict.Remove(key);

            if (dict.Count == 0)
                _playerInWindow.Remove(gang);
        }

        private void EventsHandler_PlayerLoggedOut(ITDSPlayer player)
        {
            if (!player.IsInGang)
                return;
            RemovePlayerFromPossiblePage(player, player.Gang);
        }

        public object? OpenOnlyOneEditorPage(ITDSPlayer player, GangWindowOnlyOneEditorPage key)
        {
            if (!_playerInWindow.TryGetValue(player.Gang, out Dictionary<GangWindowOnlyOneEditorPage, ITDSPlayer>? dict))
            {
                dict = new Dictionary<GangWindowOnlyOneEditorPage, ITDSPlayer>();
                _playerInWindow[player.Gang] = dict;
                dict[key] = player;
                return null;
            }

            if (!dict.ContainsKey(key))
            {
                dict[key] = player;
                return null;
            }

            if (dict[key] == player)
                return null;

            return string.Format(player.Language.PLAYER_ALREADY_ON_THIS_PAGE, dict[key].DisplayName);
        }

        public object? CloseOnlyOneEditorPage(ITDSPlayer player, GangWindowOnlyOneEditorPage key)
        {
            if (!_playerInWindow.TryGetValue(player.Gang, out Dictionary<GangWindowOnlyOneEditorPage, ITDSPlayer>? dict))
                return null;

            if (!dict.ContainsValue(player))
                return null;

            dict.Remove(key);

            if (dict.Count == 0)
                _playerInWindow.Remove(player.Gang);

            return null;
        }
    }
}
