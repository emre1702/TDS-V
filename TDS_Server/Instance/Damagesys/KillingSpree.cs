namespace TDS_Server.Instance
{

    using System;
    using System.Collections.Generic;
    using TDS_Server.Instance.Player;
    using TDS_Server.Manager.Utility;

    partial class Damagesys
    {

        private static readonly Dictionary<int, Tuple<string, int, int>> sSpreeReward =
            new Dictionary<int, Tuple<string, int, int>>
            {
                [3] = new Tuple<string, int, int>("healtharmor", 30, 0),
                [5] = new Tuple<string, int, int>("healtharmor", 50, 0),
                [10] = new Tuple<string, int, int>("healtharmor", 100, 0),
                [15] = new Tuple<string, int, int>("healtharmor", 100, 0)
            };

        private void CheckKillingSpree(TDSPlayer character)
        {
            if (!sSpreeReward.ContainsKey(character.KillingSpree))
                return;

            Tuple<string, int, int> reward = sSpreeReward[character.KillingSpree];
            string rewardtyp = reward.Item1;
            if (rewardtyp == "healtharmor")
            {
                int bonus = reward.Item2;
                character.CurrentLobby.SendAllPlayerLangNotification((lang) =>
                {
                    return Utils.GetReplaced(lang.KILLING_SPREE_HEALTHARMOR, character.Client.Name,
                        character.KillingSpree.ToString(), bonus.ToString());
                });
                character.AddHPArmor(bonus);
            }
        }

        public void AddToKillingSpree(TDSPlayer character)
        {
            
        }
    }

}
