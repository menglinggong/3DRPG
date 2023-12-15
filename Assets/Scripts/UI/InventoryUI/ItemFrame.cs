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
    private ArticleInfoBase inventoryItem;

    /// <summary>
    /// ��ť����¼�
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
    /// ������Ʒ������ʾ
    /// </summary>
    public void UpdateInfo()
    {
        itemIcon.sprite = Resources.Load<Sprite>(inventoryItem.IconPath);
        //TODO��������Ʒ����Ҫ��ʾ��Ʒ�ĳ���������
        //if(inventoryItem.GetType() == typeof())
        //itemCount.text = inventoryItem.Count.ToString();
        itemCount.text = "";
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
