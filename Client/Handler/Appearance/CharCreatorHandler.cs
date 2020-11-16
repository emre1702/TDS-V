﻿using RAGE;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Deathmatch;
using TDS.Client.Handler.Entities.GTA;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Models.CharCreator;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Appearance
{
    public class CharCreatorHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly DeathHandler _deathHandler;
        private readonly EventsHandler _eventsHandler;

        private readonly UtilsHandler _utilsHandler;

        private float _currentCamAngle;
        private Position3D _currentCamOffsetPos;
        private uint _dimension;
        private ITDSPed _displayPed;
        private Position2D _initMovePedCursorPos;
        private float _initMovingAngle;
        private float _initMovingOffsetZ;

        public CharCreatorHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, DeathHandler deathHandler,
            CamerasHandler camerasHandler, EventsHandler eventsHandler, CursorHandler cursorHandler, UtilsHandler utilsHandler)
            : base(loggingHandler)
        {
            _browserHandler = browserHandler;

            _deathHandler = deathHandler;
            _camerasHandler = camerasHandler;
            _eventsHandler = eventsHandler;
            _cursorHandler = cursorHandler;
            _utilsHandler = utilsHandler;

            Add(ToClientEvent.StartCharCreator, Start);
            Add(FromBrowserEvent.CharCreatorDataChanged, CharCreatorDataChanged);
        }

        public void Start(object[] args)
        {
            try
            {
                string json = (string)args[0];
                _dimension = Convert.ToUInt32(args[1]);
                var data = Serializer.FromServer<CharCreateData>(json);
                _browserHandler.Angular.ToggleCharCreator(true, json);
                RAGE.Chat.Show(false);
                RAGE.Game.Ui.DisplayRadar(false);

                RAGE.Elements.Player.LocalPlayer.SetAlpha(0, true);
                _cursorHandler.Visible = true;

                _eventsHandler.LobbyLeft += Stop;

                PreparePed(data);

                new TDSTimer(PrepareCamera, 2000);
                Tick += MovePed;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void Stop(SyncedLobbySettings settings)
        {
            try
            {
                _eventsHandler.LobbyLeft -= Stop;
                Tick -= MovePed;

                RAGE.Elements.Player.LocalPlayer.SetAlpha(255, true);
                _browserHandler.Angular.ToggleCharCreator(false);
                Chat.Show(true);
                RAGE.Game.Ui.DisplayRadar(true);
                _cursorHandler.Visible = false;

                _camerasHandler.BetweenRoundsCam.Deactivate(true);

                if (!(_displayPed is null))
                    _displayPed?.Destroy();
                _currentCamOffsetPos = null;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void ApplyAngle(Position3D pos, float distance, float angle)
        {
            pos.X = (float)Math.Cos(angle * Math.PI / 180) * distance;
            pos.Y = (float)Math.Sin(angle * Math.PI / 180) * distance;
        }

        private void CharCreatorDataChanged(object[] args)
        {
            try
            {
                if (_displayPed is null)
                    return;

                var key = (CharCreatorDataKey)Convert.ToInt32(args[0]);

                switch (key)
                {
                    case CharCreatorDataKey.IsMale:
                        PreparePed(Serializer.FromBrowser<CharCreateData>((string)args[1]));
                        new TDSTimer(PrepareCamera, 1000);
                        break;

                    case CharCreatorDataKey.Heritage:
                        var heritageData = Serializer.FromBrowser<CharCreateHeritageData>((string)args[1]);
                        UpdateHeritage(heritageData);
                        break;

                    case CharCreatorDataKey.Feature:
                        UpdateFaceFeature(Convert.ToInt32(args[1]), Convert.ToSingle(args[2]));
                        break;

                    case CharCreatorDataKey.Appearance:
                        UpdateAppearance(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]), Convert.ToSingle(args[3]));
                        break;

                    case CharCreatorDataKey.Hair:
                        UpdateHair(Convert.ToInt32(args[1]));
                        break;

                    case CharCreatorDataKey.HairColor:
                        UpdateHairColor(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
                        break;

                    case CharCreatorDataKey.EyeColor:
                        UpdateEyeColor(Convert.ToInt32(args[1]));
                        break;

                    case CharCreatorDataKey.FacialHairColor:
                        UpdateColor(1, 1, Convert.ToInt32(args[1]));
                        break;

                    case CharCreatorDataKey.EyebrowColor:
                        UpdateColor(2, 1, Convert.ToInt32(args[1]));
                        break;

                    case CharCreatorDataKey.BlushColor:
                        UpdateColor(5, 2, Convert.ToInt32(args[1]));
                        break;

                    case CharCreatorDataKey.LipstickColor:
                        UpdateColor(8, 2, Convert.ToInt32(args[1]));
                        break;

                    case CharCreatorDataKey.ChestHairColor:
                        UpdateColor(10, 1, Convert.ToInt32(args[1]));
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void MovePed(List<TickNametagData> _)
        {
            try
            {
                if (_displayPed is null)
                    return;
                if (_currentCamOffsetPos is null)
                    return;

                if (!Input.IsDown((int)Key.RightButton))
                {
                    _initMovePedCursorPos = null;
                    return;
                }

                var cursorPos = new Position2D(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY());
                if (_initMovePedCursorPos is null)
                {
                    _initMovePedCursorPos = cursorPos;
                    _initMovingOffsetZ = _currentCamOffsetPos.Z;
                    _initMovingAngle = _currentCamAngle;
                    return;
                }

                float differenceX = cursorPos.X - _initMovePedCursorPos.X;
                float addToAngle = differenceX * 2 * 360;
                var newAngle = _initMovingAngle + addToAngle;
                while (newAngle < 0)
                    newAngle += 360;
                while (newAngle > 360)
                    newAngle -= 360;

                ApplyAngle(_currentCamOffsetPos, 0.5f, newAngle);

                float differenceY = cursorPos.Y - _initMovePedCursorPos.Y;
                // 0,5 -> 1 => 3
                float addToHeadingY = differenceY * 2 * 3;
                _currentCamOffsetPos.Z = _initMovingOffsetZ + addToHeadingY;

                var cam = _camerasHandler.BetweenRoundsCam;
                cam.LookAt(_displayPed, PedBone.SKEL_Head, _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0.15f);
                cam.Render(false, 0);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void PrepareCamera()
        {
            try
            {
                _deathHandler.PlayerSpawn();
                _initMovePedCursorPos = null;
                RAGE.Game.Cam.DoScreenFadeIn(200);

                if (_currentCamOffsetPos is null)
                {
                    _currentCamOffsetPos = new Position3D(0, 0, 0.15f);
                    _currentCamAngle = 90;
                    ApplyAngle(_currentCamOffsetPos, 0.5f, _currentCamAngle);
                }

                var cam = _camerasHandler.BetweenRoundsCam;
                cam.LookAt(_displayPed, PedBone.SKEL_Head, _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0.15f);

                cam.Activate();
                cam.Render(true, 1000);
                //cam.PointCamAtCoord(new Position3D(-425.48f, 1123.55f, 326.5171f));
                //cam.Activate();
                //cam.RenderToPosition(new Position3D(-425.3048f, 1124.125f, 326.5871f), true, 1000);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void PreparePed(CharCreateData data)
        {
            try
            {
                var generalData = data.GeneralDataSynced.First(e => e.Slot == data.Slot);
                var heritageData = data.HeritageDataSynced.First(e => e.Slot == data.Slot);
                var featuresData = data.FeaturesDataSynced.First(e => e.Slot == data.Slot);
                var appearanceData = data.AppearanceDataSynced.First(e => e.Slot == data.Slot);
                var hairAndColorsData = data.HairAndColorsDataSynced.First(e => e.Slot == data.Slot);

                var skin = generalData.IsMale ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;
                var pos = new Vector3(-425.48f, 1123.55f, 325.85f);

                if (!(_displayPed is null))
                    _displayPed.Destroy();

                _displayPed = new TDSPed((uint)skin, pos, 345, _dimension);
                Logging.LogWarning("PreparePed ped exists: " + (!(_displayPed is null)));
                new TDSTimer(() => Logging.LogWarning("PreparePed ped exists: " + (!(_displayPed is null))), 2000, 1);

                //Todo Give him player outfits

                UpdateHeritage(heritageData);

                UpdateFaceFeature(0, featuresData.NoseWidth);
                UpdateFaceFeature(1, featuresData.NoseHeight);
                UpdateFaceFeature(2, featuresData.NoseLength);
                UpdateFaceFeature(3, featuresData.NoseBridge);
                UpdateFaceFeature(4, featuresData.NoseTip);
                UpdateFaceFeature(5, featuresData.NoseBridgeShift);
                UpdateFaceFeature(6, featuresData.BrowHeight);
                UpdateFaceFeature(7, featuresData.BrowWidth);
                UpdateFaceFeature(8, featuresData.CheekboneHeight);
                UpdateFaceFeature(9, featuresData.CheekboneWidth);
                UpdateFaceFeature(10, featuresData.CheeksWidth);
                UpdateFaceFeature(11, featuresData.Eyes);
                UpdateFaceFeature(12, featuresData.Lips);
                UpdateFaceFeature(13, featuresData.JawWidth);
                UpdateFaceFeature(14, featuresData.JawHeight);
                UpdateFaceFeature(15, featuresData.ChinLength);
                UpdateFaceFeature(16, featuresData.ChinPosition);
                UpdateFaceFeature(17, featuresData.ChinWidth);
                UpdateFaceFeature(18, featuresData.ChinShape);
                UpdateFaceFeature(19, featuresData.NeckWidth);

                UpdateAppearance(0, appearanceData.Blemishes, appearanceData.BlemishesOpacity);
                UpdateAppearance(1, appearanceData.FacialHair, appearanceData.FacialHairOpacity);
                UpdateAppearance(2, appearanceData.Eyebrows, appearanceData.EyebrowsOpacity);
                UpdateAppearance(3, appearanceData.Ageing, appearanceData.AgeingOpacity);
                UpdateAppearance(4, appearanceData.Makeup, appearanceData.MakeupOpacity);
                UpdateAppearance(5, appearanceData.Blush, appearanceData.BlushOpacity);
                UpdateAppearance(6, appearanceData.Complexion, appearanceData.ComplexionOpacity);
                UpdateAppearance(7, appearanceData.SunDamage, appearanceData.SunDamageOpacity);
                UpdateAppearance(8, appearanceData.Lipstick, appearanceData.LipstickOpacity);
                UpdateAppearance(9, appearanceData.MolesAndFreckles, appearanceData.MolesAndFrecklesOpacity);
                UpdateAppearance(10, appearanceData.ChestHair, appearanceData.ChestHairOpacity);
                UpdateAppearance(11, appearanceData.BodyBlemishes, appearanceData.BodyBlemishesOpacity);
                UpdateAppearance(12, appearanceData.AddBodyBlemishes, appearanceData.AddBodyBlemishesOpacity);

                UpdateHair(hairAndColorsData.Hair);
                UpdateHairColor(hairAndColorsData.HairColor, hairAndColorsData.HairHighlightColor);
                UpdateEyeColor(hairAndColorsData.EyeColor);
                UpdateColor(1, 1, hairAndColorsData.FacialHairColor);
                UpdateColor(2, 1, hairAndColorsData.EyebrowColor);
                UpdateColor(5, 2, hairAndColorsData.BlushColor);
                UpdateColor(8, 2, hairAndColorsData.LipstickColor);
                UpdateColor(10, 1, hairAndColorsData.ChestHairColor);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void UpdateAppearance(int overlayId, int index, float opacity)
        {
            index = index == 0 ? 255 : index - 1;
            _displayPed.SetHeadOverlay(overlayId, index, opacity);
        }

        private void UpdateColor(int overlayId, int colorType, int colorId)
        {
            _displayPed.SetHeadOverlayColor(overlayId, colorType, colorId, 0);
        }

        private void UpdateEyeColor(int index)
        {
            _displayPed.SetEyeColor(index);
        }

        private void UpdateFaceFeature(int index, float scale)
        {
            _displayPed.SetFaceFeature(index, scale);
        }

        private void UpdateHair(int id)
        {
            _displayPed.SetComponentVariation(2, id, 0, 2);
        }

        private void UpdateHairColor(int hairColor, int hairHighlightColor)
        {
            _displayPed.SetHairColor(hairColor, hairHighlightColor);
        }

        private void UpdateHeritage(CharCreateHeritageData data)
        {
            _displayPed.SetHeadBlendData(
                data.MotherIndex, data.FatherIndex, 0,
                data.MotherIndex, data.FatherIndex, 0,
                data.ResemblancePercentage, data.SkinTonePercentage, 0,
                false);
        }
    }
}
