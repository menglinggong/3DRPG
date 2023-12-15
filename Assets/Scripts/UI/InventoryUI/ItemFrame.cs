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
    private ArticleInfoBase inventoryItem;

    /// <summary>
    /// 按钮点击事件
    /// </summary>
    public UnityAction<ItemFrame> OnBtnClickAction;

    public ArticleInfoBase InventoryItem
    {
        get { return inventoryItem; }
        set { inventoryItem = value; }
    }

    private void Awake()
    {
        clickBtn = GetComponent<Button>();
        clickBtn.onClick.AddListener(() =>
        {
            OnBtnClickAction?.Invoke(this);
            SetItemSelected(true);
        });
    }

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
    /// 设置物品是否选中
    /// </summary>
    /// <param name="isSelected"></param>
    public void SetItemSelected(bool isSelected)
    {
        selectedImage.gameObject.SetActive(isSelected);
    }
}
