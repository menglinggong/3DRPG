using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using System.Text;
using static UnityEditor.Progress;

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

    [SerializeField]
    Image checkMarkImage;

    [SerializeField]
    Image equipedImage;

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

    #region 外部方法

    /// <summary>
    /// 设置物品界面显示
    /// </summary>
    public void UpdateInfo()
    {
        itemIcon.sprite = Resources.Load<Sprite>(inventoryItem.IconPath);

        int key = inventoryItem.ID / 10000;

        string text = "";
        bool isInLeft = true;
        StringBuilder str = new StringBuilder();

        switch(key)
        {
            case 1:
                text = (inventoryItem as ArticleInfo_Weapon).Aggressivity.ToString();
                isInLeft = false;
                break; 
            case 2:
                text = (inventoryItem as ArticleInfo_Bow).Aggressivity.ToString();
                isInLeft = false;
                break;
            case 3:
                str.Append("x");
                str.Append((inventoryItem as ArticleInfo_Arrow).Count.ToString());
                text = str.ToString();
                isInLeft = true;
                break;
            case 4:
                text = (inventoryItem as ArticleInfo_Shield).Defense.ToString();
                isInLeft = false;
                break;
            case 5:
                text = (inventoryItem as ArticleInfo_Cloth).Defense.ToString();
                isInLeft = false;
                break;
            case 6:
                str.Append("x");
                str.Append((inventoryItem as ArticleInfo_SourceMaterial).Count.ToString());
                text = str.ToString();
                isInLeft = true;
                break;
            case 7:
                //text = (inventoryItem as ArticleInfo_EndProduct).Count.ToString();
                //isInLeft = true;
                break;
            case 8:
                //text = (inventoryItem as ArticleInfo_Import).Count.ToString();
                //isInLeft = true;
                break;
        }

        itemCount.text = text;
        itemCount.alignment = isInLeft ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
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
    /// 设置是否装备
    /// </summary>
    /// <param name="isEquip"></param>
    public void SetEquip(bool isEquip)
    {
        equipedImage.gameObject.SetActive(isEquip);
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

    /// <summary>
    /// 物品是否有带有数量数据
    /// </summary>
    /// <returns></returns>
    public bool IsArticleWithCount()
    {
        Type type = inventoryItem.GetType();

        var fields = type.GetFields();

        foreach (var field in fields)
        {
            if (field.Name == "Count")
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 刷新格子
    /// </summary>
    public void Refresh()
    {
        if(IsArticleWithCount())
        {
            Refresh_Count();
        }
        else
        {
            Refresh_Clear();
        }
    }

    #endregion

    /// <summary>
    /// 减少数量的刷新
    /// </summary>
    private void Refresh_Count()
    {
        Type type = inventoryItem.GetType();

        var fields = type.GetFields();

        foreach (var field in fields)
        {
            if (field.Name == "Count")
            {
                int count = int.Parse(field.GetValue(inventoryItem).ToString());
                count -= 1;
                if (count <= 0)
                    Refresh_Clear();
                else
                {
                    field.SetValue(inventoryItem, count);
                    UpdateInfo();
                }

                return;
            }
        }
    }

    /// <summary>
    /// 清空所有显示的刷新
    /// </summary>
    private void Refresh_Clear()
    {
        itemIcon.sprite = null;
        itemCount.text = string.Empty;
    }
}
