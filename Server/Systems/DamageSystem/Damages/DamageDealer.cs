using GTANetworkAPI;
using System.Diagnostics.CodeAnalysis;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.DamageSystem.Damages;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.DamageSystem.Damages
{
    public class DamageDealer : IDamageDealer
    {
        private readonly IDamageProvider _damageProvider;
        private readonly IHitterHandler _hitterHandler;

        internal DamageDealer(IDamageProvider damageProvider, IHitterHandler hitterHandler)
        {
            _damageProvider = damageProvider;
            _hitterHandler = hitterHandler;
        }

        public void DamagePlayer(ITDSPlayer target, WeaponHash weapon, PedBodyPart pedBodyPart, ITDSPlayer? source)
        {
            if (!CanDealDamageToTarget(source, target))
                return;

            var isHeadshot = pedBodyPart == PedBodyPart.Head;
            var damage = _damageProvider.GetDamage(weapon, isHeadshot);

            target.HealthAndArmor.Remove(damage, out damage, out bool killed);
            AddStats(source, target, weapon, pedBodyPart, damage, killed);
            AddVisuals(source, target, weapon, damage, isHeadshot);
        }

        private bool CanDealDamageToTarget([NotNullWhen(true)] ITDSPlayer? player, ITDSPlayer target)
        {
            if (target.Dead)
                return false;
            if (player is null)
                return false;
            if (target.Lobby != player.Lobby)
                return false;
            if (target.Team == player.Team && player.Lobby?.Teams.HasAllVsAllTeams == false)
                return false;

            return true;
        }

        private void AddStats(ITDSPlayer player, ITDSPlayer target, WeaponHash weapon, PedBodyPart pedBodyPart, int damage, bool killed)
        {
            _hitterHandler.SetLastHitter(target, player, (uint)weapon, damage);

            player.WeaponStats.AddWeaponDamage(weapon, pedBodyPart, damage, killed);
            if (player.CurrentRoundStats != null)
                player.CurrentRoundStats.Damage += damage;
        }

        private void AddVisuals(ITDSPlayer player, ITDSPlayer target, WeaponHash weapon, int damage, bool isHeadshot)
        {
            NAPI.Task.RunSafe(() =>
            {
                if (player.Entity?.PlayerSettings.FightEffect.FloatingDamageInfo == true)
                    player.TriggerEvent(ToClientEvent.HitOpponent, target.RemoteId, damage);

                if (target.Health == 0 && isHeadshot)
                    target.TriggerEvent(ToClientEvent.ExplodeHead, weapon.ToString());
            });
        }
    }
}
