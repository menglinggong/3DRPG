using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不同分栏数据的基类
/// </summary>
public class LabelDataBase : MonoBehaviour
{
    /// <summary>
    /// 选中格子的坐标
    /// </summary>
    protected int selectedIndex_X = 0;
    protected int selectedIndex_Y = 0;
    /// <summary>
    /// 当前分页下标
    /// </summary>
    protected int selectedPageIndex = 0;

    /// <summary>
    /// 行列能存放的格子数量
    /// </summary>
    public Vector2 rowColumn = Vector2.zero;

    public virtual void ShowHide(bool isShow)
    {
        this.gameObject.SetActive(isShow);
    }
}
