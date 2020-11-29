using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.MapHandlers;
using TDS.Server.Data.Interfaces.GangsSystem;
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

        private ITDSBlip? _blip;

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
        }

        private void CreateMapInfo(uint dimension)
        {
            NAPI.Task.RunSafe(() =>
            {
                _blip = NAPI.Blip.CreateBlip(
                    sprite: 84,
                    position: Map.Target!.ToVector3(),
                    scale: 3f,
                    color: _area.Owner?.Entity.BlipColor ?? 4,
                    name: Map.Info.Name,
                    alpha: (byte)(_area.StartRequirements.HasCooldown ? 120 : 255),
                    drawDistance: 100f,
                    shortRange: true,
                    dimension: dimension) as ITDSBlip;
            });
        }

        private void OnConquered(IGang _, IGang? _2)
            => CreateMapInfo(_lobbiesHandler.GangLobby.MapHandler.Dimension);

        private void OnCooldownChanged()
            => CreateMapInfo(_lobbiesHandler.GangLobby.MapHandler.Dimension);
    }
}
