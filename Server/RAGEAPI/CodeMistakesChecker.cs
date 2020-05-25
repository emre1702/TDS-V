using System;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.RAGEAPI
{
    internal class CodeMistakesChecker
    {
        #region Private Fields

        private readonly ILoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Internal Constructors

        internal CodeMistakesChecker(ILoggingHandler loggingHandler)
            => _loggingHandler = loggingHandler;

        #endregion Internal Constructors

        #region Internal Methods

        internal bool CheckHasErrors()
        {
            bool hasError = false;

            hasError |= CheckIfEnumsAreNotEqual(typeof(GTANetworkAPI.WeaponHash), typeof(WeaponHash));
            hasError |= CheckIfEnumsAreNotEqual(typeof(GTANetworkAPI.Hash), typeof(NativeHash));
            hasError |= CheckIfEnumsAreNotEqual(typeof(GTANetworkAPI.PedHash), typeof(PedHash));
            hasError |= CheckIfEnumsAreNotEqual(typeof(GTANetworkAPI.VehicleHash), typeof(VehicleHash));

            return hasError;
        }

        #endregion Internal Methods

        #region Private Methods

        private bool CheckIfEnumsAreNotEqual(Type enum1, Type enum2)
        {
            var enum1TypeCode = Type.GetTypeCode(enum1);
            var enum2TypeCode = Type.GetTypeCode(enum2);

            foreach (var name in Enum.GetNames(enum1))
            {
                if (!Enum.IsDefined(enum2, name)
                    || !Convert.ChangeType(Enum.Parse(enum1, name), enum1TypeCode).Equals(Convert.ChangeType(Enum.Parse(enum2, name), enum2TypeCode)))
                {
                    _loggingHandler.LogError($"Enums mismatch detected. '{enum1.FullName}' and '{enum2.FullName}' are not equal, but have to be.", Environment.StackTrace);
                    return true;
                }
            }

            return false;
        }

        #endregion Private Methods
    }
}
