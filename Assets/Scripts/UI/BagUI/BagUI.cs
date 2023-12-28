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

    [Header("背包背景")]
    [SerializeField]
    private GameObject bagBG;

    [Header("物品分栏界面")]
    [SerializeField]
    private ColumnPanel columnPanel;

    [Header("物品功能按钮界面")]
    [SerializeField]
    private Bag_ArticleBtns articleBtnsView;

    #endregion

    #region 生命周期函数

    private void Awake()
    {
        articleBtnsView.OnPanelClose += () =>
        {
            AddListener();
        };
    }

    private void Start()
    {
        HideBag();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnPlusPerformed, OnPlusPerformed);
    }

    private void OnDisable()
    {
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

    ///// <summary>
    ///// 物品选中
    ///// </summary>
    ///// <param name="messageConst"></param>
    ///// <param name="data"></param>
    ///// <exception cref="NotImplementedException"></exception>
    //private void OnArticleUISelected(string messageConst, object data)
    //{
    //    ArticleInfoBase info = data as ArticleInfoBase;

    //}

    /// <summary>
    /// 显示物品功能按钮界面
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnShowArticleBtns(string messageConst, object data)
    {
        if (articleBtnsView.gameObject.activeSelf || ArticleManager.Instance.CurrentArticle == null)
            return;

        RemoveListener();
        articleBtnsView.Show();
    }

    #endregion

    /// <summary>
    /// 打开背包界面
    /// </summary>
    private void ShowBag()
    {
        GameManager.Instance.GamePause();
        bagBG.SetActive(true);
        AddListener();
    }

    /// <summary>
    /// 关闭背包界面
    /// </summary>
    private void HideBag()
    {
        GameManager.Instance.GameContinue();
        articleBtnsView.Hide();
        RemoveListener();
        bagBG.SetActive(false);
    }

    private void AddListener()
    {
        //EventManager.Instance.AddListener(MessageConst.ArticleConst.OnArticleUISelected, OnArticleUISelected);
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnAPerformed, OnShowArticleBtns);

        columnPanel.AddListener();
    }

    private void RemoveListener()
    {
        //EventManager.Instance.RemoveListener(MessageConst.ArticleConst.OnArticleUISelected, OnArticleUISelected);
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnAPerformed, OnShowArticleBtns);

        columnPanel.RemoveListener();
    }
}
