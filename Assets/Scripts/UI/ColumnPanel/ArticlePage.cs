using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ArticlePage : MonoBehaviour
{
    #region ����Ԫ��

    [SerializeField]
    private ToggleGroup toggleGroup;

    #endregion

    /// <summary>
    /// ��ƷuiԤ����
    /// </summary>
    ItemFrame itemFramePrefab;

    /// <summary>
    /// ������������Ʒ����
    /// </summary>
    private ItemFrame[,] itemFrames;

    int rowCount;
    int columnCount;

    #region �ⲿ����

    /// <summary>
    /// ��ʼ��ҳ��
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void Init(ItemFrame prefab, int row, int column)
    {
        itemFramePrefab = prefab;
        rowCount = row;
        columnCount = column;
        itemFrames = new ItemFrame[row, column];
    }

    /// <summary>
    /// ���ø���ѡ��
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetItemFrameSelected(int x, int y)
    {
        if (itemFrames[x, y] != null)
        {
            itemFrames[x, y].SetToggleSelected();
        }
    }

    /// <summary>
    /// ��ȡ�ǿյĸ���
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void GetItemFrameNoNull(Vector2 offset, ref int x, ref int y)
    {
        if (itemFrames[x, y] != null)
            return;

        x = Mathf.CeilToInt(x - offset.x);
        y = Mathf.CeilToInt(y + offset.y);
    }

    /// <summary>
    /// ��ʾ������Ʒ
    /// </summary>
    public void ShowArticles(List<ArticleInfoBase> articleInfos)
    {
        foreach (var articleInfo in articleInfos)
        {
            CreateItemFrame(articleInfo);
        }
    }

    /// <summary>
    /// �ƻض����ǰִ��
    /// </summary>
    public void Release()
    {
        ClearArticles();
        itemFramePrefab = null;
    }

    #endregion

    /// <summary>
    /// ����һ����Ʒui
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private void CreateItemFrame(ArticleInfoBase item)
    {
        GameObject itemFrame = ObjectPool.Instance.GetObject(itemFramePrefab.gameObject.name, itemFramePrefab.gameObject);
        itemFrame.transform.SetParent(this.transform);
        itemFrame.SetActive(true);
        itemFrame.transform.localScale = Vector3.one;
        ItemFrame frame = itemFrame.GetComponent<ItemFrame>();
        frame.SetToggleGroup(toggleGroup);
        frame.InventoryItem = item;
        frame.UpdateInfo();
        frame.OnSelected = OnFrameSelected;

        for (int i = 0;i < columnCount; i++)
        {
            for (int j = 0;j < rowCount;j++)
            {
                if (itemFrames[j,i] == null)
                {
                    itemFrames[j, i] = frame;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// ��Ʒѡ��
    /// </summary>
    /// <param name="item"></param>
    private void OnFrameSelected(ItemFrame item)
    {
        EventManager.Instance.Invoke(MessageConst.ArticleConst.OnArticleUISelected, item.InventoryItem);
    }

    /// <summary>
    /// �����Ʒ��ʾ
    /// </summary>
    private void ClearArticles()
    {
        foreach (var item in itemFrames)
        {
            if(item != null)
            {
                item.Release();
                ObjectPool.Instance.ReleaseObject(itemFramePrefab.gameObject.name, item.gameObject);
            }
        }
        itemFrames = null;
    }
}
