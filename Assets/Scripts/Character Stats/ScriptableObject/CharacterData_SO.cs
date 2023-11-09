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
    [Header("基础属性")]
    /// <summary>
    /// 最大生命值
    /// </summary>
    public float MaxHealth;

    /// <summary>
    /// 当前生命值
    /// </summary>
    public float CurrentHealth;

    /// <summary>
    /// 基础防御值
    /// </summary>
    public float BaseDefence;

    /// <summary>
    /// 当前防御值
    /// </summary>
    public float CurrentDefence;

    [Header("等级")]
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

    [Header("击杀")]
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

        BaseDefence = BaseDefence * LevelMultiplier;
        CurrentDefence = BaseDefence;
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public void GetHurt(float damage)
    {
        this.CurrentHealth = Mathf.Max(this.CurrentHealth - damage, 0);
    }
}
