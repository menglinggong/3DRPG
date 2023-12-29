using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弓类物品的信息
/// </summary>
[Serializable]
public class ArticleInfo_Bow : ArticleInfoBase
{
    /// <summary>
    /// 攻击力
    /// </summary>
    public int Aggressivity;
    /// <summary>
    /// 耐久度
    /// </summary>
    public int Durability;
    /// <summary>
    /// 弓的类型，与物品的附魔效果不冲突
    /// </summary>
    public BowKind BowKind;
    /// <summary>
    /// 弓的材质类型
    /// </summary>
    public ArticleKind_Material MaterialKind;
    /// <summary>
    /// 射程
    /// </summary>
    public int Range;
    /// <summary>
    /// 附魔效果
    /// </summary>
    public ArticleEnchanting Enchant;
    /// <summary>
    /// 附魔的值
    /// </summary>
    public int EnchantValue;

    public ArticleInfo_Bow(ArticleInfo_Bow info) : base(info)
    {
        this.Aggressivity = info.Aggressivity;
        this.Durability = info.Durability;
        this.BowKind = info.BowKind;
        this.MaterialKind = info.MaterialKind;
        this.Range = info.Range;
        this.Enchant = info.Enchant;
        this.EnchantValue = info.EnchantValue;
    }

    public ArticleInfo_Bow()
    {

    }

    public override ArticleInfoBase Copy()
    {
        ArticleInfo_Bow info = new ArticleInfo_Bow(this);
        return info;
    }
}

/// <summary>
/// 箭类物品的信息
/// </summary>
public class ArticleInfo_Arrow : ArticleInfoBase
{
    /// <summary>
    /// 箭的种类
    /// </summary>
    public ArrowKind ArrowKind;
    /// <summary>
    /// 持有的数量
    /// </summary>
    public int Count;

    public ArticleInfo_Arrow(ArticleInfo_Arrow info) : base(info)
    {
        this.ArrowKind = info.ArrowKind;
        this.Count = info.Count;
    }

    public ArticleInfo_Arrow()
    {

    }

    public override ArticleInfoBase Copy()
    {
        ArticleInfo_Arrow info = new ArticleInfo_Arrow(this);
        return info;
    }
}


/// <summary>
/// 弓的种类，包括2连弓，3连弓，远射弓，速射弓等
/// </summary>
public enum BowKind
{
    Default,            //普通弓，无特殊效果
    TwoArrowBow,        //2连弓，一次射2支箭，且只消耗一支箭
    ThreeArrowBow,      //3连弓
    FiveArrowBow,       //5连弓
    FawBow,             //远射弓，射程远，箭在更远处下落
    FastBow,            //射速更快
}

/// <summary>
/// 箭的种类
/// </summary>
public enum ArrowKind
{
    Default,                //普通木箭
    FireArrow,              //火箭
    IceArrow,               //冰箭
    ElectricityArrow,       //电箭
    ExprodeArrow,           //爆炸箭
}
