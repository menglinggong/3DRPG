using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʒ����Ϣ
/// </summary>
[Serializable]
public class ArticleInfo_Bow : ArticleInfoBase
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
    /// �������ͣ�����Ʒ�ĸ�ħЧ������ͻ
    /// </summary>
    public BowKind BowKind;
    /// <summary>
    /// ���Ĳ�������
    /// </summary>
    public ArticleKind_Material MaterialKind;
    /// <summary>
    /// ���
    /// </summary>
    public int Range;
    /// <summary>
    /// ��ħЧ��
    /// </summary>
    public ArticleEnchanting Enchant;
    /// <summary>
    /// ��ħ��ֵ
    /// </summary>
    public int EnchantValue;
}

/// <summary>
/// ������Ʒ����Ϣ
/// </summary>
public class ArticleInfo_Arrow : ArticleInfoBase
{
    /// <summary>
    /// ��������
    /// </summary>
    public ArrowKind ArrowKind;
    /// <summary>
    /// ���е�����
    /// </summary>
    public int Count;
}


/// <summary>
/// �������࣬����2������3������Զ�乭�����乭��
/// </summary>
public enum BowKind
{
    Default,            //��ͨ����������Ч��
    TwoArrowBow,        //2������һ����2֧������ֻ����һ֧��
    ThreeArrowBow,      //3����
    FiveArrowBow,       //5����
    FawBow,             //Զ�乭�����Զ�����ڸ�Զ������
    FastBow,            //���ٸ���
}

/// <summary>
/// ��������
/// </summary>
public enum ArrowKind
{
    Default,                //��ͨľ��
    FireArrow,              //���
    IceArrow,               //����
    ElectricityArrow,       //���
    ExprodeArrow,           //��ը��
}
