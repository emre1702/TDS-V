using System;
using System.Collections.Generic;
using static RAGE.Events;

namespace TDS_Client.Manager.Utility
{
    class TickManager : Script
    {
        private static readonly List<(Action, Func<bool>)> _methods = new List<(Action, Func<bool>)>();

        public TickManager()
        {
            Tick += OnTickMethod;
        }

        private static void OnTickMethod(List<TickNametagData> _)
        {
            foreach (var method in _methods)
            {
                if (method.Item2 == null || method.Item2())
                    method.Item1();
            }

        }

        public static void Add(Action method)
        {
            _methods.Add((method, null));
        }

        public static void Add(Action method, Func<bool> requirement)
        {
            _methods.Add((method, requirement));
        }

        public static void Remove(Action method)
        {
            _methods.RemoveAll(m => m.Item1 == method);
        }
    }
}
