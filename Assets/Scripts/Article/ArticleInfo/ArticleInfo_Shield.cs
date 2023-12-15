using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 盾牌类物品的信息
/// </summary>
[Serializable]
public class ArticleInfo_Shield : ArticleInfoBase
{
    /// <summary>
    /// 防御力
    /// </summary>
    public float Defense;
    /// <summary>
    /// 耐久度
    /// </summary>
    public float Durability;
    /// <summary>
    /// 附魔效果
    /// </summary>
    public ArticleEnchanting Enchant;
    /// <summary>
    /// 附魔的值
    /// </summary>
    public float EnchantValue;
    /// <summary>
    /// 材质类型
    /// </summary>
    public ArticleKind_Material MaterialKind;
}
