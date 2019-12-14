using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Enums;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Bomb
    {
        private TDSPlayer? _planter;
        private readonly List<BombPlantPlaceDto> _bombPlantPlaces = new List<BombPlantPlaceDto>();
        private TDSTimer? _bombDetonateTimer,
                      _bombPlantDefuseTimer;

        private static void SendBombPlantInfos(TDSPlayer character)
        {
            NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.BOMB_PLANT_INFO);
        }

        private void SendBombDefuseInfos()
        {
            _counterTerroristTeam.FuncIterate((character, team) =>
            {
                foreach (string str in character.Language.DEFUSE_INFO)
                {
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, str);
                }
            });
        }

        private void PlantBomb(TDSPlayer player)
        {
            _bombPlantDefuseTimer = null;
            if (player.Client.Dead)
                return;
            if (_bomb is null)
                return;
            player.Client.StopAnimation();

            Vector3 playerpos = player.Client.Position;
            var plantPlace = GetPlantPos(playerpos);
            if (plantPlace is null)
                return;

            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayerPlantedBomb);
            Workaround.DetachEntity(_bomb);
            _bomb.Position = new Vector3(playerpos.X, playerpos.Y, playerpos.Z - 0.9);
            _bomb.Rotation = new Vector3(270, 0, 0);
            plantPlace.Object.Delete();
            plantPlace.Object = NAPI.Object.CreateObject(-263709501, plantPlace.Position, new Vector3(), 255, Lobby.Dimension);
            plantPlace.Blip.Color = 49;
            plantPlace.Blip.Name = "Bomb-Plant";
            //plantPlace.Blip.Flashing = true;
            //Todo Implement after new Bridge version
            _bombAtPlayer = null;
            _planter = player;
            _bombDetonateTimer = new TDSTimer(DetonateBomb, (uint)Lobby.RoundSettings.BombDetonateTimeMs);

            Lobby.FuncIterateAllPlayers((target, team) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(target.Client, target.Language.BOMB_PLANTED);
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.BombPlanted, Serializer.ToClient(playerpos), team == _counterTerroristTeam);
            });

            SendBombDefuseInfos();
        }

        private BombPlantPlaceDto? GetPlantPos(Vector3 pos)
        {
            foreach (var place in _bombPlantPlaces)
                if (pos.DistanceTo(place.Position) < SettingsManager.DistanceToSpotToPlant)
                    return place;
            return null;
        }

        private void DefuseBomb(TDSPlayer character)
        {
            _bombPlantDefuseTimer = null;
            if (character.Client.Dead)
                return;
            if (_bomb is null)
                return;

            Vector3 playerpos = character.Client.Position;
            if (playerpos.DistanceTo(_bomb.Position) > SettingsManager.DistanceToSpotToDefuse)
                return;

            _terroristTeam.FuncIterate((targetcharacter, team) =>
            {
                Lobby.DmgSys.UpdateLastHitter(targetcharacter, character, Lobby.LobbyEntity.StartArmor + Lobby.LobbyEntity.StartHealth);
                targetcharacter.Client.Kill();
            });
            character.Client.StopAnimation();

            // COUNTER-TERROR WON //
            WinnerTeam = _counterTerroristTeam;
            Lobby.SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.BombDefused);
        }

        public bool StartBombPlanting(TDSPlayer character)
        {
            if (_bomb is null)
                return false;
            if (Lobby.CurrentRoundStatus != ERoundStatus.Round)
                return false;
            if (_bombDetonateTimer is { })
                return false;
            if (_bombPlantDefuseTimer is { })
                return false;
            if (character.Client.Dead)
                return false;
            if (character.Client.CurrentWeapon != WeaponHash.Unarmed)
                return false;

            character.Client.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(EAnimationFlag.Loop));
            _bombPlantDefuseTimer = new TDSTimer(() => PlantBomb(character), (uint)Lobby.RoundSettings.BombPlantTimeMs);
            return true;
        }

        public void StopBombPlanting(Client client)
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;

            client.StopAnimation();
        }

        public bool StartBombDefusing(TDSPlayer character)
        {
            //Todo StartBombDefusing was empty, test it
            if (_bomb is null)
                return false;
            if (Lobby.CurrentRoundStatus != ERoundStatus.Round)
                return false;
            if (_bombDetonateTimer is null)
                return false;
            if (_bombPlantDefuseTimer is { })
                return false;
            if (character.Client.Dead)
                return false;
            if (character.Client.CurrentWeapon != WeaponHash.Unarmed)
                return false;
            character.Client.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(EAnimationFlag.Loop));
            _bombPlantDefuseTimer = new TDSTimer(() => DefuseBomb(character), (uint)Lobby.RoundSettings.BombDefuseTimeMs);
            return true;
        }

        public void StopBombDefusing(Client client)
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;
            client.StopAnimation();
        }
    }
}
