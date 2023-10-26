using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 泛型单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class ISingleton<T> : MonoBehaviour where T : ISingleton<T>
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// 是否初始化完成
    /// </summary>
    public static bool IsInitialized
    {
        get
        {
            return instance != null;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this as T;
    }

    protected virtual void OnDestory()
    {
        if (instance == this)
            instance = null;
    }
}