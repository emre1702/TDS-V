﻿using System;
using System.Linq;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Shared.Default;

namespace TDS_Server.Core
{
    public class CodeMistakesChecker
    {
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;

        public CodeMistakesChecker(ILoggingHandler loggingHandler, IModAPI modAPI)
            => (_loggingHandler, _modAPI) = (loggingHandler, modAPI);

        public bool CheckHasErrors()
        {
            bool hasError = false;

            hasError |= CheckHasDuplicateValuesInEventsType(typeof(ToBrowserEvent));
            hasError |= CheckHasDuplicateValuesInEventsType(typeof(ToClientEvent));
            hasError |= CheckHasDuplicateValuesInEventsType(typeof(ToServerEvent));

            hasError |= _modAPI.CheckHasErrors(_loggingHandler);

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