using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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

    #region 获取人物数值数据的属性

    /// <summary>
    /// 最大生命值属性
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
    /// 当前生命值属性
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
    /// 基础防御值属性
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
    /// 当前防御值属性
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

    #region 获取人物攻击数值的属性

    /// <summary>
    /// 攻击距离
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
    /// 技能距离
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
    /// 技能冷却
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
    /// 最小伤害值
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
    /// 最大伤害值
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
    /// 暴击加成
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
    /// 暴击率
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
        float damage = attacker.GetRealDamage() * (target.Defence / (target.Defence + target.CurrentDefence)) * (target.IsDefence ? 0.5f : 1); 

        //以免造成负值
        target.CurrentHealth = Mathf.Max(target.CurrentHealth - damage, 0);

        //攻击者暴击且目标未防御
        if(attacker.IsCritical && !target.IsDefence)
        {
            //暴击且目标未防御，播放目标的受到伤害动画
            target.GetComponent<Animator>().SetTrigger("GetHurt");
        }

        Debug.Log(target.gameObject.name + "---" + target.CurrentHealth);

        //TODO：更新界面血条
        //TODO：如果是玩家，杀怪后增加经验
        //TODO：死亡
    }

    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="target"></param>
    public void TakeDamage(float damage, CharacterStats target)
    {
        damage *= ((target.Defence / (target.Defence + target.CurrentDefence)) * (target.IsDefence ? 0.5f : 1));

        //以免造成负值
        target.CurrentHealth = Mathf.Max(target.CurrentHealth - damage, 0);

        Debug.Log(target.gameObject.name + "---" + target.CurrentHealth);
    }

    /// <summary>
    /// 计算实际能够造成的伤害（未减去目标的实际防御值）
    /// </summary>
    /// <returns></returns>
    public float GetRealDamage()
    {
        float realDamage = Random.Range(MinDamage, MaxDamage);

        if(IsCritical)
        {
            realDamage *= CriticalMultiplier;
            Debug.Log("暴击伤害 = " + realDamage);
        }

        return realDamage;
    }

    #endregion
}
