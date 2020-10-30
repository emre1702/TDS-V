using System.Collections.Generic;
using System.Reflection;
using System.Web;
using TDS_Server.Data.CustomAttribute;

namespace TDS_Server.Data.Models
{
#nullable enable
    public delegate object FastInvokeHandler(object target, object[] paramters);

    public class CommandMethodDataDto
    {

        public bool HasCommandInfos { get; set; } = false;
        public MethodInfo MethodDefault { get; set; }  // only used when UseImplicitTypes == true
        public FastInvokeHandler FastMethodInvoker { get; set; }

        public List<CommandMultipleArgsToOneInfo> MultipleArgsToOneInfos { get; set; } = new List<CommandMultipleArgsToOneInfo>();

        // public CommandDefaultMethod? Method; // only used when UseImplicitTypes == false public
        // CommandEmptyDefaultMethod? MethodEmpty; // only used when UseImplicitTypes == false
        public List<ParameterInfo> ParameterInfos { get; set; } = new List<ParameterInfo>();

        public int? ParametersWithDefaultValueStartIndex { get; set; }
        public int Priority { get; set; }
        public TDSRemainingText? RemainingTextAttribute { get; set; }
        public int? ToOneStringAfterParameterCount { get; set; } = null;
        public object Instance { get; set; }

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
