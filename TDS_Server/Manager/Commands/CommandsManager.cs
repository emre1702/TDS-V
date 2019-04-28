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
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using System.Threading.Tasks;
using TDS_Server.Instance.Dto;
using System.Globalization;

namespace TDS_Server.Manager.Commands
{
    class CommandsManager : Script
    {
        private delegate void CommandDefaultMethod(TDSPlayer character, TDSCommandInfos commandinfos, object[] args);
        private delegate void CommandEmptyDefaultMethod(TDSPlayer character, TDSCommandInfos commandinfos);

        private class CommandData
        {
            public List<CommandMethodData> MethodDatas = new List<CommandMethodData>();
            public int? ToOneStringAfterParameterCount = null;
        }

        private class CommandMethodData
        {
            public MethodInfo MethodDefault;  // only used when UseImplicitTypes == true
            public CommandDefaultMethod? Method;    // only used when UseImplicitTypes == false
            public CommandEmptyDefaultMethod? MethodEmpty;   // only used when UseImplicitTypes == false
            public Type[] ParameterTypes = new Type[0];
            public int Priority;

            public CommandMethodData(MethodInfo methodDefault, int priority)
            {
                MethodDefault = methodDefault;
                Priority = priority;
            }
        }


        private static readonly Dictionary<string, CommandData> commandDataByCommand = new Dictionary<string, CommandData>();  // this is the primary Dictionary for commands!
        private static readonly Dictionary<string, Entity.Commands> commandsDict = new Dictionary<string, Entity.Commands>();
        private static readonly Dictionary<string, string> commandByAlias = new Dictionary<string, string>();
        private static readonly Dictionary<Type, Func<string, object?>> typeConverter = new Dictionary<Type, Func<string, object?>>();

        private const int AmountDefaultParams = 2;

        // With implicit types it's much slower than direct call (Test: 8ms in 10000) - but then you can use Methods with implicit types (e.g. AdminSay([defaultParams], string text, int number))
        // Without implicit types it's much faster but you can only use Methods with signature Method([defaultParams]) or Method([defaultParams], object[] args)
        private const bool UseImplicitTypes = true;
        

        public static async Task LoadCommands(TDSNewContext dbcontext)
        {
            LoadConverters();

            foreach (Entity.Commands command in await dbcontext.Commands.Include(c => c.CommandsAlias).AsNoTracking().ToListAsync())
            {
                commandsDict[command.Command.ToLower()] = command;

                foreach (CommandsAlias alias in command.CommandsAlias)
                {
                    commandByAlias[alias.Alias.ToLower()] = command.Command.ToLower();
                }
            }

            List<MethodInfo> methods = Assembly.GetExecutingAssembly().GetTypes()
                   .SelectMany(t => t.GetMethods())
                   .Where(m => m.GetCustomAttributes(typeof(TDSCommand), false).Length > 0)
                   .ToList();
            foreach (MethodInfo method in methods)
            {
                var attribute = method.GetCustomAttribute<TDSCommand>();
                string cmd = attribute.Command.ToLower();
                if (!commandsDict.ContainsKey(cmd))  // Only add the command if we got an entry in DB
                    continue;

                CommandData commanddata;
                if (commandDataByCommand.ContainsKey(cmd))
                    commanddata = commandDataByCommand[cmd];
                else
                    commanddata = new CommandData();
                CommandMethodData methoddata = new CommandMethodData
                (
                    priority: attribute.Priority,
                    methodDefault: method
                );
                commanddata.MethodDatas.Add(methoddata);
                
                var parameters = method.GetParameters().Skip(AmountDefaultParams);
                Type[] parametertypes = parameters.Select(p => p.ParameterType).ToArray();

                foreach (var parameter in parameters)
                {
                    #region TDSRemainingText attribute
                    if (!commanddata.ToOneStringAfterParameterCount.HasValue)
                        if (parameter.CustomAttributes.Any(d => d.AttributeType == typeof(TDSRemainingText)))
                            commanddata.ToOneStringAfterParameterCount = parameter.Position;
                    #endregion TDSRemainingText attribute

                    #region Save parameter types
                    // Don't need Type for parameters beginning at ToOneStringAfterParameterCount (because they are always strings)
                    if (!commanddata.ToOneStringAfterParameterCount.HasValue || parameter.Position <= commanddata.ToOneStringAfterParameterCount.Value)
                        parametertypes[parameter.Position - AmountDefaultParams] = parameter.ParameterType;
                    #endregion Save parameter types
                }
                methoddata.ParameterTypes = parametertypes;

                if (!UseImplicitTypes)
                {
#pragma warning disable
                    if (parametertypes.Length == 0)
                        methoddata.MethodEmpty = (CommandEmptyDefaultMethod)method.CreateDelegate(typeof(CommandEmptyDefaultMethod));
                    else
                        methoddata.Method = (CommandDefaultMethod)method.CreateDelegate(typeof(CommandDefaultMethod));
                }
#pragma warning enable

                commandDataByCommand[cmd] = commanddata;
            }

            foreach (var commanddata in commandDataByCommand.Values)
            {
                commanddata.MethodDatas.Sort((a, b) => -1 * a.Priority.CompareTo(b.Priority));
            }
        }

