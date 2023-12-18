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
    /// <summary>
    /// ��ʾ��Ʒ��Ϣ���ı�
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
    /// ��ʾ��Ʒ��Ϣ
    /// </summary>
    /// <param name="article"></param>
    public void ShowArticleInfo(Article article)
    {
        infoText.gameObject.SetActive(true);

        this.article = article.transform;
        infoText.text = article.infoBase.Name;
    }

    /// <summary>
    /// ������Ʒ��Ϣ����
    /// </summary>
    public void HideArticleInfo()
    {
        article = null;
        infoText.gameObject.SetActive(false);
    }
}
