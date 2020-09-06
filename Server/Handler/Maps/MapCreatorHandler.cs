using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;

using DB = TDS_Server.Database.Entity;

namespace TDS_Server.Handler.Maps
{
    public class MapCreatorHandler : DatabaseEntityWrapper
    {
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;
        private readonly XmlHelper _xmlHelper;

        public MapCreatorHandler(Serializer serializer, MapsLoadingHandler mapsLoadingHandler, XmlHelper xmlHelper, ISettingsHandler settingsHandler,
            TDSDbContext dbContext, ILoggingHandler loggingHandler)
            : base(dbContext, loggingHandler)
        {
            (_serializer, _mapsLoadingHandler, _xmlHelper, _settingsHandler) = (serializer, mapsLoadingHandler, xmlHelper, settingsHandler);
            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.RemoveMap, this, RemoveMap);
        }

        public void AddedMapRating(MapDto map)
        {
            if (map.Ratings.Count < _settingsHandler.ServerSettings.MapRatingAmountForCheck)
                return;

            if (map.RatingAverage >= _settingsHandler.ServerSettings.MinMapRatingForNewMaps)
                return;

            DisableNewMap(map);
        }

        public async Task<object?> Create(ITDSPlayer creator, ArraySegment<object> args)
        {
            try
            {
                var result = await SaveOrCreate(creator, (string)args[0], Constants.NewMapsPath);
                if (result.Item2 != MapCreateError.MapCreatedSuccessfully || result.Item1 is null)
                    return result;

                result.Item1.BrowserSyncedData.Id = _mapsLoadingHandler.SavedMaps.Min(m => m.BrowserSyncedData.Id) - 1;
                result.Item1.RatingAverage = 5;

                _mapsLoadingHandler.SavedMaps.Add(result.Item1);

                _mapsLoadingHandler.NewCreatedMaps.Add(result.Item1);
                return MapCreateError.MapCreatedSuccessfully;
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, creator);
                return MapCreateError.Unknown;
            }
        }

        public async void RemoveMap(ITDSPlayer player, int mapId)
        {
            if (!player.LoggedIn)
                return;

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

                await ExecuteForDBAsync(async dbContext =>
                {
                    var maps = await dbContext.Maps.Where(m => m.Id == map.BrowserSyncedData.Id).ToListAsync();
                    dbContext.RemoveRange(maps);
                    await dbContext.SaveChangesAsync();
                });
            }

            File.Delete(map.Info.FilePath);
        }

        public async Task<object?> Save(ITDSPlayer creator, ArraySegment<object> args)
        {
            try
            {
                var result = await SaveOrCreate(creator, (string)args[0], Constants.SavedMapsPath);
                if (result.Item2 != MapCreateError.MapCreatedSuccessfully || result.Item1 is null)
                    return result;

                var dbMap = new DB.Rest.Maps { CreatorId = creator.Entity!.Id, Name = result.Item1.Info.Name };

                await ExecuteForDBAsync(async dbContext =>
                {
                    await dbContext.Maps.AddAsync(dbMap);
                    await dbContext.SaveChangesAsync();
                });

                result.Item1.BrowserSyncedData.Id = dbMap.Id;
                result.Item1.RatingAverage = 5;

                _mapsLoadingHandler.NewCreatedMaps.Add(result.Item1);
                return MapCreateError.MapCreatedSuccessfully;
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, creator);
                return MapCreateError.Unknown;
            }
        }

        public object? SendPlayerMapForMapCreator(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (player.Entity is null)
                return null;

            int mapId = (int)args[0];

            MapDto? map = _mapsLoadingHandler.GetMapById(mapId);

            if (map is null)
                map = _mapsLoadingHandler.NewCreatedMaps.FirstOrDefault(m => mapId == m.BrowserSyncedData.Id);
            if (map is null)
                map = _mapsLoadingHandler.SavedMaps.FirstOrDefault(m => mapId == m.BrowserSyncedData.Id);
            if (map is null)
                map = _mapsLoadingHandler.NeedCheckMaps.FirstOrDefault(m => mapId == m.BrowserSyncedData.Id);

            if (map is null)
                return null;

            int posId = 0;

            var mapCreatorData = new MapCreateDataDto
            {
                Id = map.BrowserSyncedData.Id,
                Name = map.BrowserSyncedData.Name,
                Type = (MapType)(int)map.Info.Type,
                BombPlaces = map.BombInfo?.PlantPositions?.Select(pos => pos.ToMapCreatorPosition(posId++, MapCreatorPositionType.BombPlantPlace)).ToList(),
                MapCenter = map.LimitInfo.Center?.ToMapCreatorPosition(posId++, MapCreatorPositionType.MapCenter),
                MapEdges = map.LimitInfo.Edges?.Select(pos => pos.ToMapCreatorPosition(posId++, MapCreatorPositionType.MapLimit)).ToList(),
                Settings = new MapCreateSettings
                {
                    MinPlayers = map.Info.MinPlayers,
                    MaxPlayers = map.Info.MaxPlayers,
                },
                TeamSpawns = map.TeamSpawnsList.TeamSpawns.Select((t, teamNumber) => t.Spawns.Select(pos => pos.ToMapCreatorPosition(posId++, teamNumber)).ToList()).ToList(),
                Objects = map.Objects?.Entries?.Select(o => o.ToMapCreatorPosition(posId++, MapCreatorPositionType.Object)).ToList(),
                Vehicles = map.Vehicles?.Entries?.Select(o => o.ToMapCreatorPosition(posId++, MapCreatorPositionType.Vehicle)).ToList(),
                Target = map.Target?.ToMapCreatorPosition(posId++, MapCreatorPositionType.Target),
                Description = new Dictionary<int, string>
                {
                    [(int)Language.English] = map.Descriptions != null ? Regex.Replace(map.Descriptions.English ?? string.Empty, @"\r\n?|\n", "\\n") : string.Empty,
                    [(int)Language.German] = map.Descriptions != null ? Regex.Replace(map.Descriptions.German ?? string.Empty, @"\r\n?|\n", "\\n") : string.Empty
                }
            };

            ((MapCreateLobby)player.Lobby!).SetMap(mapCreatorData);
            return null;
        }

        public object? SendPlayerMapNamesForMapCreator(ITDSPlayer player, ref ArraySegment<object> _)
        {
            if (player.Entity is null)
                return null;

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

            return _serializer.ToBrowser(data.Where(d => d.Maps.Count > 0));
        }

        public object? SyncCurrentMapToClient(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (!(player.Lobby is MapCreateLobby lobby))
                return null;

            string json = (string)args[0];
            int tdsPlayerId = Convert.ToInt32(args[1]);
            int idCounter = Convert.ToInt32(args[2]);

            lobby.SyncCurrentMapToPlayer(json, tdsPlayerId, idCounter);
            return null;
        }

        private void DisableNewMap(MapDto map)
        {
            _mapsLoadingHandler.NewCreatedMaps.Remove(map);

            string fileName = Path.GetFileName(map.Info.FilePath);
            string fileContent = File.ReadAllText(map.Info.FilePath);
            File.WriteAllText(Constants.NeedCheckMapsPath + Utils.MakeValidFileName(fileName), fileContent);
        }

        private async Task<(MapDto?, MapCreateError)> SaveOrCreate(ITDSPlayer creator, string mapJson, string mapBasePath)
        {
            if (creator.Entity is null)
                return (null, MapCreateError.Unknown);
            var serializer = new XmlSerializer(typeof(MapDto));
            try
            {
                MapCreateDataDto mapCreateData;
                try
                {
                    mapCreateData = _serializer.FromBrowser<MapCreateDataDto>(mapJson);
                    if (mapCreateData is null)
                        return (null, MapCreateError.CouldNotDeserialize);
                }
                catch
                {
                    return (null, MapCreateError.CouldNotDeserialize);
                }

                if (_mapsLoadingHandler.GetMapByName(mapCreateData.Name) is { } || _mapsLoadingHandler.GetMapByName(mapCreateData.Name) is { })
                    return (null, MapCreateError.NameAlreadyExists);

                //foreach (var bombPlace in mapCreateData.BombPlaces)
                //    bombPlace.PosZ -= 1;

                var mapDto = new MapDto(mapCreateData, _serializer);
                mapDto.Info.IsNewMap = true;
                mapDto.Info.CreatorId = creator.Entity.Id;

                mapDto.LoadSyncedData();
                //mapDto.SyncedData.CreatorName = creator.Player.Name;

                string mapFileName = mapDto.Info.Name + "_" + (mapDto.BrowserSyncedData.CreatorName ?? "?") + "_" + Utils.GetTimestamp() + ".map";
                string mapPath = mapBasePath + Utils.MakeValidFileName(mapFileName);
                mapDto.Info.FilePath = mapPath;

                var memStrm = new MemoryStream();
                var utf8e = new UTF8Encoding();
                var xmlSink = new XmlTextWriter(memStrm, utf8e);
                serializer.Serialize(xmlSink, mapDto);
                xmlSink.Dispose();
                byte[] utf8EncodedData = memStrm.ToArray();

                var mapXml = utf8e.GetString(utf8EncodedData);
                var prettyMapXml = await _xmlHelper.GetPrettyAsync(mapXml).ConfigureAwait(true);
                await File.WriteAllTextAsync(mapPath, prettyMapXml).ConfigureAwait(true);

                return (mapDto, MapCreateError.MapCreatedSuccessfully);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, creator);
                return (null, MapCreateError.Unknown);
            }
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
