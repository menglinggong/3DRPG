using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// 人物数值对象模板
    /// </summary>
    public CharacterData_SO TemplateData;

    /// <summary>
    /// 人物攻击数值对象模板
    /// </summary>
    public AttackData_SO TemplateAttackData;

    /// <summary>
    /// 人物数值对象
    /// </summary>
    [HideInInspector]
    public CharacterData_SO CharacterData;

    /// <summary>
    /// 人物攻击数值对象
    /// </summary>
    [HideInInspector]
    public AttackData_SO AttackData;


    /// <summary>
    /// 是否暴击
    /// </summary>
    [HideInInspector]
    public bool IsCritical;

    /// <summary>
    /// 计算防御力对攻击造成伤害的抵消时增加的基础值
    /// </summary>
    public float Defence = 50;

    /// <summary>
    /// 是否防御状态
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

    #region 人物受伤之类的方法

    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="attacker"></param>
    public void TakeDamage(CharacterStats attacker, CharacterStats target)
    {
        //造成的伤害
        //目标的防御值可抵消部分伤害，使用该公式，无论目标的防御值是多少，都会造成伤害，
        //只是防御值越高，伤害越小，防御值为0，造成100%伤害
        //这种公式会存在极值效果，即防御值越大，抵消伤害的插值越小
        //添加是否防御状态，防御状态下，受到伤害减半
        float damage = attacker.GetRealDamage() * (target.Defence / (target.Defence + target.CharacterData.CurrentDefence)) * (target.IsDefence ? 0.5f : 1);

        //以免造成负值
        target.CharacterData.GetHurt(damage);
        //发送消息设置目标的血条
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, target);

        //攻击者暴击且目标未防御
        if(attacker.IsCritical && !target.IsDefence)
        {
            //暴击且目标未防御，播放目标的受到伤害动画
            target.GetComponent<Animator>().SetTrigger("GetHurt");
        }
    }

    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="target"></param>
    public void TakeDamage(float damage, CharacterStats target)
    {
        damage *= ((target.Defence / (target.Defence + target.CharacterData.CurrentDefence)) * (target.IsDefence ? 0.5f : 1));

        //以免造成负值
        target.CharacterData.GetHurt(damage);
        //发送消息
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, target);
    }

    /// <summary>
    /// 计算实际能够造成的伤害（未减去目标的实际防御值）
    /// </summary>
    /// <returns></returns>
    public float GetRealDamage()
    {
        float realDamage = UnityEngine.Random.Range(AttackData.MinDamage, AttackData.MaxDamage);

        if(IsCritical)
        {
            realDamage *= AttackData.CriticalMultiplier;
            //Debug.Log("暴击伤害 = " + realDamage);
        }

        return realDamage;
    }

    /// <summary>
    /// 更新经验值
    /// </summary>
    /// <param name="exp"></param>
    public void UpdateExp(int exp)
    {
        this.CharacterData.UpdateExp(exp);
        //发送消息，设置攻击者的经验条
        EventManager.Instance.Invoke(MessageConst.UpdateExp, this);
    }

    #endregion
}
