using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������Ʒ��Ϣ
/// </summary>
[Serializable]
public class ArticleInfo_Weapon : ArticleInfoBase
{
    /// <summary>
    /// ������
    /// </summary>
    public int Aggressivity;
    /// <summary>
    /// �;ö�
    /// </summary>
    public int Durability;
    /// <summary>
    /// ��ħЧ��
    /// </summary>
    public ArticleEnchanting Enchant;
    /// <summary>
    /// ��ħ��ֵ
    /// </summary>
    public int EnchantValue;

    /// <summary>
    /// ��������
    /// </summary>
    public WeaponKind WeaponKind;
    /// <summary>
    /// ��������-�ֳ�����
    /// </summary>
    public WeaponKind_Hand HandKind;
    /// <summary>
    /// ��Ʒ����-����
    /// </summary>
    public ArticleKind_Material MaterialKind;
    /// <summary>
    /// ��������-����Ч��
    /// </summary>
    public WeaponKind_Effect EffectKind;

    public ArticleInfo_Weapon(ArticleInfo_Weapon info) : base(info)
    {
        this.Aggressivity = info.Aggressivity;
        this.Durability = info.Durability;
        this.Enchant = info.Enchant;
        this.EnchantValue = info.EnchantValue;
        this.WeaponKind = info.WeaponKind;
        this.HandKind = info.HandKind;
        this.MaterialKind = info.MaterialKind;
        this.EffectKind = info.EffectKind;
    }

    public ArticleInfo_Weapon()
    {

    }

    public override ArticleInfoBase Copy()
    {
        ArticleInfo_Weapon info = new ArticleInfo_Weapon(this);
        return info;
    }
}

/// <summary>
/// ��������-�ֳ�����
/// </summary>
public enum WeaponKind_Hand
{
    OneHanded,          //���ֳ�
    TwoHanded,          //˫�ֳ�
}

/// <summary>
/// ��������
/// </summary>
public enum WeaponKind
{
    Sword,              //��
    Axe,                //��
    Hammer,             //��
    Spear,              //ǹ
    Staff,              //����
}

/// <summary>
/// ��������-����Ч��
/// </summary>
public enum WeaponKind_Effect
{
    Default,            //��
    Burn,               //����
    Freeze,             //����
    ElectricShock       //���
}
