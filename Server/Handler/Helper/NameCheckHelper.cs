using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
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
                    => player.Name.Equals(input, StringComparison.CurrentCultureIgnoreCase),

                IsNameCheckLevel.EqualsScName
                    => player.SocialClubName.Equals(input, StringComparison.CurrentCultureIgnoreCase),

                IsNameCheckLevel.ContainsName
                    => player.Name.Contains(input, StringComparison.CurrentCultureIgnoreCase),

                IsNameCheckLevel.ContainsScName
                    => player.SocialClubName.Contains(input, StringComparison.CurrentCultureIgnoreCase),

                _ => false
            };
        }

        #endregion Public Methods
    }
}
