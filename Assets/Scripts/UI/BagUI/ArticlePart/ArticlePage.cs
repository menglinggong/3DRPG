using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.DebugUI.Table;

public class ArticlePage : MonoBehaviour
{
    #region ����Ԫ��

    private ToggleGroup toggleGroup;

    private GridLayoutGroup gridLayoutGroup;

    private RectTransform rectTransform;

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

    Vector2 defaultSpacing = Vector2.zero;
    Vector4 defaultPadding = Vector4.zero;

    #region �ⲿ����

    private void Awake()
    {
        gridLayoutGroup = this.GetComponent<GridLayoutGroup>();
        rectTransform = this.GetComponent<RectTransform>();
        defaultSpacing = gridLayoutGroup.spacing;
        defaultPadding = new Vector4(gridLayoutGroup.padding.top, gridLayoutGroup.padding.bottom, gridLayoutGroup.padding.left, gridLayoutGroup.padding.right);
    }

    /// <summary>
    /// ��ʼ��ҳ��
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void Init(ItemFrame prefab, int row, int column, Vector2 pageSize, ToggleGroup group, out Vector2 cellSize, out Vector2 spacing)
    {
        rowCount = row;
        columnCount = column;
        this.GetComponent<RectTransform>().sizeDelta = pageSize;
        SetLayoutGroup(out cellSize, out spacing);

        Init(prefab, row, column, pageSize, group, cellSize, spacing);
    }

    public void Init(ItemFrame prefab, int row, int column, Vector2 pageSize, ToggleGroup group, Vector2 cellSize, Vector2 spacing)
    {
        itemFramePrefab = prefab;
        rowCount = row;
        columnCount = column;
        itemFrames = new ItemFrame[row, column];
        this.GetComponent<RectTransform>().sizeDelta = pageSize;
        toggleGroup = group;

        gridLayoutGroup.cellSize = cellSize;
        gridLayoutGroup.spacing = spacing;
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
    /// �ж�Ŀ������Ƿ�Ϊ��
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsItemFramNull(int x, int y)
    {
        if (itemFrames[x, y] != null)
            return false;

        return true;
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
        ArticleManager.Instance.CurrentArticle = null;
        ArticleManager.Instance.CurrentItemFram = null;
        EventManager.Instance.Invoke(MessageConst.ArticleConst.OnArticleUISelected, null);
        ClearArticles();
        itemFramePrefab = null;
    }

    #endregion

    /// <summary>
    /// ��������
    /// </summary>
    private void SetLayoutGroup(out Vector2 cellSize, out Vector2 spacing)
    {
        float spaceX = defaultSpacing.x;
        float spaceY = defaultSpacing.y;
        float width = rectTransform.rect.width - defaultPadding.z - defaultPadding.w + spaceX;
        float height = rectTransform.rect.height - defaultPadding.x - defaultPadding.y + spaceY;
        float cellSizeWidth = (width / rowCount) - spaceX;
        float cellSizeHeight = (height / columnCount) - spaceY;
        
        if (cellSizeHeight > cellSizeWidth)
        {
            cellSize = new Vector2(cellSizeWidth, cellSizeWidth);
            float value = ((height - spaceY) - (cellSizeWidth * columnCount)) / (columnCount - 1);
            spacing = new Vector2(spaceX, value);
        }
        else
        {
            cellSize = new Vector2(cellSizeHeight, cellSizeHeight);
            float value = ((width - spaceX) - (cellSizeHeight * rowCount)) / (rowCount - 1);
            spacing = new Vector2(value, spaceY);
        }
    }

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
        ArticleInfoBase info = item.InventoryItem.Copy();

        Type type = info.GetType();
        var fields = type.GetFields();

        foreach (var field in fields)
        {
            if (field.Name == "Count")
            {
                field.SetValue(info, 1);
                break;
            }
        }

        ArticleManager.Instance.CurrentArticle = info;
        ArticleManager.Instance.CurrentItemFram = item;
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