        [RemoteEvent(DToServerEvent.CommandUsed)] 
        public static void UseCommand(Client player, string msg)   // here msg is WITHOUT the command char (/) ... (e.g. "kick Pluz Test")
        {
            TDSPlayer character = player.GetChar();
            try
            {
                if (character.Entity == null || !character.Entity.PlayerStats.LoggedIn)
                    return;

                object[]? args = GetArgs(msg, out string cmd);
                TDSCommandInfos cmdinfos = new TDSCommandInfos(command: cmd);
                if (commandByAlias.ContainsKey(cmd))
                    cmd = commandByAlias[cmd];

                if (!CheckCommandExists(character, cmd, args))
                    return;

                Entity.Commands entity = commandsDict[cmd];

                if (!CheckRights(character, entity, cmdinfos))
                    return;

                CommandData commanddata = commandDataByCommand[cmd];

                if (IsInvalidArgsCount(character, commanddata, args))
                    return;

                args = HandleRemaingText(commanddata, args);

                int amountmethods = commanddata.MethodDatas.Count;
                for (int methodindex = 0; methodindex < amountmethods; ++methodindex) 
                {
                    var methoddata = commanddata.MethodDatas[methodindex];
                    bool wrongmethod = false;

                    bool dontstop = HandleArgumentsTypeConvertings(character, methoddata, methodindex, amountmethods, args, ref wrongmethod);
                    if (!dontstop)
                        return;

                    if (wrongmethod)
                        continue;

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
#pragma warning disable
                        if (args != null)
                            methoddata.Method.Invoke(character, cmdinfos, args);
                        else
                            methoddata.MethodEmpty.Invoke(character, cmdinfos);
#pragma warning enable
                    }
                }
                
            }
            catch
            {
                NAPI.Chat.SendChatMessageToPlayer(player, character.Language.COMMAND_USED_WRONG);
            }
        }

