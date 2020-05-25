using System.Collections;
using System.Collections.Generic;
using TDS_Client.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    public abstract class BaseEventHandler<T> : ICollection<EventMethodData<T>>
    {
        #region Protected Fields

        protected readonly List<EventMethodData<T>> Actions = new List<EventMethodData<T>>();

        #endregion Protected Fields

        #region Protected Constructors

        protected BaseEventHandler()
        { }

        #endregion Protected Constructors

        #region Public Properties

        public int Count => Actions.Count;

        public bool IsReadOnly => true;

        #endregion Public Properties

        #region Public Methods

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Actions.GetEnumerator();
        }

        public bool Remove(EventMethodData<T> item)
        {
            return Actions.Remove(item);
        }

        #endregion Public Methods
    }
}
