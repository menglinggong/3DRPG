using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包界面
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    Transform content;
    /// <summary>
    /// 物品描述信息文本
    /// </summary>
    [SerializeField]
    Text itemDescrip;
    /// <summary>
    /// 使用物品按钮
    /// </summary>
    [SerializeField]
    Button useItemBtn;
    /// <summary>
    /// 丢弃物品按钮
    /// </summary>
    [SerializeField]
    Button discardItemBtn;

    /// <summary>
    /// 物品ui预制体
    /// </summary>
    [SerializeField]
    ItemFrame itemFramePrefab;

    /// <summary>
    /// 物品界面集合
    /// </summary>
    private Dictionary<int, ItemFrame> itemFrameDict = new Dictionary<int, ItemFrame>();

    private void Awake()
    {
        useItemBtn.onClick.AddListener(OnUseItemBtnClick);
        discardItemBtn.onClick.AddListener(OnDiscardItemBtnClick);
        InitShowItemUI();
    }

    private void Start()
    {
        GameManager.Instance.RigisterInventoryUI(this);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// 初始化显示背包里的物品
    /// </summary>
    public void InitShowItemUI()
    {
        foreach (var inventoryItem in InventoryManager.Instance.InventoryItemDict)
        {
            itemFrameDict.Add(inventoryItem.Key, CreateItemFrame(inventoryItem.Value));
        }
    }

    /// <summary>
    /// 创建一个物品ui
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private ItemFrame CreateItemFrame(InventoryItem item)
    {
        GameObject itemFrame = ObjectPool.Instance.GetObject(itemFramePrefab.gameObject.name, itemFramePrefab.gameObject);
        itemFrame.transform.SetParent(content);
        ItemFrame frame = itemFrame.GetComponent<ItemFrame>();
        frame.InventoryItem = item;
        frame.UpdateInfo();
        frame.OnBtnClickAction = ShowInventoryInfo;

        return frame;
    }

    /// <summary>
    /// 更新界面显示
    /// </summary>
    public void UpdateUIDisplay()
    {
        //界面上需要移除的物品id
        List<int> removeList = new List<int>();
        //先判断目前显示的物品中是否有物品需要删除
        foreach (var data in itemFrameDict)
        {
            if(!InventoryManager.Instance.InventoryItemDict.ContainsKey(data.Key))
            {
                removeList.Add(data.Key);
            }
            else
            {
                data.Value.UpdateInfo();
            }
        }

        foreach (int id in removeList)
        {
            itemFrameDict[id].OnBtnClickAction = null;
            ObjectPool.Instance.ReleaseObject(itemFramePrefab.gameObject.name, itemFrameDict[id].gameObject);
            itemFrameDict.Remove(id);
        }

        //界面上需要添加的物品id
        List<int> addList = new List<int>();
        //再判断目前显示的物品中是否需要添加新物品
        foreach (var data in InventoryManager.Instance.InventoryItemDict)
        {
            if(!itemFrameDict.ContainsKey(data.Key))
            {
                addList.Add(data.Key);
            }
        }

        foreach (int id in addList)
        {
            itemFrameDict.Add(id, CreateItemFrame(InventoryManager.Instance.InventoryItemDict[id]));
        }

        if (InventoryManager.Instance.CurrentInventoryItem == null)
            ClearInventoryInfo();
    }

    /// <summary>
    /// 显示物品信息
    /// </summary>
    /// <param name="arg0"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ShowInventoryInfo(InventoryItem item)
    {
        //取消上一个物品的选中
        if(InventoryManager.Instance.CurrentInventoryItem != null)
            itemFrameDict[InventoryManager.Instance.CurrentInventoryItem.Id].SetItemSelected(false);

        var info = ItemData.GetItemInfo(item.Id);

        InventoryManager.Instance.CurrentInventoryItem = item;
        //选中当前物品
        itemFrameDict[item.Id].SetItemSelected(true);

        itemDescrip.text = info.Descrip;
    }

    /// <summary>
    /// 清空物品显示信息
    /// </summary>
    private void ClearInventoryInfo()
    {
        itemDescrip.text = "--";
    }

    /// <summary>
    /// 添加物品--由玩家调用
    /// </summary>
    /// <param name="item"></param>
    public void AddInventoryItem(InventoryItem item)
    {
        InventoryManager.Instance.AddInventoryItem(item);
        UpdateUIDisplay();
    }

    /// <summary>
    /// 丢弃物品按钮点击
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnDiscardItemBtnClick()
    {
        if (InventoryManager.Instance.CurrentInventoryItem == null)
            return;

        //TODO：目前只丢一个，后面可改为自定义数量，配合界面使用
        InventoryManager.Instance.RemoveInventoryItem(InventoryManager.Instance.CurrentInventoryItem.Id);

        UpdateUIDisplay();
    }

    /// <summary>
    /// 使用物品按钮点击
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnUseItemBtnClick()
    {
        if (InventoryManager.Instance.CurrentInventoryItem == null)
            return;

        //TODO：目前只用一个，后面可改为自定义数量，配合界面使用
        InventoryManager.Instance.UseInventoryItem(InventoryManager.Instance.CurrentInventoryItem.Id);

        UpdateUIDisplay();
    }
}
