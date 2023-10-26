using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// ������ֵ����
    /// </summary>
    public CharacterData_SO CharacterData;

    /// <summary>
    /// ���﹥����ֵ����
    /// </summary>
    public AttackData_SO AttackData;

    #region ��ȡ������ֵ���ݵ�����

    /// <summary>
    /// �������ֵ����
    /// </summary>
    public float MaxHealth
    {
        get
        {
            return CharacterData != null ? CharacterData.MaxHealth : 0;
        }
        set
        {
            CharacterData.MaxHealth = value;
        }
    }

    /// <summary>
    /// ��ǰ����ֵ����
    /// </summary>
    public float CurrentHealth
    {
        get
        {
            return CharacterData != null ? CharacterData.CurrentHealth : 0;
        }
        set
        {
            CharacterData.CurrentHealth = value;
        }
    }

    /// <summary>
    /// ��������ֵ����
    /// </summary>
    public float BaseDefence
    {
        get
        {
            return CharacterData != null ? CharacterData.BaseDefence : 0;
        }
        set
        {
            CharacterData.BaseDefence = value;
        }
    }

    /// <summary>
    /// ��ǰ����ֵ����
    /// </summary>
    public float CurrentDefence
    {
        get
        {
            return CharacterData != null ? CharacterData.CurrentDefence : 0;
        }
        set
        {
            CharacterData.CurrentDefence = value;
        }
    }

    #endregion

    #region ��ȡ���﹥����ֵ������

    /// <summary>
    /// ��������
    /// </summary>
    public float AttackRange
    {
        get
        {
            return AttackData != null ? AttackData.AttackRange : 0;
        }
        set
        {
            AttackData.AttackRange = value;
        }
    }

    /// <summary>
    /// ������ȴ
    /// </summary>
    public float CoolDown
    {
        get
        {
            return AttackData != null ? AttackData.CoolDown : 0;
        }
        set
        {
            AttackData.CoolDown = value;
        }
    }

    /// <summary>
    /// ��С�˺�ֵ
    /// </summary>
    public float MinDamage
    {
        get
        {
            return AttackData != null ? AttackData.MinDamage : 0;
        }
        set
        {
            AttackData.MinDamage = value;
        }
    }

    /// <summary>
    /// ����˺�ֵ
    /// </summary>
    public float MaxDamage
    {
        get
        {
            return AttackData != null ? AttackData.MaxDamage : 0;
        }
        set
        {
            AttackData.MaxDamage = value;
        }
    }

    /// <summary>
    /// �����ӳ�
    /// </summary>
    public float CriticalMultiplier
    {
        get
        {
            return AttackData != null ? AttackData.CriticalMultiplier : 0;
        }
        set
        {
            AttackData.CriticalMultiplier = value;
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public float CriticalChance
    {
        get
        {
            return AttackData != null ? AttackData.CriticalChance : 0;
        }
        set
        {
            AttackData.CriticalChance = value;
        }
    }

    #endregion

    /// <summary>
    /// �Ƿ񱩻�
    /// </summary>
    public bool IsCritical;


}
