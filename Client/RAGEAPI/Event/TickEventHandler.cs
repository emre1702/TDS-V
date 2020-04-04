using System;
using System.Collections;
using System.Collections.Generic;
using TDS_Client.Data.Models;
using static RAGE.Events;

namespace TDS_Client.RAGEAPI.Event
{
    public class TickEventHandler : ICollection<EventMethodData<Action>>
    {
        public int Count => _actions.Count;

        public bool IsReadOnly => true;

        private readonly List<EventMethodData<Action>> _actions = new List<EventMethodData<Action>>();

        public TickEventHandler()
        {
            Tick += OnTick;
        }

        public void Add(EventMethodData<Action> item)
        {
            _actions.Add(item);
        }

        public void Clear()
        {
            _actions.Clear();
        }

        public bool Contains(EventMethodData<Action> item)
        {
            return _actions.Contains(item);
        }

        public void CopyTo(EventMethodData<Action>[] array, int arrayIndex)
        {
            _actions.CopyTo(array, arrayIndex);
        }

        public IEnumerator<EventMethodData<Action>> GetEnumerator()
        {
            return _actions.GetEnumerator();
        }

        public bool Remove(EventMethodData<Action> item)
        {
            return _actions.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _actions.GetEnumerator();
        }

        private void OnTick(List<TickNametagData> nametags)
        {
            foreach (var a in _actions) 
                if (a.Requirement is null || a.Requirement())
                    a.Method();
        }
    }
}
