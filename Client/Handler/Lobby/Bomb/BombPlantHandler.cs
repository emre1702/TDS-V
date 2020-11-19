using RAGE.Game;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Events;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Lobby.Bomb
{
    internal class BombPlantHandler
    {
        private bool _gotBomb;
        private bool _isPlanting;
        private DxProgressRectangle _progressRect;

        private readonly LobbyMapDatasHandler _lobbyMapDatasHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly BombOnHandHandler _bombOnHandHandler;
        private readonly DxHandler _dxHandler;
        private readonly TimerHandler _timerHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        internal BombPlantHandler(LobbyMapDatasHandler lobbyMapDatasHandler, SettingsHandler settingsHandler, EventsHandler eventsHandler, DxHandler dxHandler,
            TimerHandler timerHandler, RemoteEventsSender remoteEventsSender, BombOnHandHandler bombOnHandHandler)
        {
            _lobbyMapDatasHandler = lobbyMapDatasHandler;
            _settingsHandler = settingsHandler;
            _bombOnHandHandler = bombOnHandHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _remoteEventsSender = remoteEventsSender;

            eventsHandler.LobbyLeft += _ => StopHavingBomb();
            eventsHandler.RoundEnded += _ => StopHavingBomb();

            Add(ToClientEvent.PlayerGotBomb, _ => SetHasBomb(true));
            Add(ToClientEvent.PlayerPlantedBomb, _ => StopHavingBomb());
            Add(ToClientEvent.StopBombPlantDefuse, _ => StopPlanting(false));
        }

        private void CheckPlantingOnTick(List<TickNametagData> _)
        {
            if (!IsAbleToPlant())
            {
                if (_isPlanting)
                    StopPlanting();
                return;
            }

            if (!_isPlanting)
                StartPlanting();
        }

        private void StartPlanting()
        {
            if (_isPlanting == true)
                return;
            _isPlanting = true;

            _progressRect = new DxProgressRectangle(_dxHandler, _timerHandler, _settingsHandler.Language.PLANTING, 0.5f, 0.71f, 0.2f, 0.08f,
                Color.White, Color.Black, Color.ForestGreen,
                textScale: 0.7f, alignmentX: RAGE.NUI.UIResText.Alignment.Centered, alignmentY: AlignmentY.Center, frontPriority: 900);
            var plantTime = _settingsHandler.GetSyncedLobbySettings().BombPlantTimeMs ?? 2000;
            _progressRect.SetAutomatic(plantTime);
            _remoteEventsSender.Send(ToServerEvent.StartPlanting);
        }

        private void StopPlanting(bool triggerToServer = true)
        {
            if (!_isPlanting)
                return;

            _isPlanting = false;
            _progressRect?.Remove();
            _progressRect = null;

            if (triggerToServer)
                _remoteEventsSender.Send(ToServerEvent.StopPlanting);
        }

        private void StopHavingBomb()
        {
            SetHasBomb(false);
            StopPlanting();
            _bombOnHandHandler.SetBombNotOnHand();
        }

        private void SetHasBomb(bool hasBomb)
        {
            if (_gotBomb == hasBomb)
                return;
            _gotBomb = hasBomb;
            ToggleCheckPlantingOnTick(hasBomb);
        }

        private void ToggleCheckPlantingOnTick(bool toggle)
        {
            Tick -= CheckPlantingOnTick;

            if (toggle)
                Tick += CheckPlantingOnTick;
        }

        private bool IsAbleToPlant()
        {
            var weaponHash = RAGE.Elements.Player.LocalPlayer.GetSelectedWeapon();
            if (weaponHash != (uint)WeaponHash.Unarmed)
                return false;

            if (!Pad.IsDisabledControlPressed((int)InputGroup.MOVE, (int)Control.Attack)
                && !Pad.IsControlPressed((int)InputGroup.MOVE, (int)Control.Attack))
                return false;

            if (RAGE.Elements.Player.LocalPlayer.IsDeadOrDying(true))
                return false;

            if (!IsOnPlantSpot())
                return false;

            return true;
        }

        private bool IsOnPlantSpot()
        {
            if (_lobbyMapDatasHandler.MapDatas is null || _lobbyMapDatasHandler.MapDatas.BombPlaces is null || _lobbyMapDatasHandler.MapDatas.BombPlaces.Count == 0)
                return false;
            var playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            foreach (var pos in _lobbyMapDatasHandler.MapDatas.BombPlaces)
            {
                if (Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, pos.X, pos.Y, pos.Z, pos.Z != 0) <= _settingsHandler.DistanceToSpotToPlant)
                    return true;
            }
            return false;
        }
    }
}
