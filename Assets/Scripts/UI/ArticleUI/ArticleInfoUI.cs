using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示可拾取物品信息的界面
/// </summary>
public class ArticleInfoUI : MonoBehaviour
{
    #region 界面元素

    /// <summary>
    /// 背景
    /// </summary>
    [SerializeField]
    private GameObject backGround;
    /// <summary>
    /// 显示物品信息的文本
    /// </summary>
    [SerializeField]
    private Text infoText;
    /// <summary>
    /// 可拾取提示
    /// </summary>
    [SerializeField]
    private RectTransform pickUpInfo;

    #endregion
    
    private Transform article = null;

    private Camera mainCamera;

    #region 生命周期函数

    private void Start()
    {
        HideArticleInfo();
        EventManager.Instance.AddListener(MessageConst.ArticleConst.OnShowHideArticleInfo, ShowHideUI);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(MessageConst.ArticleConst.OnShowHideArticleInfo, ShowHideUI);
    }

    private void Update()
    {
        if (article == null) return;

        Vector3 pos = article.position + Vector3.up * 0.5f;
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        infoText.rectTransform.anchoredPosition = mainCamera.WorldToScreenPoint(pos);
        pickUpInfo.anchoredPosition = infoText.rectTransform.anchoredPosition - Vector2.up * infoText.rectTransform.rect.height * 0.5f;
    }

    #endregion

    #region 内部方法

    /// <summary>
    /// 显示物品信息
    /// </summary>
    /// <param name="article"></param>
    private void ShowArticleInfo(Article article)
    {
        backGround.SetActive(true);

        this.article = article.transform;
        infoText.text = article.infoBase.Name;
    }

    /// <summary>
    /// 隐藏物品信息界面
    /// </summary>
    private void HideArticleInfo()
    {
        article = null;
        backGround.SetActive(false);
    }

    #endregion

    /// <summary>
    /// 显示/隐藏可拾取物品信息
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="article"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ShowHideUI(string messageConst, object article)
    {
        if (article == null)
            HideArticleInfo();
        else
        {
            Article info = article as Article;
            ShowArticleInfo(info);
        }
    }
}
