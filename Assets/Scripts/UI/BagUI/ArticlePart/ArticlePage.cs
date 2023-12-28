using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.DebugUI.Table;

public class ArticlePage : MonoBehaviour
{
    #region 界面元素

    private ToggleGroup toggleGroup;

    private GridLayoutGroup gridLayoutGroup;

    private RectTransform rectTransform;

    #endregion

    /// <summary>
    /// 物品ui预制体
    /// </summary>
    ItemFrame itemFramePrefab;

    /// <summary>
    /// 创建的所有物品格子
    /// </summary>
    private ItemFrame[,] itemFrames;

    int rowCount;
    int columnCount;

    Vector2 defaultSpacing = Vector2.zero;
    Vector4 defaultPadding = Vector4.zero;

    #region 外部方法

    private void Awake()
    {
        gridLayoutGroup = this.GetComponent<GridLayoutGroup>();
        rectTransform = this.GetComponent<RectTransform>();
        defaultSpacing = gridLayoutGroup.spacing;
        defaultPadding = new Vector4(gridLayoutGroup.padding.top, gridLayoutGroup.padding.bottom, gridLayoutGroup.padding.left, gridLayoutGroup.padding.right);
    }

    /// <summary>
    /// 初始化页面
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
    /// 设置格子选中
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
    /// 获取非空的格子
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void GetItemFrameNoNull(Vector2 offset, ref int x, ref int y)
    {
        if (itemFrames[x, y] != null)
            return;

        x = Mathf.CeilToInt(x - offset.x);
        x = Mathf.Clamp(x, 0, rowCount - 1);
        y = Mathf.CeilToInt(y + offset.y);
        y = Mathf.Clamp(y, 0, columnCount - 1);
    }

    /// <summary>
    /// 显示所有物品
    /// </summary>
    public void ShowArticles(List<ArticleInfoBase> articleInfos)
    {
        foreach (var articleInfo in articleInfos)
        {
            CreateItemFrame(articleInfo);
        }
    }

    /// <summary>
    /// 移回对象池前执行
    /// </summary>
    public void Release()
    {
        ClearArticles();
        itemFramePrefab = null;
    }

    #endregion

    /// <summary>
    /// 设置网格
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
    /// 创建一个物品ui
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
    /// 物品选中
    /// </summary>
    /// <param name="item"></param>
    private void OnFrameSelected(ItemFrame item)
    {
        EventManager.Instance.Invoke(MessageConst.ArticleConst.OnArticleUISelected, item.InventoryItem);
    }

    /// <summary>
    /// 清空物品显示
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
