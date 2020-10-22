using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.LobbySystem.Weapons
{
    public class DamageTestLobbyWeapons : FightLobbyWeapons
    {
        public static List<LobbyWeapons>? AllWeapons;

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
