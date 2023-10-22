using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ·ºÐÍµ¥Àý
/// </summary>
/// <typeparam name="T"></typeparam>
public class ISingleton<T> where T : class, new()
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }

}