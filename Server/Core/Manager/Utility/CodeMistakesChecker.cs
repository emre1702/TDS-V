using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Server.Default;
using TDS_Server.Manager.Logs;

namespace TDS_Server.Core.Manager.Utility
{
    static class CodeMistakesChecker
    {
        public static bool EverythingsAlright()
        {
            bool hasError = false;

            hasError = CheckHasDuplicateValuesInEventsType(typeof(ToBrowserEvent)) ? true : hasError;
            hasError = CheckHasDuplicateValuesInEventsType(typeof(ToClientEvent)) ? true : hasError;
            hasError = CheckHasDuplicateValuesInEventsType(typeof(DToServerEvent)) ? true : hasError;

            return !hasError;
        }

        private static bool CheckHasDuplicateValuesInEventsType(Type type)
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
                ErrorLogsManager.Log($"{type.Name} contains '{value}' twice!", Environment.StackTrace);
            }

            return duplicateEntries.Count > 0;
        }
    }
}
