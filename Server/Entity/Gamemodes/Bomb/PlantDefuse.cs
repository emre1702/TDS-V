using AltV.Net.Data;
using System.Collections.Generic;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Server.Entity.Gamemodes.Bomb
{
    partial class Bomb
    {
        #region Private Fields

        private readonly List<BombPlantPlaceDto> _bombPlantPlaces = new List<BombPlantPlaceDto>();

        private TDSTimer? _bombDetonateTimer,
                      _bombPlantDefuseTimer;

        private ITDSPlayer? _planter;

        #endregion Private Fields

        #region Public Methods

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
            if (player.IsDead)
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
            if (player.IsDead)
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

        #endregion Public Methods

        #region Private Methods

        private void SendBombPlantInfos(ITDSPlayer player)
        {
            player.SendMessage(player.Language.BOMB_PLANT_INFO);
        }

        private void DefuseBomb(ITDSPlayer player)
        {
            _bombPlantDefuseTimer = null;
            if (player.IsDead)
                return;
            if (_bomb is null)
                return;

            var playerpos = player.Position;
            if (playerpos.Distance(_bomb.Position) > SettingsHandler.ServerSettings.DistanceToSpotToDefuse)
                return;

            if (Lobby.IsOfficial)
                player.AddToChallenge(ChallengeType.BombDefuse);

            _terroristTeam.FuncIterate((target, team) =>
            {
                Lobby.DmgSys.UpdateLastHitter(target, player, Lobby.Entity.FightSettings.StartArmor + Lobby.Entity.FightSettings.StartHealth);
                target.Kill();
            });
            player.StopAnimation();

            // COUNTER-TERROR WON //
            WinnerTeam = _counterTerroristTeam;
            Lobby.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.BombDefused);
        }

        private BombPlantPlaceDto? GetPlantPos(Position pos)
        {
            foreach (var place in _bombPlantPlaces)
                if (pos.Distance(place.Position) < SettingsHandler.ServerSettings.DistanceToSpotToPlant)
                    return place;
            return null;
        }

        private void PlantBomb(ITDSPlayer player)
        {
            _bombPlantDefuseTimer = null;
            if (player.IsDead)
                return;
            if (_bomb is null)
                return;
            player.StopAnimation();

            Position playerpos = player.Position;
            var plantPlace = GetPlantPos(playerpos);
            if (plantPlace is null)
                return;

            if (Lobby.IsOfficial)
                player.AddToChallenge(ChallengeType.BombPlant);

            player.SendEvent(ToClientEvent.PlayerPlantedBomb);
            _bomb.Detach();
            _bomb.Position = new Position(playerpos.X, playerpos.Y, playerpos.Z - 0.9f);
            _bomb.Rotation = new DegreeRotation(270, 0, 0);
            plantPlace.Object.Delete();
            plantPlace.Object = _tdsObjectHandler.Create(-263709501, plantPlace.Position, new DegreeRotation(), 255, (int)Lobby.Dimension);
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

        #endregion Private Methods
    }
}
