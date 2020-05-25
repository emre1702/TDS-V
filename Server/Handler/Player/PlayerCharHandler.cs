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
using TDS_Shared.Data.Utility;

namespace TDS_Server.Handler.Player
{
    public class PlayerCharHandler
    {
        #region Private Fields

        private readonly TDSDbContext _dbContext;
        private readonly EventsHandler _eventsHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly Serializer _serializer;
        private readonly IServiceProvider _serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        public PlayerCharHandler(IModAPI modAPI, EventsHandler eventsHandler, Serializer serializer, IServiceProvider serviceProvider, LobbiesHandler lobbiesHandler,
            TDSDbContext dbContext, ILoggingHandler loggingHandler)
        {
            _modAPI = modAPI;
            _serializer = serializer;
            _serviceProvider = serviceProvider;
            _eventsHandler = eventsHandler;
            _lobbiesHandler = lobbiesHandler;
            _dbContext = dbContext;
            _loggingHandler = loggingHandler;

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
                player.Entity.CharDatas.AppearanceData = new PlayerCharAppearanceDatas();
                player.Entity.CharDatas.FeaturesData = new PlayerCharFeaturesDatas();
                player.Entity.CharDatas.GeneralData = new PlayerCharGeneralDatas();
                player.Entity.CharDatas.HairAndColorsData = new PlayerCharHairAndColorsDatas();
                player.Entity.CharDatas.HeritageData = new PlayerCharHeritageDatas();

                CopyJsonValues(data.AppearanceDataSynced, player.Entity.CharDatas.AppearanceData);
                CopyJsonValues(data.FeaturesDataSynced, player.Entity.CharDatas.FeaturesData);
                CopyJsonValues(data.GeneralDataSynced, player.Entity.CharDatas.GeneralData);
                CopyJsonValues(data.HairAndColorsDataSynced, player.Entity.CharDatas.HairAndColorsData);
                CopyJsonValues(data.HeritageDataSynced, player.Entity.CharDatas.HeritageData);
            });

            await player.SaveData(true);
            _modAPI.Thread.RunInMainThread(() =>
            {
                LoadPlayerChar(player);
            });

