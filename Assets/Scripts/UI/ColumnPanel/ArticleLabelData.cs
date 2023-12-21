using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 物品的分栏数据
/// </summary>
public class ArticleLabelData : LabelDataBase
{
    /// <summary>
    /// 读取数据库的表名
    /// </summary>
    public string TableName;

    /// <summary>
    /// 物品ui预制体
    /// </summary>
    [SerializeField]
    ItemFrame itemFramePrefab;

    HorizontalLayoutGroup horizontalLayoutGroup;

    /// <summary>
    /// 分页UI预制体
    /// </summary>
    [SerializeField]
    ArticlePage articlePagePrefab;

    /// <summary>
    /// 所有分页
    /// </summary>
    private List<ArticlePage> articlePages = new List<ArticlePage>();

    /// <summary>
    /// 接收键盘输入的反应时间
    /// </summary>
    private float reactionTime = 2.5f;

    /// <summary>
    /// 计时
    /// </summary>
    private float timeCount = 0;

    /// <summary>
    /// 页面的宽
    /// </summary>
    private Vector2 pageSize;

    #region 生命周期函数

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

    #region 内部方法

    /// <summary>
    /// 显示所有物品
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
    /// 清空分页
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
    /// 创建分页物体
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
    /// 分页切换动画
    /// </summary>
    private void MoveAnimation()
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(-selectedPageIndex * (pageSize.x + horizontalLayoutGroup.spacing), 0);
    }

    #endregion

    /// <summary>
    /// 界面显示隐藏
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
    /// 左摇杆的监听
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
