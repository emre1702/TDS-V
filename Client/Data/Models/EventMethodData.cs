using System;

namespace TDS_Client.Data.Models
{
    public class EventMethodData<T>
    {
        public T Method;
        public Func<bool> Requirement;

        public EventMethodData(T method) 
            => Method = method;

        public EventMethodData(T method, Func<bool> requirement)
            => (Method, Requirement) = (method, requirement);
    }
}
