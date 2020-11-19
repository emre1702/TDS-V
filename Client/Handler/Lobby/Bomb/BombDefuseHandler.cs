using RAGE.Game;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Lobby.Bomb
{
    internal class BombDefuseHandler
    {
        private Position3D _plantedPos;
        private bool _isDefusing;
        private DxProgressRectangle _progressRect;

        private readonly SettingsHandler _settingsHandler;
        private readonly DxHandler _dxHandler;
        private readonly TimerHandler _timerHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        internal BombDefuseHandler(SettingsHandler settingsHandler, EventsHandler eventsHandler, DxHandler dxHandler,
            TimerHandler timerHandler, RemoteEventsSender remoteEventsSender)
        {
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _remoteEventsSender = remoteEventsSender;

            eventsHandler.LobbyLeft += _ => SetCanNotDefuse();
            eventsHandler.RoundEnded += _ => SetCanNotDefuse();

            Add(ToClientEvent.StopBombPlantDefuse, _ => StopDefusing(false));
            Add(ToClientEvent.BombDetonated, _ => SetCanNotDefuse());
        }

        private void CheckDefusingOnTick(List<TickNametagData> _)
        {
            if (!IsAbleToDefuse())
            {
                if (_isDefusing)
                    StopDefusing();
                return;
            }

            if (!_isDefusing)
                StartDefusing();
        }

        private void StartDefusing()
        {
            if (_isDefusing)
                return;
            _isDefusing = true;

            _progressRect = new DxProgressRectangle(_dxHandler, _timerHandler, _settingsHandler.Language.DEFUSING, 0.5f, 0.71f, 0.12f, 0.05f,
                Color.White, Color.Black, Color.ForestGreen,
                textScale: 0.7f, alignmentX: RAGE.NUI.UIResText.Alignment.Centered, alignmentY: AlignmentY.Center, frontPriority: 900);
            var defuseTime = _settingsHandler.GetSyncedLobbySettings().BombDefuseTimeMs ?? 2000;
            _progressRect.SetAutomatic(defuseTime);
            _remoteEventsSender.Send(ToServerEvent.StartDefusing);
        }

        private void StopDefusing(bool triggerToServer = true)
        {
            if (!_isDefusing)
                return;

            _isDefusing = false;
            _progressRect?.Remove();
            _progressRect = null;

            if (triggerToServer)
                _remoteEventsSender.Send(ToServerEvent.StopDefusing);
        }

        internal void SetCanDefuse(Position3D plantedPos)
        {
            _plantedPos = plantedPos;
            Tick -= CheckDefusingOnTick;
            Tick += CheckDefusingOnTick;
        }

        private void SetCanNotDefuse()
        {
            if (_plantedPos == null)
                return;
            _plantedPos = null;
            Tick -= CheckDefusingOnTick;

            if (_isDefusing)
                StopDefusing(false);
        }

        private bool IsAbleToDefuse()
        {
            if (_plantedPos is null)
                return false;

            var weaponHash = RAGE.Elements.Player.LocalPlayer.GetSelectedWeapon();
            if (weaponHash != (uint)WeaponHash.Unarmed)
                return false;

            if (!Pad.IsDisabledControlPressed((int)InputGroup.MOVE, (int)Control.Attack)
                && !Pad.IsControlPressed((int)InputGroup.MOVE, (int)Control.Attack))
                return false;

            if (RAGE.Elements.Player.LocalPlayer.IsDeadOrDying(true))
                return false;

            if (!IsOnDefuseSpot())
                return false;

            return true;
        }

        private bool IsOnDefuseSpot()
        {
            if (_plantedPos == null)
                return false;
            var playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            return Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, _plantedPos.X, _plantedPos.Y, _plantedPos.Z, true)
                <= _settingsHandler.DistanceToSpotToDefuse;
        }
    }
}
