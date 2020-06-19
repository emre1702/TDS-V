﻿using System;
using System.Reflection;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.Data.Interfaces.ModAPI.ClientEvent
{
    public interface IClientEventAPI
    {
        #region Public Methods

        void Add(string eventName, object classInstance, Action action)
            => Add(eventName, classInstance, action.Method);

        void Add(string eventName, object classInstance, Action<IPlayer> action)
           => Add(eventName, classInstance, action.Method);

        void Add<T>(string eventName, object classInstance, Action<T> action)
            => Add(eventName, classInstance, action.Method);

        void Add<T1, T2>(string eventName, object classInstance, Action<T1, T2> action)
            => Add(eventName, classInstance, action.Method);

        void Add<T1, T2, T3>(string eventName, object classInstance, Action<T1, T2, T3> action)
            => Add(eventName, classInstance, action.Method);

        void Add<T1, T2, T3, T4>(string eventName, object classInstance, Action<T1, T2, T3, T4> action)
            => Add(eventName, classInstance, action.Method);

        void Add<T1, T2, T3, T4, T5>(string eventName, object classInstance, Action<T1, T2, T3, T4, T5> action)
            => Add(eventName, classInstance, action.Method);

        void Add(string eventName, object classInstance, Delegate del)
            => Add(eventName, classInstance, del.Method);

        void Add(string eventName, object classInstance, MethodInfo methodInfo);

        #endregion Public Methods
    }
}