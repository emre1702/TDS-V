using GTANetworkAPI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Weapons;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class DamageTestLobbyDeathmatch : FightLobbyDeathmatch, IDamageTestLobbyDeathmatch
    {
        public DamageTestLobbyDeathmatch(IDamageTestLobby lobby, IFightLobbyEventsHandler events, IDamagesys damage, LangHelper langHelper)
            : base(lobby, events, damage, langHelper)
        {
            AmountLifes = short.MaxValue;
        }

        public override async Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            player.Lifes = short.MaxValue;
            await base.OnPlayerDeath(player, killer, weapon).ConfigureAwait(false);

            player.DeathSpawnTimer?.Kill();
            NAPI.Task.RunSafe(() =>
            {
                player.Spawn(Lobby.MapHandler.SpawnPoint, Lobby.MapHandler.SpawnRotation);
                player.Freeze(false);
                player.SpectateHandler.SetSpectates(null);
                player.TriggerEvent(ToClientEvent.PlayerRespawned);
            });
        }

        public void SetWeaponDamage(DamageTestWeapon weaponDamageData)
        {
            Damage.SetDamage(weaponDamageData.Weapon, new DamageDto(weaponDamageData));
        }

        public IEnumerable<DamageTestWeapon> GetWeaponDamages()
        {
            var damageDict = Damage.GetDamages();
            return damageDict.Select(e => new DamageTestWeapon
            {
                Weapon = e.Key,
                Damage = e.Value.Damage,
                HeadshotDamageMultiplier = e.Value.HeadMultiplier
            });
        }

        protected override void InitDamagesys(IDamagesys damagesys)
        {
            if (DamageTestLobbyWeapons.AllWeapons is { })
                damagesys.Init(DamageTestLobbyWeapons.AllWeapons, new List<LobbyKillingspreeRewards>());
        }
    }
}
