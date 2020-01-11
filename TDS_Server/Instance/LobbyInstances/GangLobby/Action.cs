using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Common.Enum;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.LobbyEntities;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class GangLobby
    {

        public async Task StartGangwar(TDSPlayer attacker, int gangwarAreaId)
        {
            var gangwarArea = GangwarAreasManager.GetById(gangwarAreaId);
            if (gangwarArea is null)
            {
                //Todo This gangwar area does not exist (anymore).
                return;
            }

            if (gangwarArea.Owner is null)
            {
                await gangwarArea.SetCaptured(attacker.Gang);
                return;
            }

            if (!CheckCanStartAction(attacker, gangwarArea))
                return;
            if (!CheckCanStartGangwar(attacker, gangwarArea))
                return;

            gangwarArea.SetInPreparation(attacker.Gang);

            var lobby = new Arena(CreateEntity(gangwarArea), gangwarArea);

            await lobby.AddToDB();
            lobby.SetMapList(new List<MapDto> { gangwarArea.Map });

            lobby.SetRoundStatus(Enums.ERoundStatus.NewMapChoose);
            await lobby.AddPlayer(attacker, 1);

            lobby.Start();
        }

        private static Lobbies CreateEntity(GangwarArea area)
        {
            var dummyDBTeam = LobbyManager.MainMenu.Teams[0].Entity.DeepCopy();

            var attackerDBTeam = area.Attacker!.Entity.Team.DeepCopy();
            attackerDBTeam.Index = 1;

            var ownerDBTeam = area.Owner!.Entity.Team.DeepCopy();
            ownerDBTeam.Index = 2;

            var lobby = new Lobbies
            {
                AmountLifes = 1,
                LobbyMaps = new HashSet<LobbyMaps> { new LobbyMaps { MapId = area.Entity.MapId } },
                LobbyMapSettings = new LobbyMapSettings
                {
                  MapLimitType = EMapLimitType.Display
                },
                LobbyRoundSettings = new LobbyRoundSettings
                {
                  CountdownTime = (int)SettingsManager.ServerSettings.GangwarPreparationTime,
                  RoundTime = (int)SettingsManager.ServerSettings.GangwarActionTime,
                  ShowRanking = true
                },
                LobbyWeapons = LobbyManager.GetAllPossibleLobbyWeapons(EMapType.Normal),
                LobbyRewards = new LobbyRewards
                {
                    MoneyPerAssist = LobbyManager.Arena.LobbyEntity.LobbyRewards.MoneyPerAssist,
                    MoneyPerDamage = LobbyManager.Arena.LobbyEntity.LobbyRewards.MoneyPerDamage,
                    MoneyPerKill = LobbyManager.Arena.LobbyEntity.LobbyRewards.MoneyPerKill,
                },
                IsOfficial = true,
                IsTemporary = false,
                OwnerId = -1,
                Name = $"[GW-Prep] {area.Attacker.Entity.Short}",
                Type = ELobbyType.Arena,
                Teams = new List<Teams>
                {
                    dummyDBTeam,
                    attackerDBTeam,
                    ownerDBTeam
                },
                
            };


            return lobby;
        }

        private static bool CheckCanStartAction(TDSPlayer attacker, GangwarArea gangwarArea)
        {
            if (attacker.Gang == Gang.None)
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
            if (attacker.CurrentLobby?.Type != ELobbyType.GangLobby)
            {
                ErrorLogsManager.Log("Tried to start an action, but is not in GangLobby", Environment.StackTrace, attacker);
                return false;
            }

            //todo Check general cooldowns 
            //todo Check max actions per day or whatever

            return true;
        }

        private static bool CheckCanStartGangwar(TDSPlayer attacker, GangwarArea gangwarArea)
        {
            if (attacker.Gang.Entity.RankPermissions.StartGangwar > attacker.GangRank!.Rank)
            {
                //todo You don't have the permission in your gang to do that.
                return false;
            }
            if (attacker.Gang.PlayersOnline.Count < SettingsManager.ServerSettings.MinPlayersOnlineForGangwar)
            {
                //todo Not enough players in your gang are online
                return false;
            }
            if (gangwarArea.Owner!.PlayersOnline.Count < SettingsManager.ServerSettings.MinPlayersOnlineForGangwar)
            {
                //todo Not enough players in the other gang are online
                return false;
            }

            //todo Check cooldown 
            //todo Check max gangwars per day or whatever

            return true;
        }
    }
}
