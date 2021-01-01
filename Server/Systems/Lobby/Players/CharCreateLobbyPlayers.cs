using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Database.Entity.Player.Character.Clothes;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums.CharCreator;
using TDS.Shared.Data.Models.CharCreator;
using TDS.Shared.Data.Models.CharCreator.Clothes;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.Players
{
    internal class CharCreateLobbyPlayers : BaseLobbyPlayers
    {
        public CharCreateLobbyPlayers(ICharCreateLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex = 0)
        {
            if (player.Entity?.BodyDatas is null)
                return false;

            await Task.Yield();
            var worked = await base.AddPlayer(player, teamIndex).ConfigureAwait(false);
            if (!worked)
                return false;

            var bodyDatasJson = Serializer.ToClient(player.Entity.BodyDatas);
            var clothesDatasJson = Serializer.ToClient(GetClothesDataForClient(player.Entity.ClothesDatas));

            NAPI.Task.RunSafe(() =>
            {
                player.Spawn(Lobby.MapHandler.SpawnPoint, Lobby.MapHandler.SpawnRotation);
                player.SetInvincible(true);
                player.Freeze(true);
                player.SetInvisible(true);

                player.TriggerEvent(ToClientEvent.StartCharCreator, bodyDatasJson, clothesDatasJson, Lobby.MapHandler.Dimension);
            });

            return true;
        }

        private ClothesConfigs GetClothesDataForClient(PlayerClothesDatas datas)
        {
            var configs = new ClothesConfigs
            {
                SelectedSlot = datas.SelectedSlot,
                DatasPerSlot = new()
            };

            foreach (var dataPerSlot in datas.DatasPerSlot)
            {
                var clothesData = new ClothesData { Slot = dataPerSlot.Slot };
                configs.DatasPerSlot.Add(clothesData);
                foreach (var compOrProp in dataPerSlot.ComponentOrPropDatas)
                {
                    var data = new ClothesComponentOrPropData { DrawableId = compOrProp.DrawableId, TextureId = compOrProp.TextureId };
                    AddToCorrectProperty(data, clothesData, compOrProp.Key);
                }
            }

            return configs;
        }

        private void AddToCorrectProperty(ClothesComponentOrPropData addThis, ClothesData toThis, ClothesDataKey key)
        {
            switch (key)
            {
                case ClothesDataKey.Accessories: toThis.Accessory = addThis; break;
                case ClothesDataKey.Bags: toThis.Bag = addThis; break;
                case ClothesDataKey.BodyArmors: toThis.BodyArmor = addThis; break;
                case ClothesDataKey.Bracelets: toThis.Bracelet = addThis; break;
                case ClothesDataKey.Decals: toThis.Decal = addThis; break;
                case ClothesDataKey.EarAccessories: toThis.EarAccessory = addThis; break;
                case ClothesDataKey.Glasses: toThis.Glasses = addThis; break;
                case ClothesDataKey.Hands: toThis.Hands = addThis; break;
                case ClothesDataKey.Hats: toThis.Hat = addThis; break;
                case ClothesDataKey.Jackets: toThis.Jacket = addThis; break;
                case ClothesDataKey.Legs: toThis.Legs = addThis; break;
                case ClothesDataKey.Masks: toThis.Mask = addThis; break;
                case ClothesDataKey.Shirts: toThis.Shirt = addThis; break;
                case ClothesDataKey.Shoes: toThis.Shoes = addThis; break;
                case ClothesDataKey.Watches: toThis.Watch = addThis; break;
            }
        }
    }
}