        private static bool HandleArgumentsTypeConvertings(TDSPlayer player, CommandMethodData methoddata, int methodindex, int amountmethodsavailable, object[]? args, ref bool wrongmethod)
        {
            if (args == null)
                return true;
            for (int i = 0; i < Math.Min((args?.Length ?? 0), methoddata.ParameterTypes.Length); ++i)
            {
                if (args == null || args[i] == null)
                    continue;
                object? arg = typeConverter[methoddata.ParameterTypes[i]]((string)args[i]);
                #region Check for null
                if (arg == null)
                {
                    #region Check if player exists
                    if (methoddata.ParameterTypes[i] == typeof(TDSPlayer) || methoddata.ParameterTypes[i] == typeof(Client))
                    {
                        // if it's the last method (there can be an alternative method with string etc. instead of TDSPlayer/Client 
                        if (methodindex + 1 == amountmethodsavailable)
                        {
                            NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.PLAYER_DOESNT_EXIST);
                            return false;
                        }
                        wrongmethod = true;
                        return true;
                    }
                    #endregion Check if player exists
                }
                #endregion

                args[i] = arg ?? string.Empty;  // arg shouldn't be able to be null
            }
            return true;
        }

        private static object[]? HandleRemaingText(CommandData commanddata, object[]? args)
        {
            if (args != null && commanddata.ToOneStringAfterParameterCount.HasValue)
            {
                int index = commanddata.ToOneStringAfterParameterCount.Value - AmountDefaultParams;
                args[index] = string.Join(' ', args.Skip(index));
                return args.Take(index + 1).ToArray();
            }
            return args;
        }

        private static bool IsInvalidArgsCount(TDSPlayer player, CommandData commanddata, object[]? args)
        {
            if ((args?.Length ?? 0) < commanddata.MethodDatas[0].ParameterTypes.Length)
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.COMMAND_TOO_LESS_ARGUMENTS);
                return true;
            }
            return false;
        }

        private static bool CheckCommandExists(TDSPlayer player, string cmd, object[]? args)
        {
            if (!commandDataByCommand.ContainsKey(cmd))
            {
                if (SettingsManager.ErrorToPlayerOnNonExistentCommand)
                    NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.COMMAND_DOESNT_EXIST);
                if (SettingsManager.ToChatOnNonExistentCommand)
                    ChatManager.SendLobbyMessage(player, "/" + cmd + " " + string.Join(' ', args), false);
                return false;
            }
            return true;
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
                canuse = (character.Entity?.AdminLvl ?? 0) >= entity.NeededAdminLevel.Value;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.Admin;
            }
            #endregion

            #region Donator Check
            if (!canuse && entity.NeededDonation.HasValue)
            {
                needright = true;
                canuse = (character.Entity?.Donation ?? 0) >= entity.NeededDonation.Value;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.VIP;
            }
            #endregion

            #region VIP Check
            if (!canuse && entity.VipCanUse.HasValue)
            {
                needright = true;
                canuse = character.Entity?.IsVip ?? false;
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

            if (!canuse)
                NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.NOT_ALLOWED);

            return canuse;
        }

        private static object[]? GetArgs(string msg, out string cmd)
        {
            int cmdendindex = msg.IndexOf(' ');
            if (cmdendindex == -1)
            {
                cmd = msg.ToLower();
                return null;
            }
            cmd = msg.Substring(0, cmdendindex).ToLower();
            return msg.Substring(cmdendindex + 1).Split(' ');
        }

        #region Converters 
        private static void LoadConverters()
        {
            typeConverter[typeof(string)] = str => str;
            typeConverter[typeof(char)] = str => str[0];
            typeConverter[typeof(int)] = str => Convert.ToInt32(str);
            typeConverter[typeof(float)] = str => Convert.ToSingle(str);
            typeConverter[typeof(double)] = str => Convert.ToDouble(str);
            typeConverter[typeof(bool)] = str => str.Equals("true", StringComparison.CurrentCultureIgnoreCase) || str == "1";
            typeConverter[typeof(TDSPlayer?)] = GetTDSPlayerByName;
            typeConverter[typeof(Client?)] = GetClientByName;
            typeConverter[typeof(DateTime?)] = str => GetDateTimeByString(str);
        }

        private static TDSPlayer? GetTDSPlayerByName(string name)
        {
            Client? client = Utils.FindPlayer(name);
            TDSPlayer? player = client?.GetChar();
            return player != null && player.LoggedIn ? player : null;
        }

        private static Client? GetClientByName(string name)
        {
            return Utils.FindPlayer(name);
        }

        private static DateTime? GetDateTimeByString(string time)
        {
            return GetTime(time);
        }

        private static DateTime? GetTime(string time)
        {
            switch (time)
            {
                #region Seconds
                case string _ when time.EndsWith("s", true, CultureInfo.CurrentCulture):    // seconds
                    if (!double.TryParse(time[0..^1], out double seconds))
                        return null;
                    return DateTime.Now.AddSeconds(seconds);
                case string _ when time.EndsWith("sec", true, CultureInfo.CurrentCulture):    // seconds
                    if (!double.TryParse(time[0..^3], out double secs))
                        return null;
                    return DateTime.Now.AddSeconds(secs);
                #endregion

                #region Minutes
                case string _ when time.EndsWith("m", true, CultureInfo.CurrentCulture):    // minutes
                    if (!double.TryParse(time[0..^1], out double minutes))
                        return null;
                    return DateTime.Now.AddMinutes(minutes);
                case string _ when time.EndsWith("min", true, CultureInfo.CurrentCulture):    // minutes
                    if (!double.TryParse(time[0..^3], out double mins))
                        return null;
                    return DateTime.Now.AddMinutes(mins);
                #endregion

                #region Hours
                case string _ when time.EndsWith("h", true, CultureInfo.CurrentCulture):    // hours
                    if (!double.TryParse(time[0..^1], out double hours))
                        return null;
                    return DateTime.Now.AddHours(hours);
                case string _ when time.EndsWith("st", true, CultureInfo.CurrentCulture):    // hours
                    if (!double.TryParse(time[0..^2], out double hours2))
                        return null;
                    return DateTime.Now.AddHours(hours2);
                #endregion

                #region Days
                case string _ when time.EndsWith("d", true, CultureInfo.CurrentCulture):    // days
                case string _ when time.EndsWith("t", true, CultureInfo.CurrentCulture):    // days
                    if (!double.TryParse(time[0..^1], out double days))
                        return null;
                    return DateTime.Now.AddDays(days);
                #endregion

                #region Perma
                case string _ when IsPerma(time):       // perma
                    return DateTime.MaxValue;
                #endregion

                #region Unmute
                case string _ when IsUnmute(time):       // unmute
                    return DateTime.MinValue;
                #endregion

                default:
                    return null;

            };
        }

        private static bool IsPerma(string time)
        {
            return time == "-1"
                || time == "-"
                || time.Equals("perma", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("permamute", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("permaban", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("never", StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool IsUnmute(string time)
        {
            return time == "0"
                || time.Equals("unmute", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("unban", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("stop", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("no", StringComparison.CurrentCultureIgnoreCase);
        }
        #endregion
    }
}
