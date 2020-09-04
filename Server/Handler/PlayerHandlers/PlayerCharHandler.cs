using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models;
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
        #region Private Fields

        private readonly TDSDbContext _dbContext;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;

        #endregion Private Fields

        #region Public Constructors

        public PlayerCharHandler(IModAPI modAPI, EventsHandler eventsHandler, Serializer serializer, LobbiesHandler lobbiesHandler,
            TDSDbContext dbContext, ILoggingHandler loggingHandler, ISettingsHandler settingsHandler)
        {
            _modAPI = modAPI;
            _serializer = serializer;
            _lobbiesHandler = lobbiesHandler;
            _dbContext = dbContext;
            _loggingHandler = loggingHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.PlayerLoggedIn += LoadPlayerChar;
            eventsHandler.ReloadPlayerChar += LoadPlayerChar;
            eventsHandler.PlayerRegisteredBefore += InitPlayerChar;
        }

        #endregion Public Constructors

        #region Internal Methods

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

            var data = _serializer.FromBrowser<PlayerCharDatas>((string)args[0]);

            // By doing this we can ensure that player datas don't save while editing. Because else
            // this could result in PlayerCharDatas getting messed up for the player
            await player.ExecuteForDB(dbContext =>
            {
                CopyJsonValues(data.FeaturesDataSynced, player.Entity.CharDatas.FeaturesData);
                CopyJsonValues(data.GeneralDataSynced, player.Entity.CharDatas.GeneralData);
                CopyJsonValues(data.HairAndColorsDataSynced, player.Entity.CharDatas.HairAndColorsData);
                CopyJsonValues(data.HeritageDataSynced, player.Entity.CharDatas.HeritageData);
            });

            await player.SaveData(true);
            NAPI.Task.Run(() =>
            {
                LoadPlayerChar(player);
            });

            await _lobbiesHandler.MainMenu.AddPlayer(player, null);
            return null;
        }

        #endregion Internal Methods

        #region Private Methods

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
                    Slot = 0,
                    GeneralData = new List<PlayerCharGeneralDatas>(amountSlots),
                    HeritageData = new List<PlayerCharHeritageDatas>(amountSlots),
                    FeaturesData = new List<PlayerCharFeaturesDatas>(amountSlots),
                    AppearanceData = new List<PlayerCharAppearanceDatas>(amountSlots),
                    HairAndColorsData = new List<PlayerCharHairAndColorsDatas>(amountSlots)
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
            charDatas.GeneralData.Add(new PlayerCharGeneralDatas { Slot = slot, IsMale = isMale });
            charDatas.HeritageData.Add(new PlayerCharHeritageDatas
            {
                Slot = slot,
                FatherIndex = 0,
                MotherIndex = 21,
                ResemblancePercentage = isMale ? 1 : 0,
                SkinTonePercentage = isMale ? 1 : 0
            });
            charDatas.FeaturesData.Add(new PlayerCharFeaturesDatas { Slot = slot });
            charDatas.AppearanceData.Add(new PlayerCharAppearanceDatas { Slot = slot });
            charDatas.HairAndColorsData.Add(new PlayerCharHairAndColorsDatas { Slot = slot });
        }

        private void LoadPlayerChar(ITDSPlayer player)
        {
            if (player.Entity is null || player.Entity.CharDatas is null)
                return;

            var data = player.Entity.CharDatas;
            while (data.AppearanceData.Count < _settingsHandler.ServerSettings.AmountCharSlots)
            {
                AddCharSlot(data, (byte)data.AppearanceData.Count);
            }

            data.AppearanceDataSynced = data.AppearanceData.Cast<CharCreateAppearanceData>().ToList();
            data.FeaturesDataSynced = data.FeaturesData.Cast<CharCreateFeaturesData>().ToList();
            data.GeneralDataSynced = data.GeneralData.Cast<CharCreateGeneralData>().ToList();
            data.HairAndColorsDataSynced = data.HairAndColorsData.Cast<CharCreateHairAndColorsData>().ToList();
            data.HeritageDataSynced = data.HeritageData.Cast<CharCreateHeritageData>().ToList();

            if (player.ModPlayer is null)
                return;


            var currentHairAndColor = data.HairAndColorsData.First(d => d.Slot == data.Slot);
            var currentHeritageData = data.HeritageData.First(d => d.Slot == data.Slot);
            var currentGeneralData = data.GeneralData.First(d => d.Slot == data.Slot);
            var currentAppearanceData = data.AppearanceData.First(d => d.Slot == data.Slot);
            var currentFeaturesData = data.FeaturesData.First(d => d.Slot == data.Slot);

            player.ModPlayer.SetClothes(2, currentHairAndColor.Hair, 0);
            player.ModPlayer.SetCustomization(
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
                    currentFeaturesData.JawWidth, currentFeaturesData.ChinLength, currentFeaturesData.ChinPosition, currentFeaturesData.ChinWidth, currentFeaturesData.ChinShape,
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
        }

        #endregion Private Methods
    }
}
