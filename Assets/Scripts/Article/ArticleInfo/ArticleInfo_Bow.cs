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

    public ArticleInfo_Bow(ArticleInfo_Bow info) : base(info)
    {
        this.Aggressivity = info.Aggressivity;
        this.Durability = info.Durability;
        this.BowKind = info.BowKind;
        this.MaterialKind = info.MaterialKind;
        this.Range = info.Range;
        this.Enchant = info.Enchant;
        this.EnchantValue = info.EnchantValue;
    }

    public ArticleInfo_Bow()
    {

    }

    public override ArticleInfoBase Copy()
    {
        ArticleInfo_Bow info = new ArticleInfo_Bow(this);
        return info;
    }
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

    public ArticleInfo_Arrow(ArticleInfo_Arrow info) : base(info)
    {
        this.ArrowKind = info.ArrowKind;
        this.Count = info.Count;
    }

    public ArticleInfo_Arrow()
    {

    }

    public override ArticleInfoBase Copy()
    {
        ArticleInfo_Arrow info = new ArticleInfo_Arrow(this);
        return info;
    }
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
