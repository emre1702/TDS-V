using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private delegate void CommandDefaultMethod(TDSPlayer character, TDSCommandInfos commandinfos, object[] args);
        private delegate void CommandEmptyDefaultMethod(TDSPlayer character, TDSCommandInfos commandinfos);

        private class CommandMethodData
        {
            public MethodInfo MethodDefault;  // only used when UseImplicitTypes == true
            public CommandDefaultMethod Method;    // only used when UseImplicitTypes == false
            public CommandEmptyDefaultMethod MethodEmpty;   // only used when UseImplicitTypes == false
            public int? ToOneStringAfterParameterCount = null;
            public Type[] ParameterTypes;
        }

        private static readonly Dictionary<string, CommandMethodData> methoddataByCommand = new Dictionary<string, CommandMethodData>();  // this is the primary Dictionary for commands!
        private static readonly Dictionary<string, Entity.Commands> commandsDict = new Dictionary<string, Entity.Commands>();
        private static readonly Dictionary<string, string> commandByAlias = new Dictionary<string, string>();
        private static readonly Dictionary<Type, Func<string, object>> typeConverter = new Dictionary<Type, Func<string, object>>();

        private const int AmountDefaultParams = 2;

        // With implicit types it's much slower than direct call (Test: 8ms in 10000) - but then you can use Methods with implicit types (e.g. AdminSay([defaultParams], string text, int number))
        // Without implicit types it's much faster but you can only use Methods with signature Method([defaultParams]) or Method([defaultParams], object[] args)
        private const bool UseImplicitTypes = true;
        

        public static async void LoadCommands(TDSNewContext dbcontext)
        {
            LoadConverters();

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
                methoddata.MethodDefault = method;
                string cmd = method.GetCustomAttribute<TDSCommand>().Command;
                if (!commandsDict.ContainsKey(cmd))  // Only add the command if we got an entry in DB
                    continue;
                var parameters = method.GetParameters().Skip(AmountDefaultParams);
                methoddata.ParameterTypes = parameters.Select(p => p.ParameterType).ToArray();

                foreach (var parameter in parameters)
                {
                    #region TDSRemainingText attribute
                    if (parameter.CustomAttributes.Any(d => d.AttributeType == typeof(TDSRemainingText)))
                        methoddata.ToOneStringAfterParameterCount = parameter.Position;
                    #endregion TDSRemainingText attribute

                    #region Save parameter types
                    // Don't need Type for parameters beginning at ToOneStringAfterParameterCount (because they are always strings)
                    if (!methoddata.ToOneStringAfterParameterCount.HasValue || parameter.Position <= methoddata.ToOneStringAfterParameterCount.Value)
                        methoddata.ParameterTypes[parameter.Position - AmountDefaultParams] = parameter.ParameterType;
                    #endregion Save parameter types
                }

                if (!UseImplicitTypes)
                {
                    if (methoddata.ParameterTypes.Length == 0)
                        methoddata.MethodEmpty = (CommandEmptyDefaultMethod)method.CreateDelegate(typeof(CommandEmptyDefaultMethod));
                    else
                        methoddata.Method = (CommandDefaultMethod)method.CreateDelegate(typeof(CommandDefaultMethod));
                }

                methoddataByCommand[cmd] = methoddata;

            }
        }

        [RemoteEvent(DToServerEvent.CommandUsed)] 
        public static void UseCommand(Client player, string msg)   // here msg is WITHOUT the command char (/) ... (e.g. "kick Pluz Test")
        {
            TDSPlayer character = player.GetChar();
            try
            {
                if (!character.Entity.Playerstats.LoggedIn)
                    return;

                object[] args = GetArgs(msg, out string cmd);
                TDSCommandInfos cmdinfos = new TDSCommandInfos { Command = cmd };
                if (commandByAlias.ContainsKey(cmd))
                    cmd = commandByAlias[cmd];

                #region If the command doesn't exist 
                if (!methoddataByCommand.ContainsKey(cmd))
                {
                    if (SettingsManager.ErrorToPlayerOnNonExistentCommand)
                        NAPI.Chat.SendChatMessageToPlayer(player, character.Language.COMMAND_DOESNT_EXIST);
                    if (SettingsManager.ToChatOnNonExistentCommand)
                        ChatManager.OnLobbyChatMessage(player, "/" + cmd + " " + string.Join(' ', args));
                    return;
                }
                #endregion

                Entity.Commands entity = commandsDict[cmd];

                bool canuse = CheckRights(character, entity, cmdinfos);
                if (!canuse)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, character.Language.NOT_ALLOWED);
                    return;
                }

                CommandMethodData methoddata = methoddataByCommand[cmd];

                #region Check arguments count
                if ((args?.Length ?? 0) < methoddata.ParameterTypes.Length)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, character.Language.COMMAND_TOO_LESS_ARGUMENTS);
                    return;
                }
                #endregion Check arguments count

                #region Handle TDSRemainingText
                if (methoddata.ToOneStringAfterParameterCount.HasValue)
                {
                    int index = methoddata.ToOneStringAfterParameterCount.Value - AmountDefaultParams;
                    args[index] = string.Join(' ', args.Skip(index));
                    args = args.Take(index + 1).ToArray();
                }
                #endregion Handle TDSRemainingText

                #region Handle arguments type convertings (from string)
                for (int i = 0; i < Math.Min((args?.Length ?? 0), methoddata.ParameterTypes.Length); ++i)
                {
                    args[i] = typeConverter[methoddata.ParameterTypes[i]]((string)args[i]);
                    #region Check if player exists
                    if (methoddata.ParameterTypes[i] == typeof(TDSPlayer) || methoddata.ParameterTypes[i] == typeof(Client))
                    {
                        if (args[i] == null)
                        {
                            NAPI.Chat.SendChatMessageToPlayer(player, character.Language.PLAYER_DOESNT_EXIST);
                            return;
                        }
                    }
                    #endregion Check if player exists
                }
                #endregion Handle arguments type convertings (from string)

                if (UseImplicitTypes)
                {
                    object[] newargs = new object[(args?.Length ?? 0) + AmountDefaultParams];
                    newargs[0] = character;
                    newargs[1] = cmdinfos;
                    if (args != null)
                        args.CopyTo(newargs, 2);
                    methoddata.MethodDefault.Invoke(null, newargs);
                }
                else
                {
                    if (args != null)
                        methoddata.Method.Invoke(character, cmdinfos, args);
                    else
                        methoddata.MethodEmpty.Invoke(character, cmdinfos);
                }
            }
            catch(Exception ex)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, character.Language.COMMAND_USED_WRONG);
            }
        }

        private static bool CheckRights(TDSPlayer character, Entity.Commands entity, TDSCommandInfos cmdinfos)
        {
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

            return canuse;
        }

        private static object[] GetArgs(string msg, out string cmd)
        {
            int cmdendindex = msg.IndexOf(' ');
            if (cmdendindex == -1)
            {
                cmd = msg;
                return null;
            }
            cmd = msg.Substring(0, cmdendindex);
            return msg.Substring(cmdendindex + 1).Split(' ');
        }

        #region Converters 
        private static void LoadConverters()
        {
            typeConverter[typeof(string)] = str => str;
            typeConverter[typeof(char)] = str => str[0];
            typeConverter[typeof(TDSPlayer)] = GetTDSPlayerByName;
            typeConverter[typeof(Client)] = GetClientByName;
            typeConverter[typeof(int)] = str => Convert.ToInt32(str);
            typeConverter[typeof(float)] = str => Convert.ToSingle(str);
            typeConverter[typeof(double)] = str => Convert.ToDouble(str);
            typeConverter[typeof(bool)] = str => str.Equals("true", StringComparison.CurrentCultureIgnoreCase) || str == "1" ? true : false;
        }

        private static TDSPlayer GetTDSPlayerByName(string name)
        {
            Client player = Utils.FindPlayer(name);
            return player?.GetChar();
        }

        private static Client GetClientByName(string name)
        {
            return Utils.FindPlayer(name);
        }
        #endregion
    }
}
