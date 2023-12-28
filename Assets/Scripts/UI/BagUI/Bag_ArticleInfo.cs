using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包物品选中时，显示物品信息
/// 选中并非装备上物品
/// </summary>
public class Bag_ArticleInfo : MonoBehaviour
{
    [Header("物品名称文本")]
    [SerializeField]
    private Text articleName;

    [Header("物品描述信息文本")]
    [SerializeField]
    private Text articleDescrip;

    [Header("物品类型图片")]
    [SerializeField]
    private Image typeIcon;

    [Header("物品值")]
    [SerializeField]
    private Text value;

    private void Start()
    {
        EventManager.Instance.AddListener(MessageConst.ArticleConst.OnArticleUISelected, OnArticleUISelected);
    }

    private void OnDestroy()
    {
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

        SetShowName(info.Name);
        SetShowDescrip(info.Descrip);
    }


    #region 外部方法

    /// <summary>
    /// 设置显示名称
    /// </summary>
    /// <param name="name"></param>
    public void SetShowName(string name)
    {
        articleName.text = name;
    }

    /// <summary>
    /// 设置显示的描述
    /// </summary>
    /// <param name="descrip"></param>
    public void SetShowDescrip(string descrip)
    {
        articleDescrip.text = descrip;
    }

    /// <summary>
    /// 设置物品类型及数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    public void SetShowType(int id, string value)
    {

    }

    #endregion
}
