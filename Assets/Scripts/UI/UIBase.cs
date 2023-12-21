using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有界面的基础，设置界面元素选中
/// </summary>
public class UIBase : MonoBehaviour 
{
    /// <summary>
    /// 当前选中的元素
    /// </summary>
    [HideInInspector]
    public GameObject CurrentSelectedObj;
    /// <summary>
    /// 默认选中的元素
    /// </summary>
    [HideInInspector]
    public GameObject DefaultSelectedObj;

    /// <summary>
    /// 界面打开
    /// </summary>
    public virtual void OnShow()
    {
        if (CurrentSelectedObj == null)
            CurrentSelectedObj = DefaultSelectedObj;

        InputSystemManager.Instance.SetCurrentSelectedObj(CurrentSelectedObj);
        InputSystemManager.Instance.SetLastSelectedObj(CurrentSelectedObj);
    }

    /// <summary>
    /// 界面关闭
    /// </summary>
    public virtual void OnHide()
    {
        CurrentSelectedObj = InputSystemManager.Instance.GetCurrentSelectedObj();
        InputSystemManager.Instance.SetLastSelectedObj(null);
    }
}
