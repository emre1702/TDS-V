﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Helper;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Database.Entity.Challenge;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.PlayersSystem
{
    public class ChallengesHandler : IPlayerChallengesHandler
    {
        private Dictionary<ChallengeType, List<PlayerChallenges>> _challengesDict = new Dictionary<ChallengeType, List<PlayerChallenges>>();

        private readonly ILoggingHandler _logging;
        private readonly IChallengesHelper _challengesHelper;

#nullable disable
        private ITDSPlayer _player;
        private IPlayerEvents _events;
#nullable enable

        public ChallengesHandler(ILoggingHandler logging, IChallengesHelper challengesHelper)
        {
            _logging = logging;
            _challengesHelper = challengesHelper;
        }

        public void Init(ITDSPlayer player, IPlayerEvents events)
        {
            _player = player;
            _events = events;

            events.SettingsChanged += Events_SettingsChanged;
            events.Removed += Events_Removed;
        }

        private void Events_Removed()
        {
            _events.SettingsChanged -= Events_SettingsChanged;
            _events.Removed -= Events_Removed;
        }

        public void AddToChallenge(ChallengeType type, int amount = 1, bool setTheValue = false)
        {
            try
            {
                List<PlayerChallenges>? list;
                lock (_challengesDict)
                {
                    if (!_challengesDict.TryGetValue(type, out list))
                        return;
                }

                if (list.Count == 0)
                    return;
                AddToChallenges(list, amount, setTheValue);
            }
            catch (Exception ex)
            {
                _logging.LogError(ex, _player);
            }
        }

        private void AddToChallenges(List<PlayerChallenges> list, int amount = 1, bool setTheValue = false)
        {
            for (int i = list.Count - 1; i >= 0; --i)
            {
                var challenge = list[i];
                if (challenge.CurrentAmount == challenge.Amount)
                    continue;

                if (setTheValue)
                    challenge.CurrentAmount = Math.Min(amount, challenge.Amount);
                else
                    challenge.CurrentAmount = Math.Min(challenge.CurrentAmount + amount, challenge.Amount);
                NAPI.Task.RunSafe(() => _challengesHelper.SyncCurrentAmount(_player, challenge));

                if (challenge.Frequency == ChallengeFrequency.Forever)
                {
                    list.RemoveAt(i);
                    _player.Database.ExecuteForDBAsyncWithoutWait(async dbContext =>
                    {
                        dbContext.PlayerChallenges.Remove(challenge);
                        await dbContext.SaveChangesAsync().ConfigureAwait(false);
                    });
                }
            }
        }

        public void InitChallengesDict()
        {
            _challengesDict = _player.Entity!.Challenges.GroupBy(c => c.Challenge).ToDictionary(c => c.Key, c => c.ToList());
        }

        private void Events_SettingsChanged()
        {
            AddToChallenge(ChallengeType.ChangeSettings);
        }
    }
}