﻿using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.DamageSystem;
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
        public DamageTestLobbyDeathmatch(IDamageTestLobby lobby, IFightLobbyEventsHandler events, IDamageHandler damageHandler, LangHelper langHelper)
            : base(lobby, events, damageHandler, langHelper)
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
            Damage.DamageProvider.SetDamage(weaponDamageData.Weapon, new DamageDto(weaponDamageData));
        }

        public IEnumerable<DamageTestWeapon> GetWeaponDamages()
        {
            var damageDict = Damage.DamageProvider.GetDamages();
            return damageDict.Select(e => new DamageTestWeapon
            {
                Weapon = e.Key,
                Damage = e.Value.Damage,
                HeadshotDamageMultiplier = e.Value.HeadMultiplier
            });
        }

        public override void InitDamageHandler(IDamageHandler damageHandler)
        {
            if (DamageTestLobbyWeapons.AllWeapons is { })
                damageHandler.Init(Lobby, DamageTestLobbyWeapons.AllWeapons, new List<LobbyKillingspreeRewards>());
        }
    }
}
