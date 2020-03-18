using System.Collections.Generic;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;
using TDS_Shared.Instance;

namespace TDS_Server.Handler.Entities.GameModes.Bomb
{
    partial class Bomb
    {
        private ITDSPlayer? _planter;
        private readonly List<BombPlantPlaceDto> _bombPlantPlaces = new List<BombPlantPlaceDto>();
        private TDSTimer? _bombDetonateTimer,
                      _bombPlantDefuseTimer;

        private static void SendBombPlantInfos(ITDSPlayer character)
        {
            character.SendMessage(character.Language.BOMB_PLANT_INFO);
        }

        private void SendBombDefuseInfos()
        {
            _counterTerroristTeam.FuncIterate((character, team) =>
            {
                foreach (string str in character.Language.DEFUSE_INFO)
                {
                    character.SendMessage(str);
                }
            });
        }

        private void PlantBomb(ITDSPlayer player)
        {
            _bombPlantDefuseTimer = null;
            if (player.ModPlayer!.Dead)
                return;
            if (_bomb is null)
                return;
            player.ModPlayer.StopAnimation();

            Position3D playerpos = player.ModPlayer.Position;
            var plantPlace = GetPlantPos(playerpos);
            if (plantPlace is null)
                return;

            if (Lobby.IsOfficial)
                player.AddToChallenge(ChallengeType.BombPlant);

            player.SendEvent(ToClientEvent.PlayerPlantedBomb);
            _bomb.Detach();
            _bomb.Position = new Position3D(playerpos.X, playerpos.Y, playerpos.Z - 0.9);
            _bomb.Rotation = new Position3D(270, 0, 0);
            plantPlace.Object.Delete();
            plantPlace.Object = ModAPI.MapObject.Create(-263709501, plantPlace.Position, null, 255, Lobby);
            plantPlace.Blip.Color = 49;
            plantPlace.Blip.Name = "Bomb-Plant";
            //plantPlace.Blip.Flashing = true;
            //Todo Implement after new Bridge version
            _bombAtPlayer = null;
            _planter = player;
            _bombDetonateTimer = new TDSTimer(DetonateBomb, (uint)Lobby.RoundSettings.BombDetonateTimeMs);

            Lobby.FuncIterateAllPlayers((target, team) =>
            {
                target.SendMessage(target.Language.BOMB_PLANTED);
                target.SendEvent(ToClientEvent.BombPlanted, Serializer.ToClient(playerpos), team == _counterTerroristTeam);
            });

            SendBombDefuseInfos();
        }

        private BombPlantPlaceDto? GetPlantPos(Position3D pos)
        {
            foreach (var place in _bombPlantPlaces)
                if (pos.DistanceTo(place.Position) < SettingsHandler.ServerSettings.DistanceToSpotToPlant)
                    return place;
            return null;
        }

        private void DefuseBomb(ITDSPlayer player)
        {
            _bombPlantDefuseTimer = null;
            if (player.ModPlayer!.Dead)
                return;
            if (_bomb is null)
                return;

            var playerpos = player.ModPlayer.Position;
            if (playerpos.DistanceTo(_bomb.Position) > SettingsHandler.ServerSettings.DistanceToSpotToDefuse)
                return;

            if (Lobby.IsOfficial)
                player.AddToChallenge(ChallengeType.BombDefuse);

            _terroristTeam.FuncIterate((targetcharacter, team) =>
            {
                Lobby.DmgSys.UpdateLastHitter(targetcharacter, player, Lobby.Entity.FightSettings.StartArmor + Lobby.Entity.FightSettings.StartHealth);
                targetcharacter.ModPlayer!.Kill();
            });
            player.ModPlayer.StopAnimation();

            // COUNTER-TERROR WON //
            WinnerTeam = _counterTerroristTeam;
            Lobby.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.BombDefused);
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
            if (player.ModPlayer!.Dead)
                return false;
            if (player.ModPlayer.CurrentWeapon != WeaponHash.Unarmed)
                return false;

            player.ModPlayer.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(AnimationFlag.Loop));
            _bombPlantDefuseTimer = new TDSTimer(() => PlantBomb(player), (uint)Lobby.RoundSettings.BombPlantTimeMs);
            return true;
        }

        public void StopBombPlanting(ITDSPlayer player)
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;

            player.ModPlayer?.StopAnimation();
        }

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
            if (player.ModPlayer!.Dead)
                return false;
            if (player.ModPlayer.CurrentWeapon != WeaponHash.Unarmed)
                return false;
            player.ModPlayer.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(AnimationFlag.Loop));
            _bombPlantDefuseTimer = new TDSTimer(() => DefuseBomb(player), (uint)Lobby.RoundSettings.BombDefuseTimeMs);
            return true;
        }

        public void StopBombDefusing(ITDSPlayer player)
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;
            player.ModPlayer?.StopAnimation();
        }
    }
}
