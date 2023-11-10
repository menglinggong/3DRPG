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
    /// <summary>
    /// ������
    /// </summary>
    public float AttackDamage;
    /// <summary>
    /// ����ǿ��
    /// </summary>
    public float AbilityPower;
    /// <summary>
    /// ����
    /// </summary>
    public float Armor;
    /// <summary>
    /// ħ��
    /// </summary>
    public float MagicResistance;
    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public float MoveSpeed;
    /// <summary>
    /// ת���ٶ�
    /// </summary>
    public float TurnRoundSpeed;
    /// <summary>
    /// ��ȴ����
    /// </summary>
    public float CoolDownReduction;
    /// <summary>
    /// ��������
    /// </summary>
    public float CriticalChance;
    /// <summary>
    /// �����ӳ�
    /// </summary>
    public float CriticalAddition;
    /// <summary>
    /// ����
    /// </summary>
    public float AttackSpeed;
    
    /// <summary>
    /// �������ֵ
    /// </summary>
    public float MaxHealth;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    public float CurrentHealth;
    /// <summary>
    /// ��ǰ����
    /// </summary>
    public float CurrentSP;
    /// <summary>
    /// �������
    /// </summary>
    public float MaxSP;
    /// <summary>
    /// ���ٿ���
    /// </summary>
    public float ModerateResistance;
    /// <summary>
    /// ���ƿ���
    /// </summary>
    public float DominateResistance;
    /// <summary>
    /// ��������
    /// </summary>
    public float AttackRange;

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

        //TODO���������ӻ���
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="damage"></param>
    public void GetHurt(float damage)
    {
        this.CurrentHealth = Mathf.Max(this.CurrentHealth - damage, 0);
    }

    /// <summary>
    /// ����SP
    /// </summary>
    /// <param name="value"></param>
    public void ExpendSP(float value)
    {
        CurrentSP -= value;
    }
}
