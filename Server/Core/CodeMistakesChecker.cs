using System;
using System.Linq;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Default;

namespace TDS_Server.Core
{
    public class CodeMistakesChecker
    {
        #region Private Fields

        private readonly ILoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public CodeMistakesChecker(ILoggingHandler loggingHandler)
            => (_loggingHandler) = (loggingHandler);

        #endregion Public Constructors

        #region Public Methods

        public bool CheckHasErrors()
        {
            bool hasError = false;

            hasError |= CheckHasDuplicateValuesInEventsType(typeof(ToBrowserEvent));
            hasError |= CheckHasDuplicateValuesInEventsType(typeof(ToClientEvent));
            hasError |= CheckHasDuplicateValuesInEventsType(typeof(ToServerEvent));

            return hasError;
        }

        #endregion Public Methods

        #region Private Methods

        private bool CheckHasDuplicateValuesInEventsType(Type type)
        {
            var eventValues = type.GetFields().Select(f => (string?)f.GetValue(null))
                .Concat(type.GetProperties().Select(p => (string?)p.GetValue(null)));

            var duplicateEntries = eventValues
                .GroupBy(v => v)
                .Where(v => v.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            foreach (var value in duplicateEntries)
            {
                _loggingHandler.LogError($"{type.Name} contains '{value}' twice!", Environment.StackTrace);
            }

            return duplicateEntries.Count > 0;
        }

        #endregion Private Methods
    }
}
