using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Database.Entity.Challenge;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private Dictionary<ChallengeType, List<PlayerChallenges>> _challengesDict = new Dictionary<ChallengeType, List<PlayerChallenges>>();

        #endregion Private Fields

        #region Public Methods

        public void AddToChallenge(ChallengeType type, int amount = 1, bool setTheValue = false)
        {
            try
            {
                if (!_challengesDict.TryGetValue(type, out List<PlayerChallenges>? list))
                    return;

                if (list.Count == 0)
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
                    _modAPI.Thread.QueueIntoMainThread(() => _challengesHandler.SyncCurrentAmount(this, challenge));

                    if (challenge.Frequency == ChallengeFrequency.Forever)
                    {
                        list.RemoveAt(i);
                        ExecuteForDBAsyncWithoutWait(async dbContext =>
                        {
                            dbContext.PlayerChallenges.Remove(challenge);
                            await dbContext.SaveChangesAsync();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public void InitChallengesDict()
        {
            _challengesDict = Entity!.Challenges.GroupBy(c => c.Challenge).ToDictionary(c => c.Key, c => c.ToList());
        }

        #endregion Public Methods
    }
}
