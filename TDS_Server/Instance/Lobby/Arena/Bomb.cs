namespace TDS_Server.Instance.Lobby
{
    using System.Collections.Generic;
    using GTANetworkAPI;
    using TDS_Server.Dto;
    using TDS_Server.Entity;
    using TDS_Server.Enum;
    using TDS_Server.Instance.Player;
    using TDS_Server.Manager.Utility;
    using TDS_Common.Instance.Utility;
    using TDS_Common.Default;

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
        private Teams terroristTeam;
        private Teams counterTerroristTeam;

        private List<Object> bombPlantPlaces = new List<Object>();
        private List<Blip> bombPlantBlips = new List<Blip>();
        private Object bomb;
        private TDSPlayer bombAtPlayer;
        private TDSTimer bombDetonateTimer,
                      bombPlantDefuseTimer;
        private TDSPlayer planter;
        private Blip plantBlip;
        private Marker bombTakeMarker;

        private void StartBombMapChoose(MapDto map)
        {
            foreach (Vector3 bombplace in map.BombPlantPlaces)
            {
                Object place = NAPI.Object.CreateObject(-51423166, bombplace, new Vector3(), 255, Dimension);
                bombPlantPlaces.Add(place);
                Blip blip = NAPI.Blip.CreateBlip(bombplace, Dimension);
                blip.Sprite = 433;
                bombPlantBlips.Add(blip);
            }
            bomb = NAPI.Object.CreateObject(1764669601, currentMap.BombPlantPlaces[0], new Vector3(), 255, Dimension);
        }

        private void GiveBombToRandomTerrorist()
        {
            int amount = TeamPlayers[terroristTeam.Index].Count;
            if (amount > 0)
            {
                int rnd = Utils.Rnd.Next(amount);
                TDSPlayer character = TeamPlayers[terroristTeam.Index][rnd];
                if (character.Client.CurrentWeapon == WeaponHash.Unarmed)
                    BombToHand(character);
                else
                    BombToBack(character);
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerGotBomb, currentMap.BombPlantPlaces);
            }
        }

        private void SendBombPlantInfos(TDSPlayer character)
        {
            NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.BOMB_PLANT_INFO);
        }

        private void SendBombDefuseInfos()
        {
            FuncIterateAllPlayers((character, team) =>
            {
                foreach (string str in character.Language.DEFUSE_INFO)
                {
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, str);
                }
            }, counterTerroristTeam.Index);
        }

        private void BombToHand(TDSPlayer character)
        {
            bomb.Detach();
            bomb.Collisionless = true;
            bomb.AttachTo(character.Client, "SKEL_R_Finger01", new Vector3(0.1, 0, 0), new Vector3());
            if (bombAtPlayer != character)
                SendBombPlantInfos(character);
            bombAtPlayer = character;
        }

        private void BombToBack(TDSPlayer character)
        {
            bomb.Detach();
            bomb.Collisionless = true;
            bomb.AttachTo(character.Client, "SKEL_Pelvis", new Vector3(0, 0, 0.24), new Vector3(270, 0, 0));
            if (bombAtPlayer != character)
                SendBombPlantInfos(character);
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
                if (team.IsSpectatorTeam)
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
            NAPI.Explosion.CreateOwnedExplosion(planter.Client, ExplosionType.GrenadeL, bomb.Position, 200, Dimension);
            FuncIterateAllPlayers((character, team) =>
            {
                DmgSys.UpdateLastHitter(character, planter, LobbyEntity.StartHealth + LobbyEntity.StartArmor);
                character.Client.Kill();
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.BombDetonated);
            }, counterTerroristTeam.Index);
            // TERROR WON //
            if (currentRoundStatus == ERoundStatus.Round)
                SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.BombExploded);
        }

        private void PlantBomb(TDSPlayer character)
        {
            Client player = character.Client;
            if (player.Exists)
            {
                Vector3 playerpos = player.Position;
                for (int i = 0; i < currentMap.BombPlantPlaces.Count; i++)
                {
                    if (playerpos.DistanceTo(currentMap.BombPlantPlaces[i]) <= 5)
                    {
                        NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.PlayerPlantedBomb);
                        bomb.Detach();
                        bomb.Position = new Vector3(playerpos.X, playerpos.Y, playerpos.Z - 0.9);
                        bomb.Rotation = new Vector3(270, 0, 0);
                        bombPlantPlaces[i].Delete();
                        bombPlantPlaces[i] = NAPI.Object.CreateObject(-263709501, currentMap.BombPlantPlaces[i], new Vector3(), 255, Dimension);
                        bombPlantBlips[i].Color = 49;
                        //bombPlantBlips[i].Flashing = true;
#warning Implement after new Bridge version
                        bombAtPlayer = null;
                        planter = character;
                        FuncIterateAllPlayers((charac, team) =>
                        {
                            NAPI.Chat.SendChatMessageToPlayer(charac.Client, charac.Language.BOMB_PLANTED);
                        });

                        bombDetonateTimer = new TDSTimer(DetonateBomb, LobbyEntity.BombDetonateTimeMs.Value);
                        FuncIterateAllPlayers((targetcharacter, team) =>
                        {
                            NAPI.ClientEvent.TriggerClientEvent(targetcharacter.Client, DToClientEvent.BombPlanted, playerpos, team == counterTerroristTeam);
                        });
                        SendBombDefuseInfos();
                        break;
                    }
                }
                player.StopAnimation();
            }
        }

        private void DefuseBomb(TDSPlayer character)
        {
            Client player = character.Client;
            if (player.Exists)
            {
                Vector3 playerpos = player.Position;
                if (playerpos.DistanceTo(bomb.Position) <= 2)
                {
                    FuncIterateAllPlayers((targetcharacter, team) =>
                    {
                        DmgSys.UpdateLastHitter(targetcharacter, character, LobbyEntity.StartArmor + LobbyEntity.StartHealth);
                        targetcharacter.Client.Kill();
                    }, terroristTeam.Index);
                    // COUNTER-TERROR WON //
                    if (currentRoundStatus == ERoundStatus.Round)
                        SetRoundStatus(ERoundStatus.Round, ERoundEndReason.BombDefused);
                }
                player.StopAnimation();
            }
        }

        public void StartBombPlanting(TDSPlayer character)
        {
            if (bomb != null)
            {
                if (currentRoundStatus == ERoundStatus.Round)
                {
                    if (bombDetonateTimer == null)
                    {
                        if (bombPlantDefuseTimer != null)
                        {
                            bombPlantDefuseTimer.Kill();
                            bombPlantDefuseTimer = null;
                        }
                        Client player = character.Client;
                        if (!player.Dead)
                        {
                            if (player.CurrentWeapon == WeaponHash.Unarmed)
                            {
                                player.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(EAnimationFlag.Loop));
                                bombPlantDefuseTimer = new TDSTimer(() => PlantBomb(character), LobbyEntity.BombPlantTimeMs.Value);
                            }
                        }
                    }
                }
            }
        }

        public void StopBombPlanting(Client player)
        {
            if (bombPlantDefuseTimer != null)
            {
                bombPlantDefuseTimer.Kill();
                bombPlantDefuseTimer = null;
            }
            player.StopAnimation();
        }

        public void StartBombDefusing(TDSPlayer character)
        {
            if (bomb != null)
            {
                if (currentRoundStatus == ERoundStatus.Round)
                {
                    if (bombDetonateTimer != null)
                    {
                        if (bombPlantDefuseTimer != null)
                        {
                            bombPlantDefuseTimer.Kill();
                            bombPlantDefuseTimer = null;
                        }
                        Client player = character.Client;
                        if (!player.Dead)
                        {
                            if (player.CurrentWeapon == WeaponHash.Unarmed)
                            {
                                if (bombAtPlayer == null)
                                {
                                    player.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(EAnimationFlag.Loop));
                                    bombPlantDefuseTimer = new TDSTimer(() => DefuseBomb(character), LobbyEntity.BombDefuseTimeMs.Value);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void StopBombDefusing(TDSPlayer character)
        {
            if (bombPlantDefuseTimer != null)
            {
                bombPlantDefuseTimer.Kill();
                bombPlantDefuseTimer = null;
            }
            character.Client.StopAnimation();
        }

        private void DropBomb()
        {
            bomb.Detach();
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
            if (character.Client.CurrentWeapon == WeaponHash.Unarmed)
                BombToHand(character);
            else
                BombToBack(character);
            bomb.FreezePosition = false;
            bombTakeMarker.Delete();
            bombTakeMarker = null;
            lobbyBombTakeCol.Remove(this, out ColShape col);
            NAPI.ColShape.DeleteColShape(col);
        }


        private void StopBombRound()
        {
            if (bombPlantDefuseTimer != null)
            {
                bombPlantDefuseTimer.Kill();
                bombPlantDefuseTimer = null;
            }
            if (bombDetonateTimer != null)
            {
                bombDetonateTimer.Kill();
                bombDetonateTimer = null;
            }
            if (lobbyBombTakeCol.ContainsKey(this))
            {
                lobbyBombTakeCol.Remove(this, out ColShape col);
                NAPI.ColShape.DeleteColShape(col);
                bombTakeMarker.Delete();
                bombTakeMarker = null;
            }
            bombAtPlayer = null;
            planter = null;
        }

        private void ClearBombRound()
        {
            if (currentMap == null || currentMap.SyncedData.Type != EMapType.Bomb)
                return;

            foreach (Object place in bombPlantPlaces)
            {
                place.Delete();
            }
            foreach (Blip blip in bombPlantBlips)
            {
                blip.Delete();
            }
            if (bomb != null)
            {
                bomb.Delete();
                bomb = null;
            }
            bombPlantPlaces = new List<Object>();
            bombPlantBlips = new List<Blip>();
            if (plantBlip != null)
            {
                plantBlip.Delete();
                plantBlip = null;
            }
        }
    }

}
