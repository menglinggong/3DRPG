using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包界面
/// </summary>
public class BagUI : MonoBehaviour
{
    #region 界面元素

    /// <summary>
    /// 背包背景
    /// </summary>
    [SerializeField]
    private GameObject bagBG;
    
    #endregion

    #region 生命周期函数

    private void Start()
    {
        HideBag();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(MessageConst.ArticleConst.OnArticleUISelected, OnArticleUISelected);
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnPlusPerformed, OnPlusPerformed);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(MessageConst.ArticleConst.OnArticleUISelected, OnArticleUISelected);
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnPlusPerformed, OnPlusPerformed);
    }

    #endregion

    #region 事件监听

    /// <summary>
    /// +键点击（默认键盘P键），打开背包
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    private void OnPlusPerformed(string messageConst, object data)
    {
        if (!bagBG.activeSelf)
            ShowBag();
        else
            HideBag();
    }

    /// <summary>
    /// 物品选中
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnArticleUISelected(string arg0, object arg1)
    {
        ArticleInfoBase info = arg1 as ArticleInfoBase;

    }

    #endregion

    /// <summary>
    /// 打开背包界面
    /// </summary>
    private void ShowBag()
    {
        GameManager.Instance.GamePause();
        bagBG.SetActive(true);
    }

    /// <summary>
    /// 关闭背包界面
    /// </summary>
    private void HideBag()
    {
        GameManager.Instance.GameContinue();
        bagBG.SetActive(false);
    }
}
