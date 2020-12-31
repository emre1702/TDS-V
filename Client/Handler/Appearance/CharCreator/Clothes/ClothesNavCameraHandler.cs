using System;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums.CharCreator;
using TDS.Shared.Data.Enums.CharCreator;

namespace TDS.Client.Handler.Appearance.CharCreator.Clothes
{
    internal class ClothesNavCameraHandler
    {
        private readonly CharCreatorCameraHandler _cameraHandler;

        public ClothesNavCameraHandler(CharCreatorCameraHandler cameraHandler)
        {
            _cameraHandler = cameraHandler;

            RAGE.Events.Add(FromBrowserEvent.ClothesNavChanged, NavChanged);
        }

        private void NavChanged(object[] args)
        {
            var key = (ClothesDataKey)Convert.ToInt32(args[0]);
            switch (key)
            {
                case ClothesDataKey.Main:
                case ClothesDataKey.Accessories:
                case ClothesDataKey.Bags:
                    _cameraHandler.SetCameraTarget(CharCreatorCameraTarget.All);
                    break;

                case ClothesDataKey.Hats:
                case ClothesDataKey.Glasses:
                case ClothesDataKey.Masks:
                case ClothesDataKey.EarAccessories:
                    _cameraHandler.SetCameraTarget(CharCreatorCameraTarget.Head);
                    break;

                case ClothesDataKey.Jackets:
                case ClothesDataKey.Shirts:
                case ClothesDataKey.BodyArmors:
                case ClothesDataKey.Decals:
                    _cameraHandler.SetCameraTarget(CharCreatorCameraTarget.Torso);
                    break;

                case ClothesDataKey.Watches:
                case ClothesDataKey.Bracelets:
                    _cameraHandler.SetCameraTarget(CharCreatorCameraTarget.Arms);
                    break;

                case ClothesDataKey.Hands:
                    _cameraHandler.SetCameraTarget(CharCreatorCameraTarget.Hands);
                    break;

                case ClothesDataKey.Legs:
                    _cameraHandler.SetCameraTarget(CharCreatorCameraTarget.Legs);
                    break;

                case ClothesDataKey.Shoes:
                    _cameraHandler.SetCameraTarget(CharCreatorCameraTarget.Foot);
                    break;
            }
        }
    }
}