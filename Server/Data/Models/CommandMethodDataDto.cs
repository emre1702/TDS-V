using System.Collections.Generic;
using System.Reflection;
using TDS_Server.Data.CustomAttribute;

namespace TDS_Server.Data.Models
{
#nullable enable

    public class CommandMethodDataDto
    {
        #region Public Fields

        public bool HasCommandInfos = false;
        public MethodInfo MethodDefault;  // only used when UseImplicitTypes == true

        public List<CommandMultipleArgsToOneInfo> MultipleArgsToOneInfos = new List<CommandMultipleArgsToOneInfo>();

        // public CommandDefaultMethod? Method; // only used when UseImplicitTypes == false public
        // CommandEmptyDefaultMethod? MethodEmpty; // only used when UseImplicitTypes == false
        public List<ParameterInfo> ParameterInfos = new List<ParameterInfo>();

        public int? ParametersWithDefaultValueStartIndex;
        public int Priority;
        public bool IsAsync;
        public TDSRemainingText? RemainingTextAttribute;
        public int? ToOneStringAfterParameterCount = null;

        #endregion Public Fields

        #region Public Constructors

        public CommandMethodDataDto(MethodInfo methodDefault, int priority, bool isAsync)
        {
            MethodDefault = methodDefault;
            Priority = priority;
            IsAsync = isAsync;
        }

        #endregion Public Constructors

        #region Public Properties

        public int AmountDefaultParams => 1 + (HasCommandInfos ? 1 : 0);

        #endregion Public Properties
    }
}
