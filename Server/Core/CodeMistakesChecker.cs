using System;
using System.Linq;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces;
using TDS.Shared.Default;

namespace TDS.Server.Core
{
    public class CodeMistakesChecker
    {
        private readonly ILoggingHandler _loggingHandler;

        public CodeMistakesChecker(ILoggingHandler loggingHandler)
            => (_loggingHandler) = (loggingHandler);

        public bool CheckHasErrors()
        {
            bool hasError = false;

            hasError |= CheckHasDuplicateValuesInEventsType(typeof(ToBrowserEvent));
            hasError |= CheckHasDuplicateValuesInEventsType(typeof(ToClientEvent));
            hasError |= CheckHasDuplicateValuesInEventsType(typeof(ToServerEvent));

            return hasError;
        }

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
    }
}
