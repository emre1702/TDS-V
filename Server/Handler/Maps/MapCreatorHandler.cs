using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;
using TDS_Shared.Core;
using DB = TDS_Server.Database.Entity;

namespace TDS_Server.Handler.Maps
{
    public class MapCreatorHandler : DatabaseEntityWrapper
    {
        private readonly Serializer _serializer;
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly XmlHelper _xmlHelper;
        private readonly ISettingsHandler _settingsHandler;

        public MapCreatorHandler(Serializer serializer, MapsLoadingHandler mapsLoadingHandler, XmlHelper xmlHelper, ISettingsHandler settingsHandler,
            TDSDbContext dbContext, ILoggingHandler loggingHandler)
            : base(dbContext, loggingHandler)
            => (_serializer, _mapsLoadingHandler, _xmlHelper, _settingsHandler) = (serializer, mapsLoadingHandler, xmlHelper, settingsHandler);

        public async Task<MapCreateError> Create(ITDSPlayer creator, string mapJson, bool onlySave)
        {
            if (creator.Entity is null)
                return MapCreateError.Unknown;
            var serializer = new XmlSerializer(typeof(MapDto));
            try
            {
                MapCreateDataDto mapCreateData;
                try
                {
                    mapCreateData = _serializer.FromBrowser<MapCreateDataDto>(mapJson);
                    if (mapCreateData is null)
                        return MapCreateError.CouldNotDeserialize;
                }
                catch
                {
                    return MapCreateError.CouldNotDeserialize;
                }

                if (_mapsLoadingHandler.GetMapByName(mapCreateData.Name) is { } || _mapsLoadingHandler.GetMapByName(mapCreateData.Name) is { })
                    return MapCreateError.NameAlreadyExists;

                //foreach (var bombPlace in mapCreateData.BombPlaces) 
                //    bombPlace.PosZ -= 1;

                var mapDto = new MapDto(mapCreateData, _serializer);
                mapDto.Info.IsNewMap = true;
                mapDto.Info.CreatorId = creator.Entity.Id;

                mapDto.LoadSyncedData();
                //mapDto.SyncedData.CreatorName = creator.Player.Name;

                string mapFileName = mapDto.Info.Name + "_" + (mapDto.BrowserSyncedData.CreatorName ?? "?") + "_" + Utils.GetTimestamp() + ".map";
                string mapPath = (onlySave ? Constants.SavedMapsPath : Constants.NewMapsPath) + Utils.MakeValidFileName(mapFileName);
                mapDto.Info.FilePath = mapPath;

                MemoryStream memStrm = new MemoryStream();
                UTF8Encoding utf8e = new UTF8Encoding();
                XmlTextWriter xmlSink = new XmlTextWriter(memStrm, utf8e);
                serializer.Serialize(xmlSink, mapDto);
                xmlSink.Dispose();
                byte[] utf8EncodedData = memStrm.ToArray();

                string mapXml = utf8e.GetString(utf8EncodedData);
                string prettyMapXml = await _xmlHelper.GetPrettyAsync(mapXml).ConfigureAwait(true);
                await File.WriteAllTextAsync(mapPath, prettyMapXml).ConfigureAwait(true);

                if (!onlySave)
                {

                    var dbMap = new DB.Rest.Maps { CreatorId = creator.Entity.Id, Name = mapDto.Info.Name };

                    await ExecuteForDBAsync(async dbContext =>
                    {
                        await dbContext.Maps.AddAsync(dbMap);
                        await dbContext.SaveChangesAsync();
                    });
                    

                    mapDto.BrowserSyncedData.Id = dbMap.Id;
                    mapDto.RatingAverage = 5;

                    _mapsLoadingHandler.NewCreatedMaps.Add(mapDto);
                }
                else
                {
                    mapDto.BrowserSyncedData.Id = _mapsLoadingHandler.SavedMaps.Min(m => m.BrowserSyncedData.Id) - 1;
                    mapDto.RatingAverage = 5;

                    _mapsLoadingHandler.SavedMaps.Add(mapDto);
                }

                return MapCreateError.MapCreatedSuccessfully;
            }
            catch
            {
                return MapCreateError.Unknown;
            }
        }

