using System;
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

    [Header("�ȼ�")]
    /// <summary>
    /// ��ǰ�ȼ�
    /// </summary>
    public int CurrentLevel;

    /// <summary>
    /// ��ߵȼ�
    /// </summary>
    public int MaxLevel;

    /// <summary>
    /// ��������ֵ
    /// ÿ������ʱ��ı�
    /// </summary>
    public float BaseExp;

    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    public float CurrentExp;

    /// <summary>
    /// ������ļӳ�
    /// </summary>
    public float LevelBuff;

    /// <summary>
    /// �����ӳɱ�����
    /// </summary>
    public float LevelMultiplier
    {
        get
        {
            return 1 + (CurrentLevel - 1) * LevelBuff;
        }
    }

    [Header("��ɱ")]
    /// <summary>
    /// ��ɱ��õľ���ֵ
    /// </summary>
    public int KillPoint;

    /// <summary>
    /// ���¾���ֵ
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
    /// ����
    /// </summary>
    private void LevelUp()
    {
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 0, MaxLevel);

        //�޸ĵ�ǰ��������һ�����辭��
        BaseExp = (int)(BaseExp * LevelMultiplier);

        MaxHealth = MaxHealth * LevelMultiplier;
        CurrentHealth = MaxHealth;

        BaseDefence = BaseDefence * LevelMultiplier;
        CurrentDefence = BaseDefence;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="damage"></param>
    public void GetHurt(float damage)
    {
        this.CurrentHealth = Mathf.Max(this.CurrentHealth - damage, 0);
    }
}
