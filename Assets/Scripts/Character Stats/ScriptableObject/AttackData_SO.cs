using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������ֵ
/// </summary>
[CreateAssetMenu(fileName = "New Attack", menuName = "Attack Stats/Data")]
public class AttackData_SO : ScriptableObject
{
    /// <summary>
    /// ��ս��������
    /// </summary>
    public float AttackRange;

    /// <summary>
    /// ���ܹ�������
    /// </summary>
    public float SkillRange;

    /// <summary>
    /// ������ȴʱ��
    /// </summary>
    public float CoolDown;

    /// <summary>
    /// ��С�˺�ֵ
    /// </summary>
    public float MinDamage;

    /// <summary>
    /// ����˺�ֵ
    /// </summary>
    public float MaxDamage;

    /// <summary>
    /// �����ӳɰٷֱ�
    /// </summary>
    public float CriticalMultiplier;

    /// <summary>
    /// ��������
    /// </summary>
    public float CriticalChance;
}
