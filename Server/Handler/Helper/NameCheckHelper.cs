using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;

namespace TDS.Server.Handler.Helper
{
    public class NameCheckHelper
    {
        public bool IsName(ITDSPlayer player, string input, IsNameCheckLevel isNameCheckLevel)
        {
            return isNameCheckLevel switch
            {
                IsNameCheckLevel.EqualsName
                    => player.Name.Equals(input, StringComparison.OrdinalIgnoreCase),

                IsNameCheckLevel.EqualsScName
                    => player.SocialClubName.Equals(input, StringComparison.OrdinalIgnoreCase),

                IsNameCheckLevel.ContainsName
                    => player.Name.Contains(input, StringComparison.OrdinalIgnoreCase),

                IsNameCheckLevel.ContainsScName
                    => player.SocialClubName.Contains(input, StringComparison.OrdinalIgnoreCase),

                _ => false
            };
        }
    }
}
