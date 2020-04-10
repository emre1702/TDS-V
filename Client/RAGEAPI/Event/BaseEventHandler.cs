using System.Collections;
using System.Collections.Generic;
using TDS_Client.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    public abstract class BaseEventHandler<T> : ICollection<EventMethodData<T>>
    {
        public int Count => Actions.Count;

        public bool IsReadOnly => true;

        protected readonly List<EventMethodData<T>> Actions = new List<EventMethodData<T>>();

        protected BaseEventHandler()
        { }

        public void Add(EventMethodData<T> item)
        {
            if (!Contains(item))
                Actions.Add(item);
        }

        public void Clear()
        {
            Actions.Clear();
        }

        public bool Contains(EventMethodData<T> item)
        {
            return Actions.Contains(item);
        }

        public void CopyTo(EventMethodData<T>[] array, int arrayIndex)
        {
            Actions.CopyTo(array, arrayIndex);
        }

        public IEnumerator<EventMethodData<T>> GetEnumerator()
        {
            return Actions.GetEnumerator();
        }

        public bool Remove(EventMethodData<T> item)
        {
            return Actions.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Actions.GetEnumerator();
        }
    }
}
