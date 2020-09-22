using GTANetworkAPI;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Player.Char;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models.CharCreator;
using TDS_Shared.Data.Utility;

namespace TDS_Server.Handler.PlayerHandlers
{
    public class PlayerCharHandler
    {
        private readonly TDSDbContext _dbContext;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly ISettingsHandler _settingsHandler;

        public PlayerCharHandler(EventsHandler eventsHandler, LobbiesHandler lobbiesHandler,
            TDSDbContext dbContext, ILoggingHandler loggingHandler, ISettingsHandler settingsHandler)
        {
            _lobbiesHandler = lobbiesHandler;
            _dbContext = dbContext;
            _loggingHandler = loggingHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.PlayerLoggedIn += LoadPlayerChar;
            eventsHandler.ReloadPlayerChar += LoadPlayerChar;
            eventsHandler.PlayerRegisteredBefore += InitPlayerChar;
        }

        internal async Task<object?> Cancel(ITDSPlayer player, ArraySegment<object> args)
        {
            if (!(player.Lobby is CharCreateLobby))
                return null;

            await _lobbiesHandler.MainMenu.AddPlayer(player, null);
            return null;
        }

        internal async Task<object?> Save(ITDSPlayer player, ArraySegment<object> args)
        {
            if (player.Entity is null)
                return null;

            var data = Serializer.FromBrowser<CharCreateData>((string)args[0]);

            // By doing this we can ensure that player datas don't save while editing. Because else
            // this could result in PlayerCharDatas getting messed up for the player
            await player.Database.ExecuteForDB(dbContext =>
            {
                player.Entity.CharDatas.Slot = data.Slot;
                for (int i = 0; i < player.Entity.CharDatas.FeaturesData.Count; ++i)
                {
                    player.Entity.CharDatas.FeaturesData.ElementAt(i).SyncedData = data.FeaturesDataSynced[i];
                    player.Entity.CharDatas.GeneralData.ElementAt(i).SyncedData = data.GeneralDataSynced[i];
                    player.Entity.CharDatas.HairAndColorsData.ElementAt(i).SyncedData = data.HairAndColorsDataSynced[i];
                    player.Entity.CharDatas.HeritageData.ElementAt(i).SyncedData = data.HeritageDataSynced[i];
                    player.Entity.CharDatas.AppearanceData.ElementAt(i).SyncedData = data.AppearanceDataSynced[i];
                }
            });

            await player.SaveData(true);
            LoadPlayerChar(player);

            await _lobbiesHandler.MainMenu.AddPlayer(player, null);
            return null;
        }

        private void CopyJsonValues<T, R>(List<T> originalObjList, ICollection<R> newObjList)
        {
            for (int i = 0; i < originalObjList.Count; ++i)
            {
                var originalObj = originalObjList[i];
                var newObj = newObjList.ElementAt(i);

                originalObj!
                    .GetType()
                    .GetProperties()
                    .Where(p => p.GetCustomAttributes(typeof(Newtonsoft.Json.JsonPropertyAttribute), false).Length > 0)
                    .ForEach(p => p.SetValue(newObj, p.GetValue(originalObj)));
            }
        }

        private async ValueTask InitPlayerChar((ITDSPlayer player, Players dbPlayer) args)
        {
            await _semaphoreSlim.WaitAsync();

            try
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
                    SyncedData = new CharCreateData { Slot = 0 }
                };

                for (byte i = 0; i < amountSlots; ++i)
                {
                    AddCharSlot(charDatas, i);
                }

                _dbContext.Add(charDatas);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex, args.player);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void AddCharSlot(PlayerCharDatas charDatas, byte slot)
        {
            var isMale = SharedUtils.GetRandom(true, false);
            charDatas.GeneralData.Add(new PlayerCharGeneralDatas { Slot = slot, SyncedData = new CharCreateGeneralData { Slot = slot, IsMale = isMale } });
            charDatas.HeritageData.Add(new PlayerCharHeritageDatas
            {
                Slot = slot,
                SyncedData = new CharCreateHeritageData
                {
                    Slot = slot,
                    FatherIndex = 0,
                    MotherIndex = 21,
                    ResemblancePercentage = isMale ? 1 : 0,
                    SkinTonePercentage = isMale ? 1 : 0
                }
            });
            charDatas.FeaturesData.Add(new PlayerCharFeaturesDatas { Slot = slot, SyncedData = new CharCreateFeaturesData { Slot = slot } });
            charDatas.AppearanceData.Add(new PlayerCharAppearanceDatas { Slot = slot, SyncedData = new CharCreateAppearanceData { Slot = slot } });
            charDatas.HairAndColorsData.Add(new PlayerCharHairAndColorsDatas { Slot = slot, SyncedData = new CharCreateHairAndColorsData { Slot = slot } });
        }

