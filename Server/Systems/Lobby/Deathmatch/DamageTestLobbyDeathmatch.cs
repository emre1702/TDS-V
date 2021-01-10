using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.DamageSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Server.LobbySystem.Weapons;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.Deathmatch
{
    public class DamageTestLobbyDeathmatch : FightLobbyDeathmatch, IDamageTestLobbyDeathmatch
    {
        private readonly RemoteBrowserEventsHandler _remoteBrowserEventsHandler;

        public DamageTestLobbyDeathmatch(IDamageTestLobby lobby, IFightLobbyEventsHandler events, IDamageHandler damageHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(lobby, events, damageHandler)
        {
            AmountLifes = short.MaxValue;
            _remoteBrowserEventsHandler = remoteBrowserEventsHandler;

            remoteBrowserEventsHandler.Add(ToServerEvent.SetDamageTestWeaponDamage, SetDamageTestWeaponDamage, player => player.Lobby == Lobby && player.IsLobbyOwner);
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);

            _remoteBrowserEventsHandler.Remove(ToServerEvent.SetDamageTestWeaponDamage, SetDamageTestWeaponDamage);
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

        private object? SetDamageTestWeaponDamage(RemoteBrowserEventArgs args)
        {
            var weaponDamageData = Serializer.FromBrowser<DamageTestWeapon>((string)args.Args[0]);
            SetWeaponDamage(weaponDamageData);

            return null;
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