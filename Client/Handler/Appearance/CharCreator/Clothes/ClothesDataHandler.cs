using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDS.Client.Data.Defaults;
using TDS.Client.Handler.Browser;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums.CharCreator;
using TDS.Shared.Data.Models.CharCreator;
using TDS.Shared.Data.Models.CharCreator.Clothes;

namespace TDS.Client.Handler.Appearance.CharCreator.Clothes
{
    internal class ClothesDataHandler
    {
        public ClothesConfigs Data { get; private set; }

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

        public void Start(ClothesConfigs data)
        {
            Data = data;
        }

        public void Stop()
        {
            Data = null;
        }

        public void DataChanged(ClothesDataKey key, int drawableId, int textureId)
        {
            var currentData = Data.DatasPerSlot.First(d => d.Slot == Data.SelectedSlot);
            switch (key)
            {
                case ClothesDataKey.Accessories:
                    currentData.Accessory.DrawableId = drawableId;
                    currentData.Accessory.TextureId = textureId;
                    break;

                case ClothesDataKey.Bags:
                    currentData.Bag.DrawableId = drawableId;
                    currentData.Bag.TextureId = textureId;
                    break;

                case ClothesDataKey.BodyArmors:
                    currentData.BodyArmor.DrawableId = drawableId;
                    currentData.BodyArmor.TextureId = textureId;
                    break;

                case ClothesDataKey.Bracelets:
                    currentData.Bracelet.DrawableId = drawableId;
                    currentData.Bracelet.TextureId = textureId;
                    break;

                case ClothesDataKey.Decals:
                    currentData.Decal.DrawableId = drawableId;
                    currentData.Decal.TextureId = textureId;
                    break;

                case ClothesDataKey.EarAccessories:
                    currentData.EarAccessory.DrawableId = drawableId;
                    currentData.EarAccessory.TextureId = textureId;
                    break;

                case ClothesDataKey.Glasses:
                    currentData.Glasses.DrawableId = drawableId;
                    currentData.Glasses.TextureId = textureId;
                    break;

                case ClothesDataKey.Hands:
                    currentData.Hands.DrawableId = drawableId;
                    currentData.Hands.TextureId = textureId;
                    break;

                case ClothesDataKey.Hats:
                    currentData.Hat.DrawableId = drawableId;
                    currentData.Hat.TextureId = textureId;
                    break;

                case ClothesDataKey.Jackets:
                    currentData.Jacket.DrawableId = drawableId;
                    currentData.Jacket.TextureId = textureId;
                    break;

                case ClothesDataKey.Legs:
                    currentData.Legs.DrawableId = drawableId;
                    currentData.Legs.TextureId = textureId;
                    break;

                case ClothesDataKey.Masks:
                    currentData.Mask.DrawableId = drawableId;
                    currentData.Mask.TextureId = textureId;
                    break;

                case ClothesDataKey.Shirts:
                    currentData.Shirt.DrawableId = drawableId;
                    currentData.Shirt.TextureId = textureId;
                    break;

                case ClothesDataKey.Shoes:
                    currentData.Shoes.DrawableId = drawableId;
                    currentData.Shoes.TextureId = textureId;
                    break;

                case ClothesDataKey.Watches:
                    currentData.Watch.DrawableId = drawableId;
                    currentData.Watch.TextureId = textureId;
                    break;
            }
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