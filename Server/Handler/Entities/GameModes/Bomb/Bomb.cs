using System.Collections.Generic;
using System.Drawing;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Server.Data.Interfaces.ModAPI.Marker;
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

        private static readonly Dictionary<Arena, IColShape> _lobbyBombTakeCol = new Dictionary<Arena, IColShape>();
        private IMapObject? _bomb;
        private IMarker? _bombTakeMarker;

        #endregion Private Fields

        #region Private Methods

        private void BombToBack(ITDSPlayer player)
        {
            if (_bomb is null)
                return;
            _bomb.Detach();
            _bomb.SetCollisionsless(true, Lobby);
            _bomb.AttachTo(player, PedBone.SKEL_Pelvis, new Position3D(0, 0, 0.24), new Position3D(270, 0, 0));

            if (_bombAtPlayer != player)
            {
                SendBombPlantInfos(player);
                _bombAtPlayer = player;
            }
            player.SendEvent(ToClientEvent.BombNotOnHand);
        }

        private void BombToHand(ITDSPlayer player)
        {
            if (_bomb is null)
                return;
            _bomb.Detach();
            _bomb.SetCollisionsless(true, Lobby);
            _bomb.AttachTo(player, PedBone.SKEL_R_Finger01, new Position3D(0.1, 0, 0), null);

            if (_bombAtPlayer != player)
            {
                SendBombPlantInfos(player);
                _bombAtPlayer = player;
            }
            player.SendEvent(ToClientEvent.BombOnHand);
        }

        private void DetonateBomb()
        {
            // NAPI.Explosion.CreateOwnedExplosion(planter.Player, ExplosionType.GrenadeL,
            // bomb.Position, 200, Dimension); use 0x172AA1B624FA1013 as Hash instead if not getting fixed
            ModAPI.Sync.SendEvent(Lobby, ToClientEvent.BombDetonated);
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
            _bomb.Position = _bombAtPlayer.ModPlayer!.Position;
            _bombTakeMarker = ModAPI.Marker.Create(0, _bomb.Position, new Position3D(), new Position3D(), 1,
                                                        Color.FromArgb(180, 180, 0, 0), true, Lobby);
            IColShape bombtakecol = ModAPI.ColShape.CreateSphere(_bomb.Position, 2, Lobby);
            _lobbyBombTakeCol[Lobby] = bombtakecol;
            _bombAtPlayer.SendEvent(ToClientEvent.BombNotOnHand);
            _bombAtPlayer = null;
        }

        private void GiveBombToRandomTerrorist()
        {
            int amount = _terroristTeam.Players.Count;
            if (amount == 0)
                return;

            ITDSPlayer player = SharedUtils.GetRandom(_terroristTeam.Players);
            if (player.ModPlayer!.CurrentWeapon == WeaponHash.Unarmed)
                BombToHand(player);
            else
                BombToBack(player);
            player.SendEvent(ToClientEvent.PlayerGotBomb, Map.BombInfo?.PlantPositionsJson ?? "{}");
        }

        private void TakeBomb(ITDSPlayer player)
        {
            if (_bomb is null)
                return;
            ToggleBombAtHand(player, player.ModPlayer!.CurrentWeapon, player.ModPlayer.CurrentWeapon);
            //_bomb.FreezePosition = false;
            _bombTakeMarker?.Delete();
            _bombTakeMarker = null;
            _lobbyBombTakeCol.Remove(Lobby, out IColShape? col);
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
