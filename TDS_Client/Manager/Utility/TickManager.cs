using System;
using System.Collections.Generic;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Lobby;
using static RAGE.Events;

namespace TDS_Client.Manager.Utility
{
    class TickManager : Script
    {
        private static readonly List<(Action, Func<bool>)> _methods = new List<(Action, Func<bool>)>();

        public TickManager()
        {
            Tick += OnTickMethod;

            Add(Damagesys.CheckOnTick, () => Round.InFight);
            Add(() => RAGE.Game.Ui.ShowHudComponentThisFrame((int)RAGE.Game.HudComponent.Cash), () => Account.AccountData.LoggedIn);    
        }

        private static void OnTickMethod(List<TickNametagData> data)
        {
            Nametag.Draw(data);
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
