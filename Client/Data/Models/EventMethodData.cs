using System;

namespace TDS_Client.Data.Models
{
    public class EventMethodData<T>
    {
        #region Public Fields

        public T Method;
        public Func<bool> Requirement;

        #endregion Public Fields

        #region Public Constructors

        public EventMethodData(T method)
            => Method = method;

        public EventMethodData(T method, Func<bool> requirement)
            => (Method, Requirement) = (method, requirement);

        #endregion Public Constructors
    }
}