            await _lobbiesHandler.MainMenu.AddPlayer(player, null);
            return null;
        }

        #endregion Internal Methods

        #region Private Methods

        private void CopyJsonValues(object originalObj, object newObj)
        {
            originalObj
                .GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(Newtonsoft.Json.JsonPropertyAttribute), false).Length > 0)
                .ForEach(p => p.SetValue(newObj, p.GetValue(originalObj)));
        }

        private async ValueTask InitPlayerChar((ITDSPlayer player, Players dbPlayer) args)
        {
            await _semaphoreSlim.WaitAsync();

            try
            {
                var isMale = SharedUtils.GetRandom(true, false);
                var charDatas = new PlayerCharDatas
                {
                    PlayerId = args.dbPlayer.Id,
                    GeneralData = new PlayerCharGeneralDatas
                    {
                        IsMale = isMale
                    },
                    HeritageData = new PlayerCharHeritageDatas
                    {
                        FatherIndex = 0,
                        MotherIndex = 21,
                        ResemblancePercentage = isMale ? 1 : 0,
                        SkinTonePercentage = isMale ? 1 : 0
                    },
                    FeaturesData = new PlayerCharFeaturesDatas(),
                    AppearanceData = new PlayerCharAppearanceDatas(),
                    HairAndColorsData = new PlayerCharHairAndColorsDatas()
                };
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

        private void LoadPlayerChar(ITDSPlayer player)
        {
            if (player.Entity is null || player.Entity.CharDatas is null)
                return;

            var data = player.Entity.CharDatas;
            data.AppearanceDataSynced = data.AppearanceData;
            data.FeaturesDataSynced = data.FeaturesData;
            data.GeneralDataSynced = data.GeneralData;
            data.HairAndColorsDataSynced = data.HairAndColorsData;
            data.HeritageDataSynced = data.HeritageData;

            if (player.ModPlayer is null)
                return;

            player.ModPlayer.SetClothes(2, data.HairAndColorsData.Hair, 0);
            player.ModPlayer.SetCustomization(
                gender: data.GeneralData.IsMale,
                headBlend: new HeadBlend
                {
                    ShapeFirst = (byte)data.HeritageData.MotherIndex,
                    ShapeSecond = (byte)data.HeritageData.FatherIndex,
                    ShapeThird = 0,
                    SkinFirst = (byte)data.HeritageData.MotherIndex,
                    SkinSecond = (byte)data.HeritageData.FatherIndex,
                    SkinThird = 0,
                    ShapeMix = data.HeritageData.ResemblancePercentage,
                    SkinMix = data.HeritageData.SkinTonePercentage,
                    ThirdMix = 0
                },
                eyeColor: (byte)data.HairAndColorsData.EyeColor,
                hairColor: (byte)data.HairAndColorsData.HairColor,
                highlightColor: (byte)data.HairAndColorsData.HairHighlightColor,
                faceFeatures: new float[] {
                    data.FeaturesData.NoseWidth, data.FeaturesData.NoseHeight, data.FeaturesData.NoseLength, data.FeaturesData.NoseBridge,
                    data.FeaturesData.NoseTip, data.FeaturesData.NoseBridgeShift,
                    data.FeaturesData.BrowHeight, data.FeaturesData.BrowWidth, data.FeaturesData.CheekboneHeight, data.FeaturesData.CheekboneWidth,
                    data.FeaturesData.CheeksWidth,
                    data.FeaturesData.Eyes, data.FeaturesData.Lips,
                    data.FeaturesData.JawWidth, data.FeaturesData.ChinLength, data.FeaturesData.ChinPosition, data.FeaturesData.ChinWidth, data.FeaturesData.ChinShape,
                    data.FeaturesData.NeckWidth
                },
                headOverlays: new Dictionary<int, HeadOverlay>
                {
                    [0] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.Blemishes,
                        Opacity = data.AppearanceData.BlemishesOpacity,
                        Color = 0,
                        SecondaryColor = 0
                    },
                    [1] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.FacialHair,
                        Opacity = data.AppearanceData.FacialHairOpacity,
                        Color = (byte)data.HairAndColorsData.FacialHairColor,
                        SecondaryColor = 0
                    },
                    [2] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.Eyebrows,
                        Opacity = data.AppearanceData.EyebrowsOpacity,
                        Color = (byte)data.HairAndColorsData.EyebrowColor,
                        SecondaryColor = 0
                    },
                    [3] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.Ageing,
                        Opacity = data.AppearanceData.AgeingOpacity,
                        Color = 0,
                        SecondaryColor = 0
                    },
                    [4] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.Makeup,
                        Opacity = data.AppearanceData.MakeupOpacity,
                        Color = 0,
                        SecondaryColor = 0
                    },
                    [5] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.Blush,
                        Opacity = data.AppearanceData.BlushOpacity,
                        Color = (byte)data.HairAndColorsData.BlushColor,
                        SecondaryColor = 0
                    },
                    [6] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.Complexion,
                        Opacity = data.AppearanceData.ComplexionOpacity,
                        Color = 0,
                        SecondaryColor = 0
                    },
                    [7] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.SunDamage,
                        Opacity = data.AppearanceData.SunDamageOpacity,
                        Color = 0,
                        SecondaryColor = 0
                    },
                    [8] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.Lipstick,
                        Opacity = data.AppearanceData.LipstickOpacity,
                        Color = (byte)data.HairAndColorsData.LipstickColor,
                        SecondaryColor = 0
                    },
                    [9] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.MolesAndFreckles,
                        Opacity = data.AppearanceData.MolesAndFrecklesOpacity,
                        Color = 0,
                        SecondaryColor = 0
                    },
                    [10] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.ChestHair,
                        Opacity = data.AppearanceData.ChestHairOpacity,
                        Color = (byte)data.HairAndColorsData.ChestHairColor,
                        SecondaryColor = 0
                    },
                    [11] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.BodyBlemishes,
                        Opacity = data.AppearanceData.BodyBlemishesOpacity,
                        Color = 0,
                        SecondaryColor = 0
                    },
                    [12] = new HeadOverlay
                    {
                        Index = (byte)data.AppearanceData.AddBodyBlemishes,
                        Opacity = data.AppearanceData.AddBodyBlemishesOpacity,
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
