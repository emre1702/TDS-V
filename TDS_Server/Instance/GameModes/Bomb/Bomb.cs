using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Manager.Utility;
using TDS_Server.Enum;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.GameModes
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

        private Object? _bomb;
        private Marker? _bombTakeMarker;

        private static readonly Dictionary<Arena, ColShape> _lobbyBombTakeCol = new Dictionary<Arena, ColShape>();

        private void GiveBombToRandomTerrorist()
        {
            int amount = _terroristTeam.Players.Count;
            if (amount == 0)
                return;

            int rnd = CommonUtils.Rnd.Next(amount);
            TDSPlayer character = _terroristTeam.Players[rnd];
            if (character.Client.CurrentWeapon == WeaponHash.Unarmed)
                BombToHand(character);
            else
                BombToBack(character);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerGotBomb, Map.BombInfo?.PlantPositionsJson ?? "{}");
        }

        private void DetonateBomb()
        {
            // NAPI.Explosion.CreateOwnedExplosion(planter.Client, ExplosionType.GrenadeL, bomb.Position, 200, Dimension);   use 0x172AA1B624FA1013 as Hash instead if not getting fixed
            Lobby.SendAllPlayerEvent(DToClientEvent.BombDetonated, null);
            _counterTerroristTeam.FuncIterate((character, team) =>
            {
                if (character.Lifes == 0)
                    return;
                int damage = character.Client.Health + character.Client.Armor;
                Lobby.DmgSys.UpdateLastHitter(character, _planter, damage);
                character.Damage(ref damage);
                if (_planter != null && _planter.CurrentRoundStats != null)
                    _planter.CurrentRoundStats.Damage += damage;
            });
            // TERROR WON //
            if (Lobby.CurrentRoundStatus == ERoundStatus.Round)
                Lobby.SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.BombExploded);
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

        private void BombToHand(TDSPlayer character)
        {
            if (_bomb == null)
                return;
            Workaround.DetachEntity(_bomb);
            Workaround.SetEntityCollisionless(_bomb, true, Lobby);
            Workaround.AttachEntityToEntity(_bomb, character.Client, EBone.SKEL_R_Finger01, new Vector3(0.1, 0, 0), new Vector3(), Lobby);
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
            Workaround.SetEntityCollisionless(_bomb, true, Lobby);
            Workaround.AttachEntityToEntity(_bomb, character.Client, EBone.SKEL_Pelvis, new Vector3(0, 0, 0.24), new Vector3(270, 0, 0), Lobby);
            if (_bombAtPlayer != character)
                SendBombPlantInfos(character);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.BombNotOnHand);
            _bombAtPlayer = character;
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
                                                        new Color(180, 0, 0, 180), true, Lobby.Dimension);
            ColShape bombtakecol = NAPI.ColShape.CreateSphereColShape(_bomb.Position, 2);
            _lobbyBombTakeCol[Lobby] = bombtakecol;
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
            _lobbyBombTakeCol.Remove(Lobby, out ColShape col);
            NAPI.ColShape.DeleteColShape(col);
        }
    }
}
