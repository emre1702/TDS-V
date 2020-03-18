﻿using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Entities.GameModes.Bomb
{
    partial class Bomb
    {
        public override void StartRoundCountdown()
        {
            base.StartRoundCountdown();
            GiveBombToRandomTerrorist();
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

        public override void StartMapChoose()
        {
            base.StartMapChoose();
            if (Map.BombInfo is null)
                return;

            foreach (var bombplace in Map.BombInfo.PlantPositions)
            {
                var pos = new Position3D(bombplace.X, bombplace.Y, bombplace.Z);
                BombPlantPlaceDto dto = new BombPlantPlaceDto(
                    obj: ModAPI.MapObject.Create(-51423166, pos, null, 255, Lobby),
                    blip: ModAPI.Blip.Create(pos, Lobby),
                    pos: pos
                );
                dto.Blip.Sprite = SharedConstants.BombPlantPlaceBlipSprite;
                dto.Blip.Name = "Bomb-Plant";
                _bombPlantPlaces.Add(dto);
            }

            var bombPos = Map.BombInfo.PlantPositions[0];
            _bomb = ModAPI.MapObject.Create(1764669601, new Position3D(bombPos.X, bombPos.Y, bombPos.Z), null, 255, Lobby);
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
                _lobbyBombTakeCol.Remove(Lobby, out IColShape? col);
                if (col is { })
                    col.Delete();
                _bombTakeMarker?.Delete();
                _bombTakeMarker = null;
            }
        }
    }
}