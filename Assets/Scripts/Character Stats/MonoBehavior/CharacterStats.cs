using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// 人物数值对象模板
    /// </summary>
    public CharacterData_SO TemplateData;

    /// <summary>
    /// 人物数值对象
    /// </summary>
    [HideInInspector]
    public CharacterData_SO CharacterData;

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
        }
    }

    #region 人物受伤之类的方法

    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="attacker"></param>
    public void TakeDamage(CharacterStats attacker, CharacterStats target)
    {
        float damage = CalculateDamage(attacker, target); 

        target.CharacterData.GetHurt(damage);
        //发送消息设置目标的血条
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, target);

        //TODO:被攻击者造成僵直等状态
        //if (attacker.IsCritical && !target.IsDefence)
        //{
        //    //暴击且目标未防御，播放目标的受到伤害动画
        //    target.GetComponent<Animator>().SetTrigger("GetHurt");
        //}
    }

    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="target"></param>
    public void TakeDamage(float damage, CharacterStats target)
    {
        target.CharacterData.GetHurt(damage);
        //发送消息
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, target);
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
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, this);
    }

    /// <summary>
    /// 计算是否暴击
    /// </summary>
    /// <param name="attacker"></param>
    public void CalculateCritical(CharacterStats attacker)
    {
        attacker.IsCritical = Random.value <= attacker.CharacterData.CriticalChance;
    }

    /// <summary>
    /// 计算伤害，结合攻击方的攻击力，法强，暴击率，暴击加成以及被攻击方的护甲，魔抗等多种数据进行计算
    /// </summary>
    /// <returns></returns>
    public float CalculateDamage(CharacterStats attacker, CharacterStats target)
    {
        // 造成的伤害
        //目标的防御值可抵消部分伤害，使用该公式，无论目标的防御值是多少，都会造成伤害，
        //只是防御值越高，伤害越小，防御值为0，造成100%伤害
        //这种公式会存在极值效果，即防御值越大，抵消伤害的插值越小
        //添加是否防御状态，防御状态下，受到伤害减半

        //TODO:修改计算伤害的方法，伤害有物理伤害和魔法伤害
        float physicsDamage = attacker.CharacterData.AttackDamage;
        float magicDamage = attacker.CharacterData.AbilityPower;

        if (attacker.IsCritical)
        {
            physicsDamage *= attacker.CharacterData.CriticalAddition;
            magicDamage *= attacker.CharacterData.CriticalAddition;
        }

        physicsDamage = physicsDamage * (target.Defence / (target.Defence + target.CharacterData.Armor)) * (target.IsDefence ? 0.5f : 1);
        magicDamage = magicDamage * (target.Defence / (target.Defence + target.CharacterData.MagicResistance)) * (target.IsDefence ? 0.5f : 1);

        return physicsDamage + magicDamage;
    }

    #endregion
}
