using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ֵ
/// </summary>
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("��������")]
    /// <summary>
    /// �������ֵ
    /// </summary>
    public float MaxHealth;

    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    public float CurrentHealth;

    /// <summary>
    /// ��������ֵ
    /// </summary>
    public float BaseDefence;

    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    public float CurrentDefence;
}
