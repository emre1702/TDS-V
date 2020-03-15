using GTANetworkAPI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TDS_Common.Default;
using TDS_Shared.Data.Enums;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Helper;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server.Database.Entity;

using DB = TDS_Server.Database.Entity;
using System.Text.RegularExpressions;
using TDS_Common.Dto.Map.Creator;
using TDS_Server.Instance.LobbyInstances;

namespace TDS_Server.Core.Manager.Maps
{
    class MapCreator
    {
        public static IEnumerable<MapDto> AllCreatingMaps => NewCreatedMaps.Union(NeedCheckMaps);    // .Union(_savedMaps)

        public static List<MapDto> NewCreatedMaps = new List<MapDto>();
        private static List<MapDto> _savedMaps = new List<MapDto>();
        public static List<MapDto> NeedCheckMaps = new List<MapDto>();

        public async static Task<EMapCreateError> Create(TDSPlayer creator, string mapJson, bool onlySave)
        {
            if (creator.Entity is null)
                return EMapCreateError.Unknown;
            var serializer = new XmlSerializer(typeof(MapDto));
            try
            {
                MapCreateDataDto mapCreateData;
                try
                {
                    mapCreateData = Serializer.FromBrowser<MapCreateDataDto>(mapJson);
                    if (mapCreateData is null)
                        return EMapCreateError.CouldNotDeserialize;
                }
                catch
                {
                    return EMapCreateError.CouldNotDeserialize;
                }

                if (GetMapByName(mapCreateData.Name) != null || MapsLoader.GetMapByName(mapCreateData.Name) != null)
                    return EMapCreateError.NameAlreadyExists;

                //foreach (var bombPlace in mapCreateData.BombPlaces) 
                //    bombPlace.PosZ -= 1;

                var mapDto = new MapDto(mapCreateData);
                mapDto.Info.IsNewMap = true;
                mapDto.Info.CreatorId = creator.Entity.Id;

                mapDto.LoadSyncedData();
                //mapDto.SyncedData.CreatorName = creator.Player.Name;

                string mapFileName = mapDto.Info.Name + "_" + (mapDto.BrowserSyncedData.CreatorName ?? "?") + "_" + Utils.GetTimestamp() + ".map";
                string mapPath = (onlySave ? ServerConstants.SavedMapsPath : ServerConstants.NewMapsPath) + Utils.MakeValidFileName(mapFileName);
                mapDto.Info.FilePath = mapPath;

                MemoryStream memStrm = new MemoryStream();
                UTF8Encoding utf8e = new UTF8Encoding();
                XmlTextWriter xmlSink = new XmlTextWriter(memStrm, utf8e);
                serializer.Serialize(xmlSink, mapDto);
                xmlSink.Dispose();
                byte[] utf8EncodedData = memStrm.ToArray();

                string mapXml = utf8e.GetString(utf8EncodedData);
                string prettyMapXml = await XmlHelper.GetPrettyAsync(mapXml).ConfigureAwait(true);
                await File.WriteAllTextAsync(mapPath, prettyMapXml).ConfigureAwait(true);

                if (!onlySave)
                {
                    using var dbContext = new TDSDbContext();
                    var dbMap = new DB.Rest.Maps { CreatorId = creator.Entity.Id, Name = mapDto.Info.Name };
                    await dbContext.Maps.AddAsync(dbMap);
                    await dbContext.SaveChangesAsync().ConfigureAwait(true);

                    mapDto.BrowserSyncedData.Id = dbMap.Id;
                    mapDto.RatingAverage = 5;

                    NewCreatedMaps.Add(mapDto);
                }
                else
                {
                    mapDto.BrowserSyncedData.Id = _savedMaps.Min(m => m.BrowserSyncedData.Id) - 1;
                    mapDto.RatingAverage = 5;

                    _savedMaps.Add(mapDto);
                }

                return EMapCreateError.MapCreatedSuccessfully;
            }
            catch
            {
                return EMapCreateError.Unknown;
            }
        }

