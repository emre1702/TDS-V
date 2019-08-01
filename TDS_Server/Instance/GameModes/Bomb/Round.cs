using GTANetworkAPI;
using TDS_Common.Dto.Map;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;

namespace TDS_Server.Instance.GameModes
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
                if (team == null || team.IsSpectator)
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.ROUND_MISSION_BOMG_SPECTATOR);
                else if (team == _terroristTeam)
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.ROUND_MISSION_BOMB_BAD);
                else
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.ROUND_MISSION_BOMB_GOOD);
            });
            if (_bombAtPlayer == null)
                GiveBombToRandomTerrorist();
        }

        public override void StartMapChoose()
        {
            base.StartMapChoose();
            if (Map.BombInfo == null)
                return;

            foreach (Position3DDto bombplace in Map.BombInfo.PlantPositions)
            {
                Vector3 pos = bombplace.ToVector3();
                BombPlantPlaceDto dto = new BombPlantPlaceDto(
                    obj: NAPI.Object.CreateObject(-51423166, pos, new Vector3(), 255, Lobby.Dimension),
                    blip: NAPI.Blip.CreateBlip(pos, Lobby.Dimension),
                    pos: pos
                );
                dto.Blip.Sprite = Constants.BombPlantPlaceBlipSprite;
                _bombPlantPlaces.Add(dto);
            }
            _bomb = NAPI.Object.CreateObject(1764669601, Map.BombInfo?.PlantPositions[0].ToVector3(), new Vector3(), 255, Lobby.Dimension);
        }

        public override void StartMapClear()
        {
            base.StartMapClear();
            foreach (var place in _bombPlantPlaces)
            {
                place.Delete();
            }
            if (_bomb != null)
            {
                _bomb.Delete();
                _bomb = null;
            }
            if (_plantBlip != null)
            {
                _plantBlip.Delete();
                _plantBlip = null;
            }
            _bombPlantPlaces.Clear();
        }

        public override void StopRound()
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;

            _bombDetonateTimer?.Kill();
            _bombDetonateTimer = null;

            if (_lobbyBombTakeCol.ContainsKey(Lobby))
            {
                _lobbyBombTakeCol.Remove(Lobby, out ColShape col);
                NAPI.ColShape.DeleteColShape(col);
                _bombTakeMarker?.Delete();
                _bombTakeMarker = null;
            }
            _bombAtPlayer = null;
            _planter = null;
        }
    }
}
