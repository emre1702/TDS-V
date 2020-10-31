using TDS_Server.Data.Interfaces.LobbySystem.Actions;

namespace TDS_Server.LobbySystem.Actions
{
    public class GangLobbyActions : IGangLobbyActions
    {
        /*
         *  public async Task StartGangwar(ITDSPlayer attacker, int gangwarAreaId)
        {
            var gangwarArea = _gangwarAreasHandler.GetById(gangwarAreaId);
            if (gangwarArea is null)
            {
                //Todo This gangwar area does not exist (anymore).
                return;
            }

            if (gangwarArea.Owner is null)
            {
                var task = gangwarArea.SetConqueredWithoutAttack(attacker.Gang);
                if (task is { })
                    await task.ConfigureAwait(false);
                return;
            }

            if (!CheckCanStartAction(attacker, gangwarArea))
                return;
            if (!CheckCanStartGangwar(attacker, gangwarArea))
                return;

            gangwarArea.SetInPreparation(attacker.Gang);

            var lobby = ActivatorUtilities.CreateInstance<Arena>(_serviceProvider, CreateEntity(gangwarArea), gangwarArea, true);

            await lobby.AddToDB().ConfigureAwait(false);
            await NAPI.Task.RunWait(() =>
            {
                EventsHandler.OnLobbyCreated(lobby);
                lobby.SetMapList(new List<MapDto> { gangwarArea.Map });

                lobby.SetRoundStatus(RoundStatus.NewMapChoose);
                lobby.Start();
            }).ConfigureAwait(false);

            await lobby.AddPlayer(attacker, 1).ConfigureAwait(false);
        }

        private bool CheckCanStartAction(ITDSPlayer attacker, GangwarArea gangwarArea)
        {
            if (attacker.Gang == _gangsHandler.None)
            {
                //todo You are not in a gang.
                return false;
            }
            if (attacker.GangRank is null)
            {
                //todo Your gang rank is not initialized yet.
                return false;
            }
            if (attacker.Gang.Entity is null || attacker.Gang.Entity.RankPermissions is null)
            {
                //todo Your gang doesn't really exist.
                return false;
            }
            if (attacker.Gang.InAction)
            {
                //todo Your gang is already in an action
                return false;
            }
            if (gangwarArea.Owner!.InAction)
            {
                //todo The owners are already in an action
                return false;
            }
            if (attacker.Lobby?.Type != LobbyType.GangLobby)
            {
                LoggingHandler.LogError("Tried to start an action, but is not in GangLobby", Environment.StackTrace, null, attacker);
                return false;
            }

            //todo Check general cooldowns
            //todo Check max actions per day or whatever

            return true;
        }

        private bool CheckCanStartGangwar(ITDSPlayer attacker, GangwarArea gangwarArea)
        {
            if (attacker.Gang.Entity.RankPermissions.StartGangwar > attacker.GangRank!.Rank)
            {
                //todo You don't have the permission in your gang to do that.
                return false;
            }
            if (attacker.Gang.PlayersOnline.Count < SettingsHandler.ServerSettings.MinPlayersOnlineForGangwar)
            {
                //todo Not enough players in your gang are online
                return false;
            }
            if (gangwarArea.Owner!.PlayersOnline.Count < SettingsHandler.ServerSettings.MinPlayersOnlineForGangwar)
            {
                //todo Not enough players in the other gang are online
                return false;
            }

            //todo Check cooldown
            //todo Check max gangwars per day or whatever

            return true;
        }

        private Lobbies CreateEntity(GangwarArea area)
        {
            var dummyDBTeam = LobbiesHandler.MainMenu.Teams[0].Entity.DeepCopy();

            var ownerDBTeam = area.Owner!.Entity.Team.DeepCopy();
            ownerDBTeam.Index = 1;

            var attackerDBTeam = area.Attacker!.Entity.Team.DeepCopy();
            attackerDBTeam.Index = 2;

            var lobby = new Lobbies
            {
                FightSettings = new LobbyFightSettings(),
                LobbyMaps = new HashSet<LobbyMaps> { new LobbyMaps { MapId = area.Entity!.MapId } },
                LobbyMapSettings = new LobbyMapSettings
                {
                    MapLimitType = MapLimitType.Display,
                },
                LobbyRoundSettings = new LobbyRoundSettings
                {
                    CountdownTime = (int)SettingsHandler.ServerSettings.GangwarPreparationTime,
                    RoundTime = (int)SettingsHandler.ServerSettings.GangwarActionTime,
                    ShowRanking = true
                },
                LobbyWeapons = LobbiesHandler.Arena.Entity.LobbyWeapons.Select(w => new LobbyWeapons
                {
                    Ammo = w.Ammo,
                    Damage = w.Damage,
                    Hash = w.Hash,
                    HeadMultiplicator = w.HeadMultiplicator
                }).ToHashSet(),    //LobbiesHandler.GetAllPossibleLobbyWeapons(MapType.Normal),
                LobbyRewards = new LobbyRewards
                {
                    MoneyPerAssist = LobbiesHandler.Arena.Entity.LobbyRewards.MoneyPerAssist,
                    MoneyPerDamage = LobbiesHandler.Arena.Entity.LobbyRewards.MoneyPerDamage,
                    MoneyPerKill = LobbiesHandler.Arena.Entity.LobbyRewards.MoneyPerKill,
                },
                IsOfficial = true,
                IsTemporary = false,
                OwnerId = -1,
                Name = $"[GW-Prep] {area.Attacker.Entity.Short}",
                Type = LobbyType.Arena,
                Teams = new List<Teams>
                {
                    dummyDBTeam,
                    ownerDBTeam,
                    attackerDBTeam
                },
            };

            return lobby;
        }*/
    }
}
