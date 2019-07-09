using TDS_Client.Enum;
using TDS_Client.Manager.Browser;
using TDS_Common.Dto;
using TDS_Common.Enum;

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

        private static ELobbyType? _inLobbyType;
        private static bool _inFightLobby;

        public static void Joined(SyncedLobbySettingsDto settings)
        {
            if (_inLobbyType != null)
            {
                switch (_inLobbyType)
                {
                    case ELobbyType.MainMenu:
                        LeftMainMenu();
                        break;
                    case ELobbyType.MapCreateLobby:
                        LeftMapCreator();
                        break;
                    default:
                        Left();
                        break;
                }
            }

            switch (settings.Type)
            {
                case ELobbyType.MainMenu:
                    InFightLobby = false;
                    JoinedMainmenu();
                    break;
                case ELobbyType.MapCreateLobby:
                    InFightLobby = false;
                    JoinedMapCreator();
                    break;

                case ELobbyType.Arena:
                case ELobbyType.FightLobby:
                case ELobbyType.GangLobby:
                    InFightLobby = true;
                    break;
            }

            _inLobbyType = settings.Type;
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
            RAGE.Game.Cam.DoScreenFadeIn(100);
            Angular.ToggleLobbyChoiceMenu(true);
        }

        private static void JoinedMapCreator()
        {
            InFightLobby = false;
            RAGE.Game.Cam.DoScreenFadeIn(100);
            Angular.ToggleMapCreator(true);
            Angular.ToggleFreeroam(true);
        }

        private static void LeftMainMenu()
        {
            Angular.ToggleLobbyChoiceMenu(false);
        }

        private static void LeftMapCreator()
        {
            Angular.ToggleMapCreator(false);
            Angular.ToggleFreeroam(false);
        }
    }
}