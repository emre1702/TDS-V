using System;
using System.Reflection;
using TDS_Server.CustomAttribute;

namespace TDS_Server.Dto
{
    class CommandMethodDataDto
    {
        public MethodInfo MethodDefault;  // only used when UseImplicitTypes == true

        // public CommandDefaultMethod? Method;    // only used when UseImplicitTypes == false
        // public CommandEmptyDefaultMethod? MethodEmpty;   // only used when UseImplicitTypes == false
        public Type[] ParameterTypes = new Type[0];

        public int Priority;
        public int? ToOneStringAfterParameterCount = null;
        public bool HasCommandInfos = false;
        public TDSRemainingText? RemainingTextAttribute;

        public int AmountDefaultParams => 1 + (HasCommandInfos ? 1 : 0);

        public CommandMethodDataDto(MethodInfo methodDefault, int priority)
        {
            MethodDefault = methodDefault;
            Priority = priority;
        }
    }
}