        public void SendPlayerMapForMapCreator(ITDSPlayer player, int mapId)
        {
            if (player.Entity is null)
                return;

            MapDto? map = _mapsLoadingHandler.GetMapById(mapId);

            if (map is null)
                map = _mapsLoadingHandler.NewCreatedMaps.FirstOrDefault(m => mapId == m.BrowserSyncedData.Id);
            if (map is null)
                map = _mapsLoadingHandler.SavedMaps.FirstOrDefault(m => mapId == m.BrowserSyncedData.Id);
            if (map is null)
                map = _mapsLoadingHandler.NeedCheckMaps.FirstOrDefault(m => mapId == m.BrowserSyncedData.Id);

            if (map is null)
                return;

            int posId = 0;

            var mapCreatorData = new MapCreateDataDto
            {
                Id = map.BrowserSyncedData.Id,
                Name = map.BrowserSyncedData.Name,
                Type = (MapType)(int)map.Info.Type,
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

        public void SendPlayerMapNamesForMapCreator(ITDSPlayer player)
        {
            if (player.Entity is null)
                return;

            bool canLoadMapsFromOthers = _settingsHandler.CanLoadMapsFromOthers(player);
            var data = new List<LoadMapDialogGroupDto>
            {
                new LoadMapDialogGroupDto
                {
                    GroupName = "Saved",
                    Maps = _mapsLoadingHandler.SavedMaps
                    .Where(m => m.Info.CreatorId == player.Entity.Id)
                    .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                    .ToList()
                },

                new LoadMapDialogGroupDto
                {
                    GroupName = "Created",
                    Maps = _mapsLoadingHandler.NewCreatedMaps
                    .Where(m => m.Info.CreatorId == player.Entity.Id)
                    .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                    .ToList()
                },

                new LoadMapDialogGroupDto
                {
                    GroupName = "Added",
                    Maps = _mapsLoadingHandler.DefaultMaps
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
                    Maps = _mapsLoadingHandler.NewCreatedMaps
                        .Where(m => m.Info.CreatorId != player.Entity.Id)
                        .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                        .ToList()
                });

                data.Add(new LoadMapDialogGroupDto
                {
                    GroupName = "Deactivated",
                    Maps = _mapsLoadingHandler.NeedCheckMaps
                        .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                        .ToList()
                });
            }

            string json = _serializer.ToBrowser(data.Where(d => d.Maps.Count > 0));
            player.SendEvent(ToClientEvent.LoadMapNamesToLoadForMapCreator, json);
        }

        public async void RemoveMap(ITDSPlayer player, int mapId)
        {
            bool isSavedMap = true;
            MapDto? map = _mapsLoadingHandler.SavedMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
            if (map is null)
            {
                map = _mapsLoadingHandler.NewCreatedMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
                if (map is null)
                    map = _mapsLoadingHandler.NeedCheckMaps.FirstOrDefault(m => m.BrowserSyncedData.Id == mapId);
                isSavedMap = false;
            }

            if (map is null)
                return;

            bool canLoadMapsFromOthers = _settingsHandler.CanLoadMapsFromOthers(player);
            if (map.Info.CreatorId != player.Entity?.Id && !canLoadMapsFromOthers)
                return;
            if (map.Info.CreatorId != player.Entity?.Id)
                LoggingHandler.LogAdmin(LogType.RemoveMap, player, string.Empty, asvip: player.Entity?.IsVip ?? false);

            if (isSavedMap)
                _mapsLoadingHandler.SavedMaps.Remove(map);
            else
            {
                if (_mapsLoadingHandler.NewCreatedMaps.Contains(map))
                    _mapsLoadingHandler.NewCreatedMaps.Remove(map);
                else
                    _mapsLoadingHandler.NeedCheckMaps.Remove(map);

                await ExecuteForDBAsync(async dbContext => await dbContext.Maps.Where(m => m.Id == map.BrowserSyncedData.Id).DeleteFromQueryAsync());
            }

            File.Delete(map.Info.FilePath);
        }

        public void AddedMapRating(MapDto map)
        {
            if (map.Ratings.Count < _settingsHandler.ServerSettings.MapRatingAmountForCheck)
                return;

            if (map.RatingAverage >= _settingsHandler.ServerSettings.MinMapRatingForNewMaps)
                return;

            DisableNewMap(map);
        }

        private void DisableNewMap(MapDto map)
        {
            _mapsLoadingHandler.NewCreatedMaps.Remove(map);

            string fileName = Path.GetFileName(map.Info.FilePath);
            string fileContent = File.ReadAllText(map.Info.FilePath);
            File.WriteAllText(Constants.NeedCheckMapsPath + Utils.MakeValidFileName(fileName), fileContent);

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
