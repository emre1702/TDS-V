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
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Map;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.Map.Creator;
using TDS.Shared.Default;

using DB = TDS.Server.Database.Entity;

namespace TDS.Server.Handler.Maps
{
    public class MapCreatorHandler : DatabaseEntityWrapper
    {
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly XmlHelper _xmlHelper;

        public MapCreatorHandler(MapsLoadingHandler mapsLoadingHandler, XmlHelper xmlHelper, ISettingsHandler settingsHandler,
            TDSDbContext dbContext, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(dbContext)
        {
            (_mapsLoadingHandler, _xmlHelper, _settingsHandler) = (mapsLoadingHandler, xmlHelper, settingsHandler);

            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.RemoveMap, this, RemoveMap);

            remoteBrowserEventsHandler.Add(ToServerEvent.SaveMapCreatorData, Save);
            remoteBrowserEventsHandler.Add(ToServerEvent.SendMapCreatorData, Create);
            remoteBrowserEventsHandler.Add(ToServerEvent.LoadMapNamesToLoadForMapCreator, SendPlayerMapNamesForMapCreator);
            remoteBrowserEventsHandler.Add(ToServerEvent.LoadMapForMapCreator, SendPlayerMapForMapCreator, player => player.Entity is not null);
        }

        public void AddedMapRating(MapDto map)
        {
            lock (map.Ratings)
            {
                if (map.Ratings.Count < _settingsHandler.ServerSettings.MapRatingAmountForCheck)
                    return;
            }

            if (map.RatingAverage >= _settingsHandler.ServerSettings.MinMapRatingForNewMaps)
                return;

            DisableNewMap(map);
        }

        private async Task<object?> Save(RemoteBrowserEventArgs args)
        {
            try
            {
                var result = await SaveOrCreate(args.Player, (string)args.Args[0], Constants.SavedMapsPath).ConfigureAwait(false);
                if (result.Item2 != MapCreateError.MapCreatedSuccessfully || result.Item1 is null)
                    return result;

                // lock to ensure id is added to the list in _mapsLoadingHandler first
                lock (_mapsLoadingHandler)
                {
                    result.Item1.BrowserSyncedData.Id = _mapsLoadingHandler.GetNextNewCreatedMapId();
                    result.Item1.RatingAverage = 5;
                    _mapsLoadingHandler.AddSavedMap(result.Item1);
                }

                return MapCreateError.MapCreatedSuccessfully;
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex, args.Player);
                return MapCreateError.Unknown;
            }
        }

