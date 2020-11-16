using RAGE.Elements;
using System;
using System.Drawing;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Models;
using TDS.Client.Handler.Events;
using TDS.Client.Handler.Lobby;
using TDS.Client.Handler.Sync;
using TDS.Shared.Data.Enums;

namespace TDS.Client.Handler.Appearance
{
    /// <summary>
    /// Works only with Clothing_M_11_0 (shirt) and Clothing_M_11_90 (jacket). But because with
    /// jackets you can only change the 3rd index color (chest &amp; cuffs), we'll use shirts. The
    /// shirt is getting set at serverside, here we'll only set the color.
    /// </summary>
    public class ShirtTeamColorsHandler : ServiceBase
    {
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly LobbyHandler _lobbyHandler;

        public ShirtTeamColorsHandler(LoggingHandler loggingHandler, LobbyHandler lobbyHandler,
            DataSyncHandler dataSyncHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            _lobbyHandler = lobbyHandler;
            _dataSyncHandler = dataSyncHandler;

            eventsHandler.Spawn += OnSpawn;
            eventsHandler.DataChanged += EventsHandler_DataChanged;

            RAGE.Events.OnEntityStreamIn += OnEntityStreamIn;
        }

        private void EventsHandler_DataChanged(RAGE.Elements.Player player, PlayerDataKey key, object data)
        {
            if (key == PlayerDataKey.TeamIndex)
                OnEntityStreamIn(player);
        }

        private Color GetColorRelativeToMe(int hisTeamIndex)
        {
            try
            {
                var myTeamIndex = _dataSyncHandler.GetData(Player.LocalPlayer as ITDSPlayer, PlayerDataKey.TeamIndex, -1);
                if (myTeamIndex == -1)
                    return Color.FromArgb(255, 255, 255);

                return myTeamIndex == hisTeamIndex ? Color.Green : Color.DarkRed;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
                return Color.White;
            }
        }

        private void OnEntityStreamIn(Entity entity)
        {
            try
            {
                if (!(entity is ITDSPlayer player))
                    return;

                var teamIndex = _dataSyncHandler.GetData(player, PlayerDataKey.TeamIndex, -1);
                if (teamIndex == -1)
                    return;

                if (_lobbyHandler.Teams.LobbyTeams is null)
                    return;
                var team = _lobbyHandler.Teams.LobbyTeams.Count > teamIndex ? _lobbyHandler.Teams.LobbyTeams[teamIndex] : null;
                if (team is null)
                    return;

                var teamColorRelativeToMe = GetColorRelativeToMe(teamIndex);

                player.SetHeadBlendPaletteColor(teamColorRelativeToMe, 0);
                //player.SetHeadBlendPaletteColor(Color.FromArgb(164, 50, 168), 1);
                player.SetHeadBlendPaletteColor(teamColorRelativeToMe, 2);
                player.SetHeadBlendPaletteColor(team.Color, 3);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnSpawn()
        {
            OnEntityStreamIn(RAGE.Elements.Player.LocalPlayer);
        }
    }
}
