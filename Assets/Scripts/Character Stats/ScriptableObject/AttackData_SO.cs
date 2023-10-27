using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击的数值
/// </summary>
[CreateAssetMenu(fileName = "New Attack", menuName = "Attack Stats/Data")]
public class AttackData_SO : ScriptableObject
{
    /// <summary>
    /// 近战攻击距离
    /// </summary>
    public float AttackRange;

    /// <summary>
    /// 技能攻击距离
    /// </summary>
    public float SkillRange;

    /// <summary>
    /// 技能冷却时间
    /// </summary>
    public float CoolDown;

    /// <summary>
    /// 最小伤害值
    /// </summary>
    public float MinDamage;

    /// <summary>
    /// 最大伤害值
    /// </summary>
    public float MaxDamage;

    /// <summary>
    /// 暴击加成百分比
    /// </summary>
    public float CriticalMultiplier;

    /// <summary>
    /// 暴击几率
    /// </summary>
    public float CriticalChance;
}
