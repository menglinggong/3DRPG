using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// ��Ʒ����Ԫ��
/// </summary>
public class ItemFrame : MonoBehaviour
{
    /// <summary>
    /// ��Ʒͼ��
    /// </summary>
    [SerializeField]
    Image itemIcon;
    /// <summary>
    /// ѡ��
    /// </summary>
    [SerializeField]
    Image selectedImage;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    [SerializeField]
    Text itemCount;

    /// <summary>
    /// ��ť
    /// </summary>
    private Button clickBtn;

    /// <summary>
    /// ��Ʒ
    /// </summary>
    private InventoryItem inventoryItem;

    /// <summary>
    /// ��ť����¼�
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
    /// ������Ʒ������ʾ
    /// </summary>
    public void UpdateInfo()
    {
        ItemInfo info = ItemData.GetItemInfo(inventoryItem.Id);
        itemIcon.sprite = ItemData.GetItemIcon(inventoryItem.Id);
        itemCount.text = inventoryItem.Count.ToString();
    }

    /// <summary>
    /// ������Ʒ�Ƿ�ѡ��
    /// </summary>
    /// <param name="isSelected"></param>
    public void SetItemSelected(bool isSelected)
    {
        selectedImage.gameObject.SetActive(isSelected);
    }
}