        private void LoadPlayerChar(ITDSPlayer player)
        {
            if (player.Entity is null || player.Entity.CharDatas is null)
                return;

            player.SetClothes(11, 0, 0);
            var data = player.Entity.CharDatas;
            while (data.AppearanceData.Count < _settingsHandler.ServerSettings.AmountCharSlots)
            {
                AddCharSlot(data, (byte)data.AppearanceData.Count);
            }

            data.SyncedData = new CharCreateData
            {
                AppearanceDataSynced = data.AppearanceData.Select(d => d.SyncedData).ToList(),
                FeaturesDataSynced = data.FeaturesData.Select(d => d.SyncedData).ToList(),
                GeneralDataSynced = data.GeneralData.Select(d => d.SyncedData).ToList(),
                HairAndColorsDataSynced = data.HairAndColorsData.Select(d => d.SyncedData).ToList(),
                HeritageDataSynced = data.HeritageData.Select(d => d.SyncedData).ToList(),
                Slot = data.Slot
            };

            var currentHairAndColor = data.HairAndColorsData.First(d => d.SyncedData.Slot == data.SyncedData.Slot);
            var currentHeritageData = data.HeritageData.First(d => d.SyncedData.Slot == data.SyncedData.Slot);
            var currentGeneralData = data.GeneralData.First(d => d.SyncedData.Slot == data.SyncedData.Slot);
            var currentAppearanceData = data.AppearanceData.First(d => d.SyncedData.Slot == data.SyncedData.Slot);
            var currentFeaturesData = data.FeaturesData.First(d => d.SyncedData.Slot == data.SyncedData.Slot);

            NAPI.Task.Run(() =>
            {
                player.SetClothes(2, currentHairAndColor.SyncedData.Hair, 0);
                player.SetCustomization(
                    gender: currentGeneralData.SyncedData.IsMale,
                    headBlend: new HeadBlend
                    {
                        ShapeFirst = (byte)currentHeritageData.SyncedData.MotherIndex,
                        ShapeSecond = (byte)currentHeritageData.SyncedData.FatherIndex,
                        ShapeThird = 0,
                        SkinFirst = (byte)currentHeritageData.SyncedData.MotherIndex,
                        SkinSecond = (byte)currentHeritageData.SyncedData.FatherIndex,
                        SkinThird = 0,
                        ShapeMix = currentHeritageData.SyncedData.ResemblancePercentage,
                        SkinMix = currentHeritageData.SyncedData.SkinTonePercentage,
                        ThirdMix = 0
                    },
                    eyeColor: (byte)currentHairAndColor.SyncedData.EyeColor,
                    hairColor: (byte)currentHairAndColor.SyncedData.HairColor,
                    highlightColor: (byte)currentHairAndColor.SyncedData.HairHighlightColor,
                    faceFeatures: new float[] {
                    currentFeaturesData.SyncedData.NoseWidth, currentFeaturesData.SyncedData.NoseHeight, currentFeaturesData.SyncedData.NoseLength, currentFeaturesData.SyncedData.NoseBridge,
                    currentFeaturesData.SyncedData.NoseTip, currentFeaturesData.SyncedData.NoseBridgeShift,
                    currentFeaturesData.SyncedData.BrowHeight, currentFeaturesData.SyncedData.BrowWidth, currentFeaturesData.SyncedData.CheekboneHeight, currentFeaturesData.SyncedData.CheekboneWidth,
                    currentFeaturesData.SyncedData.CheeksWidth,
                    currentFeaturesData.SyncedData.Eyes, currentFeaturesData.SyncedData.Lips,
                    currentFeaturesData.SyncedData.JawWidth, currentFeaturesData.SyncedData.ChinLength, currentFeaturesData.SyncedData.ChinPosition,
                    currentFeaturesData.SyncedData.ChinWidth, currentFeaturesData.SyncedData.ChinShape,
                    currentFeaturesData.SyncedData.NeckWidth
                    },
                    headOverlays: new Dictionary<int, HeadOverlay>
                    {
                        [0] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.Blemishes,
                            Opacity = currentAppearanceData.SyncedData.BlemishesOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [1] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.FacialHair,
                            Opacity = currentAppearanceData.SyncedData.FacialHairOpacity,
                            Color = (byte)currentHairAndColor.SyncedData.FacialHairColor,
                            SecondaryColor = 0
                        },
                        [2] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.Eyebrows,
                            Opacity = currentAppearanceData.SyncedData.EyebrowsOpacity,
                            Color = (byte)currentHairAndColor.SyncedData.EyebrowColor,
                            SecondaryColor = 0
                        },
                        [3] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.Ageing,
                            Opacity = currentAppearanceData.SyncedData.AgeingOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [4] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.Makeup,
                            Opacity = currentAppearanceData.SyncedData.MakeupOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [5] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.Blush,
                            Opacity = currentAppearanceData.SyncedData.BlushOpacity,
                            Color = (byte)currentHairAndColor.SyncedData.BlushColor,
                            SecondaryColor = 0
                        },
                        [6] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.Complexion,
                            Opacity = currentAppearanceData.SyncedData.ComplexionOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [7] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.SunDamage,
                            Opacity = currentAppearanceData.SyncedData.SunDamageOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [8] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.Lipstick,
                            Opacity = currentAppearanceData.SyncedData.LipstickOpacity,
                            Color = (byte)currentHairAndColor.SyncedData.LipstickColor,
                            SecondaryColor = 0
                        },
                        [9] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.MolesAndFreckles,
                            Opacity = currentAppearanceData.SyncedData.MolesAndFrecklesOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [10] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.ChestHair,
                            Opacity = currentAppearanceData.SyncedData.ChestHairOpacity,
                            Color = (byte)currentHairAndColor.SyncedData.ChestHairColor,
                            SecondaryColor = 0
                        },
                        [11] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.BodyBlemishes,
                            Opacity = currentAppearanceData.SyncedData.BodyBlemishesOpacity,
                            Color = 0,
                            SecondaryColor = 0
                        },
                        [12] = new HeadOverlay
                        {
                            Index = (byte)currentAppearanceData.SyncedData.AddBodyBlemishes,
                            Opacity = currentAppearanceData.SyncedData.AddBodyBlemishesOpacity,
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
