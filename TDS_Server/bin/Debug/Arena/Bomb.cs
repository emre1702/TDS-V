namespace TDS.Instance.Lobby
{

    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using GTANetworkAPI;
    using TDS.Default;
    using TDS.Entity;
    using TDS.Enum;
    using TDS.Instance.Player;
    using TDS.Instance.Utility;
    using TDS.Manager.Utility;

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

        private static readonly ConcurrentDictionary<Lobby, ColShape> lobbyBombTakeCol = new ConcurrentDictionary<Lobby, ColShape>();
        private Teams terroristTeam;

#warning Todo settable
        private readonly uint bombDetonateTime = 45000;
        private readonly uint bombDefuseTime = 8000;
        private readonly uint bombPlantTime = 3000;

        private List<Object> bombPlantPlaces = new List<Object>();
        private List<Blip> bombPlantBlips = new List<Blip>();
        private Object bomb;
        private Character bombAtPlayer;
        private Timer bombDetonateTimer,
                      bombPlantDefuseTimer;
        private Character planter;
        private Blip plantBlip;
        private Marker bombTakeMarker;

        private void BombMapChose()
        {
            foreach (Vector3 bombplace in currentMap.BombPlantPlaces)
            {
                Object place = NAPI.Object.CreateObject(-51423166, bombplace, new Vector3(), 255, dimension);
                bombPlantPlaces.Add(place);
                Blip blip = NAPI.Blip.CreateBlip(bombplace, dimension);
                blip.Sprite = 433;
                bombPlantBlips.Add(blip);
            }
            bomb = NAPI.Object.CreateObject(1764669601, currentMap.BombPlantPlaces[0], new Vector3(), 255, dimension);
        }

        private void GiveBombToRandomTerrorist()
        {
            int amount = teamPlayers[terroristTeam].Count;
            if (amount > 0)
            {
                int rnd = Utils.Rnd.Next(amount);
                Character character = teamPlayers[terroristTeam][rnd];
                if (character.Player.CurrentWeapon == WeaponHash.Unarmed)
                    BombToHand(character);
                else
                    BombToBack(character);
                NAPI.ClientEvent.TriggerClientEvent(character.Player, "onClientPlayerGotBomb", currentMap.BombPlantPlaces);
            }
        }

        private void SendBombPlantInfos(Client player)
        {
            NAPI.Chat.SendChatMessageToPlayer(player, player.GetLang().BOMB_PLANT_INFO);
        }

        private void SendBombDefuseInfos()
        {
            FuncIterateAllPlayers((character, team) =>
            {
                if (team != terroristTeam)
                {
                    foreach (string str in character.Language.DEFUSE_INFO)
                    {
                        NAPI.Chat.SendChatMessageToPlayer(character.Player, str);
                    }
                }
            });
        }

        private void BombToHand(Character character)
        {
            bomb.Detach();
            bomb.Collisionless = true;
            bomb.AttachTo(character.Player, "SKEL_R_Finger01", new Vector3(0.1, 0, 0), new Vector3());
            if (bombAtPlayer != character)
                SendBombPlantInfos(character.Player);
            bombAtPlayer = character;
        }

        private void BombToBack(Character character)
        {
            bomb.Detach();
            bomb.Collisionless = true;
            bomb.AttachTo(character.Player, "SKEL_Pelvis", new Vector3(0, 0, 0.24), new Vector3(270, 0, 0));
            if (bombAtPlayer != character)
                SendBombPlantInfos(character.Player);
            bombAtPlayer = character;
        }

        private void ToggleBombAtHand(Character character, WeaponHash oldweapon, WeaponHash newweapon)
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
                    NAPI.Chat.SendChatMessageToPlayer(character.Player, character.Language.ROUND_MISSION_BOMG_SPECTATOR);
                else if (team == terroristTeam)
                    NAPI.Chat.SendChatMessageToPlayer(character.Player, character.Language.ROUND_MISSION_BOMB_BAD);
                else
                    NAPI.Chat.SendChatMessageToPlayer(character.Player, character.Language.ROUND_MISSION_BOMB_GOOD);
            });

            if (bombAtPlayer == null)
                GiveBombToRandomTerrorist();
        }

        private void DetonateBomb()
        {
            NAPI.Explosion.CreateOwnedExplosion(planter.Player, ExplosionType.GrenadeL, bomb.Position, 200, dimension);

            FuncIterateAllPlayers((character, team) =>
            {
                if (!team.IsSpectatorTeam && team != terroristTeam)
                {
                    DmgSys.LastHitterDictionary[character] = planter;
                    character.Player.Kill();
                    NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvents.ClientBombDetonated);
                }
            });

            // TERROR WON //
            if (status == ELobbyStatus.Round)
                EndRoundEarlier(ERoundEndReason.Bomb, terroristTeam);
        }

        private void PlantBomb(Character character)
        {
            Client player = character.Player;
            if (player.Exists)
            {
                Vector3 playerpos = player.Position;
                for (int i = 0; i < currentMap.BombPlantPlaces.Count; i++)
                {
                    if (playerpos.DistanceTo(currentMap.BombPlantPlaces[i]) <= 5)
                    {
                        NAPI.ClientEvent.TriggerClientEvent(player, "onClientPlayerPlantedBomb");
                        bomb.Detach();
                        bomb.Position = new Vector3(playerpos.X, playerpos.Y, playerpos.Z - 0.9);
                        bomb.Rotation = new Vector3(270, 0, 0);
                        bombPlantPlaces[i].Delete();
                        bombPlantPlaces[i] = NAPI.Object.CreateObject(-263709501, currentMap.BombPlantPlaces[i], new Vector3(), 255, dimension);
                        bombPlantBlips[i].Color = 49;
                        //bombPlantBlips[i].Flashing = true;
                        bombAtPlayer = null;
                        planter = character;
                        SendAllPlayerLangNotification("bomb_planted");
                        bombDetonateTimer = Timer.SetTimer(DetonateBomb, bombDetonateTime);
                        FuncIterateAllPlayers((targetcharacter, teamID) =>
                        {
                            NAPI.ClientEvent.TriggerClientEvent(targetcharacter.Player, "onClientBombPlanted", playerpos, teamID == counterTerroristTeamID);
                        });
                        SendBombDefuseInfos();
                        break;
                    }
                }
                player.StopAnimation();
            }
        }

        private void DefuseBomb(Character character)
        {
            Client player = character.Player;
            if (player.Exists)
            {
                Vector3 playerpos = player.Position;
                if (playerpos.DistanceTo(bomb.Position) <= 2)
                {
                    FuncIterateAllPlayers((targetcharacter, teamID) =>
                    {
                        DmgSys.LastHitterDictionary[targetcharacter] = character;
                        targetcharacter.Player.Kill();
                    }, terroristTeamID);
                    // COUNTER-TERROR WON //
                    if (status == LobbyStatus.ROUND)
                        EndRoundEarlier(RoundEndReason.BOMB, counterTerroristTeamID);
                }
                player.StopAnimation();
            }
        }

        public void StartBombPlanting(Character character)
        {
            if (bomb != null)
            {
                if (status == ELobbyStatus.Round)
                {
                    if (bombDetonateTimer == null)
                    {
                        if (bombPlantDefuseTimer != null)
                        {
                            bombPlantDefuseTimer.Kill();
                            bombPlantDefuseTimer = null;
                        }
                        Client player = character.Player;
                        if (!player.Dead)
                        {
                            if (player.CurrentWeapon == WeaponHash.Unarmed)
                            {
                                player.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(Utility.AnimationFlags.Loop));
                                bombPlantDefuseTimer = Timer.SetTimer(() => PlantBomb(character), bombPlantTime);
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

        public void StartBombDefusing(Character character)
        {
            if (bomb != null)
            {
                if (status == ELobbyStatus.Round)
                {
                    if (bombDetonateTimer != null)
                    {
                        if (bombPlantDefuseTimer != null)
                        {
                            bombPlantDefuseTimer.Kill();
                            bombPlantDefuseTimer = null;
                        }
                        Client player = character.Player;
                        if (!player.Dead)
                        {
                            if (player.CurrentWeapon == WeaponHash.Unarmed)
                            {
                                if (bombAtPlayer == null)
                                {
                                    player.PlayAnimation("misstrevor2ig_7", "plant_bomb", (int)(Utility.AnimationFlags.Loop));
                                    bombPlantDefuseTimer = Timer.SetTimer(() => DefuseBomb(character), bombDefuseTime);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void StopBombDefusing(Character character)
        {
            if (bombPlantDefuseTimer != null)
            {
                bombPlantDefuseTimer.Kill();
                bombPlantDefuseTimer = null;
            }
            character.Player.StopAnimation();
        }

        private void DropBomb()
        {
            bomb.Detach();
            bomb.FreezePosition = true;
            bomb.Position = bombAtPlayer.Player.Position;
            bombAtPlayer = null;
            bombTakeMarker = NAPI.Marker.CreateMarker(0, bomb.Position, new Vector3(), new Vector3(), 1,
                                                        new Color(180, 0, 0, 180), true, Dimension);
            ColShape bombtakecol = NAPI.ColShape.CreateSphereColShape(bomb.Position, 2);
            lobbyBombTakeCol[this] = bombtakecol;
        }

        private void TakeBomb(Character character)
        {
            if (character.Player.CurrentWeapon == WeaponHash.Unarmed)
                BombToHand(character);
            else
                BombToBack(character);
            bomb.FreezePosition = false;
            bombTakeMarker.Delete();
            bombTakeMarker = null;
            lobbyBombTakeCol.TryRemove(this, out ColShape col);
            NAPI.ColShape.DeleteColShape(col);
        }


        private void StopRoundBombAtRoundEnd()
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
                lobbyBombTakeCol.TryRemove(this, out ColShape col);
                NAPI.ColShape.DeleteColShape(col);
                bombTakeMarker.Delete();
                bombTakeMarker = null;
            }
        }

        private void StopRoundBomb()
        {
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
            bombAtPlayer = null;
            planter = null;
            if (plantBlip != null)
            {
                plantBlip.Delete();
                plantBlip = null;
            }
        }
    }

}
