
using System.Collections.Generic;
using System;

public class EventSystem
{
    private static readonly EventSystem instance = new EventSystem();
    public static EventSystem Instance
    {
        get
        {
            return instance;
        }
    }

    // 使用字典来存储事件名和对应的监听器列表
    private Dictionary<string, List<Delegate>> eventListeners = new Dictionary<string, List<Delegate>>();

    // 注册事件监听器
    public void AddListener(string eventName, Action listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        if (!eventListeners[eventName].Contains(listener))
        {
            eventListeners[eventName].Add(listener);
        }
    }
    public void AddListener<T>(string eventName, Action<T> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        if (!eventListeners[eventName].Contains(listener))
        {
            eventListeners[eventName].Add(listener);
        }
    }

    public void AddListener<T1, T2>(string eventName, Action<T1, T2> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        if (!eventListeners[eventName].Contains(listener))
        {
            eventListeners[eventName].Add(listener);
        }
    }

    public void AddListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        if (!eventListeners[eventName].Contains(listener))
        {
            eventListeners[eventName].Add(listener);
        }
    }
    public void AddListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        if (!eventListeners[eventName].Contains(listener))
        {
            eventListeners[eventName].Add(listener);
        }
    }

    // 移除事件监听器（带参数的）
    public void RemoveListener(string eventName, Action listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);

            if (eventListeners[eventName].Count == 0)
            {
                eventListeners.Remove(eventName);
            }
        }
    }
    public void RemoveListener<T>(string eventName, Action<T> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);

            if (eventListeners[eventName].Count == 0)
            {
                eventListeners.Remove(eventName);
            }
        }
    }

    public void RemoveListener<T1, T2>(string eventName, Action<T1, T2> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);

            if (eventListeners[eventName].Count == 0)
            {
                eventListeners.Remove(eventName);
            }
        }
    }

    public void RemoveListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);

            if (eventListeners[eventName].Count == 0)
            {
                eventListeners.Remove(eventName);
            }
        }
    }
    public void RemoveListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);

            if (eventListeners[eventName].Count == 0)
            {
                eventListeners.Remove(eventName);
            }
        }
    }

    // 触发事件（带参数的）
    public void Fire(string eventName)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            foreach (var listener in eventListeners[eventName])
            {
                if (listener is Action typedListener)
                {
                    typedListener.Invoke();
                }
            }
        }
    }
    public void Fire<T>(string eventName, T arg)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            foreach (var listener in eventListeners[eventName])
            {
                if (listener is Action<T> typedListener)
                {
                    typedListener.Invoke(arg);
                }
            }
        }
    }
    public void Fire<T1, T2>(string eventName, T1 arg, T2 arg2)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            foreach (var listener in eventListeners[eventName])
            {
                if (listener is Action<T1, T2> typedListener)
                {
                    typedListener.Invoke(arg, arg2);
                }
            }
        }
    }

    public void Fire<T1, T2, T3>(string eventName, T1 arg, T2 arg2, T3 arg3)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            foreach (var listener in eventListeners[eventName])
            {
                if (listener is Action<T1, T2, T3> typedListener)
                {
                    typedListener.Invoke(arg, arg2, arg3);
                }
            }
        }
    }
    public void Fire<T1, T2, T3, T4>(string eventName, T1 arg, T2 arg2, T3 arg3, T4 arg4)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            foreach (var listener in eventListeners[eventName])
            {
                if (listener is Action<T1, T2, T3, T4> typedListener)
                {
                    typedListener.Invoke(arg, arg2, arg3, arg4);
                }
            }
        }
    }
}