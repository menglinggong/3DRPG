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
    #region 界面元素

    /// <summary>
    /// 物品图标
    /// </summary>
    [SerializeField]
    Image itemIcon;
    
    /// <summary>
    /// 物品数量
    /// </summary>
    [SerializeField]
    Text itemCount;

    /// <summary>
    /// 勾选
    /// </summary>
    private Toggle toggle;

    #endregion

    /// <summary>
    /// 物品
    /// </summary>
    private ArticleInfoBase inventoryItem;

    public ArticleInfoBase InventoryItem
    {
        get { return inventoryItem; }
        set { inventoryItem = value; }
    }

    /// <summary>
    /// 选中事件
    /// </summary>
    public UnityAction<ItemFrame> OnSelected;

    private bool isOn;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn == this.isOn) return;

            this.isOn = isOn;

            if(isOn)
            {
                OnSelected?.Invoke(this);
            }
        });
    }

    #region 外部方法

    /// <summary>
    /// 设置物品界面显示
    /// </summary>
    public void UpdateInfo()
    {
        itemIcon.sprite = Resources.Load<Sprite>(inventoryItem.IconPath);
        //TODO，增加物品后需要显示物品的持有数量，
        //if(inventoryItem.GetType() == typeof())
        //itemCount.text = inventoryItem.Count.ToString();
        itemCount.text = "";
    }

    /// <summary>
    /// 设置ToggleGroup
    /// </summary>
    /// <param name="toggleGroup"></param>
    public void SetToggleGroup(ToggleGroup toggleGroup)
    {
        toggle.group = toggleGroup;
    }

    /// <summary>
    /// 设置选中
    /// </summary>
    public void SetToggleSelected()
    {
        toggle.isOn = true;
    }

    /// <summary>
    /// 移回对象池前
    /// </summary>
    public void Release()
    {
        toggle.isOn = false;
        toggle.group = null;
        inventoryItem = null;
    }

    #endregion
}
