﻿using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Database.Entity.Challenge;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        private Dictionary<ChallengeType, List<PlayerChallenges>> _challengesDict = new Dictionary<ChallengeType, List<PlayerChallenges>>();

        public void InitChallengesDict()
        {
            _challengesDict = Entity!.Challenges.GroupBy(c => c.Challenge).ToDictionary(c => c.Key, c => c.ToList());
        }

        public async void AddToChallenge(ChallengeType type, int amount = 1, bool setTheValue = false)
        {
            if (!_challengesDict.TryGetValue(type, out List<PlayerChallenges>? list))
                return;

            for (int i = list.Count - 1; i >= 0; --i)
            {
                var challenge = list[i];
                if (challenge.CurrentAmount == challenge.Amount)
                    continue;

                //Todo: Check if this is enough to save the challenge in DB
                if (setTheValue)
                    challenge.CurrentAmount = Math.Min(amount, challenge.Amount);
                else
                    challenge.CurrentAmount = Math.Min(challenge.CurrentAmount + amount, challenge.Amount);
                _challengesHandler.SyncCurrentAmount(this, challenge);

                if (challenge.Frequency == ChallengeFrequency.Forever)
                {
                    list.RemoveAt(i);
                    await ExecuteForDBAsync(async dbContext =>
                    {
                        dbContext.PlayerChallenges.Remove(challenge);
                        await dbContext.SaveChangesAsync();
                    });
                }
            }
        }
    }
}