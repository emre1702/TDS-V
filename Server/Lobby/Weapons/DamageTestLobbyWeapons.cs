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
        private static List<LobbyWeapons>? _allWeapons;

        public DamageTestLobbyWeapons(IDamageTestLobby lobby, IFightLobbyEventsHandler events, IServiceProvider serviceProvider) : base(lobby, events)
        {
            if (_allWeapons is null)
            {
                var dbContext = serviceProvider.GetRequiredService<TDSDbContext>();
                _allWeapons = dbContext.Weapons.Select(w => new LobbyWeapons
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
            => _allWeapons!;
    }
}
