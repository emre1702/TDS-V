using System;
using System.Collections.Generic;
using System.Text;
using TDS.Client.Data.Defaults;
using TDS.Client.Handler.Browser;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums.CharCreator;
using TDS.Shared.Data.Models.CharCreator.Clothes;

namespace TDS.Client.Handler.Appearance.CharCreator.Clothes
{
    internal class ClothesDataHandler
    {
        public ClothesData Data { get; private set; }

        private readonly BrowserHandler _browserHandler;

        public ClothesDataHandler(BrowserHandler browserHandler)
        {
            _browserHandler = browserHandler;

            RAGE.Events.Add(FromBrowserEvent.LoadClothesData, (_) =>
            {
                if (Data is null) return;
                browserHandler.Angular.ExecuteFast(FromBrowserEvent.LoadClothesData, Serializer.ToBrowser(Data));
            });

            RAGE.Events.Add(FromBrowserEvent.GetClothesDrawableAmount, GetDrawableAmount);
            RAGE.Events.Add(FromBrowserEvent.GetClothesTextureAmount, GetTextureAmount);
        }

        public void Start(ClothesData data)
        {
            Data = data;
        }

        public void Stop()
        {
            Data = null;
        }

        public void DataChanged(ClothesDataKey key, int drawableId, int textureId)
        {
        }

        private void GetDrawableAmount(object[] args)
        {
            var key = (ClothesDataKey)Convert.ToInt32(args[0]);
            if (key.TryGetComponentId(out var componentId))
            {
                var amount = RAGE.Elements.Player.LocalPlayer.GetNumberOfDrawableVariations(componentId);
                _browserHandler.Angular.ExecuteFast(FromBrowserEvent.GetClothesDrawableAmount, amount);
            }
            else if (key.TryGetPropId(out int propId))
            {
                var amount = RAGE.Elements.Player.LocalPlayer.GetNumberOfPropDrawableVariations(propId);
                _browserHandler.Angular.ExecuteFast(FromBrowserEvent.GetClothesDrawableAmount, amount);
            }
            else
            {
                _browserHandler.Angular.ExecuteFast(FromBrowserEvent.GetClothesDrawableAmount, 0);
            }
        }

        private void GetTextureAmount(object[] args)
        {
            var key = (ClothesDataKey)Convert.ToInt32(args[0]);
            var drawableId = Convert.ToInt32(args[1]);
            if (key.TryGetComponentId(out var componentId))
            {
                var amount = RAGE.Elements.Player.LocalPlayer.GetNumberOfTextureVariations(componentId, drawableId);
                _browserHandler.Angular.ExecuteFast(FromBrowserEvent.GetClothesTextureAmount, amount);
            }
            else if (key.TryGetPropId(out int propId))
            {
                var amount = RAGE.Elements.Player.LocalPlayer.GetNumberOfPropTextureVariations(propId, drawableId);
                _browserHandler.Angular.ExecuteFast(FromBrowserEvent.GetClothesTextureAmount, amount);
            }
            else
            {
                _browserHandler.Angular.ExecuteFast(FromBrowserEvent.GetClothesTextureAmount, 0);
            }
        }
    }
}