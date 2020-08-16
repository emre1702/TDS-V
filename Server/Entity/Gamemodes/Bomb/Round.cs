using AltV.Net;
using AltV.Net.Data;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Default;

namespace TDS_Server.Entity.Gamemodes.Bomb
{
    partial class Bomb
    {
        #region Public Methods

        public override void StartMapChoose()
        {
            base.StartMapChoose();
            if (Map.BombInfo is null)
                return;

            foreach (var bombplace in Map.BombInfo.PlantPositions)
            {
                var pos = new Position(bombplace.X, bombplace.Y, bombplace.Z);
                BombPlantPlaceDto dto = new BombPlantPlaceDto(
                    obj: _tdsObjectHandler.Create(-51423166, pos, new DegreeRotation(), 255, (int)Lobby.Dimension),
                    blip: _tdsBlipHandler.Create(SharedConstants.BombPlantPlaceBlipSprite, pos, name: "Bomb-Plant", dimension: (int)Lobby.Dimension),
                    pos: pos
                );
                _bombPlantPlaces.Add(dto);
            }

            var bombPos = Map.BombInfo.PlantPositions[0];
            _bomb = _tdsObjectHandler.Create(1764669601, new Position(bombPos.X, bombPos.Y, bombPos.Z), new DegreeRotation(), 255, (int)Lobby.Dimension);
        }

        public override void StartMapClear()
        {
            base.StartMapClear();
            foreach (var place in _bombPlantPlaces)
            {
                place.Delete();
            }
            _bombPlantPlaces.Clear();

            if (_bomb != null)
            {
                _bomb.Delete();
                _bomb = null;
            }

            if (_lobbyBombTakeCol.ContainsKey(Lobby))
            {
                _lobbyBombTakeCol.Remove(Lobby, out ITDSColShape? col);
                if (col is { })
                    col.Delete();
                _bombTakeMarker?.Delete();
                _bombTakeMarker = null;
            }
        }

        public override void StartRound()
        {
            base.StartRound();
            Lobby.FuncIterateAllPlayers((character, team) =>
            {
                if (team is null || team.IsSpectator)
                    character.SendMessage(character.Language.ROUND_MISSION_BOMG_SPECTATOR);
                else if (team == _terroristTeam)
                    character.SendMessage(character.Language.ROUND_MISSION_BOMB_BAD);
                else
                    character.SendMessage(character.Language.ROUND_MISSION_BOMB_GOOD);
            });
            if (_bombAtPlayer is null)
                GiveBombToRandomTerrorist();
        }

        public override void StartRoundCountdown()
        {
            base.StartRoundCountdown();
            GiveBombToRandomTerrorist();
        }

        public override void StopRound()
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;

            _bombDetonateTimer?.Kill();
            _bombDetonateTimer = null;

            _bombAtPlayer = null;
            _planter = null;
        }

        #endregion Public Methods
    }
}
