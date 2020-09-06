using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.ClothesMeta;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;

namespace TDS_Server.Handler
{
    public class ClothesHandler
    {
        #region Private Fields

        private readonly List<GenderOutfits> _femaleOutfits = new List<GenderOutfits>();

        private readonly ILoggingHandler _loggingHandler;

        // https://rage.mp/files/file/50-gtao-outfits/
        private readonly List<GenderOutfits> _maleOutfits = new List<GenderOutfits>();

        private readonly Serializer _serializer;
        private List<PropertyInfo>? _drawableProperties;
        private List<PropertyInfo>? _propIndexProperties;
        private List<PropertyInfo>? _propTextureProperties;
        private List<PropertyInfo>? _textureProperties;

        #endregion Private Fields

        #region Public Constructors

        public ClothesHandler(ILoggingHandler loggingHandler, Serializer serializer, EventsHandler eventsHandler)
        {
            _loggingHandler = loggingHandler;
            _serializer = serializer;

            if (!File.Exists("scriptmetadata.meta"))
            {
                loggingHandler.LogError($"scriptmetadata.meta missing in path '{Path.GetFullPath("scriptmetadata.meta")}'.", Environment.StackTrace);
                return;
            }

            if (!DoesCacheExist())
                InitCache();

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
        }

        #endregion Public Constructors

        #region Private Methods

        private bool DoesCacheExist()
        {
            if (!File.Exists("cache/maleClothes.json"))
                return false;
            if (!File.Exists("cache/femaleClothes.json"))
                return false;
            if (!File.Exists("cache/clothesSizeCache.json"))
                return false;

            var fileSize = new FileInfo("scriptmetadata.meta").Length;
            if (!int.TryParse(File.ReadAllText("cache/clothesSizeCache.json"), out int previousFileSize))
                return false;

            if (fileSize != previousFileSize)
                return false;

            return true;
        }

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            if (player.Entity is null)
                return;
            if (player.Entity.PlayerClothes is null)
                player.Entity.PlayerClothes = new PlayerClothes();
        }

        private void InitCache()
        {
            var watch = new Stopwatch();
            watch.Start();

            var _xmlSerializer = new XmlSerializer(typeof(Root));

            var metaFileInfo = new FileInfo("scriptmetadata.meta");

            using XmlReader reader = XmlReader.Create(metaFileInfo.OpenText(), new XmlReaderSettings { Async = true });
            if (!_xmlSerializer.CanDeserialize(reader))
            {
                _loggingHandler.LogError($"Could not deserialize file {metaFileInfo.FullName}.", Environment.StackTrace);
                return;
            }

            Root clothesData = (Root)_xmlSerializer.Deserialize(reader);

            InitProperties();
            ProcessItems(clothesData.Outfits.OutfitsDataMale.OutfitsData.Items, _maleOutfits);
            ProcessItems(clothesData.Outfits.OutfitsDataFemale.OutfitsData.Items, _femaleOutfits);

            Directory.CreateDirectory("cache");
            File.WriteAllText("cache/maleClothes.json", _serializer.ToClient(_maleOutfits));
            File.WriteAllText("cache/femaleClothes.json", _serializer.ToClient(_femaleOutfits));

            var fileSize = metaFileInfo.Length;
            File.WriteAllText("cache/clothesSizeCache.json", fileSize.ToString());

            watch.Stop();
            Console.WriteLine($"Clothes cache generated in {watch.ElapsedMilliseconds} ms.");
        }

        private void InitProperties()
        {
            _drawableProperties = new List<PropertyInfo>();
            _textureProperties = new List<PropertyInfo>();
            for (int i = 0; i < 12; ++i)
            {
                _drawableProperties.Add(typeof(ComponentDrawables).GetProperty("Drawable" + i)!);
                _textureProperties.Add(typeof(ComponentTextures).GetProperty("Texture" + i)!);
            }

            _propIndexProperties = new List<PropertyInfo>();
            _propTextureProperties = new List<PropertyInfo>();
            for (int i = 0; i < 9; ++i)
            {
                _propIndexProperties.Add(typeof(PropIndices).GetProperty("PropIndex" + i)!);
                _propTextureProperties.Add(typeof(PropTextures).GetProperty("PropTexture" + i)!);
            }
        }

        private void ProcessItems(Item[] items, List<GenderOutfits> listToAddTo)
        {
            foreach (var item in items)
            {
                var components = new List<OutfitData>();
                var props = new List<OutfitData>();

                for (int i = 0; i < 12; ++i)
                {
                    components.Add(new OutfitData
                    {
                        Drawable = ((Component)_drawableProperties![i].GetValue(item.ComponentDrawables)!).Value,
                        Texture = ((Component)_textureProperties![i].GetValue(item.ComponentTextures)!).Value
                    });
                }

                for (int i = 0; i < 9; ++i)
                {
                    props.Add(new OutfitData
                    {
                        Drawable = ((Component)_propIndexProperties![i].GetValue(item.PropIndices)!).Value,
                        Texture = ((Component)_propTextureProperties![i].GetValue(item.PropTextures)!).Value
                    });
                }

                listToAddTo.Add(new GenderOutfits
                {
                    Components = components,
                    Props = props
                });
            }
        }

        #endregion Private Methods

#nullable disable

        #region Private Classes

        private class GenderOutfits
        {
            #region Public Fields

            public List<OutfitData> Components;
            public List<OutfitData> Props;

            #endregion Public Fields
        }

        private class OutfitData
        {
            #region Public Fields

            public int Drawable;
            public int Texture;

            #endregion Public Fields
        }

        #endregion Private Classes

#nullable restore
    }
}
