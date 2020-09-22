using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class Bomb
    {
        private readonly List<BombPlantPlaceDto> _bombPlantPlaces = new List<BombPlantPlaceDto>();

        private TDSTimer? _bombDetonateTimer,
                      _bombPlantDefuseTimer;

        private ITDSPlayer? _planter;

        public bool StartBombDefusing(ITDSPlayer player)
        {
            //Todo StartBombDefusing was empty, test it
            if (_bomb is null)
                return false;
            if (Lobby.CurrentRoundStatus != RoundStatus.Round)
                return false;
            if (_bombDetonateTimer is null)
                return false;
            if (_bombPlantDefuseTimer is { })
                return false;
            if (player!.Dead)
                return false;
            if (player.CurrentWeapon != WeaponHash.Unarmed)
                return false;
            player.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(AnimationFlag.Loop));
            _bombPlantDefuseTimer = new TDSTimer(() => DefuseBomb(player), (uint)Lobby.RoundSettings.BombDefuseTimeMs);
            return true;
        }

        public bool StartBombPlanting(ITDSPlayer player)
        {
            if (_bomb is null)
                return false;
            if (Lobby.CurrentRoundStatus != RoundStatus.Round)
                return false;
            if (_bombDetonateTimer is { })
                return false;
            if (_bombPlantDefuseTimer is { })
                return false;
            if (player.Dead)
                return false;
            if (player.CurrentWeapon != WeaponHash.Unarmed)
                return false;

            player.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(AnimationFlag.Loop));
            _bombPlantDefuseTimer = new TDSTimer(() => PlantBomb(player), (uint)Lobby.RoundSettings.BombPlantTimeMs);
            return true;
        }

        public void StopBombDefusing(ITDSPlayer player)
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;
            player.StopAnimation();
        }

        public void StopBombPlanting(ITDSPlayer player)
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;

            player.StopAnimation();
        }

        private static void SendBombPlantInfos(ITDSPlayer character)
        {
            character.SendChatMessage(character.Language.BOMB_PLANT_INFO);
        }

        private void DefuseBomb(ITDSPlayer player)
        {
            _bombPlantDefuseTimer = null;
            if (player.Dead)
                return;
            if (_bomb is null)
                return;

            var playerpos = player.Position;
            if (playerpos.DistanceTo(_bomb.Position) > SettingsHandler.ServerSettings.DistanceToSpotToDefuse)
                return;

            if (Lobby.IsOfficial)
                player.AddToChallenge(ChallengeType.BombDefuse);

            _terroristTeam.FuncIterate(target =>
            {
                Lobby.DmgSys.UpdateLastHitter(target, player, Lobby.Entity.FightSettings.StartArmor + Lobby.Entity.FightSettings.StartHealth);
                target.Kill();
            });
            player.StopAnimation();

            // COUNTER-TERROR WON //
            WinnerTeam = _counterTerroristTeam;
            Lobby.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.BombDefused);
        }

        private BombPlantPlaceDto? GetPlantPos(Vector3 pos)
        {
            foreach (var place in _bombPlantPlaces)
                if (pos.DistanceTo(place.Position) < SettingsHandler.ServerSettings.DistanceToSpotToPlant)
                    return place;
            return null;
        }

        private void PlantBomb(ITDSPlayer player)
        {
            _bombPlantDefuseTimer = null;
            if (player.Dead)
                return;
            if (_bomb is null)
                return;
            player.StopAnimation();

            var playerPos = player.Position;
            var plantPlace = GetPlantPos(playerPos);
            if (plantPlace is null)
                return;

            if (Lobby.IsOfficial)
                player.AddToChallenge(ChallengeType.BombPlant);

            player.TriggerEvent(ToClientEvent.PlayerPlantedBomb);
            _bomb.Detach();
            _bomb.Position = new Vector3(playerPos.X, playerPos.Y, playerPos.Z - 0.9);
            _bomb.Rotation = new Vector3(270, 0, 0);
            plantPlace.Object?.Delete();
            plantPlace.Object = NAPI.Object.CreateObject(-263709501, plantPlace.Position, new Vector3(), 255, dimension: Lobby.Dimension) as ITDSObject;
            if (plantPlace.Blip is { })
            {
                plantPlace.Blip.Color = 49;
                plantPlace.Blip.Name = "Bomb-Plant";
            }

            //plantPlace.Blip.Flashing = true;
            //Todo Implement after new Bridge version
            _bombAtPlayer = null;
            _planter = player;
            _bombDetonateTimer = new TDSTimer(DetonateBomb, (uint)Lobby.RoundSettings.BombDetonateTimeMs);

            Lobby.FuncIterateAllPlayers((target, team) =>
            {
                target.SendChatMessage(target.Language.BOMB_PLANTED);
                target.TriggerEvent(ToClientEvent.BombPlanted, Serializer.ToClient(playerPos), team == _counterTerroristTeam);
            });

            SendBombDefuseInfos();
        }

        private void SendBombDefuseInfos()
        {
            _counterTerroristTeam.FuncIterate(player =>
            {
                foreach (string str in player.Language.DEFUSE_INFO)
                {
                    player.SendChatMessage(str);
                }
            });
        }
    }
}
