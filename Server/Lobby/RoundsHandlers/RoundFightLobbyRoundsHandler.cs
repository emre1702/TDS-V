using System;

namespace TDS_Server.LobbySystem.RoundsHandlers
{
    public class RoundFightLobbyRoundsHandler
    {
        internal void NewMapChoose()
        {
            throw new NotImplementedException();
        }

        internal void Countdown()
        {
            throw new NotImplementedException();
        }

        internal void InRound()
        {
            throw new NotImplementedException();
        }

        internal void RoundEnd()
        {
            throw new NotImplementedException();
        }

        internal void RoundEndRanking()
        {
            throw new NotImplementedException();
        }

        internal void MapClear()
        {
            throw new NotImplementedException();
        }
    }
}

/*
 * public LobbyRoundSettings RoundSettings => Entity.LobbyRoundSettings;

        public Gamemode? CurrentGameMode;

        public RoundStatus CurrentRoundStatus = RoundStatus.None;
        public ITDSPlayer? CurrentRoundEndBecauseOfPlayer;
        public bool RemoveAfterOneRound { get; set; }
        public RoundEndReason CurrentRoundEndReason { get; private set; }
        public Dictionary<ILanguage, string>? RoundEndReasonText { get; private set; }

        private ITeam? _currentRoundEndWinnerTeam;

        private List<RoundPlayerRankingStat>? _ranking;

        public void SetRoundStatus(RoundStatus status, RoundEndReason roundEndReason = RoundEndReason.Time)
        {
            CurrentRoundStatus = status;
            if (status == RoundStatus.RoundEnd)
                CurrentRoundEndReason = roundEndReason;
            RoundStatus nextStatus = _nextRoundStatsDict[status];
            _nextRoundStatusTimer?.Kill();
            if (!IsEmpty())
            {
                _nextRoundStatusTimer = new TDSTimer(() =>
                {
                    if (IsEmpty() && CurrentGameMode?.CanEndRound(RoundEndReason.Empty) != false)
                        SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.Empty);
                    else
                        SetRoundStatus(nextStatus);
                }, DurationsDict[status]);
                try
                {
                    if (_roundStatusMethod.ContainsKey(status))
                        NAPI.Task.Run(_roundStatusMethod[status]);
                }
                catch (Exception ex)
                {
                    LoggingHandler.LogError($"Could not call method for round status {status.ToString()} for lobby {Name} with Id {Id}. Exception: " + ex.Message, ex.StackTrace ?? "?");
                    SendMessage((lang) => lang.LOBBY_ERROR_REMOVE);
                    if (!IsOfficial)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        Remove();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
            else if (CurrentRoundStatus != RoundStatus.RoundEnd)
            {
                NAPI.Task.Run(() =>
                {
                    _roundStatusMethod[RoundStatus.RoundEnd]();
                    _roundStatusMethod[RoundStatus.MapClear]();
                });
                CurrentRoundStatus = RoundStatus.None;
            }
        }

        private void StartMapClear()
        {
            DeleteMapBlips();
            ClearTeamPlayersAmounts();
            TriggerEvent(ToClientEvent.MapClear);

            CurrentGameMode?.StartMapClear();
        }

        private void StartNewMapChoose()
        {
            MapDto? nextMap = GetNextMap();
            if (nextMap == null)
                return;
            SavePlayerLobbyStats = !nextMap.Info.IsNewMap;
            if (nextMap.Info.IsNewMap)
                SendNotification(lang => lang.TESTING_MAP_NOTIFICATION, flashing: true);
            CurrentGameMode = _gameModeByMapType[nextMap.Info.Type](this, nextMap, _serviceProvider);
            CurrentGameMode?.StartMapChoose();
            CreateTeamSpawnBlips(nextMap);
            CreateMapLimitBlips(nextMap);
            if (RoundSettings.MixTeamsAfterRound)
                MixTeams();
            TriggerEvent(ToClientEvent.MapChange, nextMap.ClientSyncedDataJson);
            _currentMap = nextMap;
            _currentMapSpectatorPosition = _currentMap.LimitInfo.Center.SwitchNamespace().AddToZ(10);
            RoundEndReasonText = null;
        }

        private void StartRoundCountdown()
        {
            _allRoundWeapons = Entity.LobbyWeapons.Where(w => CurrentGameMode != null ? CurrentGameMode.IsWeaponAllowed(w.Hash) : true);
            SetAllPlayersInCountdown();
            CurrentGameMode?.StartRoundCountdown();
        }

        private void StartRound()
        {
            StartRoundForAllPlayer();
            CurrentGameMode?.StartRound();
        }

        private async void EndRound()
        {
            bool isEmpty = IsEmpty();
            if (!_dontRemove && (Entity.IsTemporary && isEmpty || RemoveAfterOneRound))
            {
                await Remove();
                return;
            }

            NAPI.Task.Run(() =>
            {
                if (!isEmpty)
                {
                    _currentRoundEndWinnerTeam = GetRoundWinnerTeam();
                    RoundEndReasonText = GetRoundEndReasonText(_currentRoundEndWinnerTeam);

                    FuncIterateAllPlayers((character, team) =>
                    {
                        character.TriggerEvent(ToClientEvent.RoundEnd, team is null || team.IsSpectator, RoundEndReasonText != null ? RoundEndReasonText[character.Language] : string.Empty, _currentMap?.BrowserSyncedData.Id ?? 0);
                        if (character.Lifes > 0 && _currentRoundEndWinnerTeam != null && team != _currentRoundEndWinnerTeam && CurrentRoundEndReason != RoundEndReason.Death)
                            character.Kill();
                        character.Lifes = 0;
                    });
                }

                foreach (var team in Teams)
                {
                    team.AlivePlayers?.Clear();
                }

                DmgSys.Clear();

                CurrentGameMode?.StopRound();

                if (!isEmpty)
                {
                    RewardAllPlayer();
                    _ranking = GetOrderedRoundRanking();
                    SaveAllPlayerRoundStats();
                }
                else
                {
                    _ranking = null;
                }
            });

            await ExecuteForDBAsync(async (dbContext) =>
            {
                await dbContext.SaveChangesAsync();
            });

            _serverStatsHandler.AddArenaRound(CurrentRoundEndReason, IsOfficial);
        }

        private void ShowRoundRanking()
        {
            if (_ranking is null || _ranking.Count == 0)
                return;

            try
            {
                ITDSPlayer winner = _ranking.First().Player;
                ITDSPlayer? second = _ranking.ElementAtOrDefault(1)?.Player;
                ITDSPlayer? third = _ranking.ElementAtOrDefault(2)?.Player;

                //Vector3 rot = new Vector3(0, 0, 345);
                winner.Spawn(new Vector3(-425.48, 1123.55, 325.85), 345);
                winner.Freeze(true);
                winner.Dimension = Dimension;

                if (second is { })
                {
                    second.Spawn(new Vector3(-427.03, 1123.21, 325.85), 345);
                    second.Freeze(true);
                    second.Dimension = Dimension;
                }

                if (third is { })
                {
                    third.Spawn(new Vector3(-424.33, 1122.5, 325.85), 345);
                    third.Freeze(true);
                    third.Dimension = Dimension;
                }

                var othersPos = new Vector3(-425.48, 1123.55, 335.85);
                foreach (var player in Players.Values)
                {
                    if (player != winner && player != second && player != third)
                    {
                        player.Position = othersPos;
                        player.SetInvisible(true);
                    }
                    else
                        player.SetInvisible(false);
                }

                string json = Serializer.ToBrowser(_ranking);
                TriggerEvent(ToClientEvent.StartRankingShowAfterRound, json, winner.RemoteId, second?.RemoteId ?? 0, third?.RemoteId ?? 0);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError("Error occured: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace);
            }
        }

        private void RoundCheckForEnoughAlive()
        {
            int teamsInRound = GetTeamAmountStillInRound();
            if (teamsInRound <= 1 && CurrentGameMode?.CanEndRound(RoundEndReason.NewPlayer) != false)
            {
                SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.Death);
            }
        }

        private ITeam? GetRoundWinnerTeam()
        {
            if (CurrentGameMode?.WinnerTeam is { })
                return CurrentGameMode.WinnerTeam;
            return CurrentRoundEndReason switch
            {
                RoundEndReason.Death => GetTeamStillInRound(),
                RoundEndReason.Time => GetTeamWithHighestHP(),
                _ => null,
            };
        }

        private Dictionary<ILanguage, string>? GetRoundEndReasonText(ITeam? winnerTeam)
        {
            return CurrentRoundEndReason switch
            {
                RoundEndReason.Death => LangHelper.GetLangDictionary(lang =>
                    {
                        return winnerTeam != null ? string.Format(lang.ROUND_END_DEATH_INFO, winnerTeam.Entity.Name) : lang.ROUND_END_DEATH_ALL_INFO;
                    }),
                RoundEndReason.Time => LangHelper.GetLangDictionary(lang =>
                    {
                        return winnerTeam != null ? string.Format(lang.ROUND_END_TIME_INFO, winnerTeam.Entity.Name) : lang.ROUND_END_TIME_TIE_INFO;
                    }),
                RoundEndReason.BombExploded => LangHelper.GetLangDictionary(lang =>
                   {
                       return string.Format(lang.ROUND_END_BOMB_EXPLODED_INFO, winnerTeam?.Entity.Name ?? "-");
                   }),
                RoundEndReason.BombDefused => LangHelper.GetLangDictionary(lang =>
                   {
                       return string.Format(lang.ROUND_END_BOMB_DEFUSED_INFO, winnerTeam?.Entity.Name ?? "-");
                   }),
                RoundEndReason.Command => LangHelper.GetLangDictionary(lang =>
                   {
                       return string.Format(lang.ROUND_END_COMMAND_INFO, CurrentRoundEndBecauseOfPlayer?.DisplayName ?? "-");
                   }),
                RoundEndReason.NewPlayer => LangHelper.GetLangDictionary(lang =>
                   {
                       return lang.ROUND_END_NEW_PLAYER_INFO;
                   }),
                RoundEndReason.TargetEmpty => LangHelper.GetLangDictionary(lang =>
                   {
                       return lang.ROUND_END_TARGET_EMPTY_INFO;
                   }),
                RoundEndReason.Error => LangHelper.GetLangDictionary(lang =>
                   {
                       return lang.ERROR_INFO;
                   }),
                RoundEndReason.PlayerWon => LangHelper.GetLangDictionary(lang =>
                    {
                        return string.Format(lang.PLAYER_WON_INFO, CurrentRoundEndBecauseOfPlayer?.DisplayName ?? "-");
                    }),

                _ => null,
            };
        }*/
