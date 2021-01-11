using System.Collections.Generic;
using System.Reflection;
using TDS.Server.Data.CustomAttribute;

namespace TDS.Server.Data.Models
{
#nullable enable

    public delegate object FastInvokeHandler(object target, object[] parameters);

    public class CommandMethodDataDto
    {
        public bool HasCommandInfos { get; set; } = false;
        public MethodInfo MethodDefault { get; }  // only used when UseImplicitTypes == true
        public FastInvokeHandler FastMethodInvoker { get; }

        public List<CommandMultipleArgsToOneInfo> MultipleArgsToOneInfos { get; } = new List<CommandMultipleArgsToOneInfo>();

        // public CommandDefaultMethod? Method; // only used when UseImplicitTypes == false public
        // CommandEmptyDefaultMethod? MethodEmpty; // only used when UseImplicitTypes == false
        public List<ParameterInfo> ParameterInfos { get; set; } = new List<ParameterInfo>();

        public int? ParametersWithDefaultValueStartIndex { get; set; }
        public int Priority { get; }
        public RemainingTextAttribute? RemainingTextAttribute { get; set; }
        public int? ToOneStringAfterParameterCount { get; set; } = null;
        public object Instance { get; }

        public int AmountDefaultParams => HasCommandInfos ? 2 : 1;

        public CommandMethodDataDto(MethodInfo methodDefault, FastInvokeHandler fastMethodInvoker, object instance, int priority)
        {
            MethodDefault = methodDefault;
            FastMethodInvoker = fastMethodInvoker;
            Instance = instance;
            Priority = priority;
        }
    }
}