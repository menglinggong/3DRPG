using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 成品类物品的信息，比如药，烹饪的食物
/// </summary>
public class ArticleInfo_EndProduct : ArticleInfoBase
{
    /// <summary>
    /// 恢复血量的值
    /// </summary>
    public float HealthRecoverValue;

    /// <summary>
    /// 物品的特殊效果
    /// </summary>
    public EndProductEffect Effect;

    /// <summary>
    /// 特殊效果的值
    /// </summary>
    public float EffectValue;

    /// <summary>
    /// 特殊效果的持续时间
    /// </summary>
    public float EffectDuration;
}

/// <summary>
/// 素材拥有的特殊效果
/// </summary>
public enum EndProductEffect
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
    HealthRecoverComplete,      //血量完全恢复
    VigoeRecover,               //恢复精力
}
