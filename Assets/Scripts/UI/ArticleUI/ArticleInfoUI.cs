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
    /// <summary>
    /// 显示物品信息的文本
    /// </summary>
    [SerializeField]
    private Text infoText; 

    private Transform article = null;

    private Camera mainCamera;

    private Canvas canvas = null;

    private void Start()
    {
        GameManager.Instance.RegisterArticleInfoUI(this);
    }

    private void Update()
    {
        if (article == null) return;

        transform.position = article.position + Vector3.up * 0.5f;
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
            canvas = this.transform.parent.GetComponent<Canvas>();
            canvas.worldCamera = mainCamera;
        }

        transform.forward = mainCamera.transform.forward;
    }

    /// <summary>
    /// 显示物品信息
    /// </summary>
    /// <param name="article"></param>
    public void ShowArticleInfo(Article article)
    {
        infoText.gameObject.SetActive(true);

        this.article = article.transform;
        infoText.text = article.infoBase.Name;
    }

    /// <summary>
    /// 隐藏物品信息界面
    /// </summary>
    public void HideArticleInfo()
    {
        article = null;
        infoText.gameObject.SetActive(false);
    }
}
