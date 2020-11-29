using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.LobbyHandlers;
using TDS.Server.Data.Interfaces.LobbySystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Models.Map;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Handler;
using TDS.Shared.Data.Enums;

namespace TDS.Server.GangActionAreaSystem.LobbyHandlers
{
    internal class BaseAreaLobbyHandler : IBaseGangActionAreaLobbyHandler
    {
        public IGangActionLobby? InLobby { get; private set; }

        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly ILobbiesProvider _lobbiesProvider;

#nullable disable
        private IBaseGangActionArea _area;
#nullable restore

        public BaseAreaLobbyHandler(LobbiesHandler lobbiesHandler, ISettingsHandler settingsHandler, ILobbiesProvider lobbiesProvider)
        {
            _lobbiesHandler = lobbiesHandler;
            _settingsHandler = settingsHandler;
            _lobbiesProvider = lobbiesProvider;
        }

        public void Init(IBaseGangActionArea area)
        {
            _area = area;
        }

        public async Task<IGangActionLobby> SetInGangActionLobby(ITDSPlayer attacker)
        {
            var entity = await CreateEntity();
            var lobby = _lobbiesProvider.Create<IGangActionLobby>(entity);
            InLobby = lobby;
            lobby.MapHandler.SetMapList(new List<MapDto>() { _area.MapHandler.Map });
            _area.Events.TriggerAddedToLobby(lobby);

            await lobby.Players.AddPlayer(attacker, (int)GangActionLobbyTeamIndex.Attacker).ConfigureAwait(false);
            lobby.Rounds.RoundStates.StartRound();

            return lobby;
        }

        private async Task<Lobbies> CreateEntity()
        {
            await Task.Yield();
            var mainLobbySpectatorTeam = await _lobbiesHandler.MainMenu.Teams.GetTeam(0);
            var dummyDBTeam = mainLobbySpectatorTeam.Entity.DeepCopy();

            var ownerDBTeam = _area.GangsHandler.Owner!.Entity.Team.DeepCopy();
            ownerDBTeam.Index = (int)GangActionLobbyTeamIndex.Owner;

            var attackerDBTeam = _area.GangsHandler.Attacker!.Entity.Team.DeepCopy();
            attackerDBTeam.Index = (int)GangActionLobbyTeamIndex.Attacker;

            var entity = new Lobbies
            {
                FightSettings = new LobbyFightSettings(),
                LobbyMaps = new HashSet<LobbyMaps> { new LobbyMaps { MapId = _area.DatabaseHandler.Entity!.MapId } },
                LobbyMapSettings = new LobbyMapSettings
                {
                    MapLimitType = MapLimitType.Display,
                },
                LobbyRoundSettings = new LobbyRoundSettings
                {
                    CountdownTime = (int)_settingsHandler.ServerSettings.GangActionPreparationTime,
                    RoundTime = (int)_settingsHandler.ServerSettings.GangActionRoundTime,
                    ShowRanking = true
                },
                LobbyWeapons = _lobbiesHandler.Arena.Entity.LobbyWeapons.Select(w => new LobbyWeapons
                {
                    Ammo = w.Ammo,
                    Damage = w.Damage,
                    Hash = w.Hash,
                    HeadMultiplicator = w.HeadMultiplicator
                }).ToHashSet(),    //LobbiesHandler.GetAllPossibleLobbyWeapons(MapType.Normal),
                LobbyRewards = new LobbyRewards
                {
                    MoneyPerAssist = _lobbiesHandler.Arena.Entity.LobbyRewards.MoneyPerAssist,
                    MoneyPerDamage = _lobbiesHandler.Arena.Entity.LobbyRewards.MoneyPerDamage,
                    MoneyPerKill = _lobbiesHandler.Arena.Entity.LobbyRewards.MoneyPerKill,
                },
                IsOfficial = true,
                IsTemporary = false,
                OwnerId = -1,
                Name = $"[GW-Prep] {_area.Attacker!.Entity.Short}",
                Type = LobbyType.Arena,
                Teams = new List<Teams>
                {
                    dummyDBTeam,
                    ownerDBTeam,
                    attackerDBTeam
                },
            };

            return entity;
        }
    }
}
