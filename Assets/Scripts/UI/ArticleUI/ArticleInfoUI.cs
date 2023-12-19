using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ʾ��ʰȡ��Ʒ��Ϣ�Ľ���
/// </summary>
public class ArticleInfoUI : MonoBehaviour
{
    #region ����Ԫ��

    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    private GameObject backGround;
    /// <summary>
    /// ��ʾ��Ʒ��Ϣ���ı�
    /// </summary>
    [SerializeField]
    private Text infoText;
    /// <summary>
    /// ��ʰȡ��ʾ
    /// </summary>
    [SerializeField]
    private RectTransform pickUpInfo;

    #endregion
    
    private Transform article = null;

    private Camera mainCamera;

    #region �������ں���

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

    #region �ڲ�����

    /// <summary>
    /// ��ʾ��Ʒ��Ϣ
    /// </summary>
    /// <param name="article"></param>
    private void ShowArticleInfo(Article article)
    {
        backGround.SetActive(true);

        this.article = article.transform;
        infoText.text = article.infoBase.Name;
    }

    /// <summary>
    /// ������Ʒ��Ϣ����
    /// </summary>
    private void HideArticleInfo()
    {
        article = null;
        backGround.SetActive(false);
    }

    #endregion

    /// <summary>
    /// ��ʾ/���ؿ�ʰȡ��Ʒ��Ϣ
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
