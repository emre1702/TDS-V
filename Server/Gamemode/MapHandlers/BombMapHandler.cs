using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Data.Default;

namespace TDS_Server.GamemodesSystem.MapHandlers
{
    public class BombMapHandler : BaseGamemodeMapHandler, IBombGamemodeMapHandler
    {
        public List<BombPlantPlaceDto> BombPlantPlaces { get; } = new List<BombPlantPlaceDto>();
        private ITDSColshape? _lobbyBombTakeCol;
        private ITDSMarker? _bombTakeMarker;

        private readonly IRoundFightLobby _lobby;
        private readonly IBombGamemode _gamemode;

        public BombMapHandler(IRoundFightLobby lobby, IBombGamemode gamemode)
        {
            _lobby = lobby;
            _gamemode = gamemode;

            InitNewMap(lobby.CurrentMap);
        }

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);
            events.RoundClear += RoundClear;
            events.PlayerEnteredColshape += OnPlayerEnterColshape;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);
            if (events.RoundClear is { })
                events.RoundClear -= RoundClear;
            events.PlayerEnteredColshape -= OnPlayerEnterColshape;
        }

        public void CreateBombTakeMarker(ITDSObject bomb)
        {
            NAPI.Task.RunSafe(() =>
            {
                _bombTakeMarker = NAPI.Marker.CreateMarker(0, bomb.Position, new Vector3(), new Vector3(), 1,
                                                        new Color(180, 0, 0, 180), true, _lobby.MapHandler.Dimension) as ITDSMarker;
                var bombTakeCol = NAPI.ColShape.CreateSphereColShape(bomb.Position, 2, _lobby.MapHandler.Dimension) as ITDSColshape;
                _lobbyBombTakeCol = bombTakeCol;
            });
        }

        public void DeleteBombTakeMarker()
        {
            NAPI.Task.RunSafe(() =>
            {
                _bombTakeMarker?.Delete();
                _bombTakeMarker = null;
                _lobbyBombTakeCol?.Delete();
                _lobbyBombTakeCol = null;
            });
        }

        private void InitNewMap(MapDto map)
        {
            if (map.BombInfo is null)
                return;

            NAPI.Task.RunSafe(() =>
            {
                foreach (var bombplace in map.BombInfo.PlantPositions)
                {
                    var pos = new Vector3(bombplace.X, bombplace.Y, bombplace.Z);
                    var dto = new BombPlantPlaceDto(
                        obj: NAPI.Object.CreateObject(-51423166, pos, new Vector3(), 255, _lobby.MapHandler.Dimension) as ITDSObject,
                        blip: NAPI.Blip.CreateBlip(SharedConstants.BombPlantPlaceBlipSprite, pos, 1f, 0, name: "Bomb-Plant", dimension: _lobby.MapHandler.Dimension) as ITDSBlip,
                        pos: pos
                    );
                    BombPlantPlaces.Add(dto);
                }
            });
        }

        private ValueTask RoundClear()
        {
            NAPI.Task.RunSafe(() =>
            {
                foreach (var bombPlantPlace in BombPlantPlaces)
                    bombPlantPlace?.Delete();
                BombPlantPlaces.Clear();
                DeleteBombTakeMarker();
            });

            return default;
        }

        private void OnPlayerEnterColshape(ITDSColshape colshape, ITDSPlayer player)
        {
            if (_lobbyBombTakeCol == colshape)
                if (player.Lifes > 0 && player.Team == _gamemode.Teams.Terrorists)
                    _gamemode.Specials.TakeBomb(player);
        }
    }
}
