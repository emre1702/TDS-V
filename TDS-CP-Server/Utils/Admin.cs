using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TDSCPServer.Enums;

namespace TDSCPServer.Utils
{
    public static class Admin
    {
        private static readonly Dictionary<EActions, EAdminLevels> requiredDict = new Dictionary<EActions, EAdminLevels>
        {
            { EActions.RestLogError, EAdminLevels.ProjectLeader },
            { EActions.RestLogChat, EAdminLevels.Administrator },
            { EActions.RestLogLogin, EAdminLevels.Administrator },
            { EActions.RestLogRegister, EAdminLevels.Administrator },
            { EActions.RestLogLobbyOwner, EAdminLevels.Supporter },
            { EActions.RestLogLobbyJoin, EAdminLevels.Supporter },
            { EActions.RestLogVIP, EAdminLevels.Supporter },

            { EActions.AdminLogAll, EAdminLevels.User },
            { EActions.AdminLogPermaban, EAdminLevels.User },
            { EActions.AdminLogTimeban, EAdminLevels.User },
            { EActions.AdminLogUnban, EAdminLevels.User },
            { EActions.AdminLogPermamute, EAdminLevels.User },
            { EActions.AdminLogTimemute, EAdminLevels.User },
            { EActions.AdminLogUnmute, EAdminLevels.User },
            { EActions.AdminLogNext, EAdminLevels.User },
            { EActions.AdminLogKick, EAdminLevels.User },
            { EActions.AdminLogLobbyKick, EAdminLevels.User },
            { EActions.AdminLogPermabanLobby, EAdminLevels.User },
            { EActions.AdminLogTimebanLobby, EAdminLevels.User },
            { EActions.AdminLogUnbanLobby, EAdminLevels.User },
        };

        public static bool IsAllowed(ClaimsPrincipal user, EActions action)
        {
            string adminlvlstr = user.Claims.FirstOrDefault(c => c.Type == "AdminLvl").Value;
            if (adminlvlstr == null)
                return false;
            int adminlvl = Convert.ToInt32(adminlvlstr);
            if (!requiredDict.ContainsKey(action))
                return true;
            int requiredlvl = (int) requiredDict[action]; 
            return adminlvl >= requiredlvl;
        }
    }
}
