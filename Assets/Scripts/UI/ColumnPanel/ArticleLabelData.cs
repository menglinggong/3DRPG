using System;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// 界面物品集合
    /// </summary>
    private List<ItemFrame> itemFrames = new List<ItemFrame>();

    /// <summary>
    /// 界面显示隐藏
    /// </summary>
    /// <param name="isShow"></param>
    public override void ShowHide(bool isShow)
    {
        base.ShowHide(isShow);

        if(isShow)
        {
            ClearArticles();
            ShowArticles();
        }
        else
        {
            ClearArticles();
        }
    }

    /// <summary>
    /// 显示所有物品
    /// </summary>
    private void ShowArticles()
    {
        SQLManager.Instance.OpenSQLaAndConnect();

        var data = SQLManager.Instance.Select(TableName);
        var articleInfos = InventoryManager.Instance.AnalysisSQLData(data);

        SQLManager.Instance.CloseSQLConnection();

        foreach (var articleInfo in articleInfos)
        {
            CreateItemFrame(articleInfo);
        }
    }

    /// <summary>
    /// 清空物品显示
    /// </summary>
    private void ClearArticles()
    {
        foreach (var item in itemFrames)
        {
            item.SetItemSelected(false);
            ObjectPool.Instance.ReleaseObject(itemFramePrefab.gameObject.name, item.gameObject);
        }
        itemFrames.Clear();
    }

    /// <summary>
    /// 创建一个物品ui
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private ItemFrame CreateItemFrame(ArticleInfoBase item)
    {
        GameObject itemFrame = ObjectPool.Instance.GetObject(itemFramePrefab.gameObject.name, itemFramePrefab.gameObject);
        itemFrame.transform.SetParent(this.transform);
        itemFrame.SetActive(true);
        itemFrame.transform.localScale = Vector3.one;
        ItemFrame frame = itemFrame.GetComponent<ItemFrame>();
        frame.InventoryItem = item;
        frame.UpdateInfo();
        frame.OnBtnClickAction = OnFrameSelected;

        itemFrames.Add(frame);
        return frame;
    }

    /// <summary>
    /// 物品选中
    /// </summary>
    /// <param name="item"></param>
    private void OnFrameSelected(ItemFrame item)
    {
        //取消上一个物品的选中
        if (InventoryManager.Instance.CurrentArticle != null)
        {
            itemFrames.Find(a => a.InventoryItem == InventoryManager.Instance.CurrentArticle)?.SetItemSelected(false);
        }

        InventoryManager.Instance.CurrentArticle = item.InventoryItem;

        EventManager.Instance.Invoke(MessageConst.ArticleConst.OnArticleUISelected, item.InventoryItem);
        
    }
}