        public static async Task LoadNewMaps(TDSDbContext dbContext, List<DB.Rest.Maps> allDbMaps)
        {
            NewCreatedMaps = await MapsLoader.LoadMaps(dbContext, ServerConstants.NewMapsPath, false, allDbMaps).ConfigureAwait(false);
            foreach (var map in NewCreatedMaps)
            {
                // Player shouldn't be able to see the creator of the map (so they don't rate it depending of the creator)
                map.BrowserSyncedData.CreatorName = string.Empty;
                map.Info.IsNewMap = true;
            }
        }

        public static async Task LoadSavedMaps(TDSDbContext dbContext, List<DB.Rest.Maps> allDbMaps)
        {
            _savedMaps = await MapsLoader.LoadMaps(dbContext, ServerConstants.SavedMapsPath, true, allDbMaps).ConfigureAwait(false);
            foreach (var map in _savedMaps)
            {
                // Player shouldn't be able to see the creator of the map (so they don't rate it depending of the creator)
                map.Info.IsNewMap = true;
            }
        }

        public static async Task LoadNeedCheckMaps(TDSDbContext dbContext, List<DB.Rest.Maps> allDbMaps)
        {
            NeedCheckMaps = await MapsLoader.LoadMaps(dbContext, ServerConstants.NeedCheckMapsPath, false, allDbMaps).ConfigureAwait(false);
            foreach (var map in NewCreatedMaps)
            {
                map.Info.IsNewMap = true;
            }
        }

        public static MapDto? GetRandomNewMap()
        {
            if (NewCreatedMaps.Count == 0)
                return null;
            var list = NewCreatedMaps.Where(m => m.Ratings.Count < SettingsManager.MapRatingAmountForCheck).ToList();
            if (list.Count == 0)
                return null;
            return list[CommonUtils.Rnd.Next(list.Count)];
        }

        public static MapDto? GetMapById(int mapId)
        {
            return NewCreatedMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
        }

        public static MapDto? GetMapByName(string mapName)
        {
            return NewCreatedMaps.FirstOrDefault(m => m.Info.Name == mapName);
        }

        public static void SendPlayerMapForMapCreator(TDSPlayer player, int mapId)
        {
            if (player.Entity is null)
                return;
            
            MapDto? map = MapsLoader.GetMapById(mapId);

            if (map is null)
                map = NewCreatedMaps.FirstOrDefault(m => mapId == m.BrowserSyncedData.Id);
            if (map is null)
                map = _savedMaps.FirstOrDefault(m => mapId == m.BrowserSyncedData.Id);
            if (map is null)
                map = NeedCheckMaps.FirstOrDefault(m => mapId == m.BrowserSyncedData.Id);

            if (map is null)
                return;

            int posId = 0;

            var mapCreatorData = new MapCreateDataDto
            {
                Id = map.BrowserSyncedData.Id,
                Name = map.BrowserSyncedData.Name,
                Type = (EMapType)(int)map.Info.Type,
                BombPlaces = map.BombInfo?.PlantPositions?.Select(pos => pos.ToMapCreatorPosition(posId++)).ToList(),
                MapCenter = map.LimitInfo.Center?.ToMapCreatorPosition(posId++),
                MapEdges = map.LimitInfo.Edges?.Select(pos => pos.ToMapCreatorPosition(posId++)).ToList(),
                Settings = new MapCreateSettings
                {
                    MinPlayers = map.Info.MinPlayers,
                    MaxPlayers = map.Info.MaxPlayers,
                },
                TeamSpawns = map.TeamSpawnsList.TeamSpawns.Select((t, teamNumber) => t.Spawns.Select(pos => pos.ToMapCreatorPosition(posId++, teamNumber)).ToList()).ToList(),
                Objects = map.Objects?.Entries?.Select(o => o.ToMapCreatorPosition(posId++)).ToList(),
                Vehicles = map.Vehicles?.Entries?.Select(o => o.ToMapCreatorPosition(posId++)).ToList(),
                Target = map.Target?.ToMapCreatorPosition(posId++),
                Description = new Dictionary<int, string> 
                { 
                    [(int)Language.English] = map.Descriptions != null ? Regex.Replace(map.Descriptions.English ?? string.Empty, @"\r\n?|\n", "\\n") : string.Empty, 
                    [(int)Language.German] = map.Descriptions != null ? Regex.Replace(map.Descriptions.German ?? string.Empty, @"\r\n?|\n", "\\n") : string.Empty
                }

            };

            ((MapCreateLobby)player.Lobby!).SetMap(mapCreatorData);
        }

