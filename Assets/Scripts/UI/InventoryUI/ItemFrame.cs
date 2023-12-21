using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;

/// <summary>
/// ��Ʒ����Ԫ��
/// </summary>
public class ItemFrame : MonoBehaviour
{
    #region ����Ԫ��

    /// <summary>
    /// ��Ʒͼ��
    /// </summary>
    [SerializeField]
    Image itemIcon;

    [SerializeField]
    Image checkMarkImage;

    [SerializeField]
    Image equipedImage;

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    [SerializeField]
    Text itemCount;

    /// <summary>
    /// ��ѡ
    /// </summary>
    private Toggle toggle;

    #endregion

    /// <summary>
    /// ��Ʒ
    /// </summary>
    private ArticleInfoBase inventoryItem;

    public ArticleInfoBase InventoryItem
    {
        get { return inventoryItem; }
        set { inventoryItem = value; }
    }

    /// <summary>
    /// ѡ���¼�
    /// </summary>
    public UnityAction<ItemFrame> OnSelected;

    private bool isOn;

    private Tweener anima;

    private void Awake()
    {
        equipedImage.gameObject.SetActive(false);
        toggle = GetComponent<Toggle>();
        isOn = toggle.isOn;
        checkMarkImage.gameObject.SetActive(isOn);
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn == this.isOn) return;

            this.isOn = isOn;
            checkMarkImage.gameObject.SetActive(isOn);
            if (isOn)
            {
                anima = checkMarkImage.transform.DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                OnSelected?.Invoke(this);
            }
            else
            {
                anima?.Kill();
                checkMarkImage.transform.localScale = Vector3.one;
            }
        });
    }

    #region �ⲿ����

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
    /// ����ToggleGroup
    /// </summary>
    /// <param name="toggleGroup"></param>
    public void SetToggleGroup(ToggleGroup toggleGroup)
    {
        toggle.group = toggleGroup;
    }

    /// <summary>
    /// ����ѡ��
    /// </summary>
    public void SetToggleSelected()
    {
        toggle.isOn = true;
    }

    /// <summary>
    /// �����Ƿ�װ��
    /// </summary>
    /// <param name="isEquip"></param>
    public void SetEquip(bool isEquip)
    {
        equipedImage.gameObject.SetActive(isEquip);
    }

    /// <summary>
    /// �ƻض����ǰ
    /// </summary>
    public void Release()
    {
        toggle.isOn = false;
        toggle.group = null;
        inventoryItem = null;
    }

    #endregion
}
