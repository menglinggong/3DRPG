using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物数值
/// </summary>
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    /// <summary>
    /// 攻击力
    /// </summary>
    public float AttackDamage;
    /// <summary>
    /// 法术强度
    /// </summary>
    public float AbilityPower;
    /// <summary>
    /// 护甲
    /// </summary>
    public float Armor;
    /// <summary>
    /// 魔抗
    /// </summary>
    public float MagicResistance;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float MoveSpeed;
    /// <summary>
    /// 转向速度
    /// </summary>
    public float TurnRoundSpeed;
    /// <summary>
    /// 冷却缩减
    /// </summary>
    public float CoolDownReduction;
    /// <summary>
    /// 暴击概率
    /// </summary>
    public float CriticalChance;
    /// <summary>
    /// 暴击加成
    /// </summary>
    public float CriticalAddition;
    /// <summary>
    /// 攻速
    /// </summary>
    public float AttackSpeed;
    
    /// <summary>
    /// 最大生命值
    /// </summary>
    public float MaxHealth;
    /// <summary>
    /// 当前生命值
    /// </summary>
    public float CurrentHealth;
    /// <summary>
    /// 当前蓝量
    /// </summary>
    public float CurrentSP;
    /// <summary>
    /// 最大蓝量
    /// </summary>
    public float MaxSP;
    /// <summary>
    /// 减速抗性
    /// </summary>
    public float ModerateResistance;
    /// <summary>
    /// 控制抗性
    /// </summary>
    public float DominateResistance;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public float AttackRange;

    /// <summary>
    /// 当前等级
    /// </summary>
    public int CurrentLevel;
    /// <summary>
    /// 最高等级
    /// </summary>
    public int MaxLevel;
    /// <summary>
    /// 基础经验值
    /// 每次升级时会改变
    /// </summary>
    public float BaseExp;
    /// <summary>
    /// 当前经验值
    /// </summary>
    public float CurrentExp;
    /// <summary>
    /// 升级后的加成
    /// </summary>
    public float LevelBuff;
    /// <summary>
    /// 升级加成倍增器
    /// </summary>
    public float LevelMultiplier
    {
        get
        {
            return 1 + (CurrentLevel - 1) * LevelBuff;
        }
    }

    /// <summary>
    /// 击杀获得的经验值
    /// </summary>
    public int KillPoint;


    /// <summary>
    /// 更新经验值
    /// </summary>
    /// <param name="point"></param>
    public void UpdateExp(int point)
    {
        CurrentExp += point;

        if (BaseExp == 0)
            return;

        while (CurrentExp >= BaseExp)
        {
            CurrentExp = CurrentExp - BaseExp;
            LevelUp();
        }
    }

    /// <summary>
    /// 升级
    /// </summary>
    private void LevelUp()
    {
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 0, MaxLevel);

        //修改当前经验与下一级所需经验
        BaseExp = (int)(BaseExp * LevelMultiplier);

        MaxHealth = MaxHealth * LevelMultiplier;
        CurrentHealth = MaxHealth;

        //TODO：升级增加护甲
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public void GetHurt(float damage)
    {
        this.CurrentHealth = Mathf.Max(this.CurrentHealth - damage, 0);
    }

    /// <summary>
    /// 减少SP
    /// </summary>
    /// <param name="value"></param>
    public void ExpendSP(float value)
    {
        CurrentSP -= value;
    }
}
