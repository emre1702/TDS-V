using TDS_Client.Instance.MapCreator;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Event;
using TDS_Client.Manager.MapCreator;
using TDS_Client.Manager.Utility;
using TDS_Common.Dto;
using TDS_Common.Enum;
using PlayerElement = RAGE.Elements.Player;

namespace TDS_Client.Manager.Lobby
{
    internal static class Lobby
    {
        public static bool IsLobbyOwner
        {
            get => _isLobbyOwner;
            set
            {
                if (_isLobbyOwner != value)
                {
                    Browser.Angular.Main.SyncIsLobbyOwner(value);
                }
                _isLobbyOwner = value;
            }
        }

        public static bool InFightLobby
        {
            get => _inFightLobby;
            set
            {
                _inFightLobby = value;
                Browser.Angular.Main.SyncInFightLobby(value);
                Browser.Angular.Main.ToggleHUD(_inFightLobby);
                Damagesys.ResetLastHP();
            }
        }

        private static ELobbyType? _inLobbyType;
        private static bool _isLobbyOwner;
        private static bool _inFightLobby;

        public static void Joined(SyncedLobbySettingsDto oldSettings, SyncedLobbySettingsDto settings)
        {
            InstructionalButtonManager.Reset();
            PlayerElement.LocalPlayer.ResetAlpha();

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
            CustomEventManager.SetLobbyLeave(oldSettings);

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
                case ELobbyType.GangwarLobby:
                    InFightLobby = true;
                    break;
            }
            CustomEventManager.SetLobbyJoin(settings);

            _inLobbyType = settings.Type;
            LoadLobbyGeneralBinds();
        }

        private static void LoadLobbyGeneralBinds()
        {
            if (Settings.InLobbyWithMaps)
                InstructionalButtonManager.Add("Map-Voting", "F3");
        }

        private static void Left()
        {
            RoundInfo.Stop();
            Round.InFight = false;
            Bomb.Stop();
            Round.Reset(true);
            LobbyCam.StopCountdown();
            MapManager.CloseMenu();
            Ranking.Stop();
            Browser.Angular.Main.ResetMapVoting();
        }

        private static void JoinedMainmenu()
        {
            Death.PlayerSpawn();
            RAGE.Game.Cam.DoScreenFadeIn(100);
            Browser.Angular.Main.ToggleLobbyChoiceMenu(true);
        }

        private static void JoinedMapCreator()
        {
            InFightLobby = false;
            RAGE.Game.Cam.DoScreenFadeIn(100);
            Main.Start();
        }

        private static void LeftMainMenu()
        {
            Browser.Angular.Main.ToggleLobbyChoiceMenu(false);
        }

        private static void LeftMapCreator()
        {
            Main.Stop();
            MapCreatorObject.Reset();
        }
    }
}