using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Helper;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

using DB = TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Maps
{
    internal static class MapCreator
    {
        private static List<MapDto> _newCreatedMaps = new List<MapDto>();

        public async static Task<bool> Create(TDSPlayer creator, string mapJson)
        {
            if (creator.Entity == null)
                return false;
            var serializer = new XmlSerializer(typeof(MapDto));
            try
            {
                var mapDto = (MapDto)JsonConvert.DeserializeObject(mapJson);
                mapDto.LoadSyncedData();
                //mapDto.SyncedData.CreatorName = creator.Client.Name;

                string mapFileName = mapDto.Info.Name + "_" + (mapDto.SyncedData.CreatorName ?? "?") + "_" + Utils.GetTimestamp() + ".map";
                string mapPath = SettingsManager.NewMapsPath + Utils.MakeValidFileName(mapFileName);

                MemoryStream memStrm = new MemoryStream();
                UTF8Encoding utf8e = new UTF8Encoding();
                XmlTextWriter xmlSink = new XmlTextWriter(memStrm, utf8e);
                serializer.Serialize(xmlSink, mapDto);
                byte[] utf8EncodedData = memStrm.ToArray();

                string mapXml = utf8e.GetString(utf8EncodedData);
                string prettyMapXml = await XmlHelper.GetPrettyAsync(mapXml);
                await File.WriteAllTextAsync(mapPath, prettyMapXml);

                using var dbContext = new TDSNewContext();
                var dbMap = new DB.Maps { CreatorId = creator.Entity.Id, Name = mapDto.Info.Name, InTesting = true };
                await dbContext.Maps.AddAsync(dbMap);
                await dbContext.SaveChangesAsync();

                mapDto.Info.Id = dbMap.Id;
                mapDto.RatingAverage = 5;

                _newCreatedMaps.Add(mapDto);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task LoadNewMaps(TDSNewContext dbContext)
        {
            _newCreatedMaps = await MapsLoader.LoadMaps(dbContext, true);
            foreach (var map in _newCreatedMaps)
            {
                // Player shouldn't be able to see the creator of the map (so they don't rate it depending of the creator)
                map.SyncedData.CreatorName = string.Empty;
            }
        }

        public static MapDto? GetRandomNewMap()
        {
            if (_newCreatedMaps.Count == 0)
                return null;
            return _newCreatedMaps[CommonUtils.Rnd.Next(_newCreatedMaps.Count)];
        }

        public static MapDto? GetMapByName(string mapName)
        {
            return _newCreatedMaps.FirstOrDefault(m => m.Info.Name == mapName);
        }

        /*private static string GetXmlStringByMap(CreatedMap map, uint playeruid)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<MapData>")
                .AppendLine("\t<map creator='" + playeruid + "' name='" + map.Name + "' type='" + map.Type + "' minplayers='" + map.MinPlayers + "' maxplayers='" + map.MaxPlayers + "' />")
                .AppendLine("\t<english>" + map.Descriptions.English + "</english>")
                .AppendLine("\t<german>" + map.Descriptions.German + "</german>");

            if (map.MapCenter != null)
            {
                builder.AppendLine("\t<center x='" + map.MapCenter.X + "' y='" + map.MapCenter.Y + "' z='" + map.MapCenter.Z + "' />");
            }

            if (map.Type == "bomb")
            {
                for (int i = 0; i < map.BombPlaces.Length; ++i)
                {
                    Position pos = map.BombPlaces[i];
                    builder.AppendLine("\t<bomb x='" + pos.X + "' y='" + pos.Y + "' z='" + (pos.Z - 1) + "' />");
                }
            }

            for (int i = 0; i < map.MapSpawns.Length; ++i)
            {
                TeamSpawn spawn = map.MapSpawns[i];
                builder.AppendLine("\t<team" + spawn.Team + " x='" + spawn.X + "' y='" + spawn.Y + "' z='" + spawn.Z + "' rot='" + spawn.Rot + "' />");
            }

            for (int i = 0; i < map.MapLimitPositions.Length; ++i)
            {
                Position pos = map.MapLimitPositions[i];
                builder.AppendLine("\t<limit x='" + pos.X + "' y='" + pos.Y + "' z='" + pos.Z + "' />");
            }

            builder.AppendLine("</MapData>");
            return builder.ToString();
        }

        public static async void CreateNewMap(string content, uint playeruid)
        {
            try
            {
                CreatedMap map = JsonConvert.DeserializeObject<CreatedMap>(content);
                string path = Setting.NewMapsPath + playeruid + "/";
                Directory.CreateDirectory(path);
                using (StreamWriter writer = File.CreateText(path + DateTime.Now + ".xml"))
                {
                    await writer.WriteAsync(GetXmlStringByMap(map, playeruid));
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex.ToString(), Environment.StackTrace, Player.Player.GetPlayer(playeruid));
            }
        }

        public static bool DoesMapNameExist(string mapname)
        {
            return MapPathByName.ContainsKey(mapname.ToLower());
        }

        private static List<string> GetAllNewMapFileNames()
        {
            List<string> files = new List<string>();
            string[] subdirectories = Directory.GetDirectories(Setting.NewMapsPath);
            foreach (string directory in subdirectories)
            {
                string uid = Path.GetDirectoryName(directory);
                string[] filesinpath = Directory.GetFiles(directory);
                foreach (string file in filesinpath)
                {
                    string filename = Path.GetFileNameWithoutExtension(file);
                    files.Add(uid + "/" + filename);
                }
            }
            return files;
        }

        private static List<string> GetOwnNewMapFileNames(uint uid)
        {
            List<string> files = new List<string>();
            if (Directory.Exists(Setting.NewMapsPath + uid + "/"))
            {
                string[] filesinpath = Directory.GetFiles(Setting.NewMapsPath + uid + "/");
                foreach (string file in filesinpath)
                {
                    string filename = Path.GetFileNameWithoutExtension(file);
                    files.Add(uid + "/" + filename);
                }
            }
            return files;
        }

        public static void RequestNewMapsList(Client player, bool requestall = false)
        {
            uint uid = player.GetChar().Entity.Id;
            List<string> filenames;
            if (requestall)
                filenames = GetAllNewMapFileNames();
            else
                filenames = GetOwnNewMapFileNames(uid);
            NAPI.ClientEvent.TriggerClientEvent(player, DCustomEvents.RequestNewMapsList, JsonConvert.SerializeObject(filenames));
        }*/
    }
}