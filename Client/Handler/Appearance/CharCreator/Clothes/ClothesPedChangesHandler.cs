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
                _pedHandler.Ped.SetComponent(componentId, drawableId, textureId);
            else if (key.TryGetPropId(out int propId))
                _pedHandler.Ped.SetProp(propId, drawableId, textureId);
        }
    }
}