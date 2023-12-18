using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品
/// </summary>
public abstract class Article : MonoBehaviour
{
    /// <summary>
    /// 持有者
    /// </summary>
    public Transform Owner;

    [HideInInspector]
    public ArticleInfoBase infoBase = null;

    protected virtual void Awake()
    {
        InitInfoBase();
    }

    public abstract void InitInfoBase();

    /// <summary>
    /// 拾取物品的方法
    /// </summary>
    public abstract void PickUp();
}
