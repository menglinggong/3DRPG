using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// ����������
/// </summary>
public class InventoryManager : ISingleton<InventoryManager>
{
    /// <summary>
    /// �������������Ʒ
    /// </summary>
    private Dictionary<int, InventoryItem> inventoryItemDict = new Dictionary<int, InventoryItem>();

    /// <summary>
    /// ��ǰѡ�е���Ʒ
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
    /// �����Ʒ--�������
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
    /// �����Ʒ--�������
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
    /// �����Ʒ--�������
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
    /// ������Ʒ--ȫ��
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
    /// ������Ʒ--����
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
    /// ������Ʒ--����
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
    /// ʹ����Ʒ--ȫ������
    /// </summary>
    /// <param name="item"></param>
    public void UseInventoryItem(InventoryItem item)
    {
        //TODO:ʹ����Ʒ

        RemoveInventoryItem(item);
    }

    /// <summary>
    /// ʹ����Ʒ--����ʹ��
    /// </summary>
    /// <param name="item"></param>
    public void UseInventoryItem(int id)
    {
        //TODO:ʹ����Ʒ

        RemoveInventoryItem(id);
    }

    /// <summary>
    /// ʹ����Ʒ--����ʹ��
    /// </summary>
    /// <param name="item"></param>
    public void UseInventoryItem(int id, int count)
    {
        //TODO:ʹ����Ʒ

        RemoveInventoryItem(id, count);
    }
}
