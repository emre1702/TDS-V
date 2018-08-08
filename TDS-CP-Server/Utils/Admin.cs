using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TDSCPServer.Enums;

namespace TDSCPServer.Utils
{
    public static class Admin
    {
        private static readonly Dictionary<Actions, AdminLevels> requiredDict = new Dictionary<Actions, AdminLevels>
        {
            { Actions.RestLogError, AdminLevels.ProjectLeader },
            { Actions.RestLogChat, AdminLevels.Administrator },
            { Actions.RestLogLogin, AdminLevels.Administrator },
            { Actions.RestLogRegister, AdminLevels.Administrator },
            { Actions.RestLogLobbyOwner, AdminLevels.Supporter },
            { Actions.RestLogLobbyJoin, AdminLevels.Supporter },
            { Actions.RestLogVIP, AdminLevels.Supporter },

            { Actions.AdminLogAll, AdminLevels.User },
            { Actions.AdminLogPermaban, AdminLevels.User },
            { Actions.AdminLogTimeban, AdminLevels.User },
            { Actions.AdminLogUnban, AdminLevels.User },
            { Actions.AdminLogPermamute, AdminLevels.User },
            { Actions.AdminLogTimemute, AdminLevels.User },
            { Actions.AdminLogUnmute, AdminLevels.User },
            { Actions.AdminLogNext, AdminLevels.User },
            { Actions.AdminLogKick, AdminLevels.User },
            { Actions.AdminLogLobbyKick, AdminLevels.User },
            { Actions.AdminLogPermabanLobby, AdminLevels.User },
            { Actions.AdminLogTimebanLobby, AdminLevels.User },
            { Actions.AdminLogUnbanLobby, AdminLevels.User },
        };

        public static bool IsAllowed(ClaimsPrincipal user, Actions action)
        {
            string uidstr = user.Claims.FirstOrDefault(c => c.Type == "AdminLvl").Value;
            if (uidstr == null)
                return false;
            int adminlvl = Convert.ToInt32(uidstr);
            if (!requiredDict.ContainsKey(action))
                return true;
            int requiredlvl = (int) requiredDict[action]; 
            return adminlvl >= requiredlvl;
        }
    }
}
