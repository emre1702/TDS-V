using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Default;

namespace TDS_Server.LobbySystem.Gamemodes
{
    partial class Bomb
    {
        public override void StartMapChoose()
        {
            base.StartMapChoose();
            if (Map.BombInfo is null)
                return;

            foreach (var bombplace in Map.BombInfo.PlantPositions)
            {
                var pos = new Vector3(bombplace.X, bombplace.Y, bombplace.Z);
                var dto = new BombPlantPlaceDto(
                    obj: NAPI.Object.CreateObject(-51423166, pos, new Vector3(), 255, Lobby.Dimension) as ITDSObject,
                    blip: NAPI.Blip.CreateBlip(SharedConstants.BombPlantPlaceBlipSprite, pos, 1f, 0, name: "Bomb-Plant", dimension: Lobby.Dimension) as ITDSBlip,
                    pos: pos
                );
                _bombPlantPlaces.Add(dto);
            }

            var bombPos = Map.BombInfo.PlantPositions[0];
            _bomb = NAPI.Object.CreateObject(1764669601, new Vector3(bombPos.X, bombPos.Y, bombPos.Z), new Vector3(), 255, Lobby.Dimension) as ITDSObject;
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
                    character.SendChatMessage(character.Language.ROUND_MISSION_BOMG_SPECTATOR);
                else if (team == _terroristTeam)
                    character.SendChatMessage(character.Language.ROUND_MISSION_BOMB_BAD);
                else
                    character.SendChatMessage(character.Language.ROUND_MISSION_BOMB_GOOD);
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
    }
}
