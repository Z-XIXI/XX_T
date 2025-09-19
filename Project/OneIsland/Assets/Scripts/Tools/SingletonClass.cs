using System;

public class SingletonClass<T> where T : class, new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (null == _instance)
                _instance = new T();
            return _instance;
        }
    }
    protected SingletonClass()
    {
        // ��ֹ�ⲿֱ��ʵ����
        if (_instance != null)
        {
            throw new InvalidOperationException($"Singleton instance of {typeof(T).Name} already exists.");
        }
    }
}
