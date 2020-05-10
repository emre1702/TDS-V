using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.CharCreator;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Appearance
{
    public class CharCreatorHandler : ServiceBase
    {
        private IPed _displayPed;
        private uint _dimension;
        private Position2D _initMovePedCursorPos;
        private float _initMovingOffsetZ;
        private float _initMovingAngle;
        private Position3D _currentCamOffsetPos;
        private float _currentCamAngle;

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly IModAPI _modAPI;
        private readonly BrowserHandler _browserHandler;
        private readonly Serializer _serializer;
        private readonly DeathHandler _deathHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly UtilsHandler _utilsHandler;

        public CharCreatorHandler(IModAPI modAPI, LoggingHandler loggingHandler, BrowserHandler browserHandler, Serializer serializer, DeathHandler deathHandler,
            CamerasHandler camerasHandler, EventsHandler eventsHandler, CursorHandler cursorHandler, UtilsHandler utilsHandler)
            : base(modAPI, loggingHandler)
        {
            _modAPI = modAPI;
            _browserHandler = browserHandler;
            _serializer = serializer;
            _deathHandler = deathHandler;
            _camerasHandler = camerasHandler;
            _eventsHandler = eventsHandler;
            _cursorHandler = cursorHandler;
            _utilsHandler = utilsHandler;

            _tickEventMethod = new EventMethodData<TickDelegate>(MovePed);

            ModAPI.Event.Add(ToClientEvent.StartCharCreator, Start);
            ModAPI.Event.Add(FromBrowserEvent.CharCreatorDataChanged, CharCreatorDataChanged);
        }

        public void Start(object[] args)
        {
            string json = (string)args[0];
            _dimension = Convert.ToUInt32(args[1]);
            var data = _serializer.FromServer<CharCreateData>(json);
            _browserHandler.Angular.ToggleCharCreator(true, json);
            ModAPI.Chat.Show(false);
            ModAPI.LocalPlayer.Alpha = 0;
            _cursorHandler.Visible = true;

            _eventsHandler.LobbyLeft += Stop;

            PreparePed(data);

            new TDSTimer(PrepareCamera, 1000);

            _modAPI.Event.Tick.Add(_tickEventMethod);
        }

        public void Stop(SyncedLobbySettings settings)
        {
            _eventsHandler.LobbyLeft -= Stop;
            _modAPI.Event.Tick.Remove(_tickEventMethod);

            ModAPI.LocalPlayer.Alpha = 255;
            _browserHandler.Angular.ToggleCharCreator(false);
            ModAPI.Chat.Show(true);
            _cursorHandler.Visible = false;

            _camerasHandler.BetweenRoundsCam.Deactivate(true);

            if (!(_displayPed is null))
                _displayPed?.Destroy();
            _currentCamOffsetPos = null;
        }

        private void PreparePed(CharCreateData data)
        {
            var skin = data.GeneralDataSynced.IsMale ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;
            var pos = new Position3D(-425.48, 1123.55, 325.85);

            if (!(_displayPed is null))
                _displayPed.Destroy();

            _displayPed = ModAPI.Ped.Create(skin, pos, 345, _dimension);

            //Todo Give him player outfits

            UpdateHeritage(data.HeritageDataSynced);

            UpdateFaceFeature(0, data.FeaturesDataSynced.NoseWidth);
            UpdateFaceFeature(1, data.FeaturesDataSynced.NoseHeight);
            UpdateFaceFeature(2, data.FeaturesDataSynced.NoseLength);
            UpdateFaceFeature(3, data.FeaturesDataSynced.NoseBridge);
            UpdateFaceFeature(4, data.FeaturesDataSynced.NoseTip);
            UpdateFaceFeature(5, data.FeaturesDataSynced.NoseBridgeShift);
            UpdateFaceFeature(6, data.FeaturesDataSynced.BrowHeight);
            UpdateFaceFeature(7, data.FeaturesDataSynced.BrowWidth);
            UpdateFaceFeature(8, data.FeaturesDataSynced.CheekboneHeight);
            UpdateFaceFeature(9, data.FeaturesDataSynced.CheekboneWidth);
            UpdateFaceFeature(10, data.FeaturesDataSynced.CheeksWidth);
            UpdateFaceFeature(11, data.FeaturesDataSynced.Eyes);
            UpdateFaceFeature(12, data.FeaturesDataSynced.Lips);
            UpdateFaceFeature(13, data.FeaturesDataSynced.JawWidth);
            UpdateFaceFeature(14, data.FeaturesDataSynced.JawHeight);
            UpdateFaceFeature(15, data.FeaturesDataSynced.ChinLength);
            UpdateFaceFeature(16, data.FeaturesDataSynced.ChinPosition);
            UpdateFaceFeature(17, data.FeaturesDataSynced.ChinWidth);
            UpdateFaceFeature(18, data.FeaturesDataSynced.ChinShape);
            UpdateFaceFeature(19, data.FeaturesDataSynced.NeckWidth);

            UpdateAppearance(0, data.AppearanceDataSynced.Blemishes, data.AppearanceDataSynced.BlemishesOpacity);
            UpdateAppearance(1, data.AppearanceDataSynced.FacialHair, data.AppearanceDataSynced.FacialHairOpacity);
            UpdateAppearance(2, data.AppearanceDataSynced.Eyebrows, data.AppearanceDataSynced.EyebrowsOpacity);
            UpdateAppearance(3, data.AppearanceDataSynced.Ageing, data.AppearanceDataSynced.AgeingOpacity);
            UpdateAppearance(4, data.AppearanceDataSynced.Makeup, data.AppearanceDataSynced.MakeupOpacity);
            UpdateAppearance(5, data.AppearanceDataSynced.Blush, data.AppearanceDataSynced.BlushOpacity);
            UpdateAppearance(6, data.AppearanceDataSynced.Complexion, data.AppearanceDataSynced.ComplexionOpacity);
            UpdateAppearance(7, data.AppearanceDataSynced.SunDamage, data.AppearanceDataSynced.SunDamageOpacity);
            UpdateAppearance(8, data.AppearanceDataSynced.Lipstick, data.AppearanceDataSynced.LipstickOpacity);
            UpdateAppearance(9, data.AppearanceDataSynced.MolesAndFreckles, data.AppearanceDataSynced.MolesAndFrecklesOpacity);
            UpdateAppearance(10, data.AppearanceDataSynced.ChestHair, data.AppearanceDataSynced.ChestHairOpacity);
            UpdateAppearance(11, data.AppearanceDataSynced.BodyBlemishes, data.AppearanceDataSynced.BodyBlemishesOpacity);
            UpdateAppearance(12, data.AppearanceDataSynced.AddBodyBlemishes, data.AppearanceDataSynced.AddBodyBlemishesOpacity);

            UpdateHair(data.HairAndColorsDataSynced.Hair);
            UpdateHairColor(data.HairAndColorsDataSynced.HairColor, data.HairAndColorsDataSynced.HairHighlightColor);
            UpdateEyeColor(data.HairAndColorsDataSynced.EyeColor);
            UpdateColor(1, 1, data.HairAndColorsDataSynced.FacialHairColor);
            UpdateColor(2, 1, data.HairAndColorsDataSynced.EyebrowColor);
            UpdateColor(5, 2, data.HairAndColorsDataSynced.BlushColor);
            UpdateColor(8, 2, data.HairAndColorsDataSynced.LipstickColor);
            UpdateColor(10, 1, data.HairAndColorsDataSynced.ChestHairColor);
        }

        private void PrepareCamera()
        {
            _deathHandler.PlayerSpawn();
            _initMovePedCursorPos = null;
            ModAPI.Cam.DoScreenFadeIn(200);

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

        private void MovePed(int currentMs)
        {
            if (_displayPed is null)
                return;
            if (_currentCamOffsetPos is null)
                return;

            if (!_modAPI.Input.IsDown(Key.RightButton))
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

        private void ApplyAngle(Position3D pos, float distance, float angle)
        {
            pos.X = (float)Math.Cos(angle * Math.PI / 180) * distance;
            pos.Y = (float)Math.Sin(angle * Math.PI / 180) * distance;
        }

        private void UpdateHeritage(CharCreateHeritageData data)
        {
            _displayPed.SetHeadBlendData(
                data.MotherIndex, data.FatherIndex, 0,
                data.MotherIndex, data.FatherIndex, 0,
                data.ResemblancePercentage, data.SkinTonePercentage, 0,
                false);
        }

        private void UpdateFaceFeature(int index, float scale)
        {
            _displayPed.SetFaceFeature(index, scale);
        }

        private void UpdateAppearance(int overlayId, int index, float opacity)
        {
            index = index == 0 ? 255 : index - 1;
            _displayPed.SetHeadOverlay(overlayId, index, opacity);
        }

        private void UpdateHair(int id)
        {
            _displayPed.SetComponentVariation(2, id, 0, 2);
        }

        private void UpdateHairColor(int hairColor, int hairHighlightColor)
        {
            _displayPed.SetHairColor(hairColor, hairHighlightColor);
        }

        private void UpdateEyeColor(int index)
        {
            _displayPed.SetEyeColor(index);
        }

        private void UpdateColor(int overlayId, int colorType, int colorId)
        {
            _displayPed.SetHeadOverlayColor(overlayId, colorType, colorId, 0);
        }

        private void CharCreatorDataChanged(object[] args)
        {
            if (_displayPed is null)
                return;

            var key = (CharCreatorDataKey)Convert.ToInt32(args[0]);

            switch (key)
            {
                case CharCreatorDataKey.IsMale:
                    PreparePed(_serializer.FromBrowser<CharCreateData>((string)args[1]));
                    new TDSTimer(PrepareCamera, 1000);
                    break;

                case CharCreatorDataKey.Heritage:
                    var heritageData = _serializer.FromBrowser<CharCreateHeritageData>((string)args[1]);
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
    }
}
