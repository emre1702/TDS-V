using System;
using System.Collections.Generic;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Event;
using TDS_Client.Manager.Lobby;
using static RAGE.Events;

namespace TDS_Client.Manager.Utility
{
    class TickManager : Script
    {
        private static readonly List<(Action, Func<bool>)> _methods = new List<(Action, Func<bool>)>();

        private static readonly List<Action> _methodsToRemove = new List<Action>();
        private static readonly List<(Action, Func<bool>)> _methodsToAdd = new List<(Action, Func<bool>)>();

        public TickManager()
        {
            Tick += OnTickMethod;

            Add(Damagesys.CheckOnTick, () => Round.InFight);
            Add(CustomEventManager.CheckOnTick);
        }

        private static void OnTickMethod(List<TickNametagData> data)
        {
            Nametag.Draw(data);
            foreach (var method in _methods)
            {
                if (method.Item2 == null || method.Item2())
                    method.Item1();
            }

            if (_methodsToRemove.Count > 0)
            {
                foreach (var methodToRemove in _methodsToRemove)
                {
                    _methods.RemoveAll(m => m.Item1 == methodToRemove);
                }
                _methodsToRemove.Clear();
            }
            
            if (_methodsToAdd.Count > 0)
            {
                foreach (var methodToAdd in _methodsToAdd)
                {
                    _methods.Add(methodToAdd);
                }
                _methodsToAdd.Clear();
            }
        }

        public static void Add(Action method, Func<bool> requirement = null)
        {
            _methodsToAdd.Add((method, requirement));
        }

        public static void Remove(Action method)
        {
            _methodsToRemove.Add(method);
        }
    }
}
