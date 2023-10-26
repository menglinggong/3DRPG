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
}
