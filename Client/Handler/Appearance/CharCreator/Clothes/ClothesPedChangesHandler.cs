using System;
using System.Collections.Generic;
using TDS.Shared.Data.Enums.CharCreator;

namespace TDS.Client.Handler.Appearance.CharCreator.Clothes
{
    internal class ClothesPedChangesHandler
    {
        private readonly CharCreatorPedHandler _pedHandler;
        private readonly ClothesDataHandler _clothesDataHandler;

        public ClothesPedChangesHandler(CharCreatorPedHandler pedHandler, ClothesDataHandler clothesDataHandler)
            => (_pedHandler, _clothesDataHandler) = (pedHandler, clothesDataHandler);

        public void DataChanged(ClothesDataKey key, int drawableId, int textureId)
        {
            if (key == ClothesDataKey.Slot)
            {
                _pedHandler.Ped.SetClothesData(_clothesDataHandler.Data);
                return;
            }

            if (key.TryGetComponentId(out int componentId))
            {
                var paletteId = _pedHandler.Ped.GetPaletteVariation(componentId);
                _pedHandler.Ped.SetComponentVariation(componentId, drawableId, textureId, paletteId);
            }
            else if (key.TryGetPropId(out int propId))
            {
                if (drawableId != -1)
                    _pedHandler.Ped.SetPropIndex(propId, drawableId, textureId, true);
                else
                    _pedHandler.Ped.ClearProp(propId);
            }
        }
    }
}