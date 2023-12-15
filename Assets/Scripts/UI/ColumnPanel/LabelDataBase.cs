using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不同分栏数据的基类
/// </summary>
public class LabelDataBase : MonoBehaviour
{
    public virtual void ShowHide(bool isShow)
    {
        this.gameObject.SetActive(isShow);
    }
}
