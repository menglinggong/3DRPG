using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 服饰类物品的信息
/// </summary>
public class ArticleInfo_Cloth : ArticleInfoBase
{
    /// <summary>
    /// 衣服类型
    /// </summary>
    public ClothKind ClothKind;
    /// <summary>
    /// 衣服的等级
    /// </summary>
    public ClothLevel ClothLevel;
    /// <summary>
    /// 防御力
    /// </summary>
    public int Defense;
    /// <summary>
    /// 服饰的特殊效果
    /// </summary>
    public ClothEffect ClothEffect;
    /// <summary>
    /// 服饰特殊效果等级
    /// </summary>
    public ClothEffectLevel ClothEffectLevel;

    public ArticleInfo_Cloth(ArticleInfo_Cloth info) : base(info)
    {
        this.ClothKind = info.ClothKind;
        this.ClothLevel = info.ClothLevel;
        this.Defense = info.Defense;
        this.ClothEffect = info.ClothEffect;
        this.ClothEffectLevel = info.ClothEffectLevel;
    }

    public ArticleInfo_Cloth()
    {

    }

    public override ArticleInfoBase Copy()
    {
        ArticleInfo_Cloth info = new ArticleInfo_Cloth(this);
        return info;
    }
}

/// <summary>
/// 服饰类型
/// </summary>
public enum ClothKind
{
    HeadMounted,            //头戴式
    UpperOuterGarment,      //上衣
    Pant                    //裤子
}

/// <summary>
/// 服饰的等级
/// </summary>
public enum ClothLevel
{
    Default,
    One,
    Two,
    Three,
    Top
}

/// <summary>
/// 服饰的特殊效果
/// </summary>
public enum ClothEffect
{
    Default,            //无特殊效果
    AttackUp,           //攻击力提升
    DefenceUp,          //防御力提升
    SwimSpeedUp,        //游泳速度提升
    MoveSpeedUp,        //移速提升
    QuietnessUp,        //安静度提升
    VigorExpendDown,    //精力消耗减少
    ColdProof,          //防寒
    FireProof,          //防火
    AntiElectricity,    //防电
    HeatResisting,      //耐热
}

/// <summary>
/// 服饰特殊效果等级
/// </summary>
public enum ClothEffectLevel
{
    Default,
    LevelOne,
    LevelTwo,
    LevelThree,
}
