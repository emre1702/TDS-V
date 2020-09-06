using TDS_Client.Data.Defaults;
using TDS_Client.Handler.Events;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyChoiceHandler
    {
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;

        public LobbyChoiceHandler(RemoteEventsSender remoteEventsSender, SettingsHandler settingsHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _settingsHandler = settingsHandler;

            RAGE.Events.Add(FromBrowserEvent.ChooseArenaToJoin, JoinArena);
            RAGE.Events.Add(FromBrowserEvent.ChooseCharCreatorToJoin, JoinCharCreator);
            RAGE.Events.Add(FromBrowserEvent.ChooseMapCreatorToJoin, JoinMapCreator);
            RAGE.Events.Add(FromBrowserEvent.ChooseGangLobbyToJoin, JoinGangLobby);
        }

        public void JoinArena(object[] args)
        {
            _remoteEventsSender.SendFromBrowser(ToServerEvent.JoinLobby, _settingsHandler.ArenaLobbyId);
        }

        public void JoinCharCreator(object[] args)
        {
            _remoteEventsSender.SendFromBrowser(ToServerEvent.JoinLobby, _settingsHandler.CharCreatorLobbyId);
        }

        public void JoinMapCreator(object[] args)
        {
            _remoteEventsSender.SendFromBrowser(ToServerEvent.JoinLobby, _settingsHandler.MapCreatorLobbyId);
        }

        public void JoinGangLobby(object[] args)
        {
            _remoteEventsSender.SendFromBrowser(ToServerEvent.JoinLobby, _settingsHandler.GangLobbyLobbyId);
        }
    }
}