        public async void RemoveMap(ITDSPlayer player, int mapId)
        {
            try
            {
                if (!player.LoggedIn)
                    return;

                bool isSavedMap = true;
                MapDto? map = _mapsLoadingHandler.GetSavedMap(mapId);
                if (map is null)
                {
                    map = _mapsLoadingHandler.GetNewCreatedMap(mapId);
                    if (map is null)
                        map = _mapsLoadingHandler.GetNeedCheckMap(mapId);
                    isSavedMap = false;
                }

                if (map is null)
                    return;

                bool canLoadMapsFromOthers = _settingsHandler.CanLoadMapsFromOthers(player);
                if (map.Info.CreatorId != player.Entity?.Id && !canLoadMapsFromOthers)
                    return;
                if (map.Info.CreatorId != player.Entity?.Id)
                    LoggingHandler.Instance.LogAdmin(LogType.RemoveMap, player, string.Empty, asvip: player.Entity?.IsVip ?? false);

                if (isSavedMap)
                    _mapsLoadingHandler.RemoveSavedMap(map);
                else
                {
                    if (!_mapsLoadingHandler.RemoveNewCreatedMap(map))
                        _mapsLoadingHandler.RemoveNeedCheckMap(map);

                    await ExecuteForDBAsync(async dbContext =>
                    {
                        var maps = await dbContext.Maps.Where(m => m.Id == map.BrowserSyncedData.Id).ToListAsync().ConfigureAwait(false);
                        dbContext.RemoveRange(maps);
                        await dbContext.SaveChangesAsync().ConfigureAwait(false);
                    }).ConfigureAwait(false);
                }

                File.Delete(map.Info.FilePath);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private async Task<object?> Create(RemoteBrowserEventArgs args)
        {
            try
            {
                var result = await SaveOrCreate(args.Player, (string)args.Args[0], Constants.NewMapsPath).ConfigureAwait(false);
                if (result.Item2 != MapCreateError.MapCreatedSuccessfully || result.Item1 is null)
                    return result;

                var dbMap = new DB.Rest.Maps { CreatorId = args.Player.Entity!.Id, Name = result.Item1.Info.Name };

                await ExecuteForDBAsync(async dbContext =>
                {
                    dbContext.Maps.Add(dbMap);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);

                result.Item1.BrowserSyncedData.Id = dbMap.Id;
                result.Item1.RatingAverage = 5;

                _mapsLoadingHandler.AddNewCreatedMap(result.Item1);
                return MapCreateError.MapSavedSuccessfully;
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex, args.Player);
                return MapCreateError.Unknown;
            }
        }

        private object? SendPlayerMapForMapCreator(RemoteBrowserEventArgs args)
        {
            int mapId = (int)args.Args[0];

            MapDto? map = _mapsLoadingHandler.GetMapById(mapId);

            if (map is null)
                map = _mapsLoadingHandler.GetNewCreatedMap(mapId);
            if (map is null)
                map = _mapsLoadingHandler.GetSavedMap(mapId);
            if (map is null)
                map = _mapsLoadingHandler.GetNeedCheckMap(mapId);

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

            ((IMapCreatorLobby)args.Player.Lobby!).Sync.SetMap(mapCreatorData);
            return null;
        }

        private object? SendPlayerMapNamesForMapCreator(RemoteBrowserEventArgs args)
        {
            if (args.Player.Entity is null)
                return null;

            bool canLoadMapsFromOthers = _settingsHandler.CanLoadMapsFromOthers(args.Player);
            var data = new List<LoadMapDialogGroupDto>
            {
                new LoadMapDialogGroupDto
                {
                    GroupName = "Saved",
                    Maps = _mapsLoadingHandler.GetSavedMaps()
                        .Where(m => m.Info.CreatorId == args.Player.Entity.Id)
                        .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                },

                new LoadMapDialogGroupDto
                {
                    GroupName = "Created",
                    Maps = _mapsLoadingHandler.GetNewCreatedMaps()
                        .Where(m => m.Info.CreatorId == args.Player.Entity.Id)
                        .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                },

                new LoadMapDialogGroupDto
                {
                    GroupName = "Added",
                    Maps = _mapsLoadingHandler.GetDefaultMaps()
                        .Where(m => m.Info.CreatorId == args.Player.Entity.Id)
                        .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                }
            };

            if (canLoadMapsFromOthers)
            {
                data.Add(new LoadMapDialogGroupDto
                {
                    GroupName = "OthersCreated",
                    Maps = _mapsLoadingHandler.GetNewCreatedMaps()
                        .Where(m => m.Info.CreatorId != args.Player.Entity.Id)
                        .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                });

                data.Add(new LoadMapDialogGroupDto
                {
                    GroupName = "Deactivated",
                    Maps = _mapsLoadingHandler.GetNeedCheckMaps()
                        .Select(m => new LoadMapDialogMapDto { Id = m.BrowserSyncedData.Id, Name = m.Info.Name })
                });
            }

            return Serializer.ToBrowser(data.Where(d => d.Maps.Any()));
        }

        private void DisableNewMap(MapDto map)
        {
            _mapsLoadingHandler.RemoveNewCreatedMap(map);

            var fileName = Path.GetFileName(map.Info.FilePath);
            var fileContent = File.ReadAllText(map.Info.FilePath);
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
                    mapCreateData = Serializer.FromBrowser<MapCreateDataDto>(mapJson);
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

                var mapDto = new MapDto(mapCreateData);
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
                var prettyMapXml = await _xmlHelper.GetPrettyAsync(mapXml).ConfigureAwait(false);
                await File.WriteAllTextAsync(mapPath, prettyMapXml).ConfigureAwait(false);

                return (mapDto, MapCreateError.MapCreatedSuccessfully);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex, creator);
                LoggingHandler.Instance.AddErrorFile(creator.Name + "_" + Utils.GetTimestamp() + ".map-saveorcreate.json", mapJson);
                return (null, MapCreateError.Unknown);
            }
        }
    }
}