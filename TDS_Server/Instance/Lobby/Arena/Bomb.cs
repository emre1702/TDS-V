namespace TDS_Server.Instance.Lobby
{
    using GTANetworkAPI;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using TDS_Common.Default;
    using TDS_Common.Dto.Map;
    using TDS_Common.Instance.Utility;
    using TDS_Common.Manager.Utility;
    using TDS_Server.Dto;
    using TDS_Server.Enum;
    using TDS_Server.Instance.Player;
    using TDS_Server.Instance.Utility;
    using TDS_Server.Manager.Helper;
    using TDS_Server.Manager.Utility;

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

        private static readonly Dictionary<Lobby, ColShape> lobbyBombTakeCol = new Dictionary<Lobby, ColShape>();
        private readonly Team terroristTeam;
        private readonly Team counterTerroristTeam;

        private readonly List<BombPlantPlaceDto> bombPlantPlaces = new List<BombPlantPlaceDto>();

        private Object? bomb;
        private TDSPlayer? bombAtPlayer;

        private TDSTimer? bombDetonateTimer,
                      bombPlantDefuseTimer;

        private TDSPlayer? planter;
        private Blip? plantBlip;
        private Marker? bombTakeMarker;

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
                bombPlantPlaces.Add(dto);
            }
            bomb = NAPI.Object.CreateObject(1764669601, map.BombInfo.PlantPositions[0].ToVector3(), new Vector3(), 255, Dimension);
        }

        private void GiveBombToRandomTerrorist()
        {
            if (currentMap == null)
                return;
            int amount = terroristTeam.Players.Count;
            if (amount == 0)
                return;

            int rnd = CommonUtils.Rnd.Next(amount);
            TDSPlayer character = terroristTeam.Players[rnd];
            if (character.Client.CurrentWeapon == WeaponHash.Unarmed)
                BombToHand(character);
            else
                BombToBack(character);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerGotBomb, currentMap.BombInfo?.PlantPositionsJson ?? "{}");
        }

        private void SendBombPlantInfos(TDSPlayer character)
        {
            NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.BOMB_PLANT_INFO);
        }

        private void SendBombDefuseInfos()
        {
            counterTerroristTeam.FuncIterate((character, team) =>
            {
                foreach (string str in character.Language.DEFUSE_INFO)
                {
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, str);
                }
            });
        }

        private void BombToHand(TDSPlayer character)
        {
            if (bomb == null)
                return;
            Workaround.DetachEntity(bomb);
            Workaround.SetEntityCollisionless(bomb, true, this);
            Workaround.AttachEntityToEntity(bomb, character.Client, EBone.SKEL_R_Finger01, new Vector3(0.1, 0, 0), new Vector3(), this);
            if (bombAtPlayer != character)
                SendBombPlantInfos(character);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.BombOnHand);
            bombAtPlayer = character;
        }

        private void BombToBack(TDSPlayer character)
        {
            if (bomb == null)
                return;
            Workaround.DetachEntity(bomb);
            Workaround.SetEntityCollisionless(bomb, true, this);
            Workaround.AttachEntityToEntity(bomb, character.Client, EBone.SKEL_Pelvis, new Vector3(0, 0, 0.24), new Vector3(270, 0, 0), this);
            if (bombAtPlayer != character)
                SendBombPlantInfos(character);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.BombNotOnHand);
            bombAtPlayer = character;
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
                else if (team == terroristTeam)
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.ROUND_MISSION_BOMB_BAD);
                else
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.ROUND_MISSION_BOMB_GOOD);
            });
            if (bombAtPlayer == null)
                GiveBombToRandomTerrorist();
        }

        private void DetonateBomb()
        {
            // NAPI.Explosion.CreateOwnedExplosion(planter.Client, ExplosionType.GrenadeL, bomb.Position, 200, Dimension);   use 0x172AA1B624FA1013 as Hash instead if not getting fixed
            SendAllPlayerEvent(DToClientEvent.BombDetonated, null);
            counterTerroristTeam.FuncIterate((character, team) =>
            {
                if (character.Lifes == 0)
                    return;
                int damage = character.Client.Health + character.Client.Armor;
                DmgSys.UpdateLastHitter(character, planter, damage);
                character.Damage(ref damage);
                if (planter != null && planter.CurrentRoundStats != null)
                    planter.CurrentRoundStats.Damage += (uint)damage;
            });
            // TERROR WON //
            if (currentRoundStatus == ERoundStatus.Round)
                SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.BombExploded);
        }

        private void PlantBomb(TDSPlayer player)
        {
            if (player.Client.Dead)
                return;
            if (bomb == null)
                return;
            player.Client.StopAnimation();

            Vector3 playerpos = player.Client.Position;
            var plantPlace = GetPlantPos(playerpos);
            if (plantPlace == null)
                return;

            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayerPlantedBomb);
            Workaround.DetachEntity(bomb);
            bomb.Position = new Vector3(playerpos.X, playerpos.Y, playerpos.Z - 0.9);
            bomb.Rotation = new Vector3(270, 0, 0);
            plantPlace.Object.Delete();
            plantPlace.Object = NAPI.Object.CreateObject(-263709501, plantPlace.Position, new Vector3(), 255, Dimension);
            plantPlace.Blip.Color = 49;
            //bombPlantBlips[i].Flashing = true;
            //Todo Implement after new Bridge version
            bombAtPlayer = null;
            planter = player;
            bombDetonateTimer = new TDSTimer(DetonateBomb, LobbyEntity.BombDetonateTimeMs.HasValue ? LobbyEntity.BombDetonateTimeMs.Value : 50);

            FuncIterateAllPlayers((target, team) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(target.Client, target.Language.BOMB_PLANTED);
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.BombPlanted, JsonConvert.SerializeObject(playerpos), team == counterTerroristTeam);
            });

            SendBombDefuseInfos();
        }

        private BombPlantPlaceDto? GetPlantPos(Vector3 pos)
        {
            foreach (var place in bombPlantPlaces)
                if (pos.DistanceTo(place.Position) < SettingsManager.DistanceToSpotToPlant)
                    return place;
            return null;
        }

        private void DefuseBomb(TDSPlayer character)
        {
            if (character.Client.Dead)
                return;
            if (bomb == null)
                return;

            Vector3 playerpos = character.Client.Position;
            if (playerpos.DistanceTo(bomb.Position) > SettingsManager.DistanceToSpotToDefuse)
                return;

            terroristTeam.FuncIterate((targetcharacter, team) =>
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
            if (bomb == null)
                return;
            if (currentRoundStatus != ERoundStatus.Round)
                return;
            if (bombDetonateTimer != null)
                return;
            if (bombPlantDefuseTimer != null)
                return;
            if (character.Client.Dead)
                return;
            if (character.Client.CurrentWeapon != WeaponHash.Unarmed)
                return;

            character.Client.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(EAnimationFlag.Loop));
            bombPlantDefuseTimer = new TDSTimer(() => PlantBomb(character), LobbyEntity.BombPlantTimeMs.HasValue ? LobbyEntity.BombPlantTimeMs.Value : 50);
        }

        public void StopBombPlanting(Client client)
        {
            bombPlantDefuseTimer?.Kill();
            bombPlantDefuseTimer = null;

            client.StopAnimation();
        }

        public void StartBombDefusing(TDSPlayer character)
        {
            if (bomb == null)
                return;
            if (currentRoundStatus != ERoundStatus.Round)
                return;
            if (bombDetonateTimer == null)
                return;
            if (bombPlantDefuseTimer != null)
                return;
            if (character.Client.Dead)
                return;
            if (character.Client.CurrentWeapon != WeaponHash.Unarmed)
                return;
            character.Client.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(EAnimationFlag.Loop));
            bombPlantDefuseTimer = new TDSTimer(() => DefuseBomb(character), LobbyEntity.BombDefuseTimeMs.HasValue ? LobbyEntity.BombDefuseTimeMs.Value : 50);
        }

        public void StopBombDefusing(Client client)
        {
            bombPlantDefuseTimer?.Kill();
            bombPlantDefuseTimer = null;
            client.StopAnimation();
        }

        private void DropBomb()
        {
            if (bombAtPlayer == null)
                return;
            if (bomb == null)
                return;
            Workaround.DetachEntity(bomb);
            bomb.FreezePosition = true;
            bomb.Position = bombAtPlayer.Client.Position;
            bombAtPlayer = null;
            bombTakeMarker = NAPI.Marker.CreateMarker(0, bomb.Position, new Vector3(), new Vector3(), 1,
                                                        new Color(180, 0, 0, 180), true, Dimension);
            ColShape bombtakecol = NAPI.ColShape.CreateSphereColShape(bomb.Position, 2);
            lobbyBombTakeCol[this] = bombtakecol;
        }

        private void TakeBomb(TDSPlayer character)
        {
            if (bomb == null)
                return;
            if (character.Client.CurrentWeapon == WeaponHash.Unarmed)
                BombToHand(character);
            else
                BombToBack(character);
            bomb.FreezePosition = false;
            bombTakeMarker?.Delete();
            bombTakeMarker = null;
            lobbyBombTakeCol.Remove(this, out ColShape col);
            NAPI.ColShape.DeleteColShape(col);
        }

        private void StopBombRound()
        {
            bombPlantDefuseTimer?.Kill();
            bombPlantDefuseTimer = null;

            bombDetonateTimer?.Kill();
            bombDetonateTimer = null;

            if (lobbyBombTakeCol.ContainsKey(this))
            {
                lobbyBombTakeCol.Remove(this, out ColShape col);
                NAPI.ColShape.DeleteColShape(col);
                bombTakeMarker?.Delete();
                bombTakeMarker = null;
            }
            bombAtPlayer = null;
            planter = null;
        }

        private void ClearBombRound()
        {
            foreach (var place in bombPlantPlaces)
            {
                place.Delete();
            }
            if (bomb != null)
            {
                bomb.Delete();
                bomb = null;
            }
            if (plantBlip != null)
            {
                plantBlip.Delete();
                plantBlip = null;
            }
            bombPlantPlaces.Clear();
        }
    }
}