using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Map;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.FakePickups;
using TDS.Shared.Data.Default;

namespace TDS.Server.GamemodesSystem.MapHandlers
{
    public class BombMapHandler : BaseGamemodeMapHandler, IBombGamemodeMapHandler
    {
        public List<BombPlantPlaceDto> BombPlantPlaces { get; } = new List<BombPlantPlaceDto>();
        private FakePickup? _bombTakePickup;

        private readonly IRoundFightLobby _lobby;
        private readonly IBombGamemode _gamemode;
        private readonly FakePickupsHandler _fakePickupsHandler;

        public BombMapHandler(IRoundFightLobby lobby, IBombGamemode gamemode, FakePickupsHandler fakePickupsHandler)
        {
            _lobby = lobby;
            _gamemode = gamemode;
            _fakePickupsHandler = fakePickupsHandler;

            InitNewMap(lobby.CurrentMap);
        }

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);
            events.RoundClear += RoundClear;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);
            if (events.RoundClear is { })
                events.RoundClear -= RoundClear;
        }

        public void CreateBombTakePickup(ITDSObject bomb)
        {
            _bombTakePickup = _fakePickupsHandler.Create(-51423166, bomb.Position, 2f, 0, _lobby);
            _bombTakePickup.OnCollect = TakeDroppedBomb;
        }

        public void DeleteBombTakePickup()
        {
            _bombTakePickup?.Delete();
            _bombTakePickup = null;
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
                    lock (BombPlantPlaces) { BombPlantPlaces.Add(dto); }
                }
            });
        }

        private ValueTask RoundClear()
        {
            NAPI.Task.RunSafe(() =>
            {
                lock (BombPlantPlaces)
                {
                    foreach (var bombPlantPlace in BombPlantPlaces)
                        bombPlantPlace?.Delete();
                    BombPlantPlaces.Clear();
                }
                
                DeleteBombTakePickup();
            });

            return default;
        }

        private void TakeDroppedBomb(ITDSPlayer player, CancelEventArgs cancelEventArgs)
        {
            if (player.Lifes <= 0 || player.Team != _gamemode.Teams.Terrorists)
            { 
                cancelEventArgs.Cancel = true;
                return;
            }

            _gamemode.Specials.TakeBomb(player);
            DeleteBombTakePickup();
        }
    }
}
