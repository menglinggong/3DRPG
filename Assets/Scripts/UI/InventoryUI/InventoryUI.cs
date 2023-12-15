using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包界面
/// </summary>
public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// 物品描述信息文本
    /// </summary>
    [SerializeField]
    Text itemDescrip;
    /// <summary>
    /// 使用物品按钮
    /// </summary>
    [SerializeField]
    Button useItemBtn;
    /// <summary>
    /// 丢弃物品按钮
    /// </summary>
    [SerializeField]
    Button discardItemBtn;

    private void Awake()
    {
        useItemBtn.onClick.AddListener(OnUseItemBtnClick);
        discardItemBtn.onClick.AddListener(OnDiscardItemBtnClick);
    }

    private void Start()
    {
        GameManager.Instance.RigisterInventoryUI(this);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        ClearInventoryInfo();
        EventManager.Instance.AddListener(MessageConst.ArticleConst.OnArticleUISelected, OnArticleUISelected);
    }

    

    private void OnDisable()
    {
        Time.timeScale = 1;
        EventManager.Instance.RemoveListener(MessageConst.ArticleConst.OnArticleUISelected, OnArticleUISelected);
    }

    /// <summary>
    /// 物品选中
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnArticleUISelected(string arg0, object arg1)
    {
        ArticleInfoBase info = arg1 as ArticleInfoBase;

        itemDescrip.text = info.Descrip;
    }

    /// <summary>
    /// 清空物品显示信息
    /// </summary>
    private void ClearInventoryInfo()
    {
        itemDescrip.text = "--";
    }


    /// <summary>
    /// 丢弃物品按钮点击
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnDiscardItemBtnClick()
    {
        //if (InventoryManager.Instance.CurrentInventoryItem == null)
        //    return;

        ////TODO：目前只丢一个，后面可改为自定义数量，配合界面使用
        //InventoryManager.Instance.RemoveInventoryItem(InventoryManager.Instance.CurrentInventoryItem.Id);

        //UpdateUIDisplay();
    }

    /// <summary>
    /// 使用物品按钮点击
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnUseItemBtnClick()
    {
        //if (InventoryManager.Instance.CurrentInventoryItem == null)
        //    return;

        ////TODO：目前只用一个，后面可改为自定义数量，配合界面使用
        //InventoryManager.Instance.UseInventoryItem(InventoryManager.Instance.CurrentInventoryItem.Id);

        //UpdateUIDisplay();
    }
}
