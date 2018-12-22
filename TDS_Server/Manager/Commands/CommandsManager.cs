using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TDS_Server.CustomAttribute;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Server.Manager.Commands
{
    class CommandsManager : Script
    {
        private delegate void CommandMethod(TDSPlayer character, TDSCommandInfos commandinfos, params object[] args);

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

                methoddataByCommand[cmd] = methoddata;

            }
        }

        [RemoteEvent(DToServerEvent.CommandUsed)] 
        public static void UseCommand(Client player, string cmd, object[] args)
        {
            TDSPlayer character = player.GetChar();
            if (!character.Entity.Playerstats.LoggedIn)
                return;
            TDSCommandInfos cmdinfos = new TDSCommandInfos { Command = cmd };
            if (commandByAlias.ContainsKey(cmd))
                cmd = commandByAlias[cmd];

            #region If the command doesn't exist 
            if (!methoddataByCommand.ContainsKey(cmd))
            {
                if (SettingsManager.ErrorToPlayerOnNonExistentCommand)
                    NAPI.Chat.SendChatMessageToPlayer(player, character.Language.COMMAND_DOESNT_EXIST);
                if (SettingsManager.ToChatOnNonExistentCommand)
                    ChatManager.OnLobbyChatMessage(player, "/" + cmd + " " + string.Join(" ", args));
                return;
            }
            #endregion

            Entity.Commands entity = commandsDict[cmd];

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

            #region User Check
            if (!needright)
            {
                canuse = true;
                cmdinfos.WithRight = ECommandUsageRight.User;
            }
            #endregion

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
#warning Check if we need that
                //for (int i = 0; i < args.Length; ++i)   
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