        public static void SendPlayerMapNamesForMapCreator(TDSPlayer player)
        {
            if (player.Entity is null)
                return;

            bool canLoadMapsFromOthers = SettingsManager.CanLoadMapsFromOthers(player);
            var data = new List<LoadMapDialogGroupDto>
            {
                new LoadMapDialogGroupDto
                {
                    GroupName = "Saved",
                    Maps = _savedMaps
                    .Where(m => m.Info.CreatorId == player.Entity.Id)
                    .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                    .ToList()
                },

                new LoadMapDialogGroupDto
                {
                    GroupName = "Created",
                    Maps = NewCreatedMaps
                    .Where(m => m.Info.CreatorId == player.Entity.Id)
                    .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                    .ToList()
                },

                new LoadMapDialogGroupDto
                {
                    GroupName = "Added",
                    Maps = MapsLoader.AllMaps
                    .Where(m => m.Info.CreatorId == player.Entity.Id)
                    .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                    .ToList()
                }
            };

            if (canLoadMapsFromOthers)
            { 
                data.Add(new LoadMapDialogGroupDto
                {
                    GroupName = "OthersCreated",
                    Maps = NewCreatedMaps
                        .Where(m => m.Info.CreatorId != player.Entity.Id)
                        .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                        .ToList()
                });

                data.Add(new LoadMapDialogGroupDto
                {
                    GroupName = "Deactivated",
                    Maps = NeedCheckMaps
                        .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                        .ToList()
                });
            }

            string json = Serializer.ToBrowser(data.Where(d => d.Maps.Count > 0));
            NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.LoadMapNamesToLoadForMapCreator, json);
        }

        public static async void RemoveMap(TDSPlayer player, int mapId)
        {
            bool isSavedMap = true;
            MapDto? map = _savedMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
            if (map is null)
            {
                map = NewCreatedMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
                if (map is null)
                    map = NeedCheckMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
                isSavedMap = false;
            }

            if (map is null)
                return;

            bool canLoadMapsFromOthers = SettingsManager.CanLoadMapsFromOthers(player);
            if (map.Info.CreatorId != player.Entity?.Id && !canLoadMapsFromOthers)
                return;
            if (map.Info.CreatorId != player.Entity?.Id)
                AdminLogsManager.Log(ELogType.RemoveMap, player, string.Empty, asvip: player.Entity?.IsVip ?? false);

            if (isSavedMap)
                _savedMaps.Remove(map);
            else 
            {
                if (NewCreatedMaps.Contains(map))
                    NewCreatedMaps.Remove(map);
                else
                    NeedCheckMaps.Remove(map);
                using var dbContext = new TDSDbContext();
                await dbContext.Maps.Where(m => m.Id == map.BrowserSyncedData.Id).DeleteFromQueryAsync();
            } 

            File.Delete(map.Info.FilePath);
        }

        public static void AddedMapRating(MapDto map)
        {
            if (map.Ratings.Count < SettingsManager.MapRatingAmountForCheck)
                return;

            if (map.RatingAverage >= SettingsManager.MinMapRatingForNewMaps)
                return;

            DisableNewMap(map);
        }

        private static void DisableNewMap(MapDto map)
        {
            NewCreatedMaps.Remove(map);

            string fileName = Path.GetFileName(map.Info.FilePath);
            string fileContent = File.ReadAllText(map.Info.FilePath);
            File.WriteAllText(ServerConstants.NeedCheckMapsPath + Utils.MakeValidFileName(fileName), fileContent);

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
                using (StreamWriter writer = File.CreateText(path + DateTime.UtcNow + ".xml"))
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

        public static void RequestNewMapsList(Player player, bool requestall = false)
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
