using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.MapHandlers;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Models.Map;
using TDS.Server.Handler;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.GangActionAreaSystem.MapHandlers
{
    internal class BaseAreaMapHandler : IBaseGangActionAreaMapHandler
    {
#nullable disable
        public MapDto Map { get; private set; }
        private IBaseGangActionArea _area;
#nullable restore 

        private readonly List<ITDSBlip> _blips = new();

        private readonly LobbiesHandler _lobbiesHandler;

        public BaseAreaMapHandler(LobbiesHandler lobbiesHandler)
            => (_lobbiesHandler) = (lobbiesHandler);

        public void Init(IBaseGangActionArea area, MapDto map)
        {
            _area = area;
            Map = map;
            CreateMapInfo(_lobbiesHandler.GangLobby.MapHandler.Dimension);

            _area.Events.Conquered += OnConquered;
            _area.Events.CooldownStarted += OnCooldownChanged;
            _area.Events.CooldownEnded += OnCooldownChanged;
            _area.Events.AddedToLobby += OnAddedToLobby;
            _area.Events.RemovedFromLobby += OnRemovedFromLobby;
        }

        private void CreateMapInfo(uint dimension)
        {
            DeleteMapInfo(dimension);
            NAPI.Task.RunSafe(() =>
            {
                var blip = (ITDSBlip)NAPI.Blip.CreateBlip(
                    sprite: 84,
                    position: Map.Target!.ToVector3(),
                    scale: 3f,
                    color: _area.Owner?.Entity.BlipColor ?? 4,
                    name: Map.Info.Name,
                    alpha: (byte)(_area.StartRequirements.HasCooldown ? 120 : 255),
                    drawDistance: 100f,
                    shortRange: true,
                    dimension: dimension);
                lock (_blips)
                    _blips.Add(blip);

            });
        }

        private void DeleteMapInfo(uint dimension)
        {
            lock (_blips)
            {
                var blip = _blips.FirstOrDefault(b => b.Dimension == dimension);
                if (blip is { })
                {
                    NAPI.Task.RunSafe(() => blip.Delete());
                    _blips.Remove(blip);
                }
            }     
        }

        private void OnConquered(IGang _, IGang? _2)
            => CreateMapInfo(_lobbiesHandler.GangLobby.MapHandler.Dimension);

        private void OnCooldownChanged()
            => CreateMapInfo(_lobbiesHandler.GangLobby.MapHandler.Dimension);

        private void OnAddedToLobby(IGangActionLobby lobby)
            => CreateMapInfo(lobby.MapHandler.Dimension);

        private void OnRemovedFromLobby(IGangActionLobby lobby)
            => DeleteMapInfo(lobby.MapHandler.Dimension);
    }
}
