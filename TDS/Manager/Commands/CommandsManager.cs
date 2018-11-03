using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TDS.CustomAttribute;
using TDS.Default;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Player;
using TDS.Instance.Utility;
using TDS.Manager.Player;

namespace TDS.Manager.Commands
{
    class CommandsManager : Script
    {
        private delegate void CommandMethod(Character character, TDSCommandInfos commandinfos, params object[] args);

        private class CommandMethodData
        {
            public CommandMethod Method;
            public int? ToOneStringAfterParameterCount = null;
            public Dictionary<int, Type> ParameterTypes = new Dictionary<int, Type>();
        }

        private static readonly Dictionary<string, CommandMethodData> methoddataByCommand = new Dictionary<string, CommandMethodData>();  // this is the primary Dictionary for commands!
        private static readonly Dictionary<string, Entity.Commands> commandsDict = new Dictionary<string, Entity.Commands>();
        private static readonly Dictionary<string, string> commandByAlias = new Dictionary<string, string>();

        private static readonly StringBuilder strbuilder = new StringBuilder();

        public static async void LoadCommands(TDSNewContext dbcontext)
        {
            foreach (Entity.Commands command in await dbcontext.Commands.Include(c => c.CommandsAlias).AsNoTracking().ToListAsync())
            {
                commandsDict[command.Command] = command;
                foreach (CommandsAlias alias in command.CommandsAlias)
                {
                    commandByAlias[alias.Alias] = command.Command;
                }
            }

            List<MethodInfo> methods = Assembly.GetExecutingAssembly().GetTypes()
                   .SelectMany(t => t.GetMethods())
                   .Where(m => m.GetCustomAttributes(typeof(TDSCommand), false).Length > 0)
                   .ToList();
            foreach (MethodInfo method in methods)
            {
                CommandMethodData methoddata = new CommandMethodData();
                string cmd = method.GetCustomAttribute<TDSCommand>().Command;
                if (!commandsDict.ContainsKey(cmd))  // Only add the command if we got an entry in DB
                    continue;
                methoddata.Method = (CommandMethod)Delegate.CreateDelegate(typeof(CommandMethod), method);

                TDSRemainingText remainingtext = method.GetCustomAttribute<TDSRemainingText>();

                foreach (var parameter in method.GetParameters())
                {
                    #region TDSRemainingText attribute
                    if (remainingtext != null && parameter.GetCustomAttribute<TDSRemainingText>() != null)
                        methoddata.ToOneStringAfterParameterCount = parameter.Position;
                    #endregion

                    #region Save parameter types
                    // Don't need Type for parameters beginning at ToOneStringAfterParameterCount (because they are always strings)
                    if (!methoddata.ToOneStringAfterParameterCount.HasValue || parameter.Position < methoddata.ToOneStringAfterParameterCount.Value)
                        methoddata.ParameterTypes[parameter.Position] = parameter.ParameterType;
                    #endregion
                }

            }
        }

        [RemoteEvent(DCustomEvents.ClientCommandUse)]   // TODO: Add clientside
        public static void UseCommand(Client player, string cmd, object[] args)
        {
            TDSCommandInfos cmdinfos = new TDSCommandInfos { Command = cmd };
            if (commandByAlias.ContainsKey(cmd))
                cmd = commandByAlias[cmd];
            if (!methoddataByCommand.ContainsKey(cmd))
            {
                // TODO: This command does not exist output!
                return;
            }
            Entity.Commands entity = commandsDict[cmd];

            Character character = player.GetChar();
            bool canuse = false;
            bool needright = false;

            #region Lobby-Owner Check
            if (!canuse && entity.LobbyOwnerCanUse.HasValue)
            {
                needright = true;
                canuse = character.IsLobbyOwner;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.LobbyOwner;
            }
            #endregion

            #region Admin Check
            if (!canuse && entity.NeededAdminLevel.HasValue)
            {
                needright = true;
                canuse = character.Entity.AdminLvl >= entity.NeededAdminLevel.Value;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.Admin;
            }
            #endregion

            #region Donator Check
            if (!canuse && entity.NeededDonation.HasValue)
            {
                needright = true;
                canuse = character.Entity.Donation >= entity.NeededDonation.Value;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.VIP;
            }
            #endregion

            #region VIP Check
            if (!canuse && entity.VipCanUse.HasValue)
            {
                needright = true;
                canuse = character.Entity.IsVip;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.Donator;
            }
            #endregion
            if (!needright)
            {
                canuse = true;
                cmdinfos.WithRight = ECommandUsageRight.User;
            }

            if (canuse)
            {
                CommandMethodData methoddata = methoddataByCommand[cmd];

                if (methoddata.ToOneStringAfterParameterCount.HasValue)
                {
                    int index = methoddata.ToOneStringAfterParameterCount.Value;
                    strbuilder.AppendJoin(" ", args.Skip(index).ToArray());
                    args[index] = strbuilder.ToString();
                    strbuilder.Clear();
                    args = args.Take(index + 1).ToArray();
                }
                methoddata.Method(character, cmdinfos, args);

                //for (int i = 0; i < args.Length; ++i)   Check if we need that
                //{
                //    args[i] = Convert.ChangeType(args[i], methoddata.ParameterTypes[i]);
                //}
            }
            else
            {
                NAPI.Chat.SendChatMessageToPlayer(player, character.Language.NOT_ALLOWED);
            }

        }
    }
}
