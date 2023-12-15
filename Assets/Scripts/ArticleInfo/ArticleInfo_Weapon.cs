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
    public float Aggressivity;
    /// <summary>
    /// �;ö�
    /// </summary>
    public float Durability;
    /// <summary>
    /// ��ħЧ��
    /// </summary>
    public ArticleEnchanting Enchant;
    /// <summary>
    /// ��ħ��ֵ
    /// </summary>
    public float EnchantValue;

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
