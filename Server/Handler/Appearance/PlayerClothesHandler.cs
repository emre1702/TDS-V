using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Player.Character.Clothes;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums.CharCreator;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Appearance
{
    public class PlayerClothesHandler
    {
        private readonly ISettingsHandler _settingsHandler;

        public PlayerClothesHandler(RemoteBrowserEventsHandler remoteBrowserEventsHandler, EventsHandler eventsHandler, ISettingsHandler settingsHandler)
        {
            remoteBrowserEventsHandler.AddAsyncEvent(ToServerEvent.SaveClothesData, Save);

            _settingsHandler = settingsHandler;

            eventsHandler.PlayerRegisteredBefore += InitPlayerClothes;
            eventsHandler.PlayerJoinedLobby += LoadPlayerClothes;

            remoteBrowserEventsHandler.AddAsyncEvent(ToServerEvent.SaveBodyData, Save);
        }

        private async Task<object?> Save(ITDSPlayer player, ArraySegment<object> args)
        {
            try
            {
                if (player.Entity is null)
                    return "ErrorInfo";
                var newConfig = Serializer.FromBrowser<PlayerClothesDatas>((string)args[0]);

                var msg = await player.Database.ExecuteForDBAsyncUnsafe(async dbContext =>
                {
                    if (player.Entity.ClothesDatas is not { } oldConfig)
                    {
                        LoggingHandler.Instance.LogError("ClothesData should not be null! Is it maybe not loaded? Or is it really null?!", Environment.StackTrace, source: player);
                        return "ErrorInfo";
                    }

                    dbContext.Entry(oldConfig).State = EntityState.Detached;
                    TakeIds(newConfig, oldConfig!);
                    player.Entity.ClothesDatas = newConfig;
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);

                    return null;
                }).ConfigureAwait(false);

                if (msg is { })
                    return msg;

                return "";
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex, player);
                return "ErrorInfo";
            }
        }

        public async ValueTask InitPlayerClothes((ITDSPlayer player, Players dbPlayer) args)
        {
            var amountSlots = _settingsHandler.ServerSettings.AmountCharSlots;
            var clothesDatas = new PlayerClothesDatas { PlayerId = args.dbPlayer.Id, SelectedSlot = 0, DatasPerSlot = new List<PlayerClothesData>() };
            args.dbPlayer.ClothesDatas = clothesDatas;

            for (byte i = 0; i < amountSlots; ++i)
            {
                AddClothesSlot(clothesDatas, i);
            }

            await args.player.Database.ExecuteForDBAsyncUnsafe(async dbContext =>
            {
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private async void LoadPlayerClothes(ITDSPlayer player, IBaseLobby lobby)
        {
            if (player.Entity?.ClothesDatas is null)
                return;

            await Task.Yield();
            var data = player.Entity.ClothesDatas;
            while (data.DatasPerSlot.Count < _settingsHandler.ServerSettings.AmountCharSlots)
            {
                AddClothesSlot(data, (byte)(data.DatasPerSlot.Count - 1));
            }
            while (data.DatasPerSlot.Count > _settingsHandler.ServerSettings.AmountCharSlots)
            {
                data.DatasPerSlot.Remove(data.DatasPerSlot.Last());
            }
            data.SelectedSlot = Math.Min(data.SelectedSlot, (byte)(data.DatasPerSlot.Count - 1));

            var currentData = data.DatasPerSlot.First(d => d.Slot == data.SelectedSlot);
            var clothes = GetClothesDictionary(currentData);

            NAPI.Task.RunSafe(() =>
            {
                if (lobby is IArena || lobby is IGangActionLobby)
                {
                    player.SetClothes(11, 0, 0);
                    clothes.Remove(11);
                }

                player.SetClothes(clothes);
                SetAccessory(player, 0, currentData.Hat);
                SetAccessory(player, 1, currentData.Glasses);
                SetAccessory(player, 2, currentData.EarAccessory);
                SetAccessory(player, 6, currentData.Watch);
                SetAccessory(player, 7, currentData.Bracelet);
            });
        }

        private void SetAccessory(ITDSPlayer player, int slot, PlayerClothesComponentOrPropData data)
        {
            if (data.DrawableId != 0)
                player.SetAccessories(slot, data.DrawableId, data.TextureId);
            else
                player.ClearAccessory(slot);
        }

        private void AddClothesSlot(PlayerClothesDatas datas, byte slot)
        {
            var data = new PlayerClothesData
            {
                Accessory = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Accessories, Slot = slot, DrawableId = 0, TextureId = -1 },
                Bag = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Bags, Slot = slot, DrawableId = 0, TextureId = -1 },
                BodyArmor = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.BodyArmors, Slot = slot, DrawableId = 0, TextureId = -1 },
                Bracelet = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Bracelets, Slot = slot, DrawableId = 0, TextureId = -1 },
                Decal = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Decals, Slot = slot, DrawableId = 0, TextureId = -1 },
                EarAccessory = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.EarAccessories, Slot = slot, DrawableId = 0, TextureId = -1 },
                Glasses = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Glasses, Slot = slot, DrawableId = 0, TextureId = -1 },
                Hands = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Hands, Slot = slot, DrawableId = 0, TextureId = -1 },
                Hat = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Hats, Slot = slot, DrawableId = 0, TextureId = -1 },
                Jacket = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Jackets, Slot = slot, DrawableId = 0, TextureId = -1 },
                Legs = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Legs, Slot = slot, DrawableId = 0, TextureId = -1 },
                Mask = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Masks, Slot = slot, DrawableId = 0, TextureId = -1 },
                Shirt = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Shirts, Slot = slot, DrawableId = 0, TextureId = -1 },
                Shoes = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Shoes, Slot = slot, DrawableId = 0, TextureId = -1 },
                Watch = new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Watches, Slot = slot, DrawableId = 0, TextureId = -1 },
            };
            datas.DatasPerSlot.Add(data);
        }

        private Dictionary<int, ComponentVariation> GetClothesDictionary(PlayerClothesData currentData)
        {
            return new Dictionary<int, ComponentVariation>
            {
                [1] = currentData.Mask.ToComponentVariation(),
                [3] = currentData.Hands.ToComponentVariation(),
                [4] = currentData.Legs.ToComponentVariation(),
                [5] = currentData.Bag.ToComponentVariation(),
                [6] = currentData.Shoes.ToComponentVariation(),
                [7] = currentData.Accessory.ToComponentVariation(),
                [8] = currentData.Shirt.ToComponentVariation(),
                [9] = currentData.BodyArmor.ToComponentVariation(),
                [10] = currentData.Decal.ToComponentVariation(),
                [11] = currentData.Jacket.ToComponentVariation(),
            };
        }

        private void TakeIds(PlayerClothesDatas to, PlayerClothesDatas from)
        {
            to.PlayerId = from.PlayerId;
            foreach (var data in to.DatasPerSlot)
            {
                var slot = data.Slot;
                var dataFrom = from.DatasPerSlot.First(d => d.Slot == slot);
                data.PlayerId = dataFrom.PlayerId;
                data.Slot = slot;

                data.Accessory.Slot = slot;
                data.Accessory.PlayerId = dataFrom.PlayerId;
                data.Accessory.Key = ClothesDataKey.Accessories;

                data.Bag.Slot = slot;
                data.Bag.PlayerId = dataFrom.PlayerId;
                data.Bag.Key = ClothesDataKey.Bags;

                data.BodyArmor.Slot = slot;
                data.BodyArmor.PlayerId = dataFrom.PlayerId;
                data.BodyArmor.Key = ClothesDataKey.BodyArmors;

                data.Bracelet.Slot = slot;
                data.Bracelet.PlayerId = dataFrom.PlayerId;
                data.Bracelet.Key = ClothesDataKey.Bracelets;

                data.Decal.Slot = slot;
                data.Decal.PlayerId = dataFrom.PlayerId;
                data.Decal.Key = ClothesDataKey.Decals;

                data.EarAccessory.Slot = slot;
                data.EarAccessory.PlayerId = dataFrom.PlayerId;
                data.EarAccessory.Key = ClothesDataKey.EarAccessories;

                data.Glasses.Slot = slot;
                data.Glasses.PlayerId = dataFrom.PlayerId;
                data.Glasses.Key = ClothesDataKey.Glasses;

                data.Hands.Slot = slot;
                data.Hands.PlayerId = dataFrom.PlayerId;
                data.Hands.Key = ClothesDataKey.Hands;

                data.Hat.Slot = slot;
                data.Hat.PlayerId = dataFrom.PlayerId;
                data.Hat.Key = ClothesDataKey.Hats;

                data.Jacket.Slot = slot;
                data.Jacket.PlayerId = dataFrom.PlayerId;
                data.Jacket.Key = ClothesDataKey.Jackets;

                data.Legs.Slot = slot;
                data.Legs.PlayerId = dataFrom.PlayerId;
                data.Legs.Key = ClothesDataKey.Legs;

                data.Mask.Slot = slot;
                data.Mask.PlayerId = dataFrom.PlayerId;
                data.Mask.Key = ClothesDataKey.Masks;

                data.Shirt.Slot = slot;
                data.Shirt.PlayerId = dataFrom.PlayerId;
                data.Shirt.Key = ClothesDataKey.Shirts;

                data.Shoes.Slot = slot;
                data.Shoes.PlayerId = dataFrom.PlayerId;
                data.Shoes.Key = ClothesDataKey.Shoes;

                data.Watch.Slot = slot;
                data.Watch.PlayerId = dataFrom.PlayerId;
                data.Watch.Key = ClothesDataKey.Watches;
            }
        }
    }
}