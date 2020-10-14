using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.Map;
using TDS_Server.Data.RoundEndReasons;
using TDS_Server.GamemodesSystem.Gamemodes;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Server.GamemodesSystem.Specials
{
    public class BombSpecials : BaseGamemodeSpecials, IBombGamemodeSpecials
    {
        // BOMB DATA:
        // name: propBomb_01_s
        // id: 1764669601

        // BOMB PLACE WHITE:
        // name: prop_mp_placement_med
        // id: -51423166

        // BOMB PLACE RED:
        // name: prop_mp_cant_place_med
        // id: -263709501

        public ITDSObject? Bomb { get; set; }

        public TDSTimer? BombDetonateTimer { get; private set; }
        public TDSTimer? BombPlantDefuseTimer { get; private set; }

        private readonly BombGamemode _gamemode;
        private readonly ISettingsHandler _settingsHandler;

        public BombSpecials(IRoundFightLobby lobby, BombGamemode gamemode, ISettingsHandler settingsHandler) : base(lobby)
        {
            _gamemode = gamemode;
            _settingsHandler = settingsHandler;

            CreateBomb(lobby.CurrentMap);
        }

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);
            events.RoundClear += RoundClear;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);
            if (events.RoundClear is { })
                events.RoundClear += RoundClear;
        }

        private void CreateBomb(MapDto map)
        {
            var bombPos = map.BombInfo!.PlantPositions[0];
            NAPI.Task.Run(() =>
                Bomb = NAPI.Object.CreateObject(1764669601, new Vector3(bombPos.X, bombPos.Y, bombPos.Z), new Vector3(), 255, Lobby.MapHandler.Dimension) as ITDSObject);
        }

        private ValueTask RoundClear()
        {
            Bomb?.Delete();
            Bomb = null;
            BombDetonateTimer?.Kill();
            BombDetonateTimer = null;
            BombPlantDefuseTimer?.Kill();
            BombPlantDefuseTimer = null;

            return default;
        }

        public void BombToBack(ITDSPlayer player)
        {
            if (Bomb is null)
                return;

            if (_gamemode.Players.BombAtPlayer != player)
            {
                SendBombPlantInfos(player);
                _gamemode.Players.BombAtPlayer = player;
            }

            NAPI.Task.Run(() =>
            {
                Bomb.Detach();
                Bomb.SetCollisionsless(true, Lobby);
                Bomb.AttachTo(player, PedBone.SKEL_Pelvis, new Vector3(0, 0, 0.24), new Vector3(270, 0, 0));

                player.TriggerEvent(ToClientEvent.BombNotOnHand);
            });
        }

        public void BombToHand(ITDSPlayer player)
        {
            if (Bomb is null)
                return;

            if (_gamemode.Players.BombAtPlayer != player)
            {
                SendBombPlantInfos(player);
                _gamemode.Players.BombAtPlayer = player;
            }

            NAPI.Task.Run(() =>
            {
                Bomb.Detach();
                Bomb.SetCollisionsless(true, Lobby);
                Bomb.AttachTo(player, PedBone.SKEL_R_Finger01, new Vector3(0.1, 0, 0), null);

                player.TriggerEvent(ToClientEvent.BombOnHand);
            });
        }

        public void DetonateBomb()
        {
            // NAPI.Explosion.CreateOwnedExplosion(planter.Player, ExplosionType.GrenadeL,
            // bomb.Position, 200, Dimension); use 0x172AA1B624FA1013 as Hash instead if not getting fixed
            Lobby.Sync.TriggerEvent(ToClientEvent.BombDetonated);
            _gamemode.Teams.CounterTerrorists.Players.Do(player =>
            {
                if (player.Lifes == 0)
                    return;
                int damage = player.Health + player.Armor;
                Lobby.Deathmatch.Damage.UpdateLastHitter(player, _gamemode.Players.Planter, damage);
                player.HealthAndArmor.Remove(damage, out damage, out bool killed);
                if (_gamemode.Players.Planter?.CurrentRoundStats is { } stats)
                    stats.Damage += damage;
            });
            // TERROR WON //
            if (!Lobby.Rounds.RoundStates.IsCurrentStateAfterRound())
                Lobby.Rounds.RoundStates.EndRound(new BombExplodedRoundEndReason(_gamemode.Teams.Terrorists));
        }

        public void DropBomb()
        {
            if (!(_gamemode.Players.BombAtPlayer is { } bombPlayer))
                return;
            if (Bomb is null)
                return;
            NAPI.Task.Run(() =>
            {
                Bomb.Detach();
                Bomb.Freeze(true, Lobby);
                Bomb.Position = bombPlayer.Position;
                _gamemode.MapHandler.CreateBombTakeMarker(Bomb);

                bombPlayer.TriggerEvent(ToClientEvent.BombNotOnHand);
                _gamemode.Players.BombAtPlayer = null;
            });
        }

        public void GiveBombToRandomTerrorist()
        {
            int amount = _gamemode.Teams.Terrorists.Players.Amount;
            if (amount == 0)
                return;

            ITDSPlayer player = _gamemode.Teams.Terrorists.Players.GetRandom();
            NAPI.Task.Run(() =>
            {
                if (player.CurrentWeapon == WeaponHash.Unarmed)
                    BombToHand(player);
                else
                    BombToBack(player);
                player.TriggerEvent(ToClientEvent.PlayerGotBomb, Lobby.CurrentMap?.BombInfo?.PlantPositionsJson ?? "{}");
            });
        }

        public void TakeBomb(ITDSPlayer player)
        {
            if (Bomb is null)
                return;
            ToggleBombAtHand(player, player.CurrentWeapon, player.CurrentWeapon);
            //Bomb.FreezePosition = false;
            _gamemode.MapHandler.DeleteBombTakeMarker();
        }

        public void ToggleBombAtHand(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            if (newWeapon == WeaponHash.Unarmed)
            {
                BombToHand(player);
            }
            else if (oldWeapon == WeaponHash.Unarmed)
            {
                BombToBack(player);
            }
        }

        public bool StartBombDefusing(ITDSPlayer player)
        {
            if (!CanStartBombDefusing(player))
                return false;
            NAPI.Task.Run(() =>
                player.PlayAnimation("misstrevor2ig_7", "plantBomb", (int)AnimationFlag.Loop));
            BombPlantDefuseTimer = new TDSTimer(() => DefuseBomb(player), (uint)Lobby.Entity.LobbyRoundSettings.BombDefuseTimeMs);
            return true;
        }

        public bool StartBombPlanting(ITDSPlayer player)
        {
            if (!CanStartBombPlanting(player))
                return false;
            NAPI.Task.Run(() =>
                player.PlayAnimation("misstrevor2ig_7", "plantBomb", (int)AnimationFlag.Loop));
            BombPlantDefuseTimer = new TDSTimer(() => PlantBomb(player), (uint)Lobby.Entity.LobbyRoundSettings.BombPlantTimeMs);
            return true;
        }

        public void StopBombDefusing(ITDSPlayer player)
        {
            BombPlantDefuseTimer?.Kill();
            BombPlantDefuseTimer = null;

            NAPI.Task.Run(() =>
                player.StopAnimation());
        }

        public void StopBombPlanting(ITDSPlayer player)
        {
            BombPlantDefuseTimer?.Kill();
            BombPlantDefuseTimer = null;

            NAPI.Task.Run(() =>
                player.StopAnimation());
        }

        private static void SendBombPlantInfos(ITDSPlayer player)
        {
            NAPI.Task.Run(() => player.SendChatMessage(player.Language.BOMB_PLANT_INFO));
        }

        private void DefuseBomb(ITDSPlayer player)
        {
            BombPlantDefuseTimer = null;
            if (player.Dead)
                return;
            if (Bomb is null)
                return;

            var playerpos = player.Position;
            if (playerpos.DistanceTo(Bomb.Position) > _settingsHandler.ServerSettings.DistanceToSpotToDefuse)
                return;

            if (Lobby.IsOfficial)
                player.Challenges.AddToChallenge(ChallengeType.BombDefuse);

            _gamemode.Teams.Terrorists.Players.Do(target =>
            {
                Lobby.Deathmatch.Damage.UpdateLastHitter(target, player, Lobby.Entity.FightSettings.StartArmor + Lobby.Entity.FightSettings.StartHealth);
                NAPI.Task.Run(() => target.Kill());
            });
            NAPI.Task.Run(() =>
                player.StopAnimation());

            // COUNTER-TERROR WON //
            Lobby.Rounds.RoundStates.EndRound(new BombDefusedRoundEndReason(_gamemode.Teams.CounterTerrorists));
        }

        private BombPlantPlaceDto? GetPlantPos(Vector3 pos)
        {
            foreach (var place in _gamemode.MapHandler.BombPlantPlaces)
                if (pos.DistanceTo(place.Position) < _settingsHandler.ServerSettings.DistanceToSpotToPlant)
                    return place;
            return null;
        }

        private void PlantBomb(ITDSPlayer player)
        {
            BombPlantDefuseTimer = null;
            if (player.Dead)
                return;
            if (Bomb is null)
                return;
            player.StopAnimation();

            var playerPos = player.Position;
            var plantPlace = GetPlantPos(playerPos);
            if (plantPlace is null)
                return;

            if (Lobby.IsOfficial)
                player.Challenges.AddToChallenge(ChallengeType.BombPlant);

            NAPI.Task.Run(() =>
            {
                player.TriggerEvent(ToClientEvent.PlayerPlantedBomb);
                Bomb.Detach();
                Bomb.Position = new Vector3(playerPos.X, playerPos.Y, playerPos.Z - 0.9);
                Bomb.Rotation = new Vector3(270, 0, 0);
                plantPlace.Object?.Delete();
                plantPlace.Object = NAPI.Object.CreateObject(-263709501, plantPlace.Position, new Vector3(), 255, dimension: Lobby.MapHandler.Dimension) as ITDSObject;
                if (plantPlace.Blip is { })
                {
                    plantPlace.Blip.Color = 49;
                    plantPlace.Blip.Name = "Bomb-Plant";
                }

                //plantPlace.Blip.Flashing = true;
                _gamemode.Players.BombAtPlayer = null;
                _gamemode.Players.Planter = null;
            });
            BombDetonateTimer = new TDSTimer(DetonateBomb, (uint)Lobby.Entity.LobbyRoundSettings.BombDetonateTimeMs);

            Lobby.Players.DoInMain(target =>
            {
                target.SendChatMessage(target.Language.BOMB_PLANTED);
                target.TriggerEvent(ToClientEvent.BombPlanted, Serializer.ToClient(playerPos), target.Team == _gamemode.Teams.CounterTerrorists);
            });

            SendBombDefuseInfos();
        }

        private void SendBombDefuseInfos()
        {
            _gamemode.Teams.CounterTerrorists.Players.DoInMain(player =>
            {
                foreach (string str in player.Language.DEFUSE_INFO)
                {
                    player.SendChatMessage(str);
                }
            });
        }

        private bool CanStartBombDefusing(ITDSPlayer player)
        {
            if (Bomb is null)
                return false;
            if (!(Lobby.Rounds.RoundStates.CurrentState is IInRoundState))
                return false;
            if (BombDetonateTimer is null)
                return false;
            if (BombPlantDefuseTimer is { })
                return false;
            if (player!.Dead)
                return false;
            if (player.CurrentWeapon != WeaponHash.Unarmed)
                return false;

            return true;
        }

        private bool CanStartBombPlanting(ITDSPlayer player)
        {
            if (Bomb is null)
                return false;
            if (!(Lobby.Rounds.RoundStates.CurrentState is IInRoundState))
                return false;
            if (BombDetonateTimer is { })
                return false;
            if (BombPlantDefuseTimer is { })
                return false;
            if (player.Dead)
                return false;
            if (player.CurrentWeapon != WeaponHash.Unarmed)
                return false;

            return true;
        }
    }
}
