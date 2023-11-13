using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// ������ֵ����ģ��
    /// </summary>
    public CharacterData_SO TemplateData;

    /// <summary>
    /// ������ֵ����
    /// </summary>
    [HideInInspector]
    public CharacterData_SO CharacterData;

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
        }
    }

    #region ��������֮��ķ���

    /// <summary>
    /// �˺�
    /// </summary>
    /// <param name="attacker"></param>
    public void TakeDamage(CharacterStats attacker, CharacterStats target)
    {
        float damage = CalculateDamage(attacker, target); 

        target.CharacterData.GetHurt(damage);
        //������Ϣ����Ŀ���Ѫ��
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, target);

        //TODO:����������ɽ�ֱ��״̬
        //if (attacker.IsCritical && !target.IsDefence)
        //{
        //    //������Ŀ��δ����������Ŀ����ܵ��˺�����
        //    target.GetComponent<Animator>().SetTrigger("GetHurt");
        //}
    }

    /// <summary>
    /// �˺�
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="target"></param>
    public void TakeDamage(float damage, CharacterStats target)
    {
        target.CharacterData.GetHurt(damage);
        //������Ϣ
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, target);
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
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, this);
    }

    /// <summary>
    /// �����Ƿ񱩻�
    /// </summary>
    /// <param name="attacker"></param>
    public void CalculateCritical(CharacterStats attacker)
    {
        attacker.IsCritical = Random.value <= attacker.CharacterData.CriticalChance;
    }

    /// <summary>
    /// �����˺�����Ϲ������Ĺ���������ǿ�������ʣ������ӳ��Լ����������Ļ��ף�ħ���ȶ������ݽ��м���
    /// </summary>
    /// <returns></returns>
    public float CalculateDamage(CharacterStats attacker, CharacterStats target)
    {
        // ��ɵ��˺�
        //Ŀ��ķ���ֵ�ɵ��������˺���ʹ�øù�ʽ������Ŀ��ķ���ֵ�Ƕ��٣���������˺���
        //ֻ�Ƿ���ֵԽ�ߣ��˺�ԽС������ֵΪ0�����100%�˺�
        //���ֹ�ʽ����ڼ�ֵЧ����������ֵԽ�󣬵����˺��Ĳ�ֵԽС
        //����Ƿ����״̬������״̬�£��ܵ��˺�����

        //TODO:�޸ļ����˺��ķ������˺��������˺���ħ���˺�
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
