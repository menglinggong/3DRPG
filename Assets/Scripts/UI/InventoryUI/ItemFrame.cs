using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 物品界面元素
/// </summary>
public class ItemFrame : MonoBehaviour
{
    /// <summary>
    /// 物品图标
    /// </summary>
    [SerializeField]
    Image itemIcon;
    /// <summary>
    /// 选中
    /// </summary>
    [SerializeField]
    Image selectedImage;
    /// <summary>
    /// 物品数量
    /// </summary>
    [SerializeField]
    Text itemCount;

    /// <summary>
    /// 按钮
    /// </summary>
    private Button clickBtn;

    /// <summary>
    /// 物品
    /// </summary>
    private InventoryItem inventoryItem;

    /// <summary>
    /// 按钮点击事件
    /// </summary>
    public UnityAction<InventoryItem> OnBtnClickAction;

    public InventoryItem InventoryItem
    {
        get { return inventoryItem; }
        set { inventoryItem = value; }
    }

    private void Awake()
    {
        clickBtn = GetComponent<Button>();
        clickBtn.onClick.AddListener(() =>
        {
            OnBtnClickAction?.Invoke(inventoryItem);
        });
    }

    /// <summary>
    /// 设置物品界面显示
    /// </summary>
    public void UpdateInfo()
    {
        ItemInfo info = ItemData.GetItemInfo(inventoryItem.Id);
        itemIcon.sprite = ItemData.GetItemIcon(inventoryItem.Id);
        itemCount.text = inventoryItem.Count.ToString();
    }

    /// <summary>
    /// 设置物品是否选中
    /// </summary>
    /// <param name="isSelected"></param>
    public void SetItemSelected(bool isSelected)
    {
        selectedImage.gameObject.SetActive(isSelected);
    }
}
