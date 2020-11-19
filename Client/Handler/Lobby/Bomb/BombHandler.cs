using RAGE.Game;
using System;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Events;
using TDS.Client.Handler.Lobby.Bomb;
using TDS.Shared.Core;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Lobby
{
    public class BombHandler : ServiceBase
    {
        public bool BombOnHand => _bombOnHandHandler.BombOnHand;

        private bool _bombPlanted = false;

        private readonly BrowserHandler _browserHandler;
        private readonly RoundInfosHandler _roundInfosHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly BombOnHandHandler _bombOnHandHandler;
        private readonly BombDefuseHandler _bombDefuseHandler;

        public BombHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, RoundInfosHandler roundInfosHandler, SettingsHandler settingsHandler,
            UtilsHandler utilsHandler, RemoteEventsSender remoteEventsSender, DxHandler dxHandler, TimerHandler timerHandler, EventsHandler eventsHandler,
            LobbyMapDatasHandler lobbyMapDatasHandler) : base(loggingHandler)
        {
            _browserHandler = browserHandler;
            _roundInfosHandler = roundInfosHandler;
            _settingsHandler = settingsHandler;
            _utilsHandler = utilsHandler;
            _bombOnHandHandler = new BombOnHandHandler(eventsHandler);
            new BombPlantHandler(lobbyMapDatasHandler, settingsHandler, eventsHandler, dxHandler, timerHandler, remoteEventsSender, _bombOnHandHandler);
            _bombDefuseHandler = new BombDefuseHandler(settingsHandler, eventsHandler, dxHandler, timerHandler, remoteEventsSender);

            eventsHandler.LobbyLeft += _ => Stop();
            eventsHandler.RoundEnded += _ => Stop();

            Add(ToClientEvent.BombPlanted, BombPlanted);
            Add(ToClientEvent.BombDetonated, _ => Detonate());
        }

        private void Detonate()
        {
            Cam.ShakeGameplayCam(CamShakeName.LARGE_EXPLOSION_SHAKE, 1.0f);
            new TDSTimer(() => Cam.StopGameplayCamShaking(true), 4000, 1);
            _bombPlanted = false;
            _browserHandler.PlainMain.StopBombTick();
        }

        private void BombPlanted(object[] args)
        {
            _bombPlanted = true;

            var startAtMs = args.Length > 2 ? (int?)args[2] : null;
            SetRoundTimeLeftForBombPlanted(startAtMs);

            var canDefuse = Convert.ToBoolean(args[1]);
            if (canDefuse)
            {
                var pos = Serializer.FromServer<Position3D>((string)args[0]);
                _bombDefuseHandler.SetCanDefuse(pos);
            }

            _utilsHandler.Notify(_settingsHandler.Language.BOMB_PLANTED);
        }

        private void SetRoundTimeLeftForBombPlanted(int? startAtMs = 0)
        {
            // 100 because trigger etc. propably took some time
            int time = _settingsHandler.BombDetonateTimeMs - 100;

            _roundInfosHandler.SetRoundTimeLeft(time - startAtMs.Value);
            _browserHandler.PlainMain.StartBombTick(time, startAtMs.Value);
        }

        private void Stop()
        {
            if (_bombPlanted)
            {
                _bombPlanted = false;
                _browserHandler.PlainMain.StopBombTick();
            }
        }
    }
}