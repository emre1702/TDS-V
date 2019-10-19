using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Dto.ClothesMeta;
using TDS_Server.Manager.Logs;

namespace TDS_Server.Manager.Utility
{
    static class ClothesManager
    {
        // https://rage.mp/files/file/50-gtao-outfits/
        private static List<GenderOutfits> _maleOutfits = new List<GenderOutfits>();
        private static List<GenderOutfits> _femaleOutfits = new List<GenderOutfits>();

        private static List<PropertyInfo>? _drawableProperties;
        private static List<PropertyInfo>? _textureProperties;
        private static List<PropertyInfo>? _propIndexProperties;
        private static List<PropertyInfo>? _propTextureProperties;

        public static void Init()
        {
            if (!File.Exists("scriptmetadata.meta"))
            {
                ErrorLogsManager.Log($"scriptmetadata.meta missing in path '{Path.GetFullPath("scriptmetadata.meta")}'.", Environment.StackTrace);
                return;
            }

            if (!DoesCacheExist())
                InitCache();
        }

        private static void ProcessItems(Item[] items, List<GenderOutfits> listToAddTo)
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

        private static void InitCache()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Root));

            var metaFileInfo = new FileInfo("scriptmetadata.meta");

            using XmlReader reader = XmlReader.Create(metaFileInfo.OpenText(), new XmlReaderSettings { Async = true });
            if (!_xmlSerializer.CanDeserialize(reader))
            {
                ErrorLogsManager.Log($"Could not deserialize file {metaFileInfo.FullName}.", Environment.StackTrace);
                return;
            }

            Root clothesData = (Root)_xmlSerializer.Deserialize(reader);

            InitProperties();
            ProcessItems(clothesData.Outfits.OutfitsDataMale.OutfitsData.Items, _maleOutfits);
            ProcessItems(clothesData.Outfits.OutfitsDataFemale.OutfitsData.Items, _femaleOutfits);

            Directory.CreateDirectory("cache");
            File.WriteAllText("cache/maleClothes.json", JsonConvert.SerializeObject(_maleOutfits));
            File.WriteAllText("cache/femaleClothes.json", JsonConvert.SerializeObject(_femaleOutfits));

            var fileSize = metaFileInfo.Length;
            File.WriteAllText("cache/clothesSizeCache.json", fileSize.ToString());

            watch.Stop();
            Console.WriteLine($"Clothes cache generated in {watch.ElapsedMilliseconds} ms.");
        }

        private static void InitProperties()
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

        private static bool DoesCacheExist()
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

        #nullable disable
        private class OutfitData
        {
            public int Drawable;
            public int Texture;
        }

        private class GenderOutfits
        {
            public List<OutfitData> Components;
            public List<OutfitData> Props;
        }
        #nullable restore
    }
}
