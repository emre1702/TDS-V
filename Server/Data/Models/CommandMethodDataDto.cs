using System.Collections.Generic;
using System.Reflection;
using TDS_Server.Data.CustomAttribute;

namespace TDS_Server.Data.Models
{
#nullable enable

    public class CommandMethodDataDto
    {

        public bool HasCommandInfos { get; set; } = false;
        public MethodInfo MethodDefault { get; set; }  // only used when UseImplicitTypes == true

        public List<CommandMultipleArgsToOneInfo> MultipleArgsToOneInfos { get; set; } = new List<CommandMultipleArgsToOneInfo>();

        // public CommandDefaultMethod? Method; // only used when UseImplicitTypes == false public
        // CommandEmptyDefaultMethod? MethodEmpty; // only used when UseImplicitTypes == false
        public List<ParameterInfo> ParameterInfos { get; set; } = new List<ParameterInfo>();

        public int? ParametersWithDefaultValueStartIndex { get; set; }
        public int Priority { get; set; }
        public TDSRemainingText? RemainingTextAttribute { get; set; }
        public int? ToOneStringAfterParameterCount { get; set; } = null;

        public int AmountDefaultParams => HasCommandInfos ? 2 : 1;

        public CommandMethodDataDto(MethodInfo methodDefault, int priority)
        {
            MethodDefault = methodDefault;
            Priority = priority;
        }
    }
}
