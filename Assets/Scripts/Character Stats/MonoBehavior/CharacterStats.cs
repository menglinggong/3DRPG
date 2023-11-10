using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

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
        float damage = attacker.GetRealDamage() * (target.Defence / (target.Defence + target.CharacterData.CurrentDefence)) * (target.IsDefence ? 0.5f : 1);

        //������ɸ�ֵ
        target.CharacterData.GetHurt(damage);
        //������Ϣ����Ŀ���Ѫ��
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, target);

        //�����߱�����Ŀ��δ����
        if(attacker.IsCritical && !target.IsDefence)
        {
            //������Ŀ��δ����������Ŀ����ܵ��˺�����
            target.GetComponent<Animator>().SetTrigger("GetHurt");
        }
    }

    /// <summary>
    /// �˺�
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="target"></param>
    public void TakeDamage(float damage, CharacterStats target)
    {
        damage *= ((target.Defence / (target.Defence + target.CharacterData.CurrentDefence)) * (target.IsDefence ? 0.5f : 1));

        //������ɸ�ֵ
        target.CharacterData.GetHurt(damage);
        //������Ϣ
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, target);
    }

    /// <summary>
    /// ����ʵ���ܹ���ɵ��˺���δ��ȥĿ���ʵ�ʷ���ֵ��
    /// </summary>
    /// <returns></returns>
    public float GetRealDamage()
    {
        float realDamage = UnityEngine.Random.Range(AttackData.MinDamage, AttackData.MaxDamage);

        if(IsCritical)
        {
            realDamage *= AttackData.CriticalMultiplier;
            //Debug.Log("�����˺� = " + realDamage);
        }

        return realDamage;
    }

    /// <summary>
    /// ���¾���ֵ
    /// </summary>
    /// <param name="exp"></param>
    public void UpdateExp(int exp)
    {
        this.CharacterData.UpdateExp(exp);
        //������Ϣ�����ù����ߵľ�����
        EventManager.Instance.Invoke(MessageConst.UpdateExp, this);
    }

    #endregion
}
