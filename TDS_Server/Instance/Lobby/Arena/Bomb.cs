using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Dto.Map;
using TDS_Common.Instance.Utility;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Dto.Map;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Helper;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        // BOMB DATA:
        // name: prop_bomb_01_s
        // id: 1764669601

        // BOMB PLACE WHITE:
        // name: prop_mp_placement_med
        // id: -51423166

        // BOMB PLACE RED:
        // name: prop_mp_cant_place_med
        // id: -263709501

        private static readonly Dictionary<Lobby, ColShape> _lobbyBombTakeCol = new Dictionary<Lobby, ColShape>();
        private readonly Team _terroristTeam;
        private readonly Team _counterTerroristTeam;

        private readonly List<BombPlantPlaceDto> _bombPlantPlaces = new List<BombPlantPlaceDto>();

        private Object? _bomb;
        private TDSPlayer? _bombAtPlayer;

        private TDSTimer? _bombDetonateTimer,
                      _bombPlantDefuseTimer;

        private TDSPlayer? _planter;
        private Blip? _plantBlip;
        private Marker? _bombTakeMarker;

        private void StartBombMapChoose(MapDto map)
        {
            if (map.BombInfo == null || map.BombInfo.PlantPositions.Length == 0)
                return;
            foreach (MapPositionDto bombplace in map.BombInfo.PlantPositions)
            {
                Vector3 pos = bombplace.ToVector3();
                BombPlantPlaceDto dto = new BombPlantPlaceDto(
                    obj: NAPI.Object.CreateObject(-51423166, pos, new Vector3(), 255, Dimension),
                    blip: NAPI.Blip.CreateBlip(pos, Dimension),
                    pos: pos
                );
                dto.Blip.Sprite = 433;
                _bombPlantPlaces.Add(dto);
            }
            _bomb = NAPI.Object.CreateObject(1764669601, map.BombInfo.PlantPositions[0].ToVector3(), new Vector3(), 255, Dimension);
        }

        private void GiveBombToRandomTerrorist()
        {
            if (_currentMap == null)
                return;
            int amount = _terroristTeam.Players.Count;
            if (amount == 0)
                return;

            int rnd = CommonUtils.Rnd.Next(amount);
            TDSPlayer character = _terroristTeam.Players[rnd];
            if (character.Client.CurrentWeapon == WeaponHash.Unarmed)
                BombToHand(character);
            else
                BombToBack(character);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerGotBomb, _currentMap.BombInfo?.PlantPositionsJson ?? "{}");
        }

        private void SendBombPlantInfos(TDSPlayer character)
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

        private void BombToHand(TDSPlayer character)
        {
            if (_bomb == null)
                return;
            Workaround.DetachEntity(_bomb);
            Workaround.SetEntityCollisionless(_bomb, true, this);
            Workaround.AttachEntityToEntity(_bomb, character.Client, EBone.SKEL_R_Finger01, new Vector3(0.1, 0, 0), new Vector3(), this);
            if (_bombAtPlayer != character)
                SendBombPlantInfos(character);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.BombOnHand);
            _bombAtPlayer = character;
        }

        private void BombToBack(TDSPlayer character)
        {
            if (_bomb == null)
                return;
            Workaround.DetachEntity(_bomb);
            Workaround.SetEntityCollisionless(_bomb, true, this);
            Workaround.AttachEntityToEntity(_bomb, character.Client, EBone.SKEL_Pelvis, new Vector3(0, 0, 0.24), new Vector3(270, 0, 0), this);
            if (_bombAtPlayer != character)
                SendBombPlantInfos(character);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.BombNotOnHand);
            _bombAtPlayer = character;
        }

        private void ToggleBombAtHand(TDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon)
        {
            if (newweapon == WeaponHash.Unarmed)
            {
                BombToHand(character);
            }
            else if (oldweapon == WeaponHash.Unarmed)
            {
                BombToBack(character);
            }
        }

        private void StartRoundBomb()
        {
            FuncIterateAllPlayers((character, team) =>
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

        private void DetonateBomb()
        {
            // NAPI.Explosion.CreateOwnedExplosion(planter.Client, ExplosionType.GrenadeL, bomb.Position, 200, Dimension);   use 0x172AA1B624FA1013 as Hash instead if not getting fixed
            SendAllPlayerEvent(DToClientEvent.BombDetonated, null);
            _counterTerroristTeam.FuncIterate((character, team) =>
            {
                if (character.Lifes == 0)
                    return;
                int damage = character.Client.Health + character.Client.Armor;
                DmgSys.UpdateLastHitter(character, _planter, damage);
                character.Damage(ref damage);
                if (_planter != null && _planter.CurrentRoundStats != null)
                    _planter.CurrentRoundStats.Damage += damage;
            });
            // TERROR WON //
            if (_currentRoundStatus == ERoundStatus.Round)
                SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.BombExploded);
        }

        private void PlantBomb(TDSPlayer player)
        {
            _bombPlantDefuseTimer = null;
            if (player.Client.Dead)
                return;
            if (_bomb == null)
                return;
            player.Client.StopAnimation();

            Vector3 playerpos = player.Client.Position;
            var plantPlace = GetPlantPos(playerpos);
            if (plantPlace == null)
                return;

            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayerPlantedBomb);
            Workaround.DetachEntity(_bomb);
            _bomb.Position = new Vector3(playerpos.X, playerpos.Y, playerpos.Z - 0.9);
            _bomb.Rotation = new Vector3(270, 0, 0);
            plantPlace.Object.Delete();
            plantPlace.Object = NAPI.Object.CreateObject(-263709501, plantPlace.Position, new Vector3(), 255, Dimension);
            plantPlace.Blip.Color = 49;
            //bombPlantBlips[i].Flashing = true;
            //Todo Implement after new Bridge version
            _bombAtPlayer = null;
            _planter = player;
            _bombDetonateTimer = new TDSTimer(DetonateBomb, (uint)RoundSettings.BombDetonateTimeMs);

            FuncIterateAllPlayers((target, team) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(target.Client, target.Language.BOMB_PLANTED);
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.BombPlanted, JsonConvert.SerializeObject(playerpos), team == _counterTerroristTeam);
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
            if (_bomb == null)
                return;

            Vector3 playerpos = character.Client.Position;
            if (playerpos.DistanceTo(_bomb.Position) > SettingsManager.DistanceToSpotToDefuse)
                return;

            _terroristTeam.FuncIterate((targetcharacter, team) =>
            {
                DmgSys.UpdateLastHitter(targetcharacter, character, LobbyEntity.StartArmor + LobbyEntity.StartHealth);
                targetcharacter.Client.Kill();
            });
            character.Client.StopAnimation();

            // COUNTER-TERROR WON //
            SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.BombDefused);
        }

        public void StartBombPlanting(TDSPlayer character)
        {
            if (_bomb == null)
                return;
            if (_currentRoundStatus != ERoundStatus.Round)
                return;
            if (_bombDetonateTimer != null)
                return;
            if (_bombPlantDefuseTimer != null)
                return;
            if (character.Client.Dead)
                return;
            if (character.Client.CurrentWeapon != WeaponHash.Unarmed)
                return;

            character.Client.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(EAnimationFlag.Loop));
            _bombPlantDefuseTimer = new TDSTimer(() => PlantBomb(character), (uint)RoundSettings.BombPlantTimeMs);
        }

        public void StopBombPlanting(Client client)
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;

            client.StopAnimation();
        }

        public void StartBombDefusing(TDSPlayer character)
        {
            //Todo StartBombDefusing was empty, test it
            if (_bomb == null)
                return;
            if (_currentRoundStatus != ERoundStatus.Round)
                return;
            if (_bombDetonateTimer == null)
                return;
            if (_bombPlantDefuseTimer != null)
                return;
            if (character.Client.Dead)
                return;
            if (character.Client.CurrentWeapon != WeaponHash.Unarmed)
                return;
            character.Client.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(EAnimationFlag.Loop));
            _bombPlantDefuseTimer = new TDSTimer(() => DefuseBomb(character), (uint)RoundSettings.BombDefuseTimeMs);
        }

        public void StopBombDefusing(Client client)
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;
            client.StopAnimation();
        }

        private void DropBomb()
        {
            if (_bombAtPlayer == null)
                return;
            if (_bomb == null)
                return;
            Workaround.DetachEntity(_bomb);
            _bomb.FreezePosition = true;
            _bomb.Position = _bombAtPlayer.Client.Position;
            _bombAtPlayer = null;
            _bombTakeMarker = NAPI.Marker.CreateMarker(0, _bomb.Position, new Vector3(), new Vector3(), 1,
                                                        new Color(180, 0, 0, 180), true, Dimension);
            ColShape bombtakecol = NAPI.ColShape.CreateSphereColShape(_bomb.Position, 2);
            _lobbyBombTakeCol[this] = bombtakecol;
        }

        private void TakeBomb(TDSPlayer character)
        {
            if (_bomb == null)
                return;
            if (character.Client.CurrentWeapon == WeaponHash.Unarmed)
                BombToHand(character);
            else
                BombToBack(character);
            _bomb.FreezePosition = false;
            _bombTakeMarker?.Delete();
            _bombTakeMarker = null;
            _lobbyBombTakeCol.Remove(this, out ColShape col);
            NAPI.ColShape.DeleteColShape(col);
        }

        private void StopBombRound()
        {
            _bombPlantDefuseTimer?.Kill();
            _bombPlantDefuseTimer = null;

            _bombDetonateTimer?.Kill();
            _bombDetonateTimer = null;

            if (_lobbyBombTakeCol.ContainsKey(this))
            {
                _lobbyBombTakeCol.Remove(this, out ColShape col);
                NAPI.ColShape.DeleteColShape(col);
                _bombTakeMarker?.Delete();
                _bombTakeMarker = null;
            }
            _bombAtPlayer = null;
            _planter = null;
        }

        private void ClearBombRound()
        {
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
    }
}