using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品的固有信息，不变的数据
/// </summary>
[Serializable]
public class ItemInfo
{
    /// <summary>
    /// 物品id
    /// </summary>
    public int Id;
    /// <summary>
    /// 物品类型
    /// </summary>
    public ItemType Type;
    /// <summary>
    /// 物品等级
    /// </summary>
    public ItemLevel Level;
    /// <summary>
    /// 物品名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 效果
    /// </summary>
    public List<ItemEffectType> Effects;
    /// <summary>
    /// 描述
    /// </summary>
    public string Descrip;
}

/// <summary>
/// 物品的影响效果及其数值
/// </summary>
[Serializable]
public class ItemEffectType
{
    /// <summary>
    /// 影响效果的名称
    /// </summary>
    public string name;
    /// <summary>
    /// 影响效果的值，不同类型的影响效果，值的意思不同
    /// </summary>
    public float value;
}

/// <summary>
/// 物品在背包里的信息，会变的包括数量等
/// </summary>
[Serializable]
public class InventoryItem
{
    /// <summary>
    /// 物品id
    /// </summary>
    public int Id;
    /// <summary>
    /// 物品数量
    /// </summary>
    public int Count;
}

/// <summary>
/// 所有物品
/// </summary>
public class ItemData
{
    //临时使用
    private static Dictionary<int, ItemInfo> _ItemInfoDict = new Dictionary<int, ItemInfo>();

    /// <summary>
    /// 通过ID获取物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ItemInfo GetItemInfo(int id)
    {
        ItemInfo info;
        if(_ItemInfoDict.TryGetValue(id, out info))
        {
            return info;
        }
        return null;
    }

    /// <summary>
    /// 通过id获取物品图标
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Sprite GetItemIcon(int id)
    {
        return Resources.Load<Sprite>($"ItemIcons/Item_{id}");
    }

    /// <summary>
    /// 添加基础物品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="info"></param>
    public static void AddItemInfo(int id, ItemInfo info)
    {
        if(!_ItemInfoDict.ContainsKey(id))
        {
            _ItemInfoDict.Add(id, info);
        }
        else
        {
            Debug.Log("该类型的物品信息已存在！");
        }
    }
}

/// <summary>
/// 物品类型
/// 剑，盾牌，药水等
/// </summary>
public enum ItemType
{
    Sword,
    Shield,
    Spear,
    Axe,
    HpPots,
    SpPots,
}

/// <summary>
/// 物品等级
/// </summary>
public enum ItemLevel
{
    Ordinary,//普通
    Rare,//稀有
    Legend,//传说
    Myth//神话
}



/// <summary>
/// 物品
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// 该物品一次出现的数量
    /// </summary>
    public InventoryItem Inventory_Item;
    /// <summary>
    /// 该物品的各种信息
    /// </summary>
    public ItemInfo Item_Info;

    /// <summary>
    /// 触发
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.inventoryUI.AddInventoryItem(Inventory_Item);
            //InventoryManager.Instance.AddInventoryItem(Inventory_Item);
            ItemData.AddItemInfo(Item_Info.Id, Item_Info);

            this.gameObject.SetActive(false);
            //TODO销毁该物体
        }
    }
}
