using GTANetworkAPI;
using System.Collections.Generic;
using System.Drawing;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class Bomb
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

        #region Private Fields

        private static readonly Dictionary<Arena, ITDSColShape> _lobbyBombTakeCol = new Dictionary<Arena, ITDSColShape>();
        private ITDSObject? _bomb;
        private ITDSMarker? _bombTakeMarker;

        #endregion Private Fields

        #region Private Methods

        private void BombToBack(ITDSPlayer player)
        {
            if (_bomb is null)
                return;
            _bomb.Detach();
            _bomb.SetCollisionsless(true, Lobby);
            _bomb.AttachTo(player, PedBone.SKEL_Pelvis, new Vector3(0, 0, 0.24), new Vector3(270, 0, 0));

            if (_bombAtPlayer != player)
            {
                SendBombPlantInfos(player);
                _bombAtPlayer = player;
            }
            player.TriggerEvent(ToClientEvent.BombNotOnHand);
        }

        private void BombToHand(ITDSPlayer player)
        {
            if (_bomb is null)
                return;
            _bomb.Detach();
            _bomb.SetCollisionsless(true, Lobby);
            _bomb.AttachTo(player, PedBone.SKEL_R_Finger01, new Vector3(0.1, 0, 0), null);

            if (_bombAtPlayer != player)
            {
                SendBombPlantInfos(player);
                _bombAtPlayer = player;
            }
            player.TriggerEvent(ToClientEvent.BombOnHand);
        }

        private void DetonateBomb()
        {
            // NAPI.Explosion.CreateOwnedExplosion(planter.Player, ExplosionType.GrenadeL,
            // bomb.Position, 200, Dimension); use 0x172AA1B624FA1013 as Hash instead if not getting fixed
            Lobby.TriggerEvent(ToClientEvent.BombDetonated);
            _counterTerroristTeam.FuncIterate((player, team) =>
            {
                if (player.Lifes == 0)
                    return;
                int damage = player.Health + player.Armor;
                Lobby.DmgSys.UpdateLastHitter(player, _planter, damage);
                player.Damage(ref damage, out bool killed);
                if (_planter != null && _planter.CurrentRoundStats != null)
                    _planter.CurrentRoundStats.Damage += damage;
            });
            // TERROR WON //
            WinnerTeam = _terroristTeam;
            if (Lobby.CurrentRoundStatus == RoundStatus.Round)
                Lobby.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.BombExploded);
        }

        private void DropBomb()
        {
            if (_bombAtPlayer is null)
                return;
            if (_bomb is null)
                return;
            _bomb.Detach();
            _bomb.Freeze(true, Lobby);
            _bomb.Position = _bombAtPlayer.Position;
            _bombTakeMarker = NAPI.Marker.CreateMarker(0, _bomb.Position, new Vector3(), new Vector3(), 1,
                                                        new GTANetworkAPI.Color(180, 0, 0, 180), true, Lobby.Dimension) as ITDSMarker;
            var bombTakeCol = NAPI.ColShape.CreateSphereColShape(_bomb.Position, 2, Lobby.Dimension) as ITDSColShape;
            _lobbyBombTakeCol[Lobby] = bombTakeCol!;
            _bombAtPlayer.TriggerEvent(ToClientEvent.BombNotOnHand);
            _bombAtPlayer = null;
        }

        private void GiveBombToRandomTerrorist()
        {
            int amount = _terroristTeam.Players.Count;
            if (amount == 0)
                return;

            ITDSPlayer player = SharedUtils.GetRandom(_terroristTeam.Players);
            if (player.CurrentWeapon == WeaponHash.Unarmed)
                BombToHand(player);
            else
                BombToBack(player);
            player.TriggerEvent(ToClientEvent.PlayerGotBomb, Map.BombInfo?.PlantPositionsJson ?? "{}");
        }

        private void TakeBomb(ITDSPlayer player)
        {
            if (_bomb is null)
                return;
            ToggleBombAtHand(player, player.CurrentWeapon, player.CurrentWeapon);
            //_bomb.FreezePosition = false;
            _bombTakeMarker?.Delete();
            _bombTakeMarker = null;
            _lobbyBombTakeCol.Remove(Lobby, out ITDSColShape? col);
            if (col != null)
                col.Delete();
        }

        private void ToggleBombAtHand(ITDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon)
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

        #endregion Private Methods
    }
}
