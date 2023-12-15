using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 素材类物品的信息
/// TODO:待优化
/// </summary>
public class ArticleInfo_SourceMaterial : ArticleInfoBase
{
    /// <summary>
    /// 拥有的数量
    /// </summary>
    public int Count;

    /// <summary>
    /// 是否可以直接使用
    /// </summary>
    public bool IsCanDirectUse;

    /// <summary>
    /// 恢复血量的值
    /// </summary>
    public float HealthRecoverValue;
}

/// <summary>
/// 素材拥有的特殊效果
/// </summary>
public enum SourceMaterialEffect
{
    Default,                    //无特殊效果
    AttackUp,                   //攻击力提升
    DefenceUp,                  //防御力提升
    SwimSpeedUp,                //游泳速度提升
    MoveSpeedUp,                //移速提升
    QuietnessUp,                //安静度提升
    ColdProof,                  //防寒
    FireProof,                  //防火
    AntiElectricity,            //防电
    HeatResisting,              //耐热
    SourceMaterialEffectUp,     //素材的特殊效果提升
    HealthRecoverComplete,      //血量完全恢复
    VigoeRecover,               //恢复精力
}
