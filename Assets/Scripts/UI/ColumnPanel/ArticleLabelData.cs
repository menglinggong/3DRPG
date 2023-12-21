using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ʒ�ķ�������
/// </summary>
public class ArticleLabelData : LabelDataBase
{
    /// <summary>
    /// ��ȡ���ݿ�ı���
    /// </summary>
    public string TableName;

    /// <summary>
    /// ��ƷuiԤ����
    /// </summary>
    [SerializeField]
    ItemFrame itemFramePrefab;

    HorizontalLayoutGroup horizontalLayoutGroup;

    /// <summary>
    /// ��ҳUIԤ����
    /// </summary>
    [SerializeField]
    ArticlePage articlePagePrefab;

    /// <summary>
    /// ���з�ҳ
    /// </summary>
    private List<ArticlePage> articlePages = new List<ArticlePage>();

    /// <summary>
    /// ���ռ�������ķ�Ӧʱ��
    /// </summary>
    private float reactionTime = 2.5f;

    /// <summary>
    /// ��ʱ
    /// </summary>
    private float timeCount = 0;

    /// <summary>
    /// ҳ��Ŀ�
    /// </summary>
    private Vector2 pageSize;

    #region �������ں���

    private void Awake()
    {
        RectTransform rectTransform = this.transform.parent.GetComponent<RectTransform>();
        pageSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        horizontalLayoutGroup = this.GetComponent<HorizontalLayoutGroup>();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnLeftStick, OnLeftStick);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnLeftStick, OnLeftStick);
    }

    private void Update()
    {
        if (timeCount > 0)
        {
            timeCount -= Time.fixedUnscaledDeltaTime;
        }
    }

    #endregion

    #region �ڲ�����

    /// <summary>
    /// ��ʾ������Ʒ
    /// </summary>
    private void ShowArticles()
    {
        var articleInfos = ArticleManager.Instance.SelectArticle(TableName);

        int itemCount = (int)rowColumn.x * (int)rowColumn.y;
        int pageCount = (articleInfos.Count / itemCount) + 1;

        for (int i = 0; i < pageCount; i++)
        {
            CreatePage();
        }

        int index = 0;
        foreach (var page in articlePages)
        {
            List<ArticleInfoBase> infos = new List<ArticleInfoBase>();
            
            int startIndex = index * itemCount;
            int count = startIndex + itemCount;

            int endIndex = count > articleInfos.Count ? articleInfos.Count : count;

            for (int i = startIndex; i < endIndex; i++)
            {
                infos.Add(articleInfos[i]);
            }

            page.ShowArticles(infos);
            index++;
        }

        articlePages[selectedPageIndex].SetItemFrameSelected(selectedIndex_X, selectedIndex_Y);
    }

    /// <summary>
    /// ��շ�ҳ
    /// </summary>
    private void ClearArticlePages()
    {
        foreach (var item in articlePages)
        {
            item.Release();
            ObjectPool.Instance.ReleaseObject(articlePagePrefab.gameObject.name, item.gameObject);
        }
        articlePages.Clear();
    }

    /// <summary>
    /// ������ҳ����
    /// </summary>
    private void CreatePage()
    {
        GameObject page = ObjectPool.Instance.GetObject(articlePagePrefab.gameObject.name, articlePagePrefab.gameObject);
        page.transform.SetParent(this.transform);
        page.SetActive(true);
        page.transform.localScale = Vector3.one;
        page.transform.GetComponent<RectTransform>().sizeDelta = pageSize;

        ArticlePage articlePage = page.GetComponent<ArticlePage>();
        articlePage.Init(itemFramePrefab, (int)rowColumn.x, (int)rowColumn.y);

        articlePages.Add(articlePage);
    }

    /// <summary>
    /// ��ҳ�л�����
    /// </summary>
    private void MoveAnimation()
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(-selectedPageIndex * (pageSize.x + horizontalLayoutGroup.spacing), 0);
    }

    #endregion

    /// <summary>
    /// ������ʾ����
    /// </summary>
    /// <param name="isShow"></param>
    public override void ShowHide(bool isShow)
    {
        base.ShowHide(isShow);
        if (isShow)
        {
            MoveAnimation();
            ClearArticlePages();
            ShowArticles();
        }
        else
        {
            ClearArticlePages();
        }
    }

    /// <summary>
    /// ��ҡ�˵ļ���
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLeftStick(string messageConst, object data)
    {
        Vector2 offset = (Vector2)data;

        if (offset == Vector2.zero || timeCount > 0) return;

        timeCount = reactionTime;

        selectedIndex_X = Mathf.CeilToInt((selectedIndex_X + offset.x));
        selectedIndex_Y = Mathf.CeilToInt((selectedIndex_Y - offset.y));

        selectedIndex_Y = (int)Mathf.Clamp(selectedIndex_Y, 0, rowColumn.y - 1);

        if(selectedIndex_X < 0)
        {
            selectedPageIndex--;

            if(selectedPageIndex < 0)
            {
                selectedIndex_X = 0;
                selectedPageIndex = 0;
            }
            else
            {
                //selectedIndex_X = (int)rowColumn.x - 1;
                selectedIndex_X = 0;
                selectedIndex_Y = 0;
            }
        }
        else if(selectedIndex_X >= rowColumn.x)
        {
            selectedPageIndex++;

            if(selectedPageIndex >= articlePages.Count)
            {
                selectedIndex_X = (int)rowColumn.x - 1;
                selectedPageIndex = articlePages.Count - 1;
            }
            else
            {
                selectedIndex_X = 0;
                selectedIndex_Y = 0;
            }
        }

        articlePages[selectedPageIndex].GetItemFrameNoNull(offset, ref selectedIndex_X, ref selectedIndex_Y);

        articlePages[selectedPageIndex].SetItemFrameSelected(selectedIndex_X, selectedIndex_Y);
        MoveAnimation();
    }
}
