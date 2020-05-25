using System;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Helper
{
    public class NameCheckHelper
    {
        #region Public Methods

        public bool IsName(ITDSPlayer player, string input, IsNameCheckLevel isNameCheckLevel)
        {
            return isNameCheckLevel switch
            {
                IsNameCheckLevel.EqualsName
                    => player.ModPlayer?.Name.Equals(input, StringComparison.CurrentCultureIgnoreCase) == true,

                IsNameCheckLevel.EqualsScName
                    => player.ModPlayer?.SocialClubName.Equals(input, StringComparison.CurrentCultureIgnoreCase) == true,

                IsNameCheckLevel.ContainsName
                    => player.ModPlayer?.Name.Contains(input, StringComparison.CurrentCultureIgnoreCase) == true,

                IsNameCheckLevel.ContainsScName
                    => player.ModPlayer?.SocialClubName.Contains(input, StringComparison.CurrentCultureIgnoreCase) == true,

                _ => false
            };
        }

        #endregion Public Methods
    }
}
