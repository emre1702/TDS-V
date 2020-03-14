using GTANetworkAPI;
using TDS_Common.Dto.Map;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Dto.Map;
using TDS_Server.Manager.Utility;

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
                Vector3 pos = bombplace.ToVector3();
                BombPlantPlaceDto dto = new BombPlantPlaceDto(
                    obj: NAPI.Object.CreateObject(-51423166, pos, new Vector3(), 255, Lobby.Dimension),
                    blip: NAPI.Blip.CreateBlip(pos, Lobby.Dimension),
                    pos: pos
                );
                dto.Blip.Sprite = Constants.BombPlantPlaceBlipSprite;
                dto.Blip.Name = "Bomb-Plant";
                _bombPlantPlaces.Add(dto);
            }

            _bomb = NAPI.Object.CreateObject(1764669601, Map.BombInfo.PlantPositions[0].ToVector3(), new Vector3(), 255, Lobby.Dimension);
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
                _lobbyBombTakeCol.Remove(Lobby, out ColShape? col);
                if (col != null)
                    NAPI.ColShape.DeleteColShape(col);
                _bombTakeMarker?.Delete();
                _bombTakeMarker = null;
            }
        }
    }
}
