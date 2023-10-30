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
    public int BaseExp;

    /// <summary>
    /// 当前经验值
    /// </summary>
    public int CurrentExp;

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

        if (CurrentExp >= BaseExp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// 升级
    /// </summary>
    private void LevelUp()
    {
        //TODO:解决若击杀后经验值依然高于升级所需经验值，未继续升级的问题
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 0, MaxLevel);

        Debug.Log($"当前等级：{CurrentLevel}");
        //修改当前经验与下一级所需经验
        //CurrentExp = CurrentExp - BaseExp;
        BaseExp = (int)(BaseExp * LevelMultiplier);

        MaxHealth = MaxHealth * LevelMultiplier;
        CurrentHealth = MaxHealth;

        BaseDefence = BaseDefence * LevelMultiplier;
        CurrentDefence = BaseDefence;

    }
}
