using GTANetworkAPI;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Player.Char;
using TDS.Server.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Models.CharCreator;
using TDS.Shared.Data.Utility;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler.PlayerHandlers
{
    public class PlayerCharHandler
    {
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;

        private readonly ISettingsHandler _settingsHandler;

        public PlayerCharHandler(EventsHandler eventsHandler, LobbiesHandler lobbiesHandler, ILoggingHandler loggingHandler, ISettingsHandler settingsHandler)
        {
            _lobbiesHandler = lobbiesHandler;
            _loggingHandler = loggingHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.PlayerLoggedIn += LoadPlayerChar;
            eventsHandler.PlayerRegisteredBefore += InitPlayerChar;
        }

        internal async Task<object?> Cancel(ITDSPlayer player, ArraySegment<object> _)
        {
            if (!(player.Lobby is ICharCreateLobby))
                return null;

            await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0).ConfigureAwait(false);
            return null;
        }

        internal async Task<object?> Save(ITDSPlayer player, ArraySegment<object> args)
        {
            if (player.Entity is null)
                return null;

            var data = Serializer.FromBrowser<PlayerCharDatas>((string)args[0]);

            // By doing this we can ensure that player datas don't save while editing. Because else
            // this could result in PlayerCharDatas getting messed up for the player
            await player.Database.ExecuteForDB(dbContext =>
            {
                player.Entity.CharDatas.Slot = data.Slot;
                for (int i = 0; i < player.Entity.CharDatas.FeaturesData.Count; ++i)
                {
                    CopyJsonValues(player.Entity.CharDatas.FeaturesData.ElementAt(i), data.FeaturesData.ElementAt(i));
                    CopyJsonValues(player.Entity.CharDatas.GeneralData.ElementAt(i), data.GeneralData.ElementAt(i));
                    CopyJsonValues(player.Entity.CharDatas.HairAndColorsData.ElementAt(i), data.HairAndColorsData.ElementAt(i));
                    CopyJsonValues(player.Entity.CharDatas.HeritageData.ElementAt(i), data.HeritageData.ElementAt(i));
                    CopyJsonValues(player.Entity.CharDatas.AppearanceData.ElementAt(i), data.AppearanceData.ElementAt(i));
                }
            }).ConfigureAwait(false);

            await player.DatabaseHandler.SaveData(true).ConfigureAwait(false);
            LoadPlayerChar(player);

            await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0).ConfigureAwait(false);
            return null;
        }

        private void CopyJsonValues<T>(T originalObj, T newObj)
        {
            originalObj!
                .GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(Newtonsoft.Json.JsonPropertyAttribute), false).Length > 0)
                .ForEach(p => p.SetValue(newObj, p.GetValue(originalObj)));
        }

        public async ValueTask InitPlayerChar((ITDSPlayer player, Players dbPlayer) args)
        {
            byte amountSlots = _settingsHandler.ServerSettings.AmountCharSlots;

            var charDatas = new PlayerCharDatas
            {
                PlayerId = args.dbPlayer.Id,
                GeneralData = new List<PlayerCharGeneralDatas>(amountSlots),
                HeritageData = new List<PlayerCharHeritageDatas>(amountSlots),
                FeaturesData = new List<PlayerCharFeaturesDatas>(amountSlots),
                AppearanceData = new List<PlayerCharAppearanceDatas>(amountSlots),
                HairAndColorsData = new List<PlayerCharHairAndColorsDatas>(amountSlots),
            };
            args.dbPlayer.CharDatas = charDatas;

            for (byte i = 0; i < amountSlots; ++i)
                AddCharSlot(charDatas, i);

            await args.player.Database.ExecuteForDBAsyncUnsafe(async dbContext => 
            {
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            });
        }

        private void AddCharSlot(PlayerCharDatas charDatas, byte slot)
        {
            var isMale = SharedUtils.GetRandom(true, false);
            charDatas.GeneralData.Add(new PlayerCharGeneralDatas 
            { 
                Slot = slot, 
                CharDatas = charDatas, 
                PlayerId = charDatas.PlayerId,
                IsMale = isMale
            });
            charDatas.HeritageData.Add(new PlayerCharHeritageDatas
            {
                Slot = slot,
                CharDatas = charDatas,
                PlayerId = charDatas.PlayerId,
                FatherIndex = 0,
                MotherIndex = 21,
                ResemblancePercentage = isMale ? 1 : 0,
                SkinTonePercentage = isMale ? 1 : 0
            });
            charDatas.FeaturesData.Add(new PlayerCharFeaturesDatas 
            {
                Slot = slot, 
                CharDatas = charDatas, 
                PlayerId = charDatas.PlayerId, 
            });
            charDatas.AppearanceData.Add(new PlayerCharAppearanceDatas 
            { 
                Slot = slot, 
                CharDatas = charDatas, 
                PlayerId = charDatas.PlayerId
            });
            charDatas.HairAndColorsData.Add(new PlayerCharHairAndColorsDatas 
            { 
                Slot = slot, 
                CharDatas = charDatas, 
                PlayerId = charDatas.PlayerId
            });
        }

        private async void LoadPlayerChar(ITDSPlayer player)
        {
            if (player.Entity is null || player.Entity.CharDatas is null)
                return;

            await Task.Yield();
            var data = player.Entity.CharDatas;
            while (data.AppearanceData.Count < _settingsHandler.ServerSettings.AmountCharSlots)
            {
                AddCharSlot(data, (byte)data.AppearanceData.Count);
            }

            var currentHairAndColor = data.HairAndColorsData.First(d => d.Slot == data.Slot);
            var currentHeritageData = data.HeritageData.First(d => d.Slot == data.Slot);
            var currentGeneralData = data.GeneralData.First(d => d.Slot == data.Slot);
            var currentAppearanceData = data.AppearanceData.First(d => d.Slot == data.Slot);
            var currentFeaturesData = data.FeaturesData.First(d => d.Slot == data.Slot);

            NAPI.Task.RunSafe(() =>
            {
                player.SetClothes(11, 0, 0);
                player.SetClothes(2, currentHairAndColor.Hair, 0);
                player.SetCustomization(
                    gender: currentGeneralData.IsMale,
                    headBlend: new HeadBlend
                    {
                        ShapeFirst = (byte)currentHeritageData.MotherIndex,
                        ShapeSecond = (byte)currentHeritageData.FatherIndex,
                        ShapeThird = 0,
                        SkinFirst = (byte)currentHeritageData.MotherIndex,
                        SkinSecond = (byte)currentHeritageData.FatherIndex,
                        SkinThird = 0,
                        ShapeMix = currentHeritageData.ResemblancePercentage,
                        SkinMix = currentHeritageData.SkinTonePercentage,
                        ThirdMix = 0
                    },
                    eyeColor: (byte)currentHairAndColor.EyeColor,
                    hairColor: (byte)currentHairAndColor.HairColor,
                    highlightColor: (byte)currentHairAndColor.HairHighlightColor,
                    faceFeatures: new float[] {
                    currentFeaturesData.NoseWidth, currentFeaturesData.NoseHeight, currentFeaturesData.NoseLength, currentFeaturesData.NoseBridge,
                    currentFeaturesData.NoseTip, currentFeaturesData.NoseBridgeShift,
                    currentFeaturesData.BrowHeight, currentFeaturesData.BrowWidth, currentFeaturesData.CheekboneHeight, currentFeaturesData.CheekboneWidth,
                    currentFeaturesData.CheeksWidth,
                    currentFeaturesData.Eyes, currentFeaturesData.Lips,
                    currentFeaturesData.JawWidth, currentFeaturesData.ChinLength, currentFeaturesData.ChinPosition,
                    currentFeaturesData.ChinWidth, currentFeaturesData.ChinShape,
                    currentFeaturesData.NeckWidth
                    },
                    headOverlays: new Dictionary<int, HeadOverlay>
                    {
                        [0] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.Blemishes,
                            Opacity = currentAppearanceData.BlemishesOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [1] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.FacialHair,
                            Opacity = currentAppearanceData.FacialHairOpacity,
                            Color = (byte)currentHairAndColor.FacialHairColor,
                            SecondaryColor = 0
                        },
                        [2] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.Eyebrows,
                            Opacity = currentAppearanceData.EyebrowsOpacity,
                            Color = (byte)currentHairAndColor.EyebrowColor,
                            SecondaryColor = 0
                        },
                        [3] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.Ageing,
                            Opacity = currentAppearanceData.AgeingOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [4] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.Makeup,
                            Opacity = currentAppearanceData.MakeupOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [5] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.Blush,
                            Opacity = currentAppearanceData.BlushOpacity,
                            Color = (byte)currentHairAndColor.BlushColor,
                            SecondaryColor = 0
                        },
                        [6] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.Complexion,
                            Opacity = currentAppearanceData.ComplexionOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [7] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SunDamage,
                            Opacity = currentAppearanceData.SunDamageOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [8] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.Lipstick,
                            Opacity = currentAppearanceData.LipstickOpacity,
                            Color = (byte)currentHairAndColor.LipstickColor,
                            SecondaryColor = 0
                        },
                        [9] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.MolesAndFreckles,
                            Opacity = currentAppearanceData.MolesAndFrecklesOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [10] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.ChestHair,
                            Opacity = currentAppearanceData.ChestHairOpacity,
                            Color = (byte)currentHairAndColor.ChestHairColor,
                            SecondaryColor = 0
                        },
                        [11] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.BodyBlemishes,
                            Opacity = currentAppearanceData.BodyBlemishesOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [12] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.AddBodyBlemishes,
                            Opacity = currentAppearanceData.AddBodyBlemishesOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                    },
                    decorations: Array.Empty<Decoration>()
                );
            });
        }
    }
}
