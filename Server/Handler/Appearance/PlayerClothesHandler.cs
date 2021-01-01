using GTANetworkAPI;
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
using TDS.Shared.Data.Models.CharCreator;
using TDS.Shared.Data.Models.CharCreator.Clothes;
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
            eventsHandler.PlayerSpawned += LoadPlayerClothes;            
        }

        private async Task<object?> Save(ITDSPlayer player, ArraySegment<object> args)
        {
            try
            {
                if (player.Entity is null)
                    return "ErrorInfo";
                var newConfig = Serializer.FromBrowser<ClothesConfigs>((string)args[0]);

                var msg = await player.Database.ExecuteForDBAsyncUnsafe(async dbContext =>
                {
                    if (player.Entity.ClothesDatas is not { } oldConfig)
                    {
                        LoggingHandler.Instance.LogError("ClothesData should not be null! Is it maybe not loaded? Or is it really null?!", Environment.StackTrace, source: player);
                        return "ErrorInfo";
                    }

                    TakeValues(oldConfig!, newConfig, player.Id);
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

        public ValueTask InitPlayerClothes((ITDSPlayer player, Players dbPlayer) args)
        {
            var amountSlots = _settingsHandler.ServerSettings.AmountCharSlots;
            var clothesDatas = new PlayerClothesDatas { PlayerId = args.dbPlayer.Id, SelectedSlot = 0, DatasPerSlot = new List<PlayerClothesData>() };
            args.dbPlayer.ClothesDatas = clothesDatas;

            for (byte i = 0; i < amountSlots; ++i)
            {
                AddClothesSlot(clothesDatas, i);
            }
            return default;
        }

        private async void LoadPlayerClothes(ITDSPlayer player)
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
                foreach (var compOrPropData in currentData.ComponentOrPropDatas)
                {
                    var (compOrPropId, isComp) = GetCompOrPropIdAndIsComp(compOrPropData.Key);
                    if (isComp)
                    {
                        if (compOrPropId == 11 && player.Lobby is IRoundFightLobby)
                            player.SetClothes(compOrPropId, 0, 0);
                        else
                            player.SetClothes(compOrPropId, compOrPropData.DrawableId, compOrPropData.TextureId);
                    }
                    else
                        SetAccessory(player, compOrPropId, compOrPropData);
                }
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
                PlayerId = datas.PlayerId,
                Slot = slot,
                ComponentOrPropDatas = new List<PlayerClothesComponentOrPropData> {
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Accessories, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Bags, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.BodyArmors, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Bracelets, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Decals, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.EarAccessories, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Glasses, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Hands, Slot = slot, DrawableId = 0, TextureId = 0 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Hats, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Jackets, Slot = slot, DrawableId = 0, TextureId = 0 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Legs, Slot = slot, DrawableId = 0, TextureId = 0 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Masks, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Shirts, Slot = slot, DrawableId = 0, TextureId = -1 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Shoes, Slot = slot, DrawableId = 0, TextureId = 0 },
                    new PlayerClothesComponentOrPropData { PlayerId = datas.PlayerId, Key = ClothesDataKey.Watches, Slot = slot, DrawableId = 0, TextureId = -1 }
                }
            };
            datas.DatasPerSlot.Add(data);
        }

        private Dictionary<int, ComponentVariation> GetClothesDictionary(PlayerClothesData currentData)
        {
            return new Dictionary<int, ComponentVariation>
            {
                [1] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.Masks).ToComponentVariation(),
                [3] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.Hands).ToComponentVariation(),
                [4] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.Legs).ToComponentVariation(),
                [5] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.Bags).ToComponentVariation(),
                [6] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.Shoes).ToComponentVariation(),
                [7] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.Accessories).ToComponentVariation(),
                [8] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.Shirts).ToComponentVariation(),
                [9] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.BodyArmors).ToComponentVariation(),
                [10] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.Decals).ToComponentVariation(),
                [11] = currentData.ComponentOrPropDatas.First(c => c.Key == ClothesDataKey.Jackets).ToComponentVariation(),
            };
        }

        private void TakeValues(PlayerClothesDatas to, ClothesConfigs from, int playerId)
        {
            to.SelectedSlot = from.SelectedSlot;

            foreach (var fromData in from.DatasPerSlot)
            {
                var toData = to.DatasPerSlot.First(d => d.Slot == fromData.Slot);
                TakeValues(toData, fromData);
            }
        }

        private void TakeValues(PlayerClothesData toData, ClothesData fromData)
        {
            TakeValues(toData, fromData.Accessory, ClothesDataKey.Accessories);
            TakeValues(toData, fromData.Bag, ClothesDataKey.Bags);
            TakeValues(toData, fromData.BodyArmor, ClothesDataKey.BodyArmors);
            TakeValues(toData, fromData.Bracelet, ClothesDataKey.Bracelets);
            TakeValues(toData, fromData.Decal, ClothesDataKey.Decals);
            TakeValues(toData, fromData.EarAccessory, ClothesDataKey.EarAccessories);
            TakeValues(toData, fromData.Glasses, ClothesDataKey.Glasses);
            TakeValues(toData, fromData.Hands, ClothesDataKey.Hands);
            TakeValues(toData, fromData.Hat, ClothesDataKey.Hats);
            TakeValues(toData, fromData.Jacket, ClothesDataKey.Jackets);
            TakeValues(toData, fromData.Legs, ClothesDataKey.Legs);
            TakeValues(toData, fromData.Mask, ClothesDataKey.Masks);
            TakeValues(toData, fromData.Shirt, ClothesDataKey.Shirts);
            TakeValues(toData, fromData.Shoes, ClothesDataKey.Shoes);
            TakeValues(toData, fromData.Watch, ClothesDataKey.Watches);
        }

        private void TakeValues(PlayerClothesData toData, ClothesComponentOrPropData from, ClothesDataKey key)
        {
            var data = toData.ComponentOrPropDatas.First(d => d.Key == key);
            data.DrawableId = from.DrawableId;
            data.TextureId = from.TextureId;
        }

        private (int, bool) GetCompOrPropIdAndIsComp(ClothesDataKey key)
            => key switch
            {
                ClothesDataKey.Masks => (1, true),
                ClothesDataKey.Hands => (3, true),
                ClothesDataKey.Legs => (4, true),
                ClothesDataKey.Bags => (5, true),
                ClothesDataKey.Shoes => (6, true),
                ClothesDataKey.Accessories => (7, true),
                ClothesDataKey.Shirts => (8, true),
                ClothesDataKey.BodyArmors => (9, true),
                ClothesDataKey.Decals => (10, true),
                ClothesDataKey.Jackets => (11, true),

                ClothesDataKey.Hats => (0, false),
                ClothesDataKey.Glasses => (1, false),
                ClothesDataKey.EarAccessories => (2, false),
                ClothesDataKey.Watches => (6, false),
                ClothesDataKey.Bracelets => (7, false),

                _ => (-1, true)
            };
    }
}