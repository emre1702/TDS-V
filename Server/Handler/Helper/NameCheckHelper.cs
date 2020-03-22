using System;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Helper
{
    public class NameCheckHelper
    {
        public bool IsName(ITDSPlayer player, string input, IsNameCheckLevel isNameCheckLevel)
            => isNameCheckLevel switch
            {
                IsNameCheckLevel.Equals 
                    => player.DisplayName.Equals(input, StringComparison.CurrentCultureIgnoreCase) 
                        || player.ModPlayer?.Name.Equals(input, StringComparison.CurrentCultureIgnoreCase) == true
                        || player.ModPlayer?.SocialClubName.Equals(input, StringComparison.CurrentCultureIgnoreCase) == true,
                        
                IsNameCheckLevel.Contains
                    => player.DisplayName.Contains(input, StringComparison.CurrentCultureIgnoreCase)
                        || player.ModPlayer?.Name.Contains(input, StringComparison.CurrentCultureIgnoreCase) == true
                        || player.ModPlayer?.SocialClubName.Contains(input, StringComparison.CurrentCultureIgnoreCase) == true,

                _ => false
            };
    }
}
