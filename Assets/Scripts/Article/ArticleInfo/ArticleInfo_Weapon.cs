using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器类物品信息
/// </summary>
[Serializable]
public class ArticleInfo_Weapon : ArticleInfoBase
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
    /// 附魔效果
    /// </summary>
    public ArticleEnchanting Enchant;
    /// <summary>
    /// 附魔的值
    /// </summary>
    public int EnchantValue;

    /// <summary>
    /// 武器类型
    /// </summary>
    public WeaponKind WeaponKind;
    /// <summary>
    /// 武器类型-手持类型
    /// </summary>
    public WeaponKind_Hand HandKind;
    /// <summary>
    /// 物品类型-材质
    /// </summary>
    public ArticleKind_Material MaterialKind;
    /// <summary>
    /// 武器类型-特殊效果
    /// </summary>
    public WeaponKind_Effect EffectKind;

    public ArticleInfo_Weapon(ArticleInfo_Weapon info) : base(info)
    {
        this.Aggressivity = info.Aggressivity;
        this.Durability = info.Durability;
        this.Enchant = info.Enchant;
        this.EnchantValue = info.EnchantValue;
        this.WeaponKind = info.WeaponKind;
        this.HandKind = info.HandKind;
        this.MaterialKind = info.MaterialKind;
        this.EffectKind = info.EffectKind;
    }

    public ArticleInfo_Weapon()
    {

    }

    public override ArticleInfoBase Copy()
    {
        ArticleInfo_Weapon info = new ArticleInfo_Weapon(this);
        return info;
    }
}

/// <summary>
/// 武器类型-手持类型
/// </summary>
public enum WeaponKind_Hand
{
    OneHanded,          //单手持
    TwoHanded,          //双手持
}

/// <summary>
/// 武器类型
/// </summary>
public enum WeaponKind
{
    Sword,              //剑
    Axe,                //斧
    Hammer,             //锤
    Spear,              //枪
    Staff,              //法杖
}

/// <summary>
/// 武器类型-特殊效果
/// </summary>
public enum WeaponKind_Effect
{
    Default,            //无
    Burn,               //焚烧
    Freeze,             //冻结
    ElectricShock       //电击
}
