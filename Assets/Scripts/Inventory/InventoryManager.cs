using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// 背包管理器
/// </summary>
public class InventoryManager : ISingleton<InventoryManager>
{
    /// <summary>
    /// 背包里的所有物品
    /// </summary>
    private Dictionary<int, InventoryItem> inventoryItemDict = new Dictionary<int, InventoryItem>();

    /// <summary>
    /// 当前选中的物品
    /// </summary>
    [HideInInspector]
    private InventoryItem currentInventoryItem = null;

    public InventoryItem CurrentInventoryItem
    {
        get { return currentInventoryItem; }
        set { currentInventoryItem = value; }
    }

    public Dictionary<int, InventoryItem> InventoryItemDict
    {
        get { return inventoryItemDict; }
    }

    /// <summary>
    /// 添加物品--批量添加
    /// </summary>
    /// <param name="item"></param>
    public void AddInventoryItem(InventoryItem item)
    {
        if (inventoryItemDict.ContainsKey(item.Id))
            inventoryItemDict[item.Id].Count += item.Count;
        else
            inventoryItemDict.Add(item.Id, item);
    }

    /// <summary>
    /// 添加物品--批量添加
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public void AddInventoryItem(int id, int count)
    {
        if (inventoryItemDict.ContainsKey(id))
            inventoryItemDict[id].Count += count;
        else
        {
            InventoryItem item = new InventoryItem();
            item.Id = id;
            item.Count = count;
            inventoryItemDict.Add(id, item);
        }
    }

    /// <summary>
    /// 添加物品--单个添加
    /// </summary>
    /// <param name="id"></param>
    public void AddInventoryItem(int id)
    {
        if (inventoryItemDict.ContainsKey(id))
            inventoryItemDict[id].Count++;
        else
        {
            InventoryItem item = new InventoryItem();
            item.Id = id;
            item.Count = 1;
            inventoryItemDict.Add(id, item);
        }
    }


    /// <summary>
    /// 丢弃物品--全部
    /// </summary>
    /// <param name="item"></param>
    public void RemoveInventoryItem(InventoryItem item)
    {
        if (inventoryItemDict.ContainsKey(item.Id))
        {
            inventoryItemDict.Remove(item.Id);
            currentInventoryItem = null;
        }
    }

    /// <summary>
    /// 丢弃物品--单个
    /// </summary>
    /// <param name="id"></param>
    public void RemoveInventoryItem(int id)
    {
        if (inventoryItemDict.ContainsKey(id))
        {
            inventoryItemDict[id].Count--;

            if (inventoryItemDict[id].Count <= 0)
            {
                inventoryItemDict.Remove(id);
                currentInventoryItem = null;
            }
        }
    }

    /// <summary>
    /// 丢弃物品--批量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public void RemoveInventoryItem(int id, int count)
    {
        if (inventoryItemDict.ContainsKey(id))
        {
            inventoryItemDict[id].Count -= count;

            if (inventoryItemDict[id].Count <= 0)
            {
                inventoryItemDict.Remove(id);
                currentInventoryItem = null;
            }
        }
    }

    /// <summary>
    /// 使用物品--全部用完
    /// </summary>
    /// <param name="item"></param>
    public void UseInventoryItem(InventoryItem item)
    {
        //TODO:使用物品

        RemoveInventoryItem(item);
    }

    /// <summary>
    /// 使用物品--单个使用
    /// </summary>
    /// <param name="item"></param>
    public void UseInventoryItem(int id)
    {
        //TODO:使用物品

        RemoveInventoryItem(id);
    }

    /// <summary>
    /// 使用物品--批量使用
    /// </summary>
    /// <param name="item"></param>
    public void UseInventoryItem(int id, int count)
    {
        //TODO:使用物品

        RemoveInventoryItem(id, count);
    }
}
