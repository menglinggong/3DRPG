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
    public int BaseExp;

    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    public int CurrentExp;

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

        if (CurrentExp >= BaseExp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void LevelUp()
    {
        //TODO:�������ɱ����ֵ��Ȼ�����������辭��ֵ��δ��������������
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 0, MaxLevel);

        Debug.Log($"��ǰ�ȼ���{CurrentLevel}");
        //�޸ĵ�ǰ��������һ�����辭��
        //CurrentExp = CurrentExp - BaseExp;
        BaseExp = (int)(BaseExp * LevelMultiplier);

        MaxHealth = MaxHealth * LevelMultiplier;
        CurrentHealth = MaxHealth;

        BaseDefence = BaseDefence * LevelMultiplier;
        CurrentDefence = BaseDefence;

    }
}
