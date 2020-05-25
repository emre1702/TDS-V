using System;
using System.Drawing;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Client.Handler.Sync;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Handler.Appearance
{
    /// <summary>
    /// Works only with Clothing_M_11_0 (shirt) and Clothing_M_11_90 (jacket). But because with
    /// jackets you can only change the 3rd index color (chest &amp; cuffs), we'll use shirts. The
    /// shirt is getting set at serverside, here we'll only set the color.
    /// </summary>
    public class ShirtTeamColorsHandler : ServiceBase
    {
        #region Private Fields

        private readonly DataSyncHandler _dataSyncHandler;
        private readonly LobbyHandler _lobbyHandler;

        #endregion Private Fields

        #region Public Constructors

        public ShirtTeamColorsHandler(IModAPI modAPI, LoggingHandler loggingHandler, LobbyHandler lobbyHandler,
            DataSyncHandler dataSyncHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            _lobbyHandler = lobbyHandler;
            _dataSyncHandler = dataSyncHandler;

            eventsHandler.Spawn += OnSpawn;

            modAPI.Event.EntityStreamIn.Add(new EventMethodData<EntityStreamInDelegate>(OnEntityStreamIn));
        }

        #endregion Public Constructors

        #region Private Methods

        private Color GetColorRelativeToMe(int hisTeamIndex)
        {
            try
            {
                var myTeamIndex = _dataSyncHandler.GetData(ModAPI.LocalPlayer, PlayerDataKey.TeamIndex, -1);
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

        private void OnEntityStreamIn(IEntity entity)
        {
            try
            {
                if (!(entity is IPlayer player))
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
            OnEntityStreamIn(ModAPI.LocalPlayer);
        }

        #endregion Private Methods
    }
}
