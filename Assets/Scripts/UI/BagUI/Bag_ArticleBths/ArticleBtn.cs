using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class ArticleBtnEvent : UnityEvent
{

}

/// <summary>
/// 物品功能按钮
/// </summary>
public class ArticleBtn : MonoBehaviour
{
    /// <summary>
    /// 自身的勾选框
    /// </summary>
    private Toggle toggle;

    [Header("是否一直显示")]
    [SerializeField]
    private bool isAlwaysShow = false;

    [Header("支持的物品类型")]
    [SerializeField]
    private List<int> supportKeys = new List<int>();

    private bool isOn;

    private UnityAction<ArticleBtn, bool> onValueChanged;

    public UnityAction<ArticleBtn, bool> OnValueChanged
    {
        get { return onValueChanged; }
        set { onValueChanged = value; }
    }

    /// <summary>
    /// 确认选择事件
    /// </summary>
    public ArticleBtnEvent onBtnClick = new ArticleBtnEvent();

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        isOn = toggle.isOn;
        toggle.onValueChanged.AddListener((ison) =>
        {
            if (ison == this.isOn) return;

            this.isOn = ison;
            onValueChanged?.Invoke(this, isOn);
        });
    }

    #region 外部方法

    /// <summary>
    /// 判断是否在支持物品列表内
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool CheckSupport(int key)
    {
        if(isAlwaysShow)
            return true;

        return supportKeys.Contains(key);
    }

    /// <summary>
    /// 显示按钮
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏按钮
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="isOn"></param>
    public void SetSelected(bool isOn)
    {
        toggle.isOn = isOn;
    }

    #endregion
}
