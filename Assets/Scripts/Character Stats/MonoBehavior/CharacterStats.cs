using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// ������ֵ����ģ��
    /// </summary>
    public CharacterData_SO TemplateData;

    /// <summary>
    /// ���﹥����ֵ����ģ��
    /// </summary>
    public AttackData_SO TemplateAttackData;

    /// <summary>
    /// ������ֵ����
    /// </summary>
    [HideInInspector]
    public CharacterData_SO CharacterData;

    /// <summary>
    /// ���﹥����ֵ����
    /// </summary>
    [HideInInspector]
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
    /// ���ܾ���
    /// </summary>
    public float SkillRange
    {
        get
        {
            return AttackData != null ? AttackData.SkillRange : 0;
        }
        set
        {
            AttackData.SkillRange = value;
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
    [HideInInspector]
    public bool IsCritical;

    /// <summary>
    /// ����������Թ�������˺��ĵ���ʱ���ӵĻ���ֵ
    /// </summary>
    public float Defence = 50;

    /// <summary>
    /// �Ƿ����״̬
    /// </summary>
    public bool IsDefence = false;


    private void Awake()
    {
        if(TemplateData != null)
        {
            CharacterData = Instantiate(TemplateData);
            AttackData = Instantiate(TemplateAttackData);
        }
    }

    #region ��������֮��ķ���

    /// <summary>
    /// �˺�
    /// </summary>
    /// <param name="attacker"></param>
    public void TakeDamage(CharacterStats attacker, CharacterStats target)
    {
        //��ɵ��˺�
        //Ŀ��ķ���ֵ�ɵ��������˺���ʹ�øù�ʽ������Ŀ��ķ���ֵ�Ƕ��٣���������˺���
        //ֻ�Ƿ���ֵԽ�ߣ��˺�ԽС������ֵΪ0�����100%�˺�
        //���ֹ�ʽ����ڼ�ֵЧ����������ֵԽ�󣬵����˺��Ĳ�ֵԽС
        //����Ƿ����״̬������״̬�£��ܵ��˺�����
        float damage = attacker.GetRealDamage() * (target.Defence / (target.Defence + target.CurrentDefence)) * (target.IsDefence ? 0.5f : 1); 

        //������ɸ�ֵ
        target.CurrentHealth = Mathf.Max(target.CurrentHealth - damage, 0);

        //�����߱�����Ŀ��δ����
        if(attacker.IsCritical && !target.IsDefence)
        {
            //������Ŀ��δ����������Ŀ����ܵ��˺�����
            target.GetComponent<Animator>().SetTrigger("GetHurt");
        }

        Debug.Log(target.gameObject.name + "---" + target.CurrentHealth);

        //TODO�����½���Ѫ��
        //TODO���������ң�ɱ�ֺ����Ӿ���
        //TODO������
    }

    /// <summary>
    /// �˺�
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="target"></param>
    public void TakeDamage(float damage, CharacterStats target)
    {
        damage *= ((target.Defence / (target.Defence + target.CurrentDefence)) * (target.IsDefence ? 0.5f : 1));

        //������ɸ�ֵ
        target.CurrentHealth = Mathf.Max(target.CurrentHealth - damage, 0);

        Debug.Log(target.gameObject.name + "---" + target.CurrentHealth);
    }

    /// <summary>
    /// ����ʵ���ܹ���ɵ��˺���δ��ȥĿ���ʵ�ʷ���ֵ��
    /// </summary>
    /// <returns></returns>
    public float GetRealDamage()
    {
        float realDamage = Random.Range(MinDamage, MaxDamage);

        if(IsCritical)
        {
            realDamage *= CriticalMultiplier;
            Debug.Log("�����˺� = " + realDamage);
        }

        return realDamage;
    }

    #endregion
}
