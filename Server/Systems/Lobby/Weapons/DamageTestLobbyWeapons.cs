using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.LobbySystem.Weapons
{
    public class DamageTestLobbyWeapons : FightLobbyWeapons
    {
        public static List<LobbyWeapons>? AllWeapons { get; private set; }

        public DamageTestLobbyWeapons(IDamageTestLobby lobby, IFightLobbyEventsHandler events, IServiceProvider serviceProvider) : base(lobby, events)
        {
            if (AllWeapons is null)
            {
                var dbContext = serviceProvider.GetRequiredService<TDSDbContext>();
                AllWeapons = dbContext.Weapons.Select(w => new LobbyWeapons
                {
                    Ammo = 9999,
                    Damage = w.Damage,
                    Hash = w.Hash,
                    HeadMultiplicator = w.HeadShotDamageModifier,
                    Lobby = lobby.Entity.Id
                }).ToList();
            }
        }

        internal override IEnumerable<LobbyWeapons> GetAllWeapons()
            => AllWeapons!;
    }
}
