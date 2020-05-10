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
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler.Appearance
{
    /// <summary>
    /// Works only with Clothing_M_11_0 (shirt) and Clothing_M_11_90 (jacket).
    /// But because with jackets you can only change the 3rd index color (chest & cuffs), we'll use shirts.
    /// The shirt is getting set at serverside, here we'll only set the color.
    /// </summary>
    public class ShirtTeamColorsHandler : ServiceBase
    {
        private readonly LobbyHandler _lobbyHandler;
        private readonly DataSyncHandler _dataSyncHandler;

        public ShirtTeamColorsHandler(IModAPI modAPI, LoggingHandler loggingHandler, LobbyHandler lobbyHandler, 
            DataSyncHandler dataSyncHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            _lobbyHandler = lobbyHandler;
            _dataSyncHandler = dataSyncHandler;

            eventsHandler.Spawn += OnSpawn;

            modAPI.Event.EntityStreamIn.Add(new EventMethodData<EntityStreamInDelegate>(OnEntityStreamIn));
        }

        private void OnSpawn()
        {
            OnEntityStreamIn(ModAPI.LocalPlayer);
        }

        private void OnEntityStreamIn(IEntity entity)
        {
            if (!(entity is IPlayer player))
                return;

            var teamIndex = _dataSyncHandler.GetData(player, PlayerDataKey.TeamIndex, -1);
            if (teamIndex == 0)
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

        private Color GetColorRelativeToMe(int hisTeamIndex)
        {
            var myTeamIndex = _dataSyncHandler.GetData(ModAPI.LocalPlayer, PlayerDataKey.TeamIndex, -1);
            if (myTeamIndex == -1)
                return Color.FromArgb(255, 255, 255);

            return myTeamIndex == hisTeamIndex ? Color.Green : Color.DarkRed;
        }
    }
}
