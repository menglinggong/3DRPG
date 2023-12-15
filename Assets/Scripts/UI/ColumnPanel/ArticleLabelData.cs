using System;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// ������Ʒ����
    /// </summary>
    private List<ItemFrame> itemFrames = new List<ItemFrame>();

    /// <summary>
    /// ������ʾ����
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
    /// ��ʾ������Ʒ
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
    /// �����Ʒ��ʾ
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
    /// ����һ����Ʒui
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
    /// ��Ʒѡ��
    /// </summary>
    /// <param name="item"></param>
    private void OnFrameSelected(ItemFrame item)
    {
        //ȡ����һ����Ʒ��ѡ��
        if (InventoryManager.Instance.CurrentArticle != null)
        {
            itemFrames.Find(a => a.InventoryItem == InventoryManager.Instance.CurrentArticle)?.SetItemSelected(false);
        }

        InventoryManager.Instance.CurrentArticle = item.InventoryItem;

        EventManager.Instance.Invoke(MessageConst.ArticleConst.OnArticleUISelected, item.InventoryItem);
        
    }
}
