using TDS_Client.Enum;
using TDS_Client.Manager.Browser;
using TDS_Common.Dto;

namespace TDS_Client.Manager.Lobby
{
    internal static class Lobby
    {
        public static bool InFightLobby
        {
            get => _inFightLobby;
            set
            {
                _inFightLobby = value;
                Angular.SyncInFightLobby(value);
            }
        }

        private static EDefaultLobby _inDefaultLobby = EDefaultLobby.None;
        private static bool _inFightLobby;

        public static void Joined(SyncedLobbySettingsDto settings)
        {
            switch (_inDefaultLobby)
            {
                case EDefaultLobby.MainMenu:
                    LeftMainMenu();
                    break;
                case EDefaultLobby.MapCreator:
                    LeftMapCreator();
                    break;
                default:
                    Left();
                    break;
            }

            switch (settings.Id)
            {
                case 0:
                    JoinedMainmenu();
                    break;
                //case 1:
                    //_inDefaultLobby = EDefaultLobby.GangLobby;
                    //JoinedGangLobby();
                //    break;
                default:
                    _inDefaultLobby = EDefaultLobby.None;
                    InFightLobby = true;
                    break;
            }
        }

        private static void Left()
        {
            RoundInfo.Stop();
            Round.InFight = false;
            Bomb.Reset();
            Round.Reset(true);
            LobbyCam.StopCountdown();
            MapManager.CloseMenu();
            Angular.ResetMapVoting();
        }

        private static void JoinedMainmenu()
        {
            _inDefaultLobby = EDefaultLobby.MainMenu;
            InFightLobby = false;
            RAGE.Game.Cam.DoScreenFadeIn(100);
            Choice.Start();
        }

        public static void JoinedMapCreator()
        {
            _inDefaultLobby = EDefaultLobby.MapCreator;
            InFightLobby = false;
            RAGE.Game.Cam.DoScreenFadeIn(100);
            Angular.ToggleMapCreator(true);
            Angular.ToggleFreeroam(true);
        }

        private static void LeftMainMenu()
        {
            Choice.Stop();
        }

        private static void LeftMapCreator()
        {
            Angular.ToggleMapCreator(false);
            Angular.ToggleFreeroam(false);
        }
    }
